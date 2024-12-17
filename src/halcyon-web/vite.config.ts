import { defineConfig, loadEnv } from 'vite';
import react from '@vitejs/plugin-react';
import path from 'path';

// https://vitejs.dev/config/
export default defineConfig(({ mode }) => {
    const env = loadEnv(mode, process.cwd(), '');

    return {
        plugins: [react()],
        resolve: {
            alias: {
                '@': path.resolve(__dirname, './src')
            }
        },
        server: {
            port: parseInt(env.VITE_PORT),
            proxy: {
                '/api': {
                    target:
                        process.env.services__api__https__0 ||
                        process.env.services__api__http__0 ||
                        'http://localhost:5257',
                    changeOrigin: true,
                    rewrite: path => path.replace(/^\/api/, ''),
                    secure: false
                }
            }
        },
        build: {
            outDir: 'dist',
            rollupOptions: {
                input: './index.html'
            }
        }
    };
});
