// Use Railway URL in production, local development server in development
const isProd = window.location.hostname === 'cong-ha.github.io';
export const API_BASE_URL = isProd 
    ? 'https://mastacosproject-production.up.railway.app/api'
    : 'http://localhost:8080/api';
