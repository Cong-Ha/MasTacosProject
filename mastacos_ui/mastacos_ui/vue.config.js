const { defineConfig } = require('@vue/cli-service');
const webpack = require('webpack');

module.exports = defineConfig({
    transpileDependencies: true,
    publicPath: process.env.MAUI === 'true'
        ? './'
        : (process.env.NODE_ENV === 'production' ? '/MasTacosProject' : '/'),
    devServer: {
        https: false,
        proxy: {
            '/api': {
                target: 'http://mastacos:80', // Use container name!
                changeOrigin: true
            }
        },
        host: '0.0.0.0', // Allow external access
        port: 8080,      // Vue runs on this inside container
        client: {
            webSocketURL: 'ws://localhost:8081/ws'
        }
    },
    configureWebpack: {
        plugins: [
            new webpack.DefinePlugin({
                __VUE_PROD_HYDRATION_MISMATCH_DETAILS__: 'false',
                __VUE_OPTIONS_API__: 'true',
                __VUE_PROD_DEVTOOLS__: 'false'
            })
        ]
    }
});