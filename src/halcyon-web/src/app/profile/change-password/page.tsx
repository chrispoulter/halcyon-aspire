import { Metadata } from 'next';
import Link from 'next/link';
import { getProfileAction } from '@/app/actions/getProfileAction';
import { ChangePasswordForm } from '@/app/profile/change-password/change-password-form';

export const metadata: Metadata = {
    title: 'Change Password',
};

export default async function ChangePassword() {
    const profile = await getProfileAction();

    return (
        <main className="mx-auto max-w-screen-sm p-6">
            <h1 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Change Password
            </h1>

            <p className="mt-6 leading-7">
                Change your password below. Choose a strong password and
                don&apos;t reuse it for other accounts. For security reasons,
                change your password on a regular basis.
            </p>

            <ChangePasswordForm className="mt-6" />

            <p className="mt-6 leading-7">
                Forgotten your password?{' '}
                <Link
                    href="/account/forgot-password"
                    className="font-medium text-primary underline underline-offset-4"
                >
                    Request reset
                </Link>
            </p>
        </main>
    );
}
