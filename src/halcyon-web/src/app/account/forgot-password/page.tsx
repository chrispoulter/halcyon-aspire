import { Metadata } from 'next';
import { ForgotPasswordForm } from '@/app/account/forgot-password/forgot-password-form';

export const metadata: Metadata = {
    title: 'Forgot Password',
};

export default async function ForgotPassword() {
    return (
        <main className="mx-auto max-w-screen-sm p-6 md:p-10">
            <ForgotPasswordForm />
        </main>
    );
}
