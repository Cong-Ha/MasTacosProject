<script setup lang="ts">
    import { ref, computed, onMounted, watch } from 'vue';
    import type { MenuItem, CartItem } from '@/types';
    import MenuItemModal from './MenuItemModal.vue';
    import ShoppingCart from './ShoppingCart.vue';
    import { useMenuItemsStore } from '@/store/menuItemsStore';
    import {
        MDBIcon,
    } from "mdb-vue-ui-kit";

    // Initialize the store
    const menuItemsStore = useMenuItemsStore();

    // Local component state
    const selectedCategory = ref<string | null>(null);
    const selectedMenuItem = ref<MenuItem | null>(null);
    const isModalVisible = ref(false);
    const cart = ref<CartItem[]>([]);
    const currentPage = ref(1);
    const itemsPerPage = ref(8); // Show 16 items per page (4x4 grid)

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

    // Paginate the filtered items
    const paginatedMenuItems = computed(() => {
        const startIndex = (currentPage.value - 1) * itemsPerPage.value;
        const endIndex = startIndex + itemsPerPage.value;
        return filteredMenuItems.value.slice(startIndex, endIndex);
    });

    // Calculate total pages
    const totalPages = computed(() => 
        Math.ceil(filteredMenuItems.value.length / itemsPerPage.value)
    );

    // Function to determine if an item has an image
    const hasImage = (item: MenuItem): boolean => {
        // Check if imageData is not null
        return item.imageData !== null;
    };

    // Function to create image data URL with appropriate MIME type
    const getImageUrl = (item: MenuItem): string => {
        // If no image data, return empty string (should not happen due to v-if)
        if (!item.imageData) return '';

        // If the item has a contentType property, use it
        if (item.contentType) {
            return `data:${item.contentType};base64,${item.imageData}`;
        }

        // If there's no contentType available, use a generic image MIME type
        // Most browsers can detect the correct format from the data
        return `data:image/*;base64,${item.imageData}`;
    };

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

    // Page navigation methods
    const goToPage = (page: number) => {
        currentPage.value = page;
        // Scroll to top of menu items
        const menuElement = document.querySelector('.menu-items-grid');
        if (menuElement) menuElement.scrollIntoView({ behavior: 'smooth' });
    };

    const previousPage = () => {
        if (currentPage.value > 1) {
            goToPage(currentPage.value - 1);
        }
    };

    const nextPage = () => {
        if (currentPage.value < totalPages.value) {
            goToPage(currentPage.value + 1);
        }
    };

    // Reset to first page when category changes
    watch(selectedCategory, () => {
        currentPage.value = 1;
    });

    // Fetch items on component mount
    onMounted(() => {
        fetchMenuItems();
    });

    // Add state for sidebar
    const isSidebarOpen = ref(false);

    // Method to toggle sidebar
    const toggleSidebar = () => {
        isSidebarOpen.value = !isSidebarOpen.value;
    };

    // Close sidebar when category is selected (especially useful on mobile)
    const selectCategory = (category: string | null) => {
        selectedCategory.value = category;
        if (window.innerWidth < 768) { // Close automatically on mobile
            isSidebarOpen.value = false;
        }
    };
</script>

<template>
    <div class="menu-container ms-3 me-3">
        <!-- Header with shopping cart -->
        <div class="mb-4 d-flex justify-content-between align-items-center border">
            <div class="d-flex align-items-center gap-3">
                <button class="sidebar-toggle ms-4" @click="toggleSidebar">
                    <MDBIcon :icon="isSidebarOpen ? 'times' : 'bars'" />
                </button>
            </div>
            <h1 class="mb-0">Menu</h1>
            <ShoppingCart :cart-items="cart" @remove-item="removeCartItem" />
        </div>

        <!-- Categories Sidebar -->
        <div class="categories-sidebar" :class="{ 'open': isSidebarOpen }">
            <div class="sidebar-header">
                <h5 class="mb-0">Categories</h5>
                <button class="close-button" @click="toggleSidebar">
                    <MDBIcon icon="times" />
                </button>
            </div>
            <div class="categories-list">
                <button class="category-item"
                        :class="{ active: selectedCategory === null }"
                        @click="selectCategory(null)">
                    All Items
                </button>
                <button v-for="category in categories"
                        :key="category"
                        class="category-item"
                        :class="{ active: selectedCategory === category }"
                        @click="selectCategory(category)">
                    {{ category }}
                </button>
            </div>
        </div>

        <!-- Overlay for mobile -->
        <div class="sidebar-overlay" 
             :class="{ 'active': isSidebarOpen }" 
             @click="toggleSidebar"></div>

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
            <div class="col-md-12">
                <div class="row menu-items-grid g-4 border">
                    <!-- No items in selected category message -->
                    <div v-if="filteredMenuItems.length === 0" class="col-12">
                        <div class="alert alert-info">
                            <p class="mb-0">No menu items available in this category.</p>
                        </div>
                    </div>

                    <div v-for="item in paginatedMenuItems"
                         :key="item.itemId"
                         class="col-sm-12 col-md-6 col-lg-3">
                        <div class="card h-100 menu-item">
                            <!-- Image Section -->
                            <div class="card-img-container mt-3">
                                <!-- Show image data directly if available -->
                                <img v-if="hasImage(item)"
                                     :src="getImageUrl(item)"
                                     class="card-img-top menu-item-image"
                                     alt="Food item"
                                     loading="lazy" />

                                <!-- Show placeholder if no imageData -->
                                <div v-else class="menu-item-placeholder">
                                    <i class="fas fa-utensils"></i>
                                </div>
                            </div>

                            <div class="card-body border">
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
                            <div class="card-footer bg-transparent border">
                                <button class="btn btn-primary w-100" @click="openModal(item)">Add to Order</button>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Pagination controls -->
                <nav v-if="totalPages > 1" aria-label="Menu pagination" class="mt-4">
                    <ul class="pagination justify-content-center">
                        <li class="page-item" :class="{ disabled: currentPage === 1 }">
                            <button class="page-link" @click="previousPage" :disabled="currentPage === 1">
                                <span aria-hidden="true">&laquo;</span>
                                <span class="visually-hidden">Previous</span>
                            </button>
                        </li>
                        
                        <li v-for="page in totalPages" 
                            :key="page" 
                            class="page-item"
                            :class="{ active: page === currentPage }">
                            <button class="page-link" @click="goToPage(page)">{{ page }}</button>
                        </li>
                        
                        <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                            <button class="page-link" @click="nextPage" :disabled="currentPage === totalPages">
                                <span aria-hidden="true">&raquo;</span>
                                <span class="visually-hidden">Next</span>
                            </button>
                        </li>
                    </ul>
                </nav>
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
        padding: 4rem 0 2rem 0;
        color: #fff;
    }

    .menu-header {
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        padding-bottom: 1rem;
    }

    .menu-item {
        transition: transform 0.3s, box-shadow 0.3s;
        background-color: rgba(0, 0, 0, 0.7) !important;
        backdrop-filter: blur(10px);
        border: 1px solid rgba(255, 255, 255, 0.1) !important;
    }

    .menu-item:hover {
        transform: translateY(-5px);
        box-shadow: 0 5px 15px rgba(0, 0, 0, 0.3);
        border-color: rgba(255, 255, 255, 0.3) !important;
    }

    .price-badge {
        font-size: 1rem;
        padding: 0.4rem 0.6rem;
    }

    /* Image styles */
    .card-img-container {
        height: 180px;
        overflow: hidden;
        background-color: rgba(0, 0, 0, 0.5);
        display: flex;
        justify-content: center;
        align-items: center;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
    }

    .menu-item-image {
        width: 100%;
        height: 100%;
        object-fit: cover;
        transition: transform 0.3s ease;
    }

    .menu-item:hover .menu-item-image {
        transform: scale(1.05);
    }

    .menu-item-placeholder {
        width: 100%;
        height: 100%;
        display: flex;
        justify-content: center;
        align-items: center;
        color: rgba(255, 255, 255, 0.3);
    }

    .menu-item-placeholder i {
        font-size: 3rem;
    }

    /* Category sidebar styles */
    .list-group-item {
        background-color: rgba(0, 0, 0, 0.7);
        border: 1px solid rgba(255, 255, 255, 0.1);
        color: #fff;
        transition: all 0.3s ease;
    }

    .list-group-item:hover {
        background-color: rgba(255, 255, 255, 0.1);
    }

    .list-group-item.active {
        background-color: rgba(59, 113, 202, 0.8) !important;
        border-color: rgba(255, 255, 255, 0.2) !important;
    }

    /* Card body and footer styles */
    .card-body {
        background-color: transparent;
        border-color: rgba(255, 255, 255, 0.1);
    }

    .card-footer {
        background-color: transparent !important;
        border-top: 1px solid rgba(255, 255, 255, 0.1) !important;
        padding: 1rem;
    }

    /* Badge styles */
    .badge.bg-light {
        background-color: rgba(255, 255, 255, 0.1) !important;
        color: #fff !important;
    }

    .badge.bg-danger {
        background-color: rgba(220, 53, 69, 0.8) !important;
    }

    /* Alert styles */
    .alert {
        background-color: rgba(0, 0, 0, 0.7);
        border: 1px solid rgba(255, 255, 255, 0.1);
        color: #fff;
        backdrop-filter: blur(10px);
    }

    .alert-info {
        background-color: rgba(13, 202, 240, 0.2);
        border-color: rgba(13, 202, 240, 0.3);
    }

    .alert-danger {
        background-color: rgba(220, 53, 69, 0.2);
        border-color: rgba(220, 53, 69, 0.3);
    }

    .pagination {
        margin-bottom: 2rem;
    }

    .page-link {
        color: #3b71ca;
        background-color: rgba(255, 255, 255, 0.1);
        border-color: rgba(255, 255, 255, 0.2);
    }

    .page-link:hover {
        /* color: #fff; */
        /* background-color: #3b71ca; */
        border-color: white;
    }

    .page-item.active .page-link {
        background-color: white;
        border-color: #3b71ca;
    }

    .page-item.disabled .page-link {
        color: #6c757d;
        background-color: rgba(255, 255, 255, 0.05);
        border-color: rgba(255, 255, 255, 0.1);
    }

    /* Add dropdown styles */
    :deep(.dropdown-menu) {
        background-color: rgba(0, 0, 0, 0.9);
        border: 1px solid rgba(255, 255, 255, 0.1);
        backdrop-filter: blur(10px);
    }

    :deep(.dropdown-item) {
        color: #fff;
    }

    :deep(.dropdown-item:hover) {
        background-color: rgba(255, 255, 255, 0.1);
        color: #fff;
    }

    :deep(.dropdown-item.active) {
        background-color: rgba(59, 113, 202, 0.8);
        color: #fff;
    }

    :deep(.btn-light) {
        background-color: rgba(0, 0, 0, 0.7);
        border: 1px solid rgba(255, 255, 255, 0.1);
        color: #fff;
    }

    :deep(.btn-light:hover) {
        background-color: rgba(0, 0, 0, 0.8);
        border-color: rgba(255, 255, 255, 0.2);
        color: #fff;
    }

    :deep(.btn-light:focus) {
        box-shadow: 0 0 0 0.25rem rgba(255, 255, 255, 0.1);
    }

    /* Add new styles for the sidebar */
    .sidebar-toggle {
        background: rgba(0, 0, 0, 0.7);
        border: 1px solid rgba(255, 255, 255, 0.1);
        color: white;
        padding: 10px 15px;
        border-radius: 5px;
        cursor: pointer;
        backdrop-filter: blur(10px);
        transition: all 0.3s ease;
        font-size: 1.5rem;
    }

    .sidebar-toggle:hover {
        background: rgba(0, 0, 0, 0.8);
        border-color: rgba(255, 255, 255, 0.2);
    }

    .categories-sidebar {
        position: fixed;
        left: -300px;
        top: 0;
        bottom: 0;
        width: 300px;
        background: rgba(0, 0, 0, 0.95);
        backdrop-filter: blur(10px);
        z-index: 1050;
        transition: transform 0.3s ease;
        padding: 20px 0;
        border-right: 1px solid rgba(255, 255, 255, 0.1);
        display: flex;
        flex-direction: column;
    }

    .categories-sidebar.open {
        transform: translateX(300px);
    }

    .sidebar-header {
        padding: 0 20px 20px;
        border-bottom: 1px solid rgba(255, 255, 255, 0.1);
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .close-button {
        background: transparent;
        border: none;
        color: white;
        cursor: pointer;
        padding: 5px;
    }

    .categories-list {
        overflow-y: auto;
        flex-grow: 1;
        padding: 20px;
    }

    .category-item {
        display: block;
        width: 100%;
        padding: 12px 20px;
        margin-bottom: 8px;
        background: rgba(255, 255, 255, 0.1);
        border: 1px solid rgba(255, 255, 255, 0.1);
        color: white;
        text-align: left;
        border-radius: 5px;
        transition: all 0.3s ease;
        cursor: pointer;
    }

    .category-item:hover {
        background: rgba(255, 255, 255, 0.2);
        transform: translateX(5px);
    }

    .category-item.active {
        background: rgba(59, 113, 202, 0.8);
        border-color: rgba(59, 113, 202, 0.9);
    }

    .sidebar-overlay {
        position: fixed;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        background: rgba(0, 0, 0, 0.5);
        backdrop-filter: blur(3px);
        z-index: 1045;
        opacity: 0;
        visibility: hidden;
        transition: all 0.3s ease;
    }

    .sidebar-overlay.active {
        opacity: 1;
        visibility: visible;
    }

    /* Mobile Responsive Adjustments */
    @media (max-width: 767px) {
        .menu-container {
            padding: 4rem 1rem 2rem 1rem;
        }

        .sidebar-toggle {
            padding: 8px 12px;
        }
    }

    .menu-items-grid {
        margin: 0;
        padding: 1rem;
    }

    .card {
        margin: 0;
    }
</style>