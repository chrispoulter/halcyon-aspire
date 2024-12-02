import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import { env } from 'process';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    server: {
        port: env.PORT ? parseInt(env.PORT) : undefined,
    }
})
