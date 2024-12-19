import { Metadata } from 'next';
import { LoginForm } from '@/app/account/login/login-form';

export const metadata: Metadata = {
    title: 'Login',
};

export default async function Login() {
    return (
        <main className="mx-auto max-w-screen-sm p-6 md:p-10">
            <LoginForm />
        </main>
    );
}
