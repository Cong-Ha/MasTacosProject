<script setup lang="ts">
    import { ref, watch } from 'vue';
    import type { MenuItem } from '@/types';

    // Props
    interface Props {
        menuItem: MenuItem | null;
        isVisible: boolean;
    }

    const props = defineProps<Props>();

    // Emits
    const emit = defineEmits<{
        (e: 'close'): void;
        (e: 'add-to-order', orderDetails: { menuItem: MenuItem; quantity: number; specialInstructions: string }): void;
    }>();

    // State
    const quantity = ref(1);
    const specialInstructions = ref('');

    // Reset form when menu item changes
    watch(() => props.menuItem, () => {
        quantity.value = 1;
        specialInstructions.value = '';
    });

    // When modal becomes visible, add body class for styling
    watch(() => props.isVisible, (newValue) => {
        if (newValue) {
            document.body.classList.add('modal-open');
        } else {
            document.body.classList.remove('modal-open');
        }
    });

    // Methods
    const incrementQuantity = () => {
        if (quantity.value < 10) {
            quantity.value++;
        }
    };

    const decrementQuantity = () => {
        if (quantity.value > 1) {
            quantity.value--;
        }
    };

    const formatPrice = (price: number) => {
        return `$${price.toFixed(2)}`;
    };

    const closeModal = () => {
        emit('close');
    };

    const addToOrder = () => {
        if (props.menuItem) {
            emit('add-to-order', {
                menuItem: props.menuItem,
                quantity: quantity.value,
                specialInstructions: specialInstructions.value
            });
        }
    };
</script>

<template>
    <div class="modal fade" :class="{ show: isVisible }" tabindex="-1" :style="{ display: isVisible ? 'block' : 'none' }">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" v-if="menuItem">
                <div class="modal-header">
                    <h5 class="modal-title">{{ menuItem.name }}</h5>
                    <button type="button" class="btn-close" @click="closeModal"></button>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-4">
                            <div class="img-placeholder bg-light d-flex align-items-center justify-content-center" style="height: 200px;">
                                <span>{{ menuItem.category }}</span>
                            </div>
                            <div class="mt-3">
                                <span class="badge bg-success me-2">{{ formatPrice(menuItem.price) }}</span>
                                <span v-if="menuItem.popularityScore >= 80" class="badge bg-danger">Popular</span>
                            </div>
                        </div>
                        <div class="col-md-8">
                            <p>{{ menuItem.description }}</p>

                            <div class="quantity-selector mb-3">
                                <label class="form-label">Quantity:</label>
                                <div class="input-group" style="width: 150px;">
                                    <button class="btn btn-outline-secondary" type="button" @click="decrementQuantity">-</button>
                                    <input type="text" class="form-control text-center" v-model="quantity" readonly>
                                    <button class="btn btn-outline-secondary" type="button" @click="incrementQuantity">+</button>
                                </div>
                            </div>

                            <div class="mb-3">
                                <label class="form-label">Special Instructions:</label>
                                <textarea class="form-control" v-model="specialInstructions" rows="2" placeholder="Any special requests?"></textarea>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="d-flex w-100 justify-content-between align-items-center">
                        <div class="price-total">
                            <span class="me-2">Total:</span>
                            <span class="fw-bold">{{ formatPrice(menuItem.price * quantity) }}</span>
                        </div>
                        <div>
                            <button type="button" class="btn btn-secondary me-2" @click="closeModal">Cancel</button>
                            <button type="button" class="btn btn-primary" @click="addToOrder">Add to Order</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="modal-backdrop fade" :class="{ show: isVisible }" v-if="isVisible"></div>
</template>

<style scoped>
    .modal.show {
        background-color: rgba(0, 0, 0, 0.5);
    }

    body.modal-open {
        overflow: hidden;
        padding-right: 17px; /* Compensate for scrollbar */
    }

    .img-placeholder {
        border-radius: 8px;
    }

    .price-total {
        font-size: 1.2rem;
    }
</style>