const { defineConfig } = require('@vue/cli-service');
module.exports = defineConfig({
    transpileDependencies: true,
    publicPath: process.env.NODE_ENV === 'production'
        ? '/MasTacosProject'
        : '/',
    devServer: {
        proxy: {
            '/api': {
                target: 'http://mastacos:80', // Use container name!
                changeOrigin: true
            }
        },
        host: '0.0.0.0', // Allow external access
        port: 8080,      // Vue runs on this inside container
    }
});
