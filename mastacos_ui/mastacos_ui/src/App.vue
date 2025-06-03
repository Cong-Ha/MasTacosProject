<template>
  <div id="app">
    <!-- Enhanced Navbar Component -->
    <NavBar />

    <!-- Main Content -->
    <div class="container-fluid container border">
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
  background-color: #000;
  color: #fff;
}

.router-link-active {
  font-weight: bold;
}

/* Global styles */
.container-fluid {
  padding: 0;
}

/* Custom scrollbar */
::-webkit-scrollbar {
  width: 8px;
}

::-webkit-scrollbar-track {
  background: #1a1a1a;
}

::-webkit-scrollbar-thumb {
  background: #333;
  border-radius: 4px;
}

::-webkit-scrollbar-thumb:hover {
  background: #444;
}

/* Card styles */
.card {
  background-color: rgba(0, 0, 0, 0.7) !important;
  border: 1px solid rgba(255, 255, 255, 0.1) !important;
  backdrop-filter: blur(10px);
}

.card-body {
  color: #fff;
}

/* Button styles */
.btn-primary {
  background-color: rgba(59, 113, 202, 0.8) !important;
  border-color: rgba(255, 255, 255, 0.2) !important;
}

.btn-primary:hover {
  background-color: rgba(59, 113, 202, 1) !important;
  border-color: rgba(255, 255, 255, 0.4) !important;
}

/* Form styles */
.form-control {
  background-color: rgba(255, 255, 255, 0.1) !important;
  border-color: rgba(255, 255, 255, 0.2) !important;
  color: #fff !important;
}

.form-control:focus {
  background-color: rgba(255, 255, 255, 0.15) !important;
  border-color: rgba(255, 255, 255, 0.4) !important;
  box-shadow: 0 0 0 0.2rem rgba(255, 255, 255, 0.1) !important;
}

.form-control::placeholder {
  color: rgba(255, 255, 255, 0.5) !important;
}

/* Table styles */
.table {
  color: #fff !important;
}

.table-light {
  background-color: rgba(255, 255, 255, 0.1) !important;
  color: #fff !important;
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

/* Background gradient */
body {
  background: linear-gradient(135deg, #000000 0%, #1a1a1a 100%);
  min-height: 100vh;
  margin: 0;
  color: #fff;
}
</style>