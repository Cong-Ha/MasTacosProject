import 'mdb-vue-ui-kit/css/mdb.min.css'
import '@fortawesome/fontawesome-free/css/all.min.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'
import { setupAxiosInterceptors } from '@/services/httpInterceptors';
import { useAuthStore } from '@/store/auth';

// Setup axios interceptors
setupAxiosInterceptors();

// Create the app and pinia
const app = createApp(App);
const pinia = createPinia();

app.use(pinia);
app.use(router);

// Initialize auth state after pinia is available
const authStore = useAuthStore();
authStore.initializeAuth().then(() => {
    app.mount('#app');
});