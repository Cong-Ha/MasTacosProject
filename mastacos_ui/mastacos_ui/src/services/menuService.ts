import axios from 'axios';
import type { MenuItem, NewMenuItem } from '@/types';
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
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
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
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
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
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
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
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    },

    /**
     * Create a new menu item
     * @param menuItem MenuItem object without ID
     * @returns Promise with created MenuItem object (including ID)
     */
    async createMenuItem(menuItem: NewMenuItem): Promise<MenuItem> {
        try {
            console.log('Creating new menu item:', menuItem);
            const response = await api.post('/menuItems', menuItem);
            return response.data;
        } catch (error) {
            console.error('Error creating menu item:', error);
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    },

    /**
     * Update an existing menu item
     * @param menuItem MenuItem object with ID
     * @returns Promise with updated MenuItem object
     */
    async updateMenuItem(menuItem: MenuItem): Promise<MenuItem> {
        try {
            console.log(`Updating menu item ${menuItem.itemId}:`, menuItem);
            const response = await api.put(`/menuItems/${menuItem.itemId}`, menuItem);
            return response.data;
        } catch (error) {
            console.error(`Error updating menu item ${menuItem.itemId}:`, error);
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    },

    /**
     * Delete a menu item
     * @param id Menu item ID to delete
     * @returns Promise with success status
     */
    async deleteMenuItem(id: number): Promise<void> {
        try {
            console.log(`Deleting menu item ${id}`);
            await api.delete(`/menuItems/${id}`);
        } catch (error) {
            console.error(`Error deleting menu item ${id}:`, error);
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    },

    /**
     * Update menu item status (active/inactive)
     * @param id Menu item ID
     * @param isActive New active status
     * @returns Promise with updated MenuItem object
     */
    async updateMenuItemStatus(id: number, isActive: boolean): Promise<MenuItem> {
        try {
            console.log(`Updating menu item ${id} status to ${isActive}`);
            const response = await api.patch(`/menuItems/${id}/status`, { isActive });
            return response.data;
        } catch (error) {
            console.error(`Error updating menu item ${id} status:`, error);
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    },

    /**
     * Upload an image for a menu item
     * @param id Menu item ID
     * @param file Image file to upload
     * @returns Promise with success message
     */
    async uploadImage(id: number, file: File): Promise<{ message: string }> {
        try {
            console.log(`Uploading image for menu item ${id}`);

            // Need to use a different axios instance for form data
            const formData = new FormData();
            formData.append('file', file);

            const response = await axios.post(`${API_BASE_URL}/menuItems/${id}/image`, formData, {
                headers: {
                    'Content-Type': 'multipart/form-data'
                }
            });

            return response.data;
        } catch (error) {
            console.error(`Error uploading image for menu item ${id}:`, error);
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    },

    /**
     * Delete image from a menu item
     * @param id Menu item ID
     * @returns Promise with success message
     */
    async deleteImage(id: number): Promise<{ message: string }> {
        try {
            console.log(`Deleting image from menu item ${id}`);
            const response = await api.delete(`/menuItems/${id}/image`);
            return response.data;
        } catch (error) {
            console.error(`Error deleting image from menu item ${id}:`, error);
            if (axios.isAxiosError(error) && error.response) {
                console.error('Response status:', error.response.status);
                console.error('Response data:', error.response.data);
            }
            throw error;
        }
    }
};