import { LoginForm } from '@/app/account/login/login-form';

export default async function Login() {
    return (
        <div className="mx-auto max-w-screen-sm p-6 md:p-10">
            <LoginForm />
        </div>
    );
}
