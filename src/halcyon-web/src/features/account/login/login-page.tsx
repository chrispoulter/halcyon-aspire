import { Link, useNavigate } from 'react-router';
import { useAuth } from '@/components/auth-provider';
import { Metadata } from '@/components/metadata';
import { useLogin } from '@/features/account/hooks/use-login';
import {
    LoginForm,
    LoginFormValues,
} from '@/features/account/login/login-form';
import { toast } from '@/hooks/use-toast';

export function LoginPage() {
    const navigate = useNavigate();

    const { setAuth } = useAuth();

    const { mutate: login, isPending: isSaving } = useLogin();

    function onSubmit(data: LoginFormValues) {
        login(data, {
            onSuccess: (data) => {
                setAuth(data.accessToken);
                navigate('/');
            },
            onError: (error) => {
                toast({
                    variant: 'destructive',
                    title: 'Error',
                    description: error.message,
                });
            },
        });
    }
    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="Login" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
                Login
            </h1>

            <p className="leading-7">
                Enter your email address below to login to your account.
            </p>

            <LoginForm loading={isSaving} onSubmit={onSubmit} />

            <div className="space-y-2">
                <p className="text-sm text-muted-foreground">
                    Not already a member?{' '}
                    <Link
                        to="/account/register"
                        className="underline underline-offset-4"
                    >
                        Register now
                    </Link>
                </p>
                <p className="text-sm text-muted-foreground">
                    Forgotten your password?{' '}
                    <Link
                        to="/account/forgot-password"
                        className="underline underline-offset-4"
                    >
                        Request reset
                    </Link>
                </p>
            </div>
        </main>
    );
}
