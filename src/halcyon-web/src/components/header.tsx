'use client';

import Link from 'next/link';
import { Avatar, AvatarImage, AvatarFallback } from '@/components/ui/avatar';
import { ModeToggle } from '@/components/mode-toggle';
import { Button } from '@/components/ui/button';
import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from '@/components/ui/dropdown-menu';

export function Header() {
    const onLogout = () => {
        console.log('logging out');
    };

    return (
        <header className="border-b">
            <nav className="flex items-center justify-between gap-2 px-6 py-2 mx-auto max-w-screen-sm">
                <Link
                    href="/"
                    className="scroll-m-20 text-2xl font-semibold tracking-tight"
                >
                    Halcyon
                </Link>
                <Link href="/" className="ml-auto text-sm">
                    Home
                </Link>

                <Link href="/user" className='text-sm'>Users</Link>

                <Button asChild variant="secondary">
                    <Link href="/account/register">Register</Link>
                </Button>

                <Button asChild variant="secondary">
                    <Link href="/account/login">Login</Link>
                </Button>

                <ModeToggle />

                <DropdownMenu>
                    <DropdownMenuTrigger asChild>
                        <Button
                            variant="outline"
                            className="h-auto w-auto rounded-full p-1"
                        >
                            <Avatar>
                                <AvatarImage
                                    src="https://github.com/shadcn.png"
                                    alt="@shadcn"
                                />
                                <AvatarFallback>CN</AvatarFallback>
                                <span className="sr-only">
                                    Toggle profile menu
                                </span>
                            </Avatar>
                        </Button>
                    </DropdownMenuTrigger>
                    <DropdownMenuContent align="end">
                        <DropdownMenuItem asChild>
                            <Link href="/profile">My Profile</Link>
                        </DropdownMenuItem>
                        <DropdownMenuSeparator />
                        <DropdownMenuItem onClick={onLogout}>
                            Log out
                        </DropdownMenuItem>
                    </DropdownMenuContent>
                </DropdownMenu>
            </nav>
        </header>
    );
}
