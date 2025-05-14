import { defineStore } from 'pinia'
import type { MenuItem, NewMenuItem } from '@/types'
import { menuService } from '@/services/menuService'

export const useMenuItemsStore = defineStore('menuItems', {
    state: () => ({
        items: [] as MenuItem[],
        loading: false,
        error: null as string | null,
        lastFetched: null as Date | null,
    }),

    getters: {
        // get all actve menu items
        activeItems: (state) => state.items.filter(item => item.isActive),

        //get all categories from all items (for admin)
        allCategories: (state) => {
            const uniqueCategories = new Set(state.items.map(item => item.category))
            return Array.from(uniqueCategories).sort()
        },

        // get categories only from active items (for customer view)
        activeCategories: (state) => {
            const activeItems = state.items.filter(item => item.isActive)
            const uniqueCategories = new Set(activeItems.map(item => item.category))
            return Array.from(uniqueCategories).sort()
        },

        //check if data needs refreshing
        needsRefresh: (state) => {
            if (!state.lastFetched) return true

            //define cache duration (5 mins)
            const CACHE_DURATION = 5 * 60 * 1000
            const now = new Date().getTime()
            const lastFetchedTime = state.lastFetched.getTime()

            return (now - lastFetchedTime) > CACHE_DURATION
        }
    },

    actions: {
        async fetchMenuItems(force = false) {
            if (this.loading) return

            //check if we need to refresh data
            if (!force && !this.needsRefresh && this.items.length > 0) {
                console.log('Using cached data')
                return
            }
            this.loading = true
            this.error = null

            try {
                console.log('Fetching menu items from API')
                const data = await menuService.getMenuItems()
                this.items = data
                this.lastFetched = new Date()
            } catch (err) {
                if (err instanceof Error) {
                    this.error = err.message
                } else {
                    this.error = 'An unknown error occurred'
                }
                console.error('Failed to fetch menu items:', err)
            } finally {
                this.loading = false
            }
        },

        //create a new menu item
        async createMenuItem(menuItem: NewMenuItem) {
            try {
                const createdItem = await menuService.createMenuItem(menuItem)
                //add to local state
                this.items.push(createdItem)
                return createdItem
            } catch (error) {
                console.error("Error creating menu item:", error)
                throw error
            }
        },

        // Update an existing menu item
        async updateMenuItem(menuItem: MenuItem) {
            try {
                const updatedItem = await menuService.updateMenuItem(menuItem)
                // Update in local state
                const index = this.items.findIndex(item => item.itemId === menuItem.itemId)
                if (index !== -1) {
                    this.items[index] = updatedItem
                }
                return updatedItem
            } catch (error) {
                console.error('Error updating menu item:', error)
                throw error
            }
        },

        // Delete a menu item
        async deleteMenuItem(itemId: number) {
            try {
                await menuService.deleteMenuItem(itemId)
                // Remove from local state
                const index = this.items.findIndex(item => item.itemId === itemId)
                if (index !== -1) {
                    this.items.splice(index, 1)
                }
            } catch (error) {
                console.error('Error deleting menu item:', error)
                throw error
            }
        },

        // Update a menu item's active status
        async updateMenuItemStatus(itemId: number, isActive: boolean): Promise<MenuItem> {
            try {
                // Call the API
                const updatedItem = await menuService.updateMenuItemStatus(itemId, isActive);

                // Update the item in the store - create a new array for reactivity
                const index = this.items.findIndex(item => item.itemId === itemId);
                if (index !== -1) {
                    // Create a new array to ensure Vue's reactivity system detects the change
                    const updatedItems = [...this.items];
                    updatedItems[index] = updatedItem;
                    this.items = updatedItems; // Replace the entire array with the updated one
                }

                return updatedItem;
            } catch (error) {
                console.error('Error updating menu item status:', error);
                throw error;
            }
        }
    }
})