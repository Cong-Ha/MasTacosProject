<script setup lang="ts">
import { ref, computed } from 'vue';
import type { CartItem } from '@/types';

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
    <button class="cart-button m-4"
            @click="isCartOpen = !isCartOpen"
            :class="{ 'has-items': cartItems.length > 0 }">
      <i class="fas fa-shopping-cart"></i>
      <span v-if="cartItems.length > 0" class="cart-count">{{ totalItems }}</span>
    </button>

    <div class="cart-dropdown" :class="{ show: isCartOpen }">
      <div class="cart-header d-flex justify-content-between align-items-center">
        <h5 class="mb-0">Your Order</h5>
        <button type="button" class="btn-close" @click="isCartOpen = false"></button>
      </div>

      <div v-if="cartItems.length === 0" class="empty-cart">
        <i class="fas fa-shopping-cart text-muted"></i>
        <p>Your cart is empty</p>
      </div>

      <div v-else class="cart-items">
        <div v-for="(item, index) in cartItems"
             :key="index"
             class="cart-item">
          <div class="cart-item-details">
            <h6 class="cart-item-name">{{ item.menuItem.name }}</h6>
            <div class="d-flex justify-content-between">
              <span class="cart-item-quantity">Qty: {{ item.quantity }}</span>
              <span class="cart-item-price">${{ (item.menuItem.price * item.quantity).toFixed(2) }}</span>
            </div>
            <small v-if="item.specialInstructions" class="text-muted d-block mt-1">
              {{ item.specialInstructions }}
            </small>
          </div>
          <button type="button" 
                  class="btn btn-sm text-danger"
                  title="Remove item"
                  @click="removeItem(index)">
            <i class="fas fa-trash-alt"></i>
          </button>
        </div>
      </div>

      <div v-if="cartItems.length > 0" class="cart-footer">
        <div class="d-flex justify-content-between align-items-center w-100">
          <div class="cart-total">
            <span class="me-2">Total:</span>
            <span class="fw-bold">${{ subtotal.toFixed(2) }}</span>
          </div>
          <button class="btn btn-primary">Checkout</button>
        </div>
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
  z-index: 1000;
}

.cart-button {
  background-color: rgba(0, 0, 0, 0.7);
  border: 1px solid rgba(255, 255, 255, 0.1);
  color: #fff;
  padding: 0.75rem 1.5rem;
  border-radius: 2rem;
  cursor: pointer;
  transition: all 0.3s ease;
  backdrop-filter: blur(10px);
  font-size: 1.25rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
}

.cart-button:hover {
  background-color: rgba(255, 255, 255, 0.1);
  border-color: rgba(255, 255, 255, 0.3);
}

.cart-button.has-items {
  background-color: rgba(59, 113, 202, 0.8);
  border-color: rgba(255, 255, 255, 0.2);
}

.cart-button.has-items:hover {
  background-color: rgba(59, 113, 202, 1);
  border-color: rgba(255, 255, 255, 0.4);
}

.cart-button i {
  font-size: 1.5rem;
}

.cart-count {
  position: absolute;
  top: -10px;
  right: -10px;
  background-color: rgba(220, 53, 69, 0.9);
  color: white;
  border-radius: 50%;
  padding: 0.35rem 0.6rem;
  font-size: 0.9rem;
  min-width: 1.8rem;
  min-height: 1.8rem;
  display: flex;
  align-items: center;
  justify-content: center;
  text-align: center;
  border: 2px solid rgba(0, 0, 0, 0.8);
  font-weight: bold;
}

.cart-dropdown {
  position: absolute;
  top: 100%;
  right: 0;
  margin-top: 0.5rem;
  min-width: 300px;
  background-color: rgba(0, 0, 0, 0.95);
  border: 1px solid rgba(255, 255, 255, 0.1);
  border-radius: 0.5rem;
  box-shadow: 0 0.5rem 1rem rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(10px);
  color: #fff;
  opacity: 0;
  visibility: hidden;
  transform: translateY(10px);
  transition: all 0.3s ease;
  z-index: 1000;
}

.cart-dropdown.show {
  opacity: 1;
  visibility: visible;
  transform: translateY(0);
}

.cart-header {
  padding: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  font-weight: bold;
}

.cart-items {
  max-height: 300px;
  overflow-y: auto;
}

.cart-item {
  padding: 1rem;
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.cart-item:last-child {
  border-bottom: none;
}

.cart-item-details {
  flex-grow: 1;
  margin-right: 1rem;
}

.cart-item-name {
  font-weight: 500;
  margin-bottom: 0.25rem;
}

.cart-item-price {
  color: rgba(255, 255, 255, 0.7);
  font-size: 0.9rem;
}

.btn.text-danger {
  color: rgba(220, 53, 69, 0.8) !important;
  background: none;
  border: none;
  padding: 0.25rem;
  cursor: pointer;
  transition: color 0.3s ease;
}

.btn.text-danger:hover {
  color: rgba(220, 53, 69, 1) !important;
}

.btn.text-danger i {
  font-size: 1rem;
}

.cart-footer {
  padding: 1rem;
  border-top: 1px solid rgba(255, 255, 255, 0.1);
  display: flex;
  justify-content: space-between;
  align-items: center;
  background-color: rgba(0, 0, 0, 0.5);
  border-bottom-left-radius: 0.5rem;
  border-bottom-right-radius: 0.5rem;
}

.cart-total {
  font-weight: bold;
}

/* Scrollbar styles */
.cart-items::-webkit-scrollbar {
  width: 6px;
}

.cart-items::-webkit-scrollbar-track {
  background: rgba(255, 255, 255, 0.1);
}

.cart-items::-webkit-scrollbar-thumb {
  background-color: rgba(255, 255, 255, 0.3);
  border-radius: 3px;
}

.cart-items::-webkit-scrollbar-thumb:hover {
  background-color: rgba(255, 255, 255, 0.4);
}

.empty-cart {
  padding: 2rem;
  text-align: center;
  color: rgba(255, 255, 255, 0.5);
}

.empty-cart i {
  font-size: 2rem;
  margin-bottom: 1rem;
  opacity: 0.5;
}

.cart-backdrop {
  position: fixed;
  top: 0;
  right: 0;
  bottom: 0;
  left: 0;
  z-index: 990;
  background-color: rgba(0, 0, 0, 0.5);
  backdrop-filter: blur(3px);
}

.btn-close {
  filter: invert(1) grayscale(100%) brightness(200%);
  opacity: 0.7;
  transition: opacity 0.3s ease;
}

.btn-close:hover {
  opacity: 1;
}

.cart-item-quantity {
  color: rgba(255, 255, 255, 0.7);
  font-size: 0.9rem;
}

/* Checkout button styles */
.btn-primary {
  background-color: rgba(59, 113, 202, 0.8);
  border-color: rgba(255, 255, 255, 0.2);
  transition: all 0.3s ease;
}

.btn-primary:hover {
  background-color: rgba(59, 113, 202, 1);
  border-color: rgba(255, 255, 255, 0.4);
}

/* Add responsive adjustments */
@media (max-width: 767px) {
  .cart-button {
    padding: 0.6rem 1.2rem;
    font-size: 1.1rem;
  }

  .cart-button i {
    font-size: 1.3rem;
  }

  .cart-count {
    padding: 0.25rem 0.5rem;
    font-size: 0.8rem;
    min-width: 1.6rem;
    min-height: 1.6rem;
  }
}
</style>