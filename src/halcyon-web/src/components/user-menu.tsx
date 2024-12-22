'use client';

import Link from 'next/link';
import { User, LogOut } from 'lucide-react';
import { Avatar, AvatarImage, AvatarFallback } from '@/components/ui/avatar';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';
import { SessionPayload } from '@/lib/definitions';

type UserMenu = {
    session: SessionPayload;
    onLogout: () => void;
};

export function UserMenu({ session, onLogout }: UserMenu) {
    if (!session) {
        return (
            <nav className="flex items-center gap-2">
                <Button asChild variant="secondary">
                    <Link href="/account/register">Register</Link>
                </Button>

                <Button asChild variant="secondary">
                    <Link href="/account/login">Login</Link>
                </Button>
            </nav>
        );
    }

    return (
        <DropdownMenu>
            <DropdownMenuTrigger asChild>
                <Avatar>
                    <AvatarImage
                        src="https://github.com/shadcn.png"
                        alt={`${session.firstName} ${session.lastName}`}
                    />
                    <AvatarFallback>{`${session.firstName[0]} ${session.lastName[0]}`}</AvatarFallback>
                    <span className="sr-only">Toggle profile menu</span>
                </Avatar>
            </DropdownMenuTrigger>
            <DropdownMenuContent className="w-56">
                <DropdownMenuLabel className="flex flex-col gap-2">
                    <span className="truncate">System Adminstrator</span>
                    <span className="truncate text-sm text-muted-foreground">
                        {session.emailAddress}
                    </span>
                    {(session.roles ?? []).map((role) => (
                        <Badge
                            key={role}
                            variant="secondary"
                            className="justify-center"
                        >
                            {role}
                        </Badge>
                    ))}
                </DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuItem asChild>
                    <Link href="/profile">
                        <User />
                        <span>Profile</span>
                    </Link>
                </DropdownMenuItem>
                <DropdownMenuSeparator />
                <DropdownMenuItem onClick={onLogout}>
                    <LogOut />
                    <span>Log out</span>
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    );
}
