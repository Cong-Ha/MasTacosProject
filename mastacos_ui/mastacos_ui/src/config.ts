interface ImportMetaEnv {
    readonly VITE_API_URL: string
    readonly PROD: boolean
}

interface ImportMeta {
    readonly env: ImportMetaEnv
}

// Use Railway URL in production, local development server in development
const isProd = window.location.hostname === 'cong-ha.github.io';
export const API_BASE_URL = isProd 
    ? 'https://cong-ha-mastacos--80.prod1a.defang.dev'
    : 'http://localhost:8080';
