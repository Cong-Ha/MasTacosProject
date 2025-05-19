<template>
  <div id="app">
    <!-- Enhanced Navbar Component -->
    <NavBar />

    <!-- Main Content -->
    <div class="container-fluid">
      <router-view />
    </div>
  </div>
</template>

<script setup lang="ts">
import NavBar from '@/components/NavBar.vue';
import { onMounted } from 'vue';
import { useAuth } from '@/store/auth';

const { initializeAuth } = useAuth();

// Initialize authentication state when app starts
onMounted(() => {
  initializeAuth();
});
</script>

<style>
#app {
  font-family: Roboto, Helvetica, Arial, sans-serif;
  min-height: 100vh;
  background-color: #f8f9fa;
}

.router-link-active {
  font-weight: bold;
}

/* Global styles for better UX */
.container-fluid {
  padding: 0;
}

/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #f1f1f1;
}

::-webkit-scrollbar-thumb {
  background: #888;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #555;
}

/* Loading animation */
@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.fade-in {
  animation: fadeIn 0.5s ease-out;
}

/* Responsive utilities */
@media (max-width: 768px) {
  .container-fluid {
    padding: 0 15px;
  }
}
</style>