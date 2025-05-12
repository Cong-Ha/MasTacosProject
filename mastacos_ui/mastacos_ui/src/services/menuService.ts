import axios from 'axios';
import type { MenuItem } from '@/types';
import { API_BASE_URL } from '@/config';

/**
 * Create axios instance with default configuration
 */
const api = axios.create({
    baseURL: API_BASE_URL,
    timeout: 10000, // 10 seconds timeout
    headers: {
        'Content-Type': 'application/json'
    }
});

/**
 * Service for handling menu item related API calls
 */
export const menuService = {
    /**
     * Fetch all menu items
     * @returns Promise with array of MenuItem objects
     */
    async getMenuItems(): Promise<MenuItem[]> {
        try {
            console.log(`Fetching menu items from: ${API_BASE_URL}/menuItems`);
            const response = await api.get('/menuItems');
            return response.data;
        } catch (error) {
            console.error('Error fetching menu items:', error);
            if (axios.isAxiosError(error)) {
                console.error('Response status:', error.response?.status);
                console.error('Response data:', error.response?.data);
            }
            throw error;
        }
    },

    /**
     * Fetch menu items by category
     * @param category Category to filter by
     * @returns Promise with filtered array of MenuItem objects
     */
    async getMenuItemsByCategory(category: string): Promise<MenuItem[]> {
        try {
            console.log(`Fetching menu items for category ${category}`);
            const response = await api.get(`/menuItems/category/${category}`);
            return response.data;
        } catch (error) {
            console.error(`Error fetching menu items for category ${category}:`, error);
            if (axios.isAxiosError(error)) {
                console.error('Response status:', error.response?.status);
                console.error('Response data:', error.response?.data);
            }
            throw error;
        }
    },

    /**
     * Fetch a single menu item by ID
     * @param id Menu item ID
     * @returns Promise with MenuItem object
     */
    async getMenuItem(id: number): Promise<MenuItem> {
        try {
            console.log(`Fetching menu item ${id}`);
            const response = await api.get(`/menuItems/${id}`);
            return response.data;
        } catch (error) {
            console.error(`Error fetching menu item ${id}:`, error);
            if (axios.isAxiosError(error)) {
                console.error('Response status:', error.response?.status);
                console.error('Response data:', error.response?.data);
            }
            throw error;
        }
    },

    /**
     * Fetch popular menu items (popularityScore >= 80)
     * @returns Promise with array of popular MenuItem objects
     */
    async getPopularItems(): Promise<MenuItem[]> {
        try {
            console.log('Fetching popular menu items');
            const response = await api.get('/menuItems/popular');
            return response.data;
        } catch (error) {
            console.error('Error fetching popular menu items:', error);
            if (axios.isAxiosError(error)) {
                console.error('Response status:', error.response?.status);
                console.error('Response data:', error.response?.data);
            }
            throw error;
        }
    }
};