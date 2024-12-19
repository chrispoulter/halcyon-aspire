'use client';

import Link from 'next/link';
import { LogOut, User } from 'lucide-react';
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
import { ModeToggle } from '@/components/mode-toggle';

export function Header() {
    const onLogout = () => {
        console.log('logging out');
    };

    return (
        <header className="mx-auto flex max-w-screen-sm items-center justify-between gap-2 p-6">
            <Link
                href="/"
                className="scroll-m-20 text-xl font-semibold tracking-tight"
            >
                Halcyon
            </Link>

            <nav className="flex items-center gap-2 leading-7">
                <Link
                    href="/"
                    className="font-medium text-primary underline underline-offset-4"
                >
                    Home
                </Link>
                <Link
                    href="/user"
                    className="font-medium text-primary underline underline-offset-4"
                >
                    Users
                </Link>
            </nav>

            <Button asChild variant="secondary" className="ml-auto">
                <Link href="/account/register">Register</Link>
            </Button>

            <Button asChild variant="secondary">
                <Link href="/account/login">Login</Link>
            </Button>

            <ModeToggle />

            <DropdownMenu>
                <DropdownMenuTrigger asChild>
                    <Avatar>
                        <AvatarImage
                            src="https://github.com/shadcn.png"
                            alt="System Administrator"
                        />
                        <AvatarFallback>CN</AvatarFallback>
                        <span className="sr-only">Toggle profile menu</span>
                    </Avatar>
                </DropdownMenuTrigger>
                <DropdownMenuContent className="w-56">
                    <DropdownMenuLabel className="flex flex-col gap-2">
                        <span className="truncate">System Adminstrator</span>
                        <span className="truncate text-sm text-muted-foreground">
                            system.administrator@example.com
                        </span>
                        <Badge variant="secondary" className="justify-center">
                            System Administrator
                        </Badge>
                        <Badge variant="secondary" className="justify-center">
                            User Administrator
                        </Badge>
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
        </header>
    );
}
