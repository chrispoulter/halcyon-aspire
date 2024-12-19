import { ForgotPasswordForm } from '@/app/account/forgot-password/forgot-password-form';



export default async function ForgotPassword() {
    return (
        <div className="mx-auto max-w-screen-sm p-6 md:p-10">
            <ForgotPasswordForm />
        </div>
    );
}
