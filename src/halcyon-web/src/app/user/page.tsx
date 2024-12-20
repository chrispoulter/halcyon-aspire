import { Metadata } from 'next';
import Link from 'next/link';
import { AlertCircle } from 'lucide-react';
import { searchUsersAction } from '@/app/actions/searchUsersAction';
import { Alert, AlertTitle, AlertDescription } from '@/components/ui/alert';

export const metadata: Metadata = {
    title: 'Users',
};

export default async function ChangePassword({}) {
    const result = await searchUsersAction({
        page: 1,
        size: 10,
        sort: 'NAME_ASC',
        search: '',
    });

    if ('errors' in result) {
        return (
            <main className="mx-auto max-w-screen-sm p-6">
                <Alert variant="destructive">
                    <AlertCircle className="h-4 w-4" />
                    <AlertTitle>Error</AlertTitle>
                    <AlertDescription>
                        {JSON.stringify(result.errors)}
                    </AlertDescription>
                </Alert>
            </main>
        );
    }

    return (
        <main className="mx-auto max-w-screen-sm p-6">
            <h1 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Users
            </h1>

            {result.items.map((user) => (
                <Link
                    key={user.id}
                    href={`/user/${user.id}`}
                    className="mt-6 block border p-6"
                >
                    <span className="block font-semibold leading-7">
                        {user.firstName} {user.lastName}
                    </span>
                    <span className="block leading-7">{user.emailAddress}</span>
                </Link>
            ))}
        </main>
    );
}
