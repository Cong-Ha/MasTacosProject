<script setup lang="ts">
import {
  MDBNavbar,
  MDBNavbarBrand,
  MDBNavbarNav,
  MDBNavbarItem,
  MDBIcon
} from "mdb-vue-ui-kit";
import { ref, watch } from 'vue';
import { useAuthStore } from '@/store/auth';
import { useRouter } from 'vue-router';
import { storeToRefs } from 'pinia';

const authStore = useAuthStore();
const router = useRouter();
const isNavCollapsed = ref(true);

// Destructure reactive properties from the store
const {
  isAuthenticated,
  user,
  userFullName,
  isAdmin
} = storeToRefs(authStore);

// Add some debugging
watch(isAuthenticated, (newVal) => {
  console.log('Navbar: isAuthenticated changed to:', newVal);
}, { immediate: true });

watch(user, (newVal) => {
  console.log('Navbar: user changed to:', newVal);
}, { immediate: true });

const handleLogout = async (): Promise<void> => {
  try {
    await authStore.logout();
    router.push('/');
  } catch (error) {
    console.error('Logout error:', error);
    // Force navigation even if logout API call fails
    router.push('/');
  }
};

const navigateTo = (path: string): void => {
  router.push(path);
  // Close mobile menu after navigation
  isNavCollapsed.value = true;
};

const toggleNav = (): void => {
  isNavCollapsed.value = !isNavCollapsed.value;
};
</script>

<template>
  <MDBNavbar dark class="navbar-dark fixed-top">
    <div class="container-fluid">
      <!-- Brand -->
      <MDBNavbarBrand href="#" @click.prevent="navigateTo('/')" class="me-2">
        <img src="@/assets/logo.png" height="30" alt="Ma's Tacos" class="me-2" />
        Ma's Tacos
      </MDBNavbarBrand>

      <!-- Hamburger button -->
      <button
        class="navbar-toggler"
        type="button"
        @click="toggleNav"
        :aria-expanded="!isNavCollapsed"
        aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>

      <!-- Collapsible content -->
      <div
        class="collapse navbar-collapse"
        :class="{ show: !isNavCollapsed }">
        <MDBNavbarNav class="ms-auto">
          <!-- Menu Items -->
          <MDBNavbarItem>
            <a
              href="#"
              class="nav-link"
              @click.prevent="navigateTo('/menu')"
              :class="{ active: $route.path === '/menu' }">
              <MDBIcon icon="utensils" class="me-1" />
              Menu
            </a>
          </MDBNavbarItem>

          <!-- Guest navigation -->
          <template v-if="!isAuthenticated">
            <MDBNavbarItem>
              <a
                href="#"
                class="nav-link"
                @click.prevent="navigateTo('/login')"
                :class="{ active: $route.path === '/login' }">
                <MDBIcon icon="sign-in-alt" class="me-1" />
                Login
              </a>
            </MDBNavbarItem>
          </template>

          <!-- Authenticated user navigation -->
          <template v-else>
            <!-- Admin link -->
            <MDBNavbarItem v-if="isAdmin">
              <a
                href="#"
                class="nav-link"
                @click.prevent="navigateTo('/admin')"
                :class="{ active: $route.path === '/admin' }">
                <MDBIcon icon="cog" class="me-1" />
                Admin
              </a>
            </MDBNavbarItem>

            <!-- User profile link -->
            <MDBNavbarItem>
              <a
                href="#"
                class="nav-link"
                @click.prevent="navigateTo('/profile')"
                :class="{ active: $route.path === '/profile' }">
                <MDBIcon icon="user-edit" class="me-1" />
                Profile
              </a>
            </MDBNavbarItem>

            <!-- Welcome message -->
            <MDBNavbarItem>
              <span class="nav-link">
                <MDBIcon icon="user" class="me-1" />
                {{ userFullName || user?.email }}
              </span>
            </MDBNavbarItem>

            <!-- Logout button -->
            <MDBNavbarItem>
              <a
                href="#"
                class="nav-link text-danger"
                @click.prevent="handleLogout">
                <MDBIcon icon="sign-out-alt" class="me-1" />
                Logout
              </a>
            </MDBNavbarItem>
          </template>
        </MDBNavbarNav>
      </div>
    </div>
  </MDBNavbar>
  <!-- Add a spacer div to prevent content from being hidden under the fixed navbar -->
  <div class="navbar-spacer"></div>
</template>

<style scoped>
.navbar {
  background-color: rgba(0, 0, 0, 0.8);
  backdrop-filter: blur(10px);
  border-bottom: 1px solid rgba(255, 255, 255, 0.1);
  height: 60px; /* Set a fixed height for the navbar */
}

/* Add spacer to prevent content from being hidden */
.navbar-spacer {
  height: 60px; /* Match the navbar height */
  width: 100%;
}

.navbar-brand {
  color: #fff;
  font-weight: 600;
}

.nav-link {
  color: rgba(255, 255, 255, 0.8);
  transition: all 0.3s ease;
  padding: 0.5rem 1rem;
  border-radius: 0.25rem;
}

.nav-link:hover,
.nav-link.active {
  color: #fff;
  background-color: rgba(255, 255, 255, 0.1);
}

.navbar-toggler {
  display: block;
  border-color: rgba(255, 255, 255, 0.1);
  padding: 0.25rem 0.5rem;
}

.navbar-toggler:focus {
  box-shadow: 0 0 0 0.2rem rgba(255, 255, 255, 0.1);
}

.navbar-toggler-icon {
  background-image: url("data:image/svg+xml,%3csvg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 30 30'%3e%3cpath stroke='rgba(255, 255, 255, 0.8)' stroke-linecap='round' stroke-miterlimit='10' stroke-width='2' d='M4 7h22M4 15h22M4 23h22'/%3e%3c/svg%3e");
}

/* Animation for mobile menu */
.navbar-collapse {
  position: absolute;
  top: 100%;
  right: 0;
  left: 0;
  transition: all 0.3s ease;
  padding: 0.5rem;
  margin-top: 0.5rem;
}

.navbar-collapse.show {
  background-color: rgba(0, 0, 0, 0.95);
  backdrop-filter: blur(10px);
  border-radius: 0.5rem;
  border: 1px solid rgba(255, 255, 255, 0.1);
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  margin: 0.5rem;
}

/* Remove media query styles that were specific to mobile */
.nav-link {
  padding: 0.5rem 1rem;
  margin: 0.25rem 0;
  text-align: left;
  display: flex;
  align-items: center;
}

.container-fluid {
  padding-right: 1rem;
  padding-left: 1rem;
}
</style>