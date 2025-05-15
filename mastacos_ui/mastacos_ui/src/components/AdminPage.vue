<template>
    <div class="container my-5">
        <div class="row mb-4">
            <div class="col">
                <h2 class="text-primary">Menu Item Management</h2>
            </div>
            <div class="col text-end">
                <button type="button" class="btn btn-success" @click="openAddItemModal">
                    <i class="fas fa-plus me-2"></i>Add New Item
                </button>
            </div>
        </div>

        <!-- Loading state -->
        <div v-if="isLoading" class="text-center my-5">
            <div class="spinner-border text-primary" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <p class="mt-2">Loading menu items...</p>
        </div>

        <!-- Error state -->
        <div v-if="storeError" class="alert alert-danger mb-4">
            <i class="fas fa-exclamation-triangle me-2"></i>
            {{ storeError }}
            <button type="button" class="btn btn-outline-danger btn-sm ms-3" @click="fetchMenuItems">
                Retry
            </button>
        </div>

        <!-- Search and filters -->
        <div v-if="!isLoading && !storeError" class="row mb-4">
            <div class="col-md-6">
                <div class="input-group">
                    <span class="input-group-text"><i class="fas fa-search"></i></span>
                    <input type="text"
                           class="form-control"
                           placeholder="Search menu items..."
                           v-model="searchTerm" />
                </div>
            </div>
            <div class="col-md-3">
                <select class="form-select" v-model="categoryFilter">
                    <option value="">All Categories</option>
                    <option v-for="category in allCategories" :key="category" :value="category">
                        {{ category }}
                    </option>
                </select>
            </div>
            <div class="col-md-3">
                <select class="form-select" v-model="statusFilter">
                    <option value="">All Status</option>
                    <option value="true">Active</option>
                    <option value="false">Inactive</option>
                </select>
            </div>
        </div>

        <!-- Table -->
        <div v-if="!isLoading && !storeError && storeItems.length > 0" class="table-responsive shadow-4 rounded-5">
            <table class="table table-hover align-middle mb-0">
                <thead class="table-light">
                    <tr>
                        <th scope="col">ID</th>
                        <th scope="col">Image</th>
                        <th scope="col">
                            <div class="d-flex align-items-center">
                                Name
                                <i class="fas fa-sort ms-1" @click="sortBy('name')"></i>
                            </div>
                        </th>
                        <th scope="col">Description</th>
                        <th scope="col">
                            <div class="d-flex align-items-center">
                                Price
                                <i class="fas fa-sort ms-1" @click="sortBy('price')"></i>
                            </div>
                        </th>
                        <th scope="col">Category</th>
                        <th scope="col">Status</th>
                        <th scope="col">
                            <div class="d-flex align-items-center">
                                Popularity
                                <i class="fas fa-sort ms-1" @click="sortBy('popularityScore')"></i>
                            </div>
                        </th>
                        <th scope="col">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="item in filteredItems" :key="item.itemId">
                        <td>{{ item.itemId }}</td>
                        <td>
                            <div class="item-image-preview">
                                <!-- Using @error to show placeholder if image fails to load -->
                                <img :src="`/api/MenuItems/${item.itemId}/image`"
                                     alt="Menu item thumbnail"
                                     class="img-thumbnail"
                                     style="width: 50px; height: 50px; object-fit: cover;"
                                     @error="displayPlaceholder" />
                            </div>
                        </td>
                        <td>{{ item.name }}</td>
                        <td>
                            <span class="text-truncate d-inline-block" style="max-width: 200px;">
                                {{ item.description }}
                            </span>
                        </td>
                        <td>${{ item.price.toFixed(2) }}</td>
                        <td>
                            <span class="badge rounded-pill bg-info">{{ item.category }}</span>
                        </td>
                        <td>
                            <span class="badge rounded-pill"
                                  :class="item.isActive ? 'bg-success' : 'bg-danger'">
                                {{ item.isActive ? 'Active' : 'Inactive' }}
                            </span>
                        </td>
                        <td>
                            <div class="d-flex align-items-center">
                                <span class="me-2">{{ item.popularityScore }}</span>
                                <div class="progress w-100" style="height: 6px;">
                                    <div class="progress-bar"
                                         role="progressbar"
                                         :style="`width: ${item.popularityScore}%`"
                                         :aria-valuenow="item.popularityScore"
                                         aria-valuemin="0"
                                         aria-valuemax="100"></div>
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="btn-group" role="group">
                                <button type="button"
                                        class="btn btn-sm btn-outline-primary"
                                        @click="editItem(item)">
                                    <i class="fas fa-edit"></i>
                                </button>
                                <button type="button"
                                        class="btn btn-sm btn-outline-danger"
                                        @click="confirmDelete(item)">
                                    <i class="fas fa-trash"></i>
                                </button>
                                <button type="button"
                                        class="btn btn-sm"
                                        :class="item.isActive ? 'btn-outline-warning' : 'btn-outline-success'"
                                        @click="toggleStatus(item)">
                                    <i :class="item.isActive ? 'fas fa-eye-slash' : 'fas fa-eye'"></i>
                                </button>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>

        <!-- Empty state -->
        <div v-if="!isLoading && !storeError && storeItems.length === 0" class="card text-center my-5">
            <div class="card-body">
                <i class="fas fa-utensils fa-3x text-muted mb-3"></i>
                <h5>No Menu Items Found</h5>
                <p>There are no menu items available. Add your first menu item to get started.</p>
                <button type="button" class="btn btn-primary" @click="openAddItemModal">
                    Add Menu Item
                </button>
            </div>
        </div>

        <!-- Pagination -->
        <div v-if="!isLoading && !storeError && totalPages > 1" class="d-flex justify-content-center mt-4">
            <nav aria-label="Page navigation">
                <ul class="pagination">
                    <li class="page-item" :class="{ disabled: currentPage === 1 }">
                        <a class="page-link" href="#" @click.prevent="currentPage--">Previous</a>
                    </li>
                    <li v-for="page in totalPages"
                        :key="page"
                        class="page-item"
                        :class="{ active: currentPage === page }">
                        <a class="page-link" href="#" @click.prevent="currentPage = page">{{ page }}</a>
                    </li>
                    <li class="page-item" :class="{ disabled: currentPage === totalPages }">
                        <a class="page-link" href="#" @click.prevent="currentPage++">Next</a>
                    </li>
                </ul>
            </nav>
        </div>

        <!-- Add/Edit Item Modal -->
        <div class="modal fade"
             :class="{ 'show': showItemModal }"
             :style="{ display: showItemModal ? 'block' : 'none' }"
             tabindex="-1"
             aria-labelledby="itemModalLabel">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="itemModalLabel">
                            {{ editingItem.itemId ? 'Edit Menu Item' : 'Add New Menu Item' }}
                        </h5>
                        <button type="button"
                                class="btn-close"
                                @click="showItemModal = false"
                                aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div v-if="formLoading" class="text-center my-4">
                            <div class="spinner-border text-primary" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>

                        <div v-if="formError" class="alert alert-danger">
                            {{ formError }}
                        </div>

                        <form v-if="!formLoading" @submit.prevent="validateAndSave" ref="itemForm">
                            <div class="mb-3">
                                <label for="itemName" class="form-label">Name <span class="text-danger">*</span></label>
                                <input type="text"
                                       class="form-control"
                                       :class="{ 'is-invalid': v$.name.$error }"
                                       id="itemName"
                                       v-model="editingItem.name"
                                       @blur="v$.name.$touch()"
                                       required />
                                <div class="error-container">
                                    <div v-if="v$.name.$error" class="text-danger small mt-1">
                                        <span v-if="v$.name.required.$invalid">Name is required.</span>
                                        <span v-if="v$.name.minLength.$invalid">Name must be at least 3 characters.</span>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="itemDescription" class="form-label">Description</label>
                                <textarea class="form-control"
                                          :class="{ 'is-invalid': v$.description.$error }"
                                          id="itemDescription"
                                          rows="3"
                                          v-model="editingItem.description"
                                          @blur="v$.description.$touch()"></textarea>
                                <div class="error-container">
                                    <div v-if="v$.description.$error" class="text-danger small mt-1">
                                        <span v-if="v$.description.maxLength.$invalid">Description must be no more than 500 characters.</span>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="itemPrice" class="form-label">Price <span class="text-danger">*</span></label>
                                <div class="input-group">
                                    <span class="input-group-text">$</span>
                                    <input type="number"
                                           class="form-control"
                                           :class="{ 'is-invalid': v$.price.$error }"
                                           id="itemPrice"
                                           v-model.number="editingItem.price"
                                           @blur="v$.price.$touch()"
                                           step="0.01"
                                           min="0"
                                           required />
                                </div>
                                <div class="error-container">
                                    <div v-if="v$.price.$error" class="text-danger small mt-1">
                                        <span v-if="v$.price.required.$invalid">Price is required.</span>
                                        <span v-if="v$.price.minValue.$invalid">Price must be greater than or equal to 0.</span>
                                    </div>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label for="itemCategory" class="form-label">Category <span class="text-danger">*</span></label>
                                <input type="text"
                                       class="form-control"
                                       :class="{ 'is-invalid': v$.category.$error }"
                                       id="itemCategory"
                                       v-model="editingItem.category"
                                       @blur="v$.category.$touch()"
                                       list="categories"
                                       required />
                                <datalist id="categories">
                                    <option v-for="category in allCategories" :key="category" :value="category" />
                                </datalist>
                                <div class="error-container">
                                    <div v-if="v$.category.$error" class="text-danger small mt-1">
                                        <span v-if="v$.category.required.$invalid">Category is required.</span>
                                        <span v-if="v$.category.minLength.$invalid">Category must be at least 3 characters.</span>
                                    </div>
                                </div>
                            </div>
                            <!-- Image upload field -->
                            <div class="mb-3">
                                <label for="itemImage" class="form-label">Item Image</label>

                                <div class="image-upload-container">
                                    <!-- Image preview -->
                                    <div v-if="imagePreview" class="image-preview-container mb-2">
                                        <!-- Show selected image preview -->
                                        <img :src="imagePreview"
                                             alt="Selected image preview"
                                             class="img-thumbnail"
                                             style="max-height: 200px; max-width: 100%;" />
                                        <button type="button"
                                                class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1"
                                                @click="removeImage">
                                            <i class="fas fa-times"></i>
                                        </button>
                                    </div>

                                    <!-- Show existing image if editing and no new image selected -->
                                    <div v-else-if="editingItem.itemId && !imageRemoved" class="image-preview-container mb-2">
                                        <img :src="`/api/MenuItems/${editingItem.itemId}/image`"
                                             alt="Item preview"
                                             class="img-thumbnail"
                                             style="max-height: 200px; max-width: 100%;"
                                             @error="handleModalImageError" />
                                        <button type="button"
                                                class="btn btn-sm btn-danger position-absolute top-0 end-0 m-1"
                                                @click="removeImage">
                                            <i class="fas fa-times"></i>
                                        </button>
                                    </div>

                                    <!-- Always show file input or change button -->
                                    <div class="mt-2">
                                        <!-- File input -->
                                        <div v-if="showFileInput" class="mb-2">
                                            <div class="input-group">
                                                <input type="file"
                                                       class="form-control"
                                                       id="itemImage"
                                                       ref="fileInput"
                                                       @change="handleImageChange"
                                                       accept="image/jpeg,image/png,image/gif" />
                                                <button type="button"
                                                        class="btn btn-outline-secondary"
                                                        @click="showFileInput = false"
                                                        title="Cancel">
                                                    <i class="fas fa-times"></i>
                                                </button>
                                            </div>
                                            <div class="form-text text-muted">
                                                Maximum file size: 2MB. Supported formats: JPG, PNG, GIF.
                                            </div>
                                        </div>

                                        <!-- Toggle button - shows/hides file input -->
                                        <div v-if="!showFileInput">
                                            <button type="button"
                                                    class="btn btn-sm btn-outline-primary"
                                                    @click="showFileInput = true">
                                                <i class="fas fa-upload me-1"></i>
                                                {{ imagePreview ? 'Change Image' : 'Upload Image' }}
                                            </button>
                                        </div>
                                    </div>

                                    <!-- Error message area -->
                                    <div class="error-container mt-2">
                                        <div v-if="imageError" class="text-danger small">
                                            {{ imageError }}
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <!-- Status toggle -->
                            <div class="form-check form-switch mb-3">
                                <input class="form-check-input"
                                       type="checkbox"
                                       id="itemStatus"
                                       v-model="editingItem.isActive" />
                                <label class="form-check-label" for="itemStatus">
                                    {{ editingItem.isActive ? 'Active' : 'Inactive' }}
                                </label>
                            </div>

                            <!-- Popularity score -->
                            <div class="mb-3">
                                <label for="itemPopularity" class="form-label">
                                    Popularity Score: {{ editingItem.popularityScore }}
                                </label>
                                <input type="range"
                                       class="form-range"
                                       min="0"
                                       max="100"
                                       id="itemPopularity"
                                       v-model.number="editingItem.popularityScore" />
                            </div>
                        </form>
                    </div>
                    <div class="modal-footer">
                        <button type="button"
                                class="btn btn-secondary"
                                @click="showItemModal = false">
                            Cancel
                        </button>
                        <button type="button"
                                class="btn btn-primary"
                                @click="validateAndSave"
                                :disabled="formLoading || v$.$invalid">
                            Save
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Backdrop for item modal -->
        <div class="modal-backdrop fade"
             :class="{ 'show': showItemModal }"
             v-if="showItemModal"></div>

        <!-- Delete Confirmation Modal -->
        <div class="modal fade"
             :class="{ 'show': showDeleteModal }"
             :style="{ display: showDeleteModal ? 'block' : 'none' }"
             tabindex="-1"
             aria-labelledby="deleteModalLabel">
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="deleteModalLabel">Confirm Delete</h5>
                        <button type="button"
                                class="btn-close"
                                @click="showDeleteModal = false"
                                aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                        <div v-if="formLoading" class="text-center my-4">
                            <div class="spinner-border text-primary" role="status">
                                <span class="visually-hidden">Loading...</span>
                            </div>
                        </div>

                        <div v-if="formError" class="alert alert-danger">
                            {{ formError }}
                        </div>

                        <p v-if="!formLoading">
                            Are you sure you want to delete <strong>{{ itemToDelete?.name }}</strong>?
                            This action cannot be undone.
                        </p>
                    </div>
                    <div class="modal-footer">
                        <button type="button"
                                class="btn btn-secondary"
                                @click="showDeleteModal = false">
                            Cancel
                        </button>
                        <button type="button"
                                class="btn btn-danger"
                                @click="deleteItem"
                                :disabled="formLoading">
                            Delete
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <!-- Backdrop for delete modal -->
        <div class="modal-backdrop fade"
             :class="{ 'show': showDeleteModal }"
             v-if="showDeleteModal"></div>
    </div>
</template>
<script setup lang="ts">
    import { ref, computed, onMounted, watch } from 'vue';
    import type { MenuItem, NewMenuItem } from '@/types';
    import { useMenuItemsStore } from '@/store/menuItemsStore';
    import { useVuelidate } from '@vuelidate/core';
    import { required, minLength, maxLength, minValue } from '@vuelidate/validators';
    import { menuService } from '@/services/menuService';

    // Initialize store
    const menuItemsStore = useMenuItemsStore();

    // Form state (local to component)
    const formLoading = ref(false);
    const formError = ref<string | null>(null);

    // Pagination
    const itemsPerPage = ref(10);
    const currentPage = ref(1);

    // Filtering and searching
    const searchTerm = ref('');
    const categoryFilter = ref('');
    const statusFilter = ref('');
    const sortColumn = ref('');
    const sortDirection = ref('asc');

    // Image handling refs
    const imageFile = ref<File | null>(null);
    const imagePreview = ref<string | null>(null);
    const imageError = ref<string | null>(null);
    const showFileInput = ref(false);
    const fileInput = ref<HTMLInputElement | null>(null);

    // Flag to track if user has explicitly removed the image
    const imageRemoved = ref(false);

    // Modal state
    const showItemModal = ref(false);
    const showDeleteModal = ref(false);
    const editingItem = ref<MenuItem>({
        itemId: 0,
        name: '',
        description: '',
        price: 0,
        category: '',
        isActive: true,
        popularityScore: 50
    });
    const itemToDelete = ref<MenuItem | null>(null);

    // Vuelidate validation rules
    const rules = computed(() => {
        return {
            name: {
                required,
                minLength: minLength(3)
            },
            description: {
                maxLength: maxLength(500)
            },
            price: {
                required,
                minValue: minValue(0)
            },
            category: {
                required,
                minLength: minLength(3)
            }
        };
    });

    // Initialize Vuelidate
    const v$ = useVuelidate(rules, editingItem);

    // Get store state via computed properties
    const storeItems = computed(() => menuItemsStore.items);
    const isLoading = computed(() => menuItemsStore.loading);
    const storeError = computed(() => menuItemsStore.error);
    const allCategories = computed(() => menuItemsStore.allCategories);

    // Watch modal state changes to manage body class for scrolling
    watch(showItemModal, (isOpen) => {
        if (isOpen) {
            document.body.classList.add('modal-open');
        } else {
            // Only remove if the other modal isn't open
            if (!showDeleteModal.value) {
                document.body.classList.remove('modal-open');
            }
            // Reset validation state when modal closes
            v$.value.$reset();
        }
    });

    watch(showDeleteModal, (isOpen) => {
        if (isOpen) {
            document.body.classList.add('modal-open');
        } else {
            // Only remove if the other modal isn't open
            if (!showItemModal.value) {
                document.body.classList.remove('modal-open');
            }
        }
    });

    // Fetch all menu items using store
    const fetchMenuItems = async (): Promise<void> => {
        await menuItemsStore.fetchMenuItems(true); // Force refresh
    };

    // Function to display placeholder when image fails to load
    const displayPlaceholder = (event: Event): void => {
        const imgElement = event.target as HTMLImageElement;
        // Set a placeholder icon instead of broken image
        imgElement.outerHTML = `<div class="d-flex justify-content-center align-items-center" style="width: 50px; height: 50px;">
            <i class="fas fa-image text-muted"></i>
        </div>`;
    };

    // Handle modal image error
    const handleModalImageError = (event: Event): void => {
        const imgElement = event.target as HTMLImageElement;
        // Show placeholder image in modal
        imgElement.outerHTML = `<div class="d-flex justify-content-center align-items-center bg-light" style="width: 100%; height: 200px;">
            <i class="fas fa-image fa-2x text-muted"></i>
            <p class="text-muted ms-2 mb-0">No image available</p>
        </div>`;
    };

    // Computed properties
    const filteredItems = computed(() => {
        let result = [...storeItems.value];

        // Apply search filter
        if (searchTerm.value) {
            const term = searchTerm.value.toLowerCase();
            result = result.filter(item =>
                item.name.toLowerCase().includes(term) ||
                item.description.toLowerCase().includes(term) ||
                item.category.toLowerCase().includes(term)
            );
        }

        // Apply category filter
        if (categoryFilter.value) {
            result = result.filter(item => item.category === categoryFilter.value);
        }

        // Apply status filter
        if (statusFilter.value !== '') {
            const isActive = statusFilter.value === 'true';
            result = result.filter(item => item.isActive === isActive);
        }

        // Apply sorting
        if (sortColumn.value) {
            result.sort((a, b) => {
                // Special case for sorting by ID
                if (sortColumn.value === 'itemId') {
                    return sortDirection.value === 'asc'
                        ? a.itemId - b.itemId
                        : b.itemId - a.itemId;
                }

                const aValue = a[sortColumn.value as keyof MenuItem];
                const bValue = b[sortColumn.value as keyof MenuItem];

                if (typeof aValue === 'string' && typeof bValue === 'string') {
                    return sortDirection.value === 'asc'
                        ? aValue.localeCompare(bValue)
                        : bValue.localeCompare(aValue);
                } else {
                    return sortDirection.value === 'asc'
                        ? Number(aValue) - Number(bValue)
                        : Number(bValue) - Number(aValue);
                }
            });
        }

        // Apply pagination
        const startIndex = (currentPage.value - 1) * itemsPerPage.value;
        const paginatedItems = result.slice(startIndex, startIndex + itemsPerPage.value);

        return paginatedItems;
    });

    const totalPages = computed(() => {
        let filteredCount = storeItems.value.length;

        // Apply filters for accurate page count
        if (searchTerm.value || categoryFilter.value || statusFilter.value !== '') {
            filteredCount = storeItems.value.filter(item => {
                let matches = true;

                // Apply search filter
                if (searchTerm.value) {
                    const term = searchTerm.value.toLowerCase();
                    matches = matches && (
                        item.name.toLowerCase().includes(term) ||
                        item.description.toLowerCase().includes(term) ||
                        item.category.toLowerCase().includes(term)
                    );
                }

                // Apply category filter
                if (categoryFilter.value) {
                    matches = matches && (item.category === categoryFilter.value);
                }

                // Apply status filter
                if (statusFilter.value !== '') {
                    const isActive = statusFilter.value === 'true';
                    matches = matches && (item.isActive === isActive);
                }

                return matches;
            }).length;
        }

        // Make sure we don't divide by zero and always return at least 1 page
        return Math.max(1, Math.ceil(filteredCount / itemsPerPage.value));
    });

    // Methods
    const sortBy = (column: string): void => {
        if (sortColumn.value === column) {
            // Toggle direction if already sorting by this column
            sortDirection.value = sortDirection.value === 'asc' ? 'desc' : 'asc';
        } else {
            // Set new sort column and default to ascending
            sortColumn.value = column;
            sortDirection.value = 'asc';
        }
    };

    // Handle image file selection
    const handleImageChange = (event: Event): void => {
        const target = event.target as HTMLInputElement;
        if (!target.files || target.files.length === 0) {
            return;
        }

        const file = target.files[0];
        imageError.value = null;

        // Validate file type
        const allowedTypes = ['image/jpeg', 'image/png', 'image/gif'];
        if (!allowedTypes.includes(file.type)) {
            imageError.value = 'Invalid file type. Please upload JPG, PNG, or GIF.';
            resetFileInput();
            return;
        }

        // Validate file size (max 2MB)
        const maxSize = 2 * 1024 * 1024; // 2MB
        if (file.size > maxSize) {
            imageError.value = 'File is too large. Maximum size is 2MB.';
            resetFileInput();
            return;
        }

        // Store file and create preview
        imageFile.value = file;
        imageRemoved.value = false;

        // Create preview
        const reader = new FileReader();
        reader.onload = (e) => {
            imagePreview.value = e.target?.result as string || null;
            showFileInput.value = false;
        };
        reader.readAsDataURL(file);
    };

    // Remove image
    const removeImage = (): void => {
        imageFile.value = null;
        imagePreview.value = null;
        imageRemoved.value = true;
        showFileInput.value = true;
        resetFileInput();
    };

    // Reset file input
    const resetFileInput = (): void => {
        if (fileInput.value) {
            fileInput.value.value = '';
        }
        imageFile.value = null;
    };

    // Open add item modal
    const openAddItemModal = (): void => {
        // Reset the editing item
        editingItem.value = {
            itemId: 0,
            name: '',
            description: '',
            price: 0,
            category: '',
            isActive: true,
            popularityScore: 50
        };

        // Reset image state
        imageFile.value = null;
        imagePreview.value = null;
        imageError.value = null;
        imageRemoved.value = false;
        showFileInput.value = false;

        formError.value = null;
        // Reset Vuelidate state
        v$.value.$reset();
        showItemModal.value = true;
    };

    // Edit existing item
    const editItem = (item: MenuItem): void => {
        // Clone the item to avoid direct mutation
        editingItem.value = { ...item };
        console.log("Editing item:", editingItem.value);

        // Reset image state
        imageFile.value = null;
        imagePreview.value = null;
        imageError.value = null;
        imageRemoved.value = false;
        showFileInput.value = false;

        formError.value = null;
        // Reset validation when editing existing item
        v$.value.$reset();
        showItemModal.value = true;
    };

    // Method to validate before saving
    const validateAndSave = async (): Promise<void> => {
        const result = await v$.value.$validate();
        if (!result) {
            formError.value = 'Please correct the validation errors before saving.';
            return;
        }
        await saveItem();
    };

    const saveItem = async (): Promise<void> => {
        formLoading.value = true;
        formError.value = null;

        console.log("Saving item with ID:", editingItem.value.itemId);

        try {
            let createdOrUpdatedItem: MenuItem;

            if (editingItem.value.itemId === 0) {
                // Create new menu item
                const newItemData: NewMenuItem = {
                    name: editingItem.value.name,
                    description: editingItem.value.description,
                    price: editingItem.value.price,
                    category: editingItem.value.category,
                    isActive: editingItem.value.isActive,
                    popularityScore: editingItem.value.popularityScore
                };

                console.log("Creating new item:", newItemData);
                createdOrUpdatedItem = await menuItemsStore.createMenuItem(newItemData);
            } else {
                // Update existing menu item
                console.log("Updating existing item:", editingItem.value);
                createdOrUpdatedItem = await menuItemsStore.updateMenuItem(editingItem.value);
            }

            // Handle image upload if there's a new image
            if (imageFile.value) {
                try {
                    await menuService.uploadImage(createdOrUpdatedItem.itemId, imageFile.value);
                } catch (error) {
                    console.error('Error uploading image:', error);
                    // Don't fail the whole operation if image upload fails
                }
            } else if (imageRemoved.value) {
                // If image was removed, delete it
                try {
                    await menuService.deleteImage(createdOrUpdatedItem.itemId);
                } catch (error) {
                    console.error('Error deleting image:', error);
                    // Don't fail the whole operation if image deletion fails
                }
            }

            // Refresh menu items
            await fetchMenuItems();

            showItemModal.value = false; // Hide the modal
        } catch (err) {
            formError.value = 'Failed to save menu item. Please try again.';
            console.error('Error saving menu item:', err);
        } finally {
            formLoading.value = false;
        }
    };

    const confirmDelete = (item: MenuItem): void => {
        itemToDelete.value = item;
        formError.value = null;
        showDeleteModal.value = true; // Show the delete modal
    };

    const deleteItem = async (): Promise<void> => {
        if (!itemToDelete.value) return;

        formLoading.value = true;
        formError.value = null;

        try {
            // Delete using store action
            await menuItemsStore.deleteMenuItem(itemToDelete.value.itemId);
            showDeleteModal.value = false; // Hide the modal

            // Refresh the list
            await fetchMenuItems();
        } catch (err) {
            formError.value = 'Failed to delete menu item. Please try again.';
            console.error('Error deleting menu item:', err);
        } finally {
            formLoading.value = false;
        }
    };

    const toggleStatus = async (item: MenuItem): Promise<void> => {
        try {
            const newStatus = !item.isActive;
            // Use store to update status
            await menuItemsStore.updateMenuItemStatus(item.itemId, newStatus);
        } catch (err) {
            console.error('Error toggling menu item status:', err);
            formError.value = 'Failed to update item status. Please try again.';
            // Hide error after 3 seconds
            setTimeout(() => {
                formError.value = null;
            }, 3000);
        }
    };

    // Fetch items on component mount
    onMounted(() => {
        fetchMenuItems();
    });
</script>
<style scoped>
    /* Additional custom styles */
    .image-preview-container {
        position: relative;
        display: inline-block;
    }

    .table-responsive {
        overflow-x: auto;
    }

    .fas.fa-sort {
        cursor: pointer;
        opacity: 0.5;
    }

        .fas.fa-sort:hover {
            opacity: 1;
        }

    .shadow-4 {
        box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.15);
    }

    .rounded-5 {
        border-radius: 0.5rem;
    }

    /* Validation error styles */
    .error-container {
        min-height: 24px; /* Reserve space for error messages */
    }

    .is-invalid {
        border-color: #dc3545;
    }

    /* Image styles */
    .item-image-preview {
        width: 50px;
        height: 50px;
        overflow: hidden;
        border-radius: 4px;
        display: flex;
        justify-content: center;
        align-items: center;
        background-color: #f8f9fa;
    }

        .item-image-preview img {
            width: 100%;
            height: 100%;
            object-fit: cover;
        }

    /* Modal styles */
    .modal.fade.show {
        opacity: 1;
    }

    .modal-backdrop.fade.show {
        opacity: 0.5;
    }

    /* Make sure the modal backdrop is behind the modal */
    .modal-backdrop {
        z-index: 1040;
    }

    .modal {
        z-index: 1050;
    }

    /* Override body padding-right that Bootstrap adds */
    body.modal-open {
        overflow: hidden;
        padding-right: 0 !important;
    }
</style>