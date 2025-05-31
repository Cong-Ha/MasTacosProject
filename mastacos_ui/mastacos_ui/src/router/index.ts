import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import MenuItemsList from '../components/MenuItemsList.vue'
import { authGuard, guestGuard, adminGuard } from '@/guards/authGuards'

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'Home',
        component: () => import('../components/LandingPage.vue')
    },
    {
        path: '/MasTacosProject',
        name: 'GithubPages',
        component: () => import('../components/LandingPage.vue')
    },
    {
        path: '/menu',
        name: 'Menu',
        component: MenuItemsList  // Directly imported home page
    },
    {
        path: '/admin',
        name: 'Admin',
        component: () => import('../components/AdminPage.vue'),  // Lazy loaded admin page
        beforeEnter: adminGuard,
        meta: {
            requiresAuth: true,
            isAdmin: true
        }
    },
    {
        path: '/login',
        name: 'Login',
        component: () => import('../components/LoginPage.vue'),
        beforeEnter: guestGuard, // Redirect to home if already logged in
        meta: {
            requiresGuest: true
        }
    },
    {
        path: '/register',
        name: 'Register',
        component: () => import('../components/RegisterPage.vue'),
        beforeEnter: guestGuard, // Redirect to home if already logged in
        meta: {
            requiresGuest: true
        }
    },
    // Protected routes (require authentication)
    {
        path: '/profile',
        name: 'Profile',
        component: () => import('../components/ProfilePage.vue'),
        beforeEnter: authGuard,
        meta: {
            requiresAuth: true
        }
    },
    // {
    //     path: '/orders',
    //     name: 'Orders',
    //     component: () => import('../components/OrdersPage.vue'),
    //     beforeEnter: authGuard,
    //     meta: {
    //         requiresAuth: true
    //     }
    // },
    // {
    //     path: '/reservations',
    //     name: 'Reservations',
    //     component: () => import('../components/ReservationsPage.vue'),
    //     beforeEnter: authGuard,
    //     meta: {
    //         requiresAuth: true
    //     }
    // },
    // Unauthorized page for role-based access control
    {
        path: '/unauthorized',
        name: 'Unauthorized',
        component: () => import('../components/UnauthorizedPage.vue'),
        meta: {
            title: 'Unauthorized Access'
        }
    },
    // Catch-all route for 404 pages
    {
        path: '/:pathMatch(.*)*',
        name: 'NotFound',
        component: () => import('../components/NotFound.vue')  // Lazy loaded 404 page
    }
]

const router = createRouter({
    history: createWebHistory(),
    routes
})

// Global navigation guard (optional - for additional functionality)
router.beforeEach((to, from, next) => {
    // Set page title based on route meta or name
    if (to.meta?.title) {
        document.title = `${to.meta.title} - Ma's Tacos`;
    } else {
        document.title = `${to.name as string} - Ma's Tacos`;
    }

    // You can add additional global logic here, such as:
    // - Loading indicators
    // - Analytics tracking
    // - Progress bars

    next();
});

export default router