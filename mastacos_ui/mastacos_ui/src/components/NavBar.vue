<script setup lang="ts">
import {
  MDBNavbar,
  MDBNavbarBrand,
  MDBNavbarNav,
  MDBNavbarItem,
  MDBNavbarToggler,
  MDBCollapse,
  MDBIcon,
  MDBSpinner
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
  isLoading,
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
  <MDBNavbar expand="lg" light bg="light" class="shadow-sm">
    <div class="container-fluid">
      <!-- Brand -->
      <MDBNavbarBrand href="/MasTacosProject" @click.prevent="navigateTo('/MasTacosProject')" class="fw-bold ms-3">
        <MDBIcon icon="utensils" class="me-2 text-primary" />
        Ma's Tacos
      </MDBNavbarBrand>

      <!-- Mobile toggle button -->
      <MDBNavbarToggler @click="toggleNav" aria-label="Toggle navigation">
        <MDBIcon icon="bars" />
      </MDBNavbarToggler>

      <!-- Collapsible content -->
      <MDBCollapse :show="!isNavCollapsed" navbar>
        <!-- Loading indicator -->
        <div v-if="isLoading" class="navbar-nav me-auto">
          <div class="nav-item d-flex align-items-center">
            <MDBSpinner size="sm" role="status" class="me-2"></MDBSpinner>
            <span class="text-muted">Loading...</span>
          </div>
        </div>

        <!-- Navigation items -->
        <template v-else>
          <!-- Left navigation -->
          <MDBNavbarNav class="me-auto mb-2 mb-lg-0">
            <MDBNavbarItem>
              <a
                  href="#"
                  class="nav-link"
                  @click.prevent="navigateTo('/')"
                  :class="{ active: $route.path === '/' }">
                <MDBIcon icon="home" class="me-1" />
                Menu
              </a>
            </MDBNavbarItem>

            <!-- Admin link (only show if user is admin) -->
            <MDBNavbarItem v-if="isAuthenticated && isAdmin">
              <a
                  href="#"
                  class="nav-link"
                  @click.prevent="navigateTo('/admin')"
                  :class="{ active: $route.path.startsWith('/admin') }">
                <MDBIcon icon="cogs" class="me-1" />
                Admin
              </a>
            </MDBNavbarItem>
          </MDBNavbarNav>

          <!-- Right navigation -->
          <MDBNavbarNav class="ms-auto">
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
              <!-- User profile link -->
              <MDBNavbarItem class="d-none d-md-block">
                <a
                    href="#"
                    class="nav-link"
                    @click.prevent="navigateTo('/profile')"
                    :class="{ active: $route.path === '/profile' }">
                  <MDBIcon icon="user-edit" class="me-1" />
                  Profile
                </a>
              </MDBNavbarItem>

              <!-- Welcome message (using nav-link styling for consistency) -->
              <MDBNavbarItem class="d-none d-lg-block">
                <span class="nav-link mb-0">
                  <MDBIcon icon="user" class="me-1" />
                  {{ userFullName || user?.email }}
                </span>
              </MDBNavbarItem>

              <!-- Logout button (styled as nav-link for consistency) -->
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
        </template>
      </MDBCollapse>
    </div>
  </MDBNavbar>
</template>

<style scoped>
.navbar-brand {
  font-size: 1.5rem;
  transition: color 0.3s ease;
}

.navbar-brand:hover {
  color: var(--mdb-primary) !important;
}

.nav-link {
  transition: all 0.3s ease;
  border-radius: 5px;
  margin: 0 2px;
}

.nav-link:hover {
  background-color: rgba(0, 123, 255, 0.1);
}

.nav-link.active {
  color: var(--mdb-primary) !important;
  font-weight: 500;
  background-color: rgba(0, 123, 255, 0.1);
}

.dropdown-toggle::after {
  margin-left: 0.5rem;
}

.dropdown-item {
  transition: background-color 0.3s ease;
}

.dropdown-item:hover {
  background-color: rgba(0, 123, 255, 0.1);
}

/* Mobile-specific styles */
@media (max-width: 991.98px) {
  .navbar-nav {
    text-align: center;
  }

  .dropdown-menu {
    border: none;
    box-shadow: none;
    background-color: transparent;
  }

  .dropdown-item {
    padding: 0.5rem 1rem;
  }
}
</style>