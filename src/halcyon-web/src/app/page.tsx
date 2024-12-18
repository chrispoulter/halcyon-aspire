import { trace } from '@opentelemetry/api';
import { ModeToggle } from '@/components/mode-toggle';
import { ForgotPasswordForm } from '@/app/forgot-password-form';
import { LoginForm } from './login-form';

export const dynamic = 'force-dynamic';

async function getApiHealth() {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('getApiHealth', async (span) => {
            try {
                const response = await fetch(
                    `${process.env.services__api__https__0}/health`
                );
                return await response.text();
            } finally {
                span.end();
            }
        });
}

export default async function Home() {
    const health = await getApiHealth();

    return (
        <div className="flex min-h-svh w-full items-center justify-center p-6 md:p-10">
            <div className="w-full max-w-sm">
                <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
                    Taxing Laughter: The Joke Tax Chronicles {health}
                </h1>
                <p className="leading-7 [&:not(:first-child)]:mt-6">
                    The king, seeing how much happier his subjects were,
                    realized the error of his ways and repealed the joke tax.
                </p>
                <p className="leading-7 [&:not(:first-child)]:mt-6">
                    <ModeToggle />
                </p>

                <ForgotPasswordForm className="[&:not(:first-child)]:mt-6" />
                <LoginForm className="[&:not(:first-child)]:mt-6" />
            </div>
        </div>
    );
}
