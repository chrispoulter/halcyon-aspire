import type { Metadata } from 'next';
import { Open_Sans } from 'next/font/google';
import { ThemeProvider } from '@/components/theme-provider';
import { Header } from '@/components/header';
import { Footer } from '@/components/footer';
import { Toaster } from '@/components/ui/toaster';
import { cn } from '@/lib/utils';

import './globals.css';

const openSans = Open_Sans({
    variable: '--font-open-sans',
    subsets: ['latin'],
});

export const metadata: Metadata = {
    title: 'Halcyon',
    description:
        'A Next.js web project template. Built with a sense of peace and tranquillity.',
    keywords: [
        'nextjs',
        'react',
        'typescript',
        'app-router',
        'zod',
        'react-hook-form',
        'tailwindcss',
        'shadcn-ui',
        'docker',
        'eslint',
        'prettier',
    ],
};

export default function RootLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    return (
        <html lang="en" suppressHydrationWarning>
            <body
                className={cn(
                    'min-h-screen bg-background font-sans antialiased',
                    openSans.variable
                )}
            >
                <ThemeProvider
                    attribute="class"
                    defaultTheme="system"
                    enableSystem
                    disableTransitionOnChange
                >
                    <Header />
                    <main>{children}</main>
                    <Footer />
                    <Toaster />
                </ThemeProvider>
            </body>
        </html>
    );
}
