import 'mdb-vue-ui-kit/css/mdb.min.css'
import '@fortawesome/fontawesome-free/css/all.min.css'
import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import { createPinia } from 'pinia'

const app = createApp(App).use(createPinia())
app.use(router)
app.mount('#app')
