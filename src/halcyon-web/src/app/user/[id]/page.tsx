import { Metadata } from 'next';
import { getUserAction } from '@/app/actions/getUserAction';
import { UpdateUserForm } from '@/app/user/[id]/update-user-form';

export const metadata: Metadata = {
    title: 'Update User',
};

export default async function ResetPassword({
    params,
}: {
    params: Promise<{ id: string }>;
}) {
    const id = (await params).id;
    const user = await getUserAction({ id });

    return (
        <main className="mx-auto max-w-screen-sm p-6">
            <h1 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Update User
            </h1>

            <p className="mt-6 leading-7">
                Update the user&apos;s details below. The email address is used
                to login to the account.
            </p>

            <UpdateUserForm user={user} className="mt-6" />
        </main>
    );
}
