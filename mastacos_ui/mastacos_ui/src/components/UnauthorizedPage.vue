<script setup lang="ts">
import {
  MDBCard,
  MDBCardBody,
  MDBCardTitle,
  MDBCardText,
  MDBBtn,
  MDBIcon
} from "mdb-vue-ui-kit";
import { useRouter } from 'vue-router';
import { useAuth } from '@/store/auth';

const router = useRouter();
const { isAuthenticated, logout } = useAuth();

const goHome = (): void => {
  router.push('/');
};

const goToLogin = (): void => {
  router.push('/login');
};

const handleLogout = async (): Promise<void> => {
  await logout();
  router.push('/');
};
</script>

<template>
  <div class="d-flex justify-content-center align-items-center" style="min-height: 80vh;">
    <MDBCard style="max-width: 500px; width: 100%;" class="text-center">
      <MDBCardBody>
        <MDBIcon
            icon="exclamation-triangle"
            size="4x"
            class="text-warning mb-4">
        </MDBIcon>

        <MDBCardTitle class="mb-3">
          <h2>Access Denied</h2>
        </MDBCardTitle>

        <MDBCardText class="mb-4 text-muted">
          <template v-if="!isAuthenticated">
            You need to be logged in to access this page.
          </template>
          <template v-else>
            You don't have permission to access this page.
            Please contact an administrator if you believe this is an error.
          </template>
        </MDBCardText>

        <div class="d-flex gap-3 justify-content-center flex-wrap">
          <MDBBtn color="primary" @click="goHome">
            <MDBIcon icon="home" class="me-2" />
            Go Home
          </MDBBtn>

          <template v-if="!isAuthenticated">
            <MDBBtn color="success" @click="goToLogin">
              <MDBIcon icon="sign-in-alt" class="me-2" />
              Login
            </MDBBtn>
          </template>

          <template v-else>
            <MDBBtn color="secondary" outline @click="handleLogout">
              <MDBIcon icon="sign-out-alt" class="me-2" />
              Logout
            </MDBBtn>
          </template>
        </div>
      </MDBCardBody>
    </MDBCard>
  </div>
</template>

<style scoped>
.gap-3 {
  gap: 1rem;
}
</style>