'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';
import { isInPast } from '@/lib/dates';

const actionSchema = z.object({
    id: z
        .string({ message: 'Id is a required field' })
        .min(1, 'Id is a required field')
        .uuid('Id must be a valid UUID'),
    emailAddress: z
        .string({ message: 'Email Address is a required field' })
        .min(1, 'Email Address is a required field')
        .max(254, 'Password must be no more than 254 characters')
        .email('Email Address must be a valid email'),
    firstName: z
        .string({
            message: 'Confirm Password is a required field',
        })
        .min(1, 'First Name is a required field')
        .max(50, 'First Name must be no more than 50 characters'),
    lastName: z
        .string({ message: 'Last Name is a required field' })
        .min(1, 'Last Name is a required field')
        .max(50, 'Last Name must be no more than 50 characters'),
    dateOfBirth: z
        .string({
            message: 'Date of Birth is a required field',
        })
        .min(1, 'Date Of Birth is a required field')
        .date('Date Of Birth must be a valid date')
        .refine(isInPast, { message: 'Date Of Birth must be in the past' }),
    version: z.string({ message: 'Version must be a string' }),
});

export async function updateUserAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('updateUserAction', async (span) => {
            try {
                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const { id, ...rest } = request.data;

                const response = await fetch(
                    `${process.env.services__api__https__0}/user/${id}`,
                    {
                        method: 'PUT',
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${process.env.API_TOKEN}`,
                        },
                        body: JSON.stringify(rest),
                    }
                );

                if (!response.ok) {
                    return {
                        errors: [
                            'An error occurred while processing your request',
                        ],
                    };
                }

                return await response.json();
            } finally {
                span.end();
            }
        });
}
