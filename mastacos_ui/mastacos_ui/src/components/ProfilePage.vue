<script setup lang="ts">
import {
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBInput,
  MDBBtn,
  MDBRow,
  MDBCol,
  MDBIcon,
  MDBBadge,
  MDBSpinner
} from "mdb-vue-ui-kit";
import { ref, onMounted } from 'vue';
import { useAuth } from '@/store/auth';

const { user, userFullName, userEmail, userRoles, isLoading } = useAuth();

// Form data
const firstName = ref<string>('');
const lastName = ref<string>('');
const email = ref<string>('');
const phoneNumber = ref<string>('');

// Form state
const isEditing = ref<boolean>(false);
const isSaving = ref<boolean>(false);
const successMessage = ref<string>('');
const errorMessage = ref<string>('');

// Initialize form with user data
onMounted(() => {
  if (user) {
    firstName.value = user.firstName || '';
    lastName.value = user.lastName || '';
    email.value = user.email || '';
    // phoneNumber would need to be added to UserInfo type if you want to support it
  }
});

const toggleEdit = (): void => {
  if (isEditing.value) {
    // Cancel editing - reset form
    if (user) {
      firstName.value = user.firstName || '';
      lastName.value = user.lastName || '';
      email.value = user.email || '';
    }
    errorMessage.value = '';
    successMessage.value = '';
  }
  isEditing.value = !isEditing.value;
};

const saveProfile = async (): Promise<void> => {
  try {
    isSaving.value = true;
    errorMessage.value = '';

    // Here you would make an API call to update the user profile
    // For now, we'll just simulate a successful save
    await new Promise(resolve => setTimeout(resolve, 1000));

    successMessage.value = 'Profile updated successfully!';
    isEditing.value = false;

    // Clear success message after 3 seconds
    setTimeout(() => {
      successMessage.value = '';
    }, 3000);
  } catch (error) {
    errorMessage.value = 'Failed to update profile. Please try again.';
    console.error('Profile update error:', error);
  } finally {
    isSaving.value = false;
  }
};

const getRoleBadgeColor = (role: string): string => {
  switch (role.toLowerCase()) {
    case 'admin':
      return 'danger';
    case 'customer':
      return 'primary';
    default:
      return 'secondary';
  }
};
</script>

<template>
  <div class="container py-5">
    <div class="row justify-content-center">
      <div class="col-lg-8 col-xl-6">
        <!-- Profile Header -->
        <MDBCard class="mb-4">
          <MDBCardBody class="text-center">
            <div class="mb-3">
              <MDBIcon icon="user-circle" size="4x" class="text-primary"></MDBIcon>
            </div>
            <h4 class="mb-2">{{ userFullName || 'User Profile' }}</h4>
            <p class="text-muted mb-3">{{ userEmail }}</p>
            <div class="d-flex justify-content-center gap-2 flex-wrap">
              <MDBBadge
                  v-for="role in userRoles"
                  :key="role"
                  :color="getRoleBadgeColor(role)"
                  pill>
                {{ role }}
              </MDBBadge>
            </div>
          </MDBCardBody>
        </MDBCard>

        <!-- Profile Form -->
        <MDBCard>
          <MDBCardBody>
            <div class="d-flex justify-content-between align-items-center mb-4">
              <MDBCardTitle class="mb-0">
                <MDBIcon icon="user-edit" class="me-2"></MDBIcon>
                Personal Information
              </MDBCardTitle>
              <MDBBtn
                  v-if="!isEditing"
                  color="primary"
                  size="sm"
                  @click="toggleEdit">
                <MDBIcon icon="edit" class="me-1"></MDBIcon>
                Edit
              </MDBBtn>
            </div>

            <div v-if="successMessage" class="alert alert-success mb-4">
              {{ successMessage }}
            </div>
            <div v-if="errorMessage" class="alert alert-danger mb-4">
              {{ errorMessage }}
            </div>

            <form @submit.prevent="saveProfile">
              <MDBRow class="mb-3">
                <MDBCol md="6">
                  <MDBInput
                      label="First Name"
                      type="text"
                      v-model="firstName"
                      :disabled="!isEditing || isLoading"
                      required>
                  </MDBInput>
                </MDBCol>
                <MDBCol md="6">
                  <MDBInput
                      label="Last Name"
                      type="text"
                      v-model="lastName"
                      :disabled="!isEditing || isLoading"
                      required>
                  </MDBInput>
                </MDBCol>
              </MDBRow>

              <div class="mb-3">
                <MDBInput
                    label="Email"
                    type="email"
                    v-model="email"
                    :disabled="!isEditing || isLoading"
                    required>
                </MDBInput>
                <small class="form-text text-muted">
                  Email changes may require verification
                </small>
              </div>

              <div class="mb-4">
                <MDBInput
                    label="Phone Number (Optional)"
                    type="tel"
                    v-model="phoneNumber"
                    :disabled="!isEditing || isLoading">
                </MDBInput>
              </div>

              <!-- Action buttons -->
              <div v-if="isEditing" class="d-flex gap-3 justify-content-end">
                <MDBBtn
                    color="secondary"
                    outline
                    @click="toggleEdit"
                    :disabled="isSaving">
                  Cancel
                </MDBBtn>
                <MDBBtn
                    type="submit"
                    color="primary"
                    :disabled="isSaving">
                  <MDBSpinner
                      v-if="isSaving"
                      size="sm"
                      role="status"
                      tag="span"
                      class="me-2">
                  </MDBSpinner>
                  Save Changes
                </MDBBtn>
              </div>
            </form>
          </MDBCardBody>
        </MDBCard>

        <!-- Account Actions -->
        <MDBCard class="mt-4">
          <MDBCardBody>
            <MDBCardTitle class="mb-3">
              <MDBIcon icon="cog" class="me-2"></MDBIcon>
              Account Settings
            </MDBCardTitle>

            <div class="d-grid gap-3">
              <MDBBtn color="warning" outline>
                <MDBIcon icon="key" class="me-2"></MDBIcon>
                Change Password
              </MDBBtn>
              <MDBBtn color="info" outline>
                <MDBIcon icon="bell" class="me-2"></MDBIcon>
                Notification Preferences
              </MDBBtn>
              <MDBBtn color="danger" outline>
                <MDBIcon icon="user-times" class="me-2"></MDBIcon>
                Delete Account
              </MDBBtn>
            </div>
          </MDBCardBody>
        </MDBCard>
      </div>
    </div>
  </div>
</template>

<style scoped>
.gap-2 {
  gap: 0.5rem;
}

.gap-3 {
  gap: 1rem;
}

.d-grid {
  display: grid;
}

@media (max-width: 576px) {
  .d-flex.gap-3 {
    flex-direction: column;
  }
}
</style>