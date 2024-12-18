import { createContext, useContext, useEffect, useState } from 'react';

const STORAGE_KEY = 'color-theme';

type Theme = 'dark' | 'light';

type ThemeProviderProps = {
    children: React.ReactNode;
};

type ThemeProviderState = {
    theme: Theme;
    toggleTheme: () => void;
};

const initialState: ThemeProviderState = {
    theme: 'dark',
    toggleTheme: () => null
};

const ThemeProviderContext = createContext<ThemeProviderState>(initialState);

export function ThemeProvider({ children, ...props }: ThemeProviderProps) {
    const prefersColorScheme: Theme = window.matchMedia(
        '(prefers-color-scheme: dark)'
    ).matches
        ? 'dark'
        : 'light';

    const localColorScheme = localStorage.getItem(STORAGE_KEY) as
        | Theme
        | undefined;

    const [theme, setTheme] = useState<Theme>(
        () => localColorScheme || prefersColorScheme
    );

    useEffect(() => {
        if (theme === 'dark') {
            window.document.documentElement.classList.add('dark');
        } else {
            window.document.documentElement.classList.remove('dark');
        }
    }, [theme]);

    const value = {
        theme,
        toggleTheme: () => {
            const newTheme = theme === 'dark' ? 'light' : 'dark';
            localStorage.setItem(STORAGE_KEY, newTheme);
            setTheme(newTheme);
        }
    };

    return (
        <ThemeProviderContext {...props} value={value}>
            {children}
        </ThemeProviderContext>
    );
}



export const useTheme = () => {
    const context = useContext(ThemeProviderContext);

    if (context === undefined) {
        throw new Error('useTheme must be used within a ThemeProvider');
    }

    return context;
};
