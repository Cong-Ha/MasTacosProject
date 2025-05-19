import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios';
import authService from '@/services/authService';
import router from '@/router';

// Request interceptor to add auth token to requests
axios.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = authService.getToken();

        if (token && authService.isAuthenticated()) {
            // Add the token to the Authorization header
            config.headers.Authorization = `Bearer ${token}`;
        }

        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Response interceptor to handle token expiration and other auth errors
axios.interceptors.response.use(
    (response) => {
        // Return successful responses as-is
        return response;
    },
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean };

        // Handle 401 Unauthorized responses
        if (error.response?.status === 401) {
            // Avoid infinite loops
            if (!originalRequest._retry) {
                originalRequest._retry = true;

                // Check if this is a token expiration issue
                if (!authService.isAuthenticated()) {
                    // Token is expired or invalid, logout user
                    await authService.logout();

                    // Redirect to login page with current route as redirect
                    const currentPath = router.currentRoute.value.fullPath;
                    if (currentPath !== '/login' && currentPath !== '/register') {
                        router.push({
                            path: '/login',
                            query: { redirect: currentPath }
                        });
                    }

                    return Promise.reject(error);
                }
            }
        }

        // Handle 403 Forbidden responses
        if (error.response?.status === 403) {
            // User doesn't have permission, redirect to unauthorized page
            router.push('/unauthorized');
            return Promise.reject(error);
        }

        // Handle 423 Locked responses (account lockout)
        if (error.response?.status === 423) {
            // Account is locked, show appropriate error
            return Promise.reject(error);
        }

        // For all other errors, just reject the promise
        return Promise.reject(error);
    }
);

// Setup function to configure base URL and other defaults
export function setupAxiosInterceptors(baseURL?: string): void {
    if (baseURL) {
        axios.defaults.baseURL = baseURL;
    }

    // Set default timeout
    axios.defaults.timeout = 10000; // 10 seconds

    // Set default headers
    axios.defaults.headers.common['Content-Type'] = 'application/json';

    // Initialize auth header if user is already logged in
    const token = authService.getToken();
    if (token && authService.isAuthenticated()) {
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }
}

// Export the configured axios instance
export { axios as apiClient };