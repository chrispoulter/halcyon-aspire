import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    return {
        plugins: [react()],
        define: {
            'import.meta.env.npm_package_version': JSON.stringify(
                env.npm_package_version
            ),
        },
        resolve: {
            alias: {
                '@': path.resolve(__dirname, './src'),
            },
        },
        server: {
            watch: {
                usePolling: env.USE_POLLING === 'true',
            },
        },
    };
});
