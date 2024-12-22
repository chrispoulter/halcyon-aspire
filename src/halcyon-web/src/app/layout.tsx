import type { Metadata, Viewport } from 'next';
import { ThemeProvider } from '@/components/theme-provider';
import { Header } from '@/components/header';
import { Footer } from '@/components/footer';
import { Toaster } from '@/components/ui/toaster';

import './globals.css';

export const metadata: Metadata = {
    title: {
        template: '%s // Halcyon',
        default: 'Halcyon',
    },
    description:
        'A Next.js web project template. Built with a sense of peace and tranquillity.',
    keywords: [
        'nextjs',
        'react',
        'typescript',
        'app-router',
        'shadcn-ui',
        'zod',
        'react-hook-form',
        'tailwindcss',
        'docker',
        'eslint',
        'prettier',
    ],
    applicationName: 'Halcyon',
    appleWebApp: {
        title: 'Halcyon',
    },
    formatDetection: {
        telephone: false,
    },
};

export const viewport: Viewport = {
    themeColor: '#111827',
};

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body className="min-h-screen bg-background font-sans antialiased">
                <ThemeProvider
                    attribute="class"
                    defaultTheme="system"
                    enableSystem
                    disableTransitionOnChange
                >
                    <Header />
                    {children}
                    <Footer />
                    <Toaster />
                </ThemeProvider>
            </body>
        </html>
    );
}
