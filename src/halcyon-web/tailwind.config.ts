import type { Config } from 'tailwindcss';
import forms from '@tailwindcss/forms';

const config: Config = {
    darkMode: 'class',
    content: ['./index.html', './src/**/*.{js,ts,jsx,tsx}'],
    theme: {
        extend: {}
    },
    plugins: [forms]
};

export default config;
