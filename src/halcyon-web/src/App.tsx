import { useEffect, useState } from 'react';

function App() {
    const [count, setCount] = useState(0);

    const [result, setResult] = useState<string | null>(null);

    useEffect(() => {
        fetch('/api/health')
            .then(res => res.text())
            .then(data => setResult(data));
    }, []);

    return (
        <main className="mx-auto mb-8 max-w-screen-md border-b border-gray-200 px-4 pb-8 dark:border-gray-700">
            <h1 className="mb-5 text-2xl font-bold dark:text-white">
                Vite + React
            </h1>
            <p className="mb-3 text-gray-500 dark:text-gray-400">
                <button
                    className="rounded-lg bg-blue-700 px-5 py-2.5 text-sm font-medium text-white hover:bg-blue-800 focus:outline-none focus:ring-4 focus:ring-blue-300 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800"
                    onClick={() => setCount(count => count + 1)}
                >
                    count is {count}
                </button>
            </p>

            <h2 className="mb-3 text-xl font-bold dark:text-white">
                Api {import.meta.env.VITE_API_URL}
            </h2>
            <p className="mb-3 text-gray-500 dark:text-gray-400">{result}</p>
        </main>
    );
}

export default App;
