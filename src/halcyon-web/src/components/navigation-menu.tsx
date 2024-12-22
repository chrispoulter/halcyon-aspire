import Link from 'next/link';
import { SessionPayload } from '@/lib/definitions';
import { Role } from '@/lib/auth';

type NavigationMenuProps = {
    session: SessionPayload;
};

export function NavigationMenu({ session }: NavigationMenuProps) {
    return (
        <nav className="flex items-center gap-2 leading-7">
            <Link
                href="/"
                className="font-medium text-primary underline underline-offset-4"
            >
                Home
            </Link>
            {session &&
                [Role.SYSTEM_ADMINISTRATOR, Role.USER_ADMINISTRATOR].some(
                    (value) => session.roles?.includes(value)
                ) && (
                    <Link
                        href="/user"
                        className="font-medium text-primary underline underline-offset-4"
                    >
                        Users
                    </Link>
                )}
        </nav>
    );
}
