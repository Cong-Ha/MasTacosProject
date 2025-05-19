import { NavigationGuardNext, RouteLocationNormalized } from 'vue-router';
import { useAuthStore } from '@/store/auth';

// Auth guard for protected routes
export const authGuard = (
    to: RouteLocationNormalized,
    from: RouteLocationNormalized,
    next: NavigationGuardNext
): void => {
    const authStore = useAuthStore();

    if (authStore.isAuthenticated) {
        next();
    } else {
        // Redirect to login with the original destination
        next({
            path: '/login',
            query: { redirect: to.fullPath }
        });
    }
};

// Guest guard for auth pages (login/register) - prevents authenticated users from accessing these pages
export const guestGuard = (
    to: RouteLocationNormalized,
    from: RouteLocationNormalized,
    next: NavigationGuardNext
): void => {
    const authStore = useAuthStore();

    if (authStore.isAuthenticated) {
        // User is already authenticated, redirect to home
        next('/');
    } else {
        next();
    }
};

// Role guard for role-based access control
export const roleGuard = (requiredRoles: string[]) => {
    return (
        to: RouteLocationNormalized,
        from: RouteLocationNormalized,
        next: NavigationGuardNext
    ): void => {
        const authStore = useAuthStore();

        if (!authStore.isAuthenticated) {
            next({
                path: '/login',
                query: { redirect: to.fullPath }
            });
            return;
        }

        if (authStore.hasAnyRole(requiredRoles)) {
            next();
        } else {
            // User doesn't have required role, redirect to unauthorized page
            next({
                path: '/unauthorized',
                query: { redirect: to.fullPath }
            });
        }
    };
};

// Admin guard (shorthand for admin role)
export const adminGuard = roleGuard(['Admin']);

// Customer guard (shorthand for customer role)
export const customerGuard = roleGuard(['Customer']);