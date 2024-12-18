'use client';

import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { toast } from '@/hooks/use-toast';
import { Button } from '@/components/ui/button';
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from '@/components/ui/form';
import { Input } from '@/components/ui/input';
import { forgotPasswordAction } from './actions/forgotPasswordAction';
import {
    Card,
    CardHeader,
    CardTitle,
    CardDescription,
    CardContent,
} from '@/components/ui/card';

const formSchema = z.object({
    emailAddress: z
        .string({ message: 'Email Address is a required field' })
        .min(1, 'Email Address is a required field')
        .email('Email Address must be a valid email'),
});

type ForgotPasswordFormProps = {
    className?: string;
};

export function ForgotPasswordForm({ className }: ForgotPasswordFormProps) {
    const form = useForm<z.infer<typeof formSchema>>({
        resolver: zodResolver(formSchema),
        defaultValues: {
            emailAddress: '',
        },
    });

    async function onSubmit(data: z.infer<typeof formSchema>) {
        const result = await forgotPasswordAction(data);
        console.log('result', result);

        toast({
            title: 'Instructions as to how to reset your password have been sent to you via email.',
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
                <CardTitle className="text-2xl">Forgot Password</CardTitle>
                <CardDescription>
                    Request a password reset link by providing your email
                    address.
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
                        <Button type="submit" className="w-full">
                            Request Reset
                        </Button>
                    </form>
                </Form>
            </CardContent>
        </Card>
    );
}
