import { useEffect, useState } from 'react';
import { ThemeProvider } from '@/components/theme-provider';
import { ModeToggle } from '@/components/mode-toggle';
import { Button } from '@/components/button';

function App() {
    const [count, setCount] = useState(0);

    const [result, setResult] = useState<string | null>(null);

    useEffect(() => {
        fetch('/api/health')
            .then(res => res.text())
            .then(data => setResult(data));
    }, []);

    return (
        <ThemeProvider>
            <main className="mx-auto mb-8 max-w-screen-md border-b border-gray-200 px-4 pb-8 dark:border-gray-700">
                <h1 className="mb-5 text-2xl font-bold dark:text-white">
                    Vite + React <ModeToggle />
                </h1>
                <p className="mb-3 text-gray-500 dark:text-gray-400">
                    <Button onClick={() => setCount(count => count + 1)}>
                        count is {count}
                    </Button>
                </p>

                <h2 className="mb-3 text-xl font-bold dark:text-white">
                    Api {import.meta.env.VITE_API_URL}
                </h2>
                <p className="mb-3 text-gray-500 dark:text-gray-400">
                    {result}
                </p>
            </main>
        </ThemeProvider>
    );
}

export default App;
