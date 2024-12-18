import { ModeToggle } from '@/components/mode-toggle';
import { Button } from '@/components/ui/button';
import { trace } from '@opentelemetry/api';

export const dynamic = 'force-dynamic';

async function fetchApiHealth() {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('fetchApiHealth', async (span) => {
            try {
                const response = await fetch(
                    `${process.env.services__api__http__0}/health`
                );
                return await response.text();
            } finally {
                span.end();
            }
        });
}

export default async function Home() {
    const healthy = await fetchApiHealth();

    return (
        <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
            <div className="w-full max-w-sm">
                <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
                    Taxing Laughter: The Joke Tax Chronicles {healthy}
                </h1>
                <p className="leading-7 [&:not(:first-child)]:mt-6">
                    The king, seeing how much happier his subjects were,
                    realized the error of his ways and repealed the joke tax.
                </p>
                <p className="leading-7 [&:not(:first-child)]:mt-6">
                    <Button>Click me</Button>
                </p>
                <p className="leading-7 [&:not(:first-child)]:mt-6">
                    <ModeToggle />
                </p>
            </div>
        </div>
    );
}
