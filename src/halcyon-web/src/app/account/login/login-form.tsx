'use client';

import Link from 'next/link';
import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { loginAction } from '@/app/actions/loginAction';
import { toast } from '@/hooks/use-toast';
import {
    Card,
    CardHeader,
    CardTitle,
    CardDescription,
    CardContent,
} from '@/components/ui/card';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { Button } from '@/components/ui/button';

const formSchema = z.object({
    emailAddress: z
        .string({ message: 'Email Address is a required field' })
        .min(1, 'Email Address is a required field')
        .email('Email Address must be a valid email'),
    password: z
        .string({ message: 'Password is a required field' })
        .min(1, 'Password is a required field'),
});

type LoginFormProps = {
    className?: string;
};

export function LoginForm({ className }: LoginFormProps) {
    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            emailAddress: '',
            password: '',
        },
    });

    async function onSubmit(data: z.infer<typeof formSchema>) {
        const result = await loginAction(data);
        console.log('result', result);

        toast({
            title: 'You have been successfully logged in.',
            description: (
                <pre className="mt-2 w-[340px] rounded-md bg-slate-950 p-4">
                    <code className="text-white">
                        {JSON.stringify(result, null, 2)}
                    </code>
                </pre>
            ),
        });
    }

    return (
        <Card className={className}>
            <CardHeader>
                <CardTitle className="text-2xl">Login</CardTitle>
                <CardDescription>
                    Enter your email below to login to your account.
                </CardDescription>
            </CardHeader>
            <CardContent>
                <Form {...form}>
                    <form
                        noValidate
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-6"
                    >
                        <FormField
                            control={form.control}
                            name="emailAddress"
                            render={({ field }) => (
                                <>
                                    <FormItem>
                                        <FormLabel>Email Address</FormLabel>
                                        <FormControl>
                                            <Input type="email" {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </>
                            )}
                        />
                        <FormField
                            control={form.control}
                            name="password"
                            render={({ field }) => (
                                <>
                                    <FormItem>
                                        <div className="flex items-center">
                                            <FormLabel>Password</FormLabel>
                                            <Link
                                                href="/account/forgot-password"
                                                className="ml-auto inline-block text-sm underline-offset-4 hover:underline"
                                            >
                                                Forgot your password?
                                            </Link>
                                        </div>
                                        <FormControl>
                                            <Input type="password" {...field} />
                                        </FormControl>
                                        <FormMessage />
                                    </FormItem>
                                </>
                            )}
                        />
                        <Button type="submit" className="w-full">
                            Login
                        </Button>
                    </form>
                </Form>
                <div className="mt-4 text-center text-sm">
                    Don&apos;t have an account?{' '}
                    <Link
                        href="/account/register"
                        className="underline underline-offset-4"
                    >
                        Sign up
                    </Link>
                </div>
            </CardContent>
        </Card>
    );
}
