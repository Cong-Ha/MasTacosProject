<script setup lang="ts">
    import { ref, computed } from 'vue';
    import type { MenuItem } from '@/types';

    export interface CartItem {
        menuItem: MenuItem;
        quantity: number;
        specialInstructions: string;
    }

    // Props
    interface Props {
        cartItems: CartItem[];
    }

    const props = defineProps<Props>();

    // Emits
    const emit = defineEmits<{
        (e: 'remove-item', index: number): void
    }>();

    // State
    const isCartOpen = ref(false);

    // Computed properties
    const subtotal = computed(() => {
        return props.cartItems.reduce((total, item) => {
            return total + (item.menuItem.price * item.quantity);
        }, 0);
    });

    const totalItems = computed(() => {
        return props.cartItems.reduce((count, item) => {
            return count + item.quantity;
        }, 0);
    });

    // Methods
    const removeItem = (index: number) => {
        emit('remove-item', index);
    };
</script>

<template>
    <div class="shopping-cart">
        <button class="cart-button"
                @click="isCartOpen = !isCartOpen"
                :class="{ 'has-items': cartItems.length > 0 }">
            <i class="bi bi-cart"></i>
            <span v-if="cartItems.length > 0" class="cart-count">{{ totalItems }}</span>
        </button>

        <div class="cart-dropdown" :class="{ show: isCartOpen }">
            <div class="cart-header">
                <h5 class="mb-0">Your Order</h5>
                <button class="btn-close" @click="isCartOpen = false"></button>
            </div>

            <div v-if="cartItems.length === 0" class="empty-cart">
                <i class="bi bi-cart text-muted"></i>
                <p>Your cart is empty</p>
            </div>

            <div v-else class="cart-items">
                <div v-for="(item, index) in cartItems"
                     :key="index"
                     class="cart-item">
                    <div class="item-details">
                        <h6 class="item-name">{{ item.menuItem.name }}</h6>
                        <div class="item-meta">
                            <span class="item-quantity">{{ item.quantity }}x</span>
                            <span class="item-price">${{ (item.menuItem.price * item.quantity).toFixed(2) }}</span>
                        </div>
                        <small v-if="item.specialInstructions" class="item-instructions">
                            {{ item.specialInstructions }}
                        </small>
                    </div>
                    <button class="btn btn-sm text-danger"
                            title="Remove item"
                            @click="removeItem(index)">
                        <i class="bi bi-trash"></i>
                    </button>
                </div>
            </div>

            <div v-if="cartItems.length > 0" class="cart-footer">
                <div class="d-flex justify-content-between mb-3">
                    <span>Subtotal:</span>
                    <span class="fw-bold">${{ subtotal.toFixed(2) }}</span>
                </div>
                <button class="btn btn-primary w-100">Checkout</button>
            </div>
        </div>

        <!-- Backdrop when cart is open -->
        <div v-if="isCartOpen"
             class="cart-backdrop"
             @click="isCartOpen = false"></div>
    </div>
</template>

<style scoped>
    .shopping-cart {
        position: relative;
    }

    .cart-button {
        width: 48px;
        height: 48px;
        border-radius: 50%;
        background-color: #fff;
        border: 1px solid #dee2e6;
        color: #495057;
        display: flex;
        align-items: center;
        justify-content: center;
        font-size: 1.25rem;
        position: relative;
        transition: all 0.3s;
        cursor: pointer;
    }

        .cart-button:hover {
            background-color: #f8f9fa;
        }

        .cart-button.has-items {
            background-color: #007bff;
            color: white;
            border-color: #007bff;
        }

    .cart-count {
        position: absolute;
        top: -5px;
        right: -5px;
        background-color: #dc3545;
        color: white;
        border-radius: 50%;
        width: 20px;
        height: 20px;
        font-size: 12px;
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .cart-dropdown {
        position: absolute;
        top: 100%;
        right: 0;
        margin-top: 10px;
        width: 320px;
        background-color: white;
        border-radius: 8px;
        box-shadow: 0 5px 15px rgba(0,0,0,0.1);
        z-index: 1000;
        opacity: 0;
        visibility: hidden;
        transform: translateY(10px);
        transition: all 0.3s;
    }

        .cart-dropdown.show {
            opacity: 1;
            visibility: visible;
            transform: translateY(0);
        }

    .cart-header {
        padding: 1rem;
        border-bottom: 1px solid #e9ecef;
        display: flex;
        justify-content: space-between;
        align-items: center;
    }

    .cart-items {
        max-height: 300px;
        overflow-y: auto;
        padding: 1rem;
    }

    .cart-item {
        padding: 0.75rem 0;
        border-bottom: 1px solid #e9ecef;
        display: flex;
        justify-content: space-between;
        align-items: flex-start;
    }

        .cart-item:last-child {
            border-bottom: none;
        }

    .item-details {
        flex: 1;
    }

    .item-name {
        margin-bottom: 0.25rem;
    }

    .item-meta {
        display: flex;
        justify-content: space-between;
        color: #6c757d;
        font-size: 0.9rem;
    }

    .item-instructions {
        display: block;
        color: #6c757d;
        font-style: italic;
        margin-top: 0.25rem;
    }

    .empty-cart {
        padding: 2rem;
        text-align: center;
        color: #6c757d;
    }

        .empty-cart i {
            font-size: 2rem;
            margin-bottom: 0.5rem;
        }

    .cart-footer {
        padding: 1rem;
        border-top: 1px solid #e9ecef;
    }

    .cart-backdrop {
        position: fixed;
        top: 0;
        right: 0;
        bottom: 0;
        left: 0;
        z-index: 990;
    }
</style>