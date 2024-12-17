import { Moon, Sun } from 'lucide-react';
import { useTheme } from '@/components/theme-provider';
import { cn } from '@/lib/utils';

type ModeToggleProps = {
    className?: string;
};

export function ModeToggle({ className }: ModeToggleProps) {
    const { theme, toggleTheme } = useTheme();

    return (
        <button
            type="button"
            aria-label="Toggle dark mode"
            className={cn(
                'rounded-lg p-2.5 text-sm text-gray-500 hover:bg-gray-100 focus:outline-none focus:ring-4 focus:ring-gray-200 dark:text-gray-400 dark:hover:bg-gray-700 dark:focus:ring-gray-700',
                className
            )}
            onClick={toggleTheme}
        >
            {theme === 'dark' ? (
                <Sun aria-label="Currently light mode" />
            ) : (
                <Moon aria-label="Currently dark mode" />
            )}
        </button>
    );
}
