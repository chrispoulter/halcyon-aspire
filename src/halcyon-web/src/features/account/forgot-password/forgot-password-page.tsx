import { useNavigate } from 'react-router';
import { Metadata } from '@/components/metadata';
import {
    ForgotPasswordForm,
    type ForgotPasswordFormValues,
} from '@/features/account/forgot-password/forgot-password-form';
import { useForgotPassword } from '@/features/account/hooks/use-forgot-password';
import { toast } from '@/hooks/use-toast';

export function ForgotPasswordPage() {
    const navigate = useNavigate();

    const { mutate: forgotPassword, isPending: isSaving } = useForgotPassword();

    function onSubmit(data: ForgotPasswordFormValues) {
        forgotPassword(data, {
            onSuccess: () => {
                toast({
                    title: 'Success',
                    description:
                        'Instructions as to how to reset your password have been sent to you via email.',
                });

                navigate('/account/login');
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
            <Metadata title="Forgot Password" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight lg:text-5xl">
                Forgot Password
            </h1>

            <p className="leading-7">
                Request a password reset link by providing your email address.
            </p>

            <ForgotPasswordForm loading={isSaving} onSubmit={onSubmit} />
        </main>
    );
}
