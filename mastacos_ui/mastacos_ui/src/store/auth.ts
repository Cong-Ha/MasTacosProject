import { defineStore } from 'pinia';
import { ref, computed, readonly } from 'vue';
import authService, { type UserInfo, type AuthResponse } from '@/services/authService';

export const useAuthStore = defineStore('auth', () => {
    // State
    const isAuthenticated = ref<boolean>(false);
    const user = ref<UserInfo | null>(null);
    const isLoading = ref<boolean>(true);
    const isInitialized = ref<boolean>(false);

    // Getters (computed properties)
    const userFullName = computed(() => {
        if (user.value) {
            return `${user.value.firstName} ${user.value.lastName}`.trim();
        }
        return '';
    });

    const userEmail = computed(() => user.value?.email || '');

    const userRoles = computed(() => user.value?.roles || []);

    const isAdmin = computed(() => userRoles.value.includes('Admin'));

    const isCustomer = computed(() => userRoles.value.includes('Customer'));

    // Actions
    const initializeAuth = async (): Promise<void> => {
        if (isInitialized.value) return; // Prevent multiple initializations

        try {
            isLoading.value = true;

            if (authService.isAuthenticated()) {
                // Get user info from localStorage first for immediate UI update
                const cachedUser = authService.getUserInfo();
                if (cachedUser) {
                    user.value = cachedUser;
                    isAuthenticated.value = true;
                }

                // Then refresh user info from API
                try {
                    const currentUser = await authService.getCurrentUser();
                    if (currentUser) {
                        user.value = currentUser;
                        isAuthenticated.value = true;
                    } else {
                        // API call failed, clear auth state
                        await logout();
                    }
                } catch (error) {
                    console.error('Failed to refresh user info:', error);
                    // API might be down, but keep cached user if available
                    if (!cachedUser) {
                        await logout();
                    }
                }
            } else {
                isAuthenticated.value = false;
                user.value = null;
            }
        } catch (error) {
            console.error('Auth initialization error:', error);
            isAuthenticated.value = false;
            user.value = null;
        } finally {
            isLoading.value = false;
            isInitialized.value = true;
        }
    };

    const login = async (email: string, password: string, rememberMe: boolean = false): Promise<AuthResponse> => {
        try {
            const response = await authService.login(email, password, rememberMe);

            if (response.success && response.user) {
                isAuthenticated.value = true;
                user.value = response.user;
            }

            return response;
        } catch (error) {
            console.error('Login error in store:', error);
            throw error;
        }
    };

    const register = async (registerData: {
        firstName: string;
        lastName: string;
        email: string;
        password: string;
        confirmPassword: string;
    }): Promise<AuthResponse> => {
        try {
            const response = await authService.register(registerData);

            if (response.success && response.user) {
                isAuthenticated.value = true;
                user.value = response.user;
            }

            return response;
        } catch (error) {
            console.error('Registration error in store:', error);
            throw error;
        }
    };

    const logout = async (): Promise<void> => {
        try {
            await authService.logout();
        } catch (error) {
            console.error('Logout error in store:', error);
        } finally {
            // Always clear the store state
            isAuthenticated.value = false;
            user.value = null;
        }
    };

    const refreshUser = async (): Promise<void> => {
        try {
            if (!isAuthenticated.value) return;

            const currentUser = await authService.getCurrentUser();
            if (currentUser) {
                user.value = currentUser;
            } else {
                await logout();
            }
        } catch (error) {
            console.error('Refresh user error:', error);
            await logout();
        }
    };

    // Role checking methods
    const hasRole = (role: string): boolean => {
        return userRoles.value.includes(role);
    };

    const hasAnyRole = (roles: string[]): boolean => {
        return roles.some(role => userRoles.value.includes(role));
    };

    const hasAllRoles = (roles: string[]): boolean => {
        return roles.every(role => userRoles.value.includes(role));
    };

    // Listen for storage changes (for multi-tab support)
    if (typeof window !== 'undefined') {
        window.addEventListener('storage', (event) => {
            if (event.key === 'auth_token' || event.key === 'user_info') {
                // Re-initialize auth state when storage changes
                initializeAuth();
            }
        });
    }

    return {
        // State
        isAuthenticated: readonly(isAuthenticated),
        user: readonly(user),
        isLoading: readonly(isLoading),
        isInitialized: readonly(isInitialized),

        // Getters
        userFullName,
        userEmail,
        userRoles,
        isAdmin,
        isCustomer,

        // Actions
        initializeAuth,
        login,
        register,
        logout,
        refreshUser,
        hasRole,
        hasAnyRole,
        hasAllRoles
    };
});

// For backward compatibility, create a composable that uses the store
export function useAuth() {
    const authStore = useAuthStore();

    return {
        // State
        isAuthenticated: authStore.isAuthenticated,
        user: authStore.user,
        isLoading: authStore.isLoading,

        // Getters
        userFullName: authStore.userFullName,
        userEmail: authStore.userEmail,
        userRoles: authStore.userRoles,
        isAdmin: authStore.isAdmin,
        isCustomer: authStore.isCustomer,

        // Actions
        login: authStore.login,
        register: authStore.register,
        logout: authStore.logout,
        initializeAuth: authStore.initializeAuth,
        refreshUser: authStore.refreshUser,
        hasRole: authStore.hasRole,
        hasAnyRole: authStore.hasAnyRole,
        hasAllRoles: authStore.hasAllRoles
    };
}