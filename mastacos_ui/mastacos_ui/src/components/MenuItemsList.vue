<script setup lang="ts">
    import { ref, computed, onMounted } from 'vue';
    import { menuService } from '@/services/menuService';
    import type { MenuItem } from '@/types';
    import MenuItemModal from './MenuItemModal.vue';
    import ShoppingCart from './ShoppingCart.vue';
    import type { CartItem } from './ShoppingCart.vue';

    // State
    const menuItems = ref<MenuItem[]>([]);
    const loading = ref(true);
    const error = ref<string | null>(null);
    const selectedCategory = ref<string | null>(null);
    const selectedMenuItem = ref<MenuItem | null>(null);
    const isModalVisible = ref(false);
    const cart = ref<CartItem[]>([]);

    // Computed properties
    const categories = computed(() => {
        const uniqueCategories = new Set(menuItems.value.map(item => item.category));
        return Array.from(uniqueCategories).sort();
    });

    const filteredMenuItems = computed(() => {
        if (!selectedCategory.value) return menuItems.value;
        return menuItems.value.filter(item => item.category === selectedCategory.value);
    });

    // Methods
    const fetchMenuItems = async () => {
        loading.value = true;
        error.value = null;

        try {
            const data = await menuService.getMenuItems();
            menuItems.value = data;
        } catch (err) {
            if (err instanceof Error) {
                error.value = err.message;
            } else {
                error.value = 'An unknown error occurred';
            }
            console.error('Failed to fetch menu items:', err);
        } finally {
            loading.value = false;
        }
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

    // Lifecycle hooks
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
                    <div v-for="item in filteredMenuItems"
                         :key="item.id"
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