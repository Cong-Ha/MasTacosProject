<script setup lang="ts">
import {
  MDBInput,
  MDBIcon,
  MDBBtn,
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBSpinner
} from "mdb-vue-ui-kit";
import { ref, computed, reactive } from "vue";
import { useRouter } from 'vue-router';
import { useVuelidate } from '@vuelidate/core';
import {
  required,
  email,
  minLength,
  sameAs,
  helpers
} from '@vuelidate/validators';
import authService, { type RegisterRequest } from '@/services/authService';
import { AxiosError } from 'axios';

const router = useRouter();

// Form state
const state = reactive({
  firstName: '',
  lastName: '',
  email: '',
  password: '',
  confirmPassword: ''
});

const errorMessage = ref<string>('');
const loading = ref<boolean>(false);

// Custom password validators
const hasLowercase = helpers.withMessage(
    'Password must contain at least one lowercase letter',
    (value: string) => !value || /[a-z]/.test(value)
);

const hasUppercase = helpers.withMessage(
    'Password must contain at least one uppercase letter',
    (value: string) => !value || /[A-Z]/.test(value)
);

const hasNumber = helpers.withMessage(
    'Password must contain at least one number',
    (value: string) => !value || /[0-9]/.test(value)
);

const hasSpecialChar = helpers.withMessage(
    'Password must contain at least one special character',
    (value: string) => !value || /[^a-zA-Z0-9]/.test(value)
);

// Validation rules
const rules = {
  firstName: {
    required: helpers.withMessage('First name is required', required)
  },
  lastName: {
    required: helpers.withMessage('Last name is required', required)
  },
  email: {
    required: helpers.withMessage('Email is required', required),
    email: helpers.withMessage('Please enter a valid email address', email)
  },
  password: {
    required: helpers.withMessage('Password is required', required),
    minLength: helpers.withMessage('Password must be at least 8 characters', minLength(8)),
    hasLowercase,
    hasUppercase,
    hasNumber,
    hasSpecialChar
  },
  confirmPassword: {
    required: helpers.withMessage('Please confirm your password', required),
    sameAs: helpers.withMessage('Passwords do not match', sameAs(computed(() => state.password)))
  }
};

const v$ = useVuelidate(rules, state);

// Helper function to get field error message
const getFieldError = (field: any) => {
  if (field.$error && field.$errors.length > 0) {
    return field.$errors[0].$message;
  }
  return '';
};

// Check if form is valid
const isFormValid = computed(() => {
  return !v$.value.$invalid;
});

const handleRegister = async (): Promise<void> => {
  // Trigger validation
  const isFormCorrect = await v$.value.$validate();

  if (!isFormCorrect) {
    errorMessage.value = 'Please fix the errors below';
    return;
  }

  try {
    loading.value = true;
    errorMessage.value = '';

    const registerData: RegisterRequest = {
      firstName: state.firstName.trim(),
      lastName: state.lastName.trim(),
      email: state.email.trim(),
      password: state.password,
      confirmPassword: state.confirmPassword
    };

    const response = await authService.register(registerData);

    if (response.success) {
      // Registration successful, redirect to login with success message
      router.push('/login?registered=true');
    } else {
      errorMessage.value = response.message || 'Registration failed';
    }
  } catch (error) {
    const axiosError = error as AxiosError;
    if (axiosError.response) {
      if (axiosError.response.data) {
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
        } else if (Array.isArray(errorData)) {
          // Handle array of error messages (like your password error)
          errorMessage.value = errorData.join(', ');
        } else if (typeof errorData === 'string') {
          errorMessage.value = errorData;
        } else {
          errorMessage.value = 'Registration failed. Please try again.';
        }
      }
    } else {
      errorMessage.value = 'Network error. Please check your connection.';
    }
    console.error('Registration error:', error);
  } finally {
    loading.value = false;
  }
};

const goToLogin = (): void => {
  router.push('/login');
};
</script>

<template>
  <div class="d-flex justify-content-center align-items-center" style="min-height: 80vh;">
    <MDBCard style="max-width: 500px; width: 100%;">
      <MDBCardBody>
        <MDBCardTitle class="text-center mb-4">
          <MDBIcon far icon="user-plus" size="3x" class="mb-3"></MDBIcon>
          <h4 class="fw-bold">Create Account</h4>
        </MDBCardTitle>

        <div v-if="errorMessage" class="alert alert-danger mb-4">{{ errorMessage }}</div>

        <form @submit.prevent="handleRegister">
          <div class="row mb-4">
            <div class="col-md-6">
              <MDBInput
                  label="First Name"
                  type="text"
                  v-model="state.firstName"
                  :class="{ 'field-error': v$.firstName.$error }"
                  @blur="v$.firstName.$touch"
                  required>
              </MDBInput>
              <div v-if="v$.firstName.$error" class="error-message">
                {{ getFieldError(v$.firstName) }}
              </div>
            </div>
            <div class="col-md-6">
              <MDBInput
                  label="Last Name"
                  type="text"
                  v-model="state.lastName"
                  :class="{ 'field-error': v$.lastName.$error }"
                  @blur="v$.lastName.$touch"
                  required>
              </MDBInput>
              <div v-if="v$.lastName.$error" class="error-message">
                {{ getFieldError(v$.lastName) }}
              </div>
            </div>
          </div>

          <div class="mb-4">
            <MDBInput
                label="Email"
                type="email"
                class="form-icon-trailing"
                v-model="state.email"
                :class="{ 'field-error': v$.email.$error }"
                @blur="v$.email.$touch"
                required>
              <MDBIcon icon="envelope" class="trailing"></MDBIcon>
            </MDBInput>
            <div v-if="v$.email.$error" class="error-message">
              {{ getFieldError(v$.email) }}
            </div>
          </div>

          <div class="mb-4">
            <MDBInput
                label="Password"
                type="password"
                class="form-icon-trailing"
                v-model="state.password"
                :class="{ 'field-error': v$.password.$error }"
                @blur="v$.password.$touch"
                required>
              <MDBIcon icon="lock" class="trailing"></MDBIcon>
            </MDBInput>
            <small class="form-text text-muted">
              Password must be at least 8 characters with uppercase, lowercase, number, and special character
            </small>
            <div v-if="v$.password.$error" class="error-message">
              <div v-for="error in v$.password.$errors" :key="error.$uid" class="error-item">
                {{ error.$message }}
              </div>
            </div>
          </div>

          <div class="mb-4">
            <MDBInput
                label="Confirm Password"
                type="password"
                class="form-icon-trailing"
                v-model="state.confirmPassword"
                :class="{ 'field-error': v$.confirmPassword.$error }"
                @blur="v$.confirmPassword.$touch"
                required>
              <MDBIcon icon="lock" class="trailing"></MDBIcon>
            </MDBInput>
            <div v-if="v$.confirmPassword.$error" class="error-message">
              {{ getFieldError(v$.confirmPassword) }}
            </div>
          </div>

          <MDBBtn
              type="submit"
              color="primary"
              block
              :disabled="loading || !isFormValid">
            <MDBSpinner v-if="loading" size="sm" role="status" tag="span" class="me-2"></MDBSpinner>
            Create Account
          </MDBBtn>

          <div class="text-center mt-4">
            <MDBCardText>
              Already have an account?
              <a href="#!" class="text-decoration-none" @click.prevent="goToLogin">Sign in</a>
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

/* Custom field error styling - works better with MDB */
.field-error {
  border-color: #dc3545 !important;
  box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25) !important;
}

.error-message {
  color: #dc3545;
  font-size: 0.875em;
  margin-top: 0.5rem;
  margin-bottom: 0;
}

.error-item {
  margin-bottom: 0.25rem;
}

.error-item:last-child {
  margin-bottom: 0;
}

/* Ensure proper spacing between form groups */
.mb-4 {
  margin-bottom: 1.5rem !important;
}

/* Override any conflicting MDB styles */
.form-control:focus {
  border-color: #86b7fe;
  box-shadow: 0 0 0 0.2rem rgba(13, 110, 253, 0.25);
}

.field-error:focus {
  border-color: #dc3545 !important;
  box-shadow: 0 0 0 0.2rem rgba(220, 53, 69, 0.25) !important;
}
</style>