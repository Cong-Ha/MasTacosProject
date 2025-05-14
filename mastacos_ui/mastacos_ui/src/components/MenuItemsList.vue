<script setup lang="ts">
    import { ref, computed, onMounted } from 'vue';
    import type { MenuItem } from '@/types';
    import MenuItemModal from './MenuItemModal.vue';
    import ShoppingCart from './ShoppingCart.vue';
    import type { CartItem } from './ShoppingCart.vue';
    import { useMenuItemsStore } from '@/store/menuItemsStore';

    // Initialize the store
    const menuItemsStore = useMenuItemsStore();

    // Local component state
    const selectedCategory = ref<string | null>(null);
    const selectedMenuItem = ref<MenuItem | null>(null);
    const isModalVisible = ref(false);
    const cart = ref<CartItem[]>([]);

    // Get computed properties from the store
    const loading = computed(() => menuItemsStore.loading);
    const error = computed(() => menuItemsStore.error);

    // Get active menu items from store
    const activeMenuItems = computed(() =>
        menuItemsStore.items.filter(item => item.isActive === true)
    );

    // Get categories only from active menu items
    const categories = computed(() => {
        const uniqueCategories = new Set(activeMenuItems.value.map(item => item.category));
        return Array.from(uniqueCategories).sort();
    });

    // Filter items based on selected category
    const filteredMenuItems = computed(() => {
        if (!selectedCategory.value) return activeMenuItems.value;
        return activeMenuItems.value.filter(item => item.category === selectedCategory.value);
    });

    // Methods
    const fetchMenuItems = async () => {
        // Use the store's method to fetch items
        await menuItemsStore.fetchMenuItems();
    };

    const openModal = (item: MenuItem) => {
        selectedMenuItem.value = item;
        isModalVisible.value = true;
    };

    const closeModal = () => {
        isModalVisible.value = false;
        // Slight delay to allow for transition effect
        setTimeout(() => {
            selectedMenuItem.value = null;
        }, 300);
    };

    const addToOrder = (orderDetails: CartItem) => {
        cart.value.push(orderDetails);
        console.log('Added to cart:', orderDetails);
        console.log('Current cart:', cart.value);
        closeModal();
    };

    const removeCartItem = (index: number) => {
        cart.value.splice(index, 1);
    };

    // Fetch menu items on component mount
    onMounted(() => {
        fetchMenuItems();
    });
</script>

<template>
    <div class="menu-container">
        <!-- Header with shopping cart -->
        <div class="menu-header mb-4 d-flex justify-content-between align-items-center">
            <h1 class="mb-0">Ma's Tacos Menu</h1>
            <ShoppingCart :cart-items="cart"
                          @remove-item="removeCartItem" />
        </div>

        <!-- Loading state -->
        <div v-if="loading" class="text-center my-5">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-3">Loading menu items...</p>
        </div>

        <!-- Error state -->
        <div v-else-if="error" class="alert alert-danger mx-auto" style="max-width: 500px;">
            <h4 class="alert-heading">Error Loading Menu</h4>
            <p>{{ error }}</p>
            <button class="btn btn-outline-danger" @click="fetchMenuItems">Try Again</button>
        </div>

        <!-- Empty state when no active menu items -->
        <div v-else-if="activeMenuItems.length === 0" class="alert alert-info mx-auto" style="max-width: 500px;">
            <h4 class="alert-heading">No Menu Items Available</h4>
            <p>There are currently no active menu items. Please check back later!</p>
        </div>

        <!-- Menu items display -->
        <div v-else class="row">
            <div class="col-md-3 mb-4">
                <!-- Category filter -->
                <div class="card">
                    <div class="card-header">
                        <h5 class="mb-0">Categories</h5>
                    </div>
                    <div class="card-body">
                        <div class="list-group">
                            <button class="list-group-item list-group-item-action"
                                    :class="{ active: selectedCategory === null }"
                                    @click="selectedCategory = null">
                                All Items
                            </button>
                            <button v-for="category in categories"
                                    :key="category"
                                    class="list-group-item list-group-item-action"
                                    :class="{ active: selectedCategory === category }"
                                    @click="selectedCategory = category">
                                {{ category }}
                            </button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-md-9">
                <div class="row">
                    <!-- No items in selected category message -->
                    <div v-if="filteredMenuItems.length === 0" class="col-12">
                        <div class="alert alert-info">
                            <p class="mb-0">No menu items available in this category.</p>
                        </div>
                    </div>

                    <div v-for="item in filteredMenuItems"
                         :key="item.itemId"
                         class="col-md-6 col-lg-4 mb-4">
                        <div class="card h-100 menu-item">
                            <div class="card-body">
                                <div class="d-flex justify-content-between">
                                    <h5 class="card-title">{{ item.name }}</h5>
                                    <span class="badge bg-success price-badge">${{ item.price.toFixed(2) }}</span>
                                </div>
                                <p class="card-text">{{ item.description }}</p>
                                <div class="d-flex justify-content-between align-items-center mt-auto">
                                    <span class="badge bg-light text-dark">{{ item.category }}</span>
                                    <span v-if="item.popularityScore >= 80" class="badge bg-danger">Popular</span>
                                </div>
                            </div>
                            <div class="card-footer bg-white border-top-0">
                                <button class="btn btn-primary w-100" @click="openModal(item)">Add to Order</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Menu Item Modal -->
        <MenuItemModal :menu-item="selectedMenuItem"
                       :is-visible="isModalVisible"
                       @close="closeModal"
                       @add-to-order="addToOrder" />
    </div>
</template>

<style scoped>
    .menu-container {
        padding: 2rem 0;
    }

    .menu-header {
        border-bottom: 1px solid #dee2e6;
        padding-bottom: 1rem;
    }

    .menu-item {
        transition: transform 0.3s, box-shadow 0.3s;
    }

        .menu-item:hover {
            transform: translateY(-5px);
            box-shadow: 0 5px 15px rgba(0, 0, 0, 0.1);
        }

    .price-badge {
        font-size: 1rem;
        padding: 0.4rem 0.6rem;
    }
</style>