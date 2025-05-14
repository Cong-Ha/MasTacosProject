import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import MenuItemsList from '../components/MenuItemsList.vue'

const routes: Array<RouteRecordRaw> = [
    {
        path: '/',
        name: 'Home',
        component: MenuItemsList  // Directly imported home page
    },
    {
        path: '/admin',
        name: 'Admin',
        component: () => import('../components/AdminPage.vue'),  // Lazy loaded admin page
        meta: {
            requiresAuth: true,
            isAdmin: true
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

// Optional: Add navigation guards for admin authentication
//router.beforeEach((to, from, next) => {
//    // Example authentication check (replace with your actual auth logic)
//    const isAuthenticated = false // Your auth check here
//    const isAdmin = false // Your admin check here

//    if (to.meta.requiresAuth && !isAuthenticated) {
//        next('/login') // Redirect to login if not authenticated
//    } else if (to.meta.isAdmin && !isAdmin) {
//        next('/') // Redirect to home if not admin
//    } else {
//        next() // Proceed as normal
//    }
//})

export default router