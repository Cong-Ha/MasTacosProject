<script setup lang="ts">
import {
  MDBInput,
  MDBIcon,
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBCheckbox,
  MDBSpinner
} from "mdb-vue-ui-kit";
import { ref, onMounted } from "vue";
import { useRouter, useRoute, RouteLocationNormalizedLoaded } from 'vue-router';
import { useAuthStore } from '@/store/auth';
import { AxiosError } from 'axios';

// interface ValidationErrors {
//   [key: string]: string[];
// }

const router = useRouter();
const route: RouteLocationNormalizedLoaded = useRoute();
const authStore = useAuthStore();

const email = ref<string>('');
const password = ref<string>('');
const rememberMe = ref<boolean>(false);
const errorMessage = ref<string>('');
const successMessage = ref<string>('');
const loading = ref<boolean>(false);

onMounted(() => {
  if (route.query.registered === 'true') {
    successMessage.value = 'Registration successful! Please log in.';
  }

  // Redirect if already logged in
  if (authStore.isAuthenticated) {
    router.push('/');
  }
});

const handleLogin = async (): Promise<void> => {
  try {
    loading.value = true;
    errorMessage.value = '';

    // Use the Pinia store for login
    const response = await authStore.login(email.value, password.value, rememberMe.value);

    if (response.success) {
      // Wait a brief moment to ensure the store has updated
      await new Promise(resolve => setTimeout(resolve, 100));

      // Redirect to home page or requested page
      const redirectPath = (route.query.redirect as string) || '/';
      router.push(redirectPath);
    } else {
      errorMessage.value = response.message || 'Login failed';
    }
  } catch (error) {
    const axiosError = error as AxiosError;
    if (axiosError.response) {
      if (axiosError.response.status === 401) {
        errorMessage.value = 'Invalid email or password';
      } else if (axiosError.response.status === 423) {
        errorMessage.value = 'Your account is locked. Please try again later.';
      } else if (axiosError.response.data) {
        // Handle validation errors
        const errorData = axiosError.response.data as any;

        if (errorData.message) {
          errorMessage.value = errorData.message;
        } else if (errorData.errors) {
          // Handle ModelState validation errors
          const errorMessages: string[] = [];
          for (const key in errorData.errors) {
            if (Array.isArray(errorData.errors[key])) {
              errorMessages.push(...errorData.errors[key]);
            }
          }
          errorMessage.value = errorMessages.join(', ');
        } else if (typeof errorData === 'string') {
          errorMessage.value = errorData;
        } else {
          errorMessage.value = 'An error occurred during login';
        }
      }
    } else {
      errorMessage.value = 'Network error. Please check your connection.';
    }
    console.error('Login error:', error);
  } finally {
    loading.value = false;
  }
};

const goToRegister = (): void => {
  router.push('/register');
};
</script>

<template>
  <div class="d-flex justify-content-center align-items-center" style="min-height: 80vh;">
    <MDBCard style="max-width: 400px; width: 100%;">
      <MDBCardBody>
        <MDBCardTitle class="text-center mb-4">
          <MDBIcon far icon="user-circle" size="3x" class="mb-3"></MDBIcon>
          <h4 class="fw-bold">Login</h4>
        </MDBCardTitle>

        <div v-if="successMessage" class="alert alert-success mb-4">{{ successMessage }}</div>
        <div v-if="errorMessage" class="alert alert-danger mb-4">{{ errorMessage }}</div>

        <form @submit.prevent="handleLogin">
          <div class="mb-4">
            <MDBInput
                label="Email"
                type="email"
                class="form-icon-trailing"
                v-model="email"
                required>
              <MDBIcon icon="envelope" class="trailing"></MDBIcon>
            </MDBInput>
          </div>

          <div class="mb-4">
            <MDBInput
                label="Password"
                type="password"
                class="form-icon-trailing"
                v-model="password"
                required>
              <MDBIcon icon="lock" class="trailing"></MDBIcon>
            </MDBInput>
          </div>

          <div class="d-flex justify-content-between mb-4">
            <MDBCheckbox label="Remember me" v-model="rememberMe" />
            <a href="#!" class="text-decoration-none">Forgot password?</a>
          </div>

          <MDBBtn type="submit" color="primary" block :disabled="loading">
            <MDBSpinner v-if="loading" size="sm" role="status" tag="span" class="me-2"></MDBSpinner>
            Sign in
          </MDBBtn>

          <div class="text-center mt-4">
            <MDBCardText>
              Don't have an account?
              <a href="#!" class="text-decoration-none" @click.prevent="goToRegister">Register</a>
            </MDBCardText>
          </div>
        </form>
      </MDBCardBody>
    </MDBCard>
  </div>
</template>

<style scoped>
.form-icon-trailing .trailing {
  color: #6c757d;
}

a {
  color: #3b71ca;
}

a:hover {
  color: #2e5aa5;
  cursor: pointer;
}
</style>