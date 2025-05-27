import axios, { AxiosResponse } from 'axios';
import { API_BASE_URL } from '@/config';

// Types
interface LoginRequest {
    email: string;
    password: string;
    rememberMe: boolean;
}

interface RegisterRequest {
    email: string;
    password: string;
    confirmPassword: string;
    firstName: string;
    lastName: string;
}

interface AuthResponse {
    success: boolean;
    message: string;
    token?: string;
    user?: UserInfo;
}

interface UserInfo {
    id: string;
    email: string;
    firstName: string;
    lastName: string;
    roles: string[];
}

// Constants
const TOKEN_KEY = 'auth_token';
const USER_KEY = 'user_info';

class AuthService {
    private baseURL: string;

    constructor() {
        this.baseURL = API_BASE_URL;
    }

    // Login method
    async login(email: string, password: string, rememberMe: boolean = false): Promise<AuthResponse> {
        try {
            const response: AxiosResponse<AuthResponse> = await axios.post(
                `${this.baseURL}/auth/login`,
                {
                    email,
                    password,
                    rememberMe
                } as LoginRequest
            );

            if (response.data.success && response.data.token) {
                // Store token and user info
                localStorage.setItem(TOKEN_KEY, response.data.token);
                if (response.data.user) {
                    localStorage.setItem(USER_KEY, JSON.stringify(response.data.user));
                }

                // Set default authorization header for future requests
                this.setAuthHeader(response.data.token);
            }

            return response.data;
        } catch (error) {
            console.error('Login error:', error);
            throw error;
        }
    }

    // Register method
    async register(registerData: RegisterRequest): Promise<AuthResponse> {
        try {
            const response: AxiosResponse<AuthResponse> = await axios.post(
                `${this.baseURL}/auth/register`,
                registerData
            );

            if (response.data.success && response.data.token) {
                // Store token and user info
                localStorage.setItem(TOKEN_KEY, response.data.token);
                if (response.data.user) {
                    localStorage.setItem(USER_KEY, JSON.stringify(response.data.user));
                }

                // Set default authorization header for future requests
                this.setAuthHeader(response.data.token);
            }

            return response.data;
        } catch (error) {
            console.error('Registration error:', error);
            throw error;
        }
    }

    // Logout method
    async logout(): Promise<void> {
        try {
            // Call logout endpoint if needed
            await axios.post(`${this.baseURL}/auth/logout`);
        } catch (error) {
            console.error('Logout error:', error);
        } finally {
            // Always clear local storage and auth header
            localStorage.removeItem(TOKEN_KEY);
            localStorage.removeItem(USER_KEY);
            delete axios.defaults.headers.common['Authorization'];
        }
    }

    // Get current user info
    async getCurrentUser(): Promise<UserInfo | null> {
        try {
            const response: AxiosResponse<UserInfo> = await axios.get(
                `${this.baseURL}/auth/user`
            );
            return response.data;
        } catch (error) {
            console.error('Get current user error:', error);
            // If request fails, clear stored auth data
            this.logout();
            return null;
        }
    }

    // Check if user is authenticated
    isAuthenticated(): boolean {
        const token = localStorage.getItem(TOKEN_KEY);
        if (!token) return false;

        // Check if token is expired
        try {
            const payload = JSON.parse(atob(token.split('.')[1]));
            const currentTime = Date.now() / 1000;
            return payload.exp > currentTime;
        } catch (error) {
            // If token is malformed, consider user not authenticated
            return false;
        }
    }

    // Get stored token
    getToken(): string | null {
        return localStorage.getItem(TOKEN_KEY);
    }

    // Get stored user info
    getUserInfo(): UserInfo | null {
        const userInfo = localStorage.getItem(USER_KEY);
        return userInfo ? JSON.parse(userInfo) : null;
    }

    // Set authorization header
    private setAuthHeader(token: string): void {
        axios.defaults.headers.common['Authorization'] = `Bearer ${token}`;
    }

    // Initialize the service (call this when your app starts)
    initialize(): void {
        const token = this.getToken();
        if (token && this.isAuthenticated()) {
            this.setAuthHeader(token);
        } else {
            // Clear invalid token
            this.logout();
        }
    }

    // Check if user has specific role
    hasRole(role: string): boolean {
        const userInfo = this.getUserInfo();
        return userInfo?.roles?.includes(role) || false;
    }

    // Check if user has any of the specified roles
    hasAnyRole(roles: string[]): boolean {
        const userInfo = this.getUserInfo();
        if (!userInfo?.roles) return false;
        return roles.some(role => userInfo.roles.includes(role));
    }
}

// Create and export a singleton instance
const authService = new AuthService();

// Initialize the service
authService.initialize();

export default authService;

// Export types for use in components
export type { LoginRequest, RegisterRequest, AuthResponse, UserInfo };