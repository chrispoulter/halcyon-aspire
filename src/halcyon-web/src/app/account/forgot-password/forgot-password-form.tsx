'use client';

import { zodResolver } from '@hookform/resolvers/zod';
import { useForm } from 'react-hook-form';
import { z } from 'zod';
import { forgotPasswordAction } from '@/app/actions/forgotPasswordAction';
import { toast } from '@/hooks/use-toast';
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
import { cn } from '@/lib/utils';

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
        });
    }

    return (
        <Form {...form}>
            <form
                noValidate
                onSubmit={form.handleSubmit(onSubmit)}
                className={cn('space-y-6', className)}
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
                    Submit
                </Button>
            </form>
        </Form>
    );
}
