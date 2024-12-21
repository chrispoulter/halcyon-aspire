'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';
import { Role } from '@/lib/auth';
import { verifySession } from '@/lib/dal';
import { isInPast } from '@/lib/dates';

const actionSchema = z.object({
    id: z
        .string({ message: 'Id must be a valid string' })
        .min(1, 'Id is a required field')
        .uuid('Id must be a valid UUID'),
    emailAddress: z
        .string({ message: 'Email Address must be a valid string' })
        .min(1, 'Email Address is a required field')
        .max(254, 'Password must be no more than 254 characters')
        .email('Email Address must be a valid email'),
    password: z
        .string({ message: 'Password must be a valid string' })
        .min(8, 'Password must be at least 8 characters')
        .max(50, 'Password must be no more than 50 characters'),
    firstName: z
        .string({
            message: 'Confirm Password is a required field',
        })
        .min(1, 'First Name is a required field')
        .max(50, 'First Name must be no more than 50 characters'),
    lastName: z
        .string({ message: 'Last Name must be a valid string' })
        .min(1, 'Last Name is a required field')
        .max(50, 'Last Name must be no more than 50 characters'),
    dateOfBirth: z
        .string({
            message: 'Date of Birth must be a valid string',
        })
        .min(1, 'Date Of Birth is a required field')
        .date('Date Of Birth must be a valid date')
        .refine(isInPast, { message: 'Date Of Birth must be in the past' }),
    roles: z
        .array(
            z.nativeEnum(Role, { message: 'Role must be a valid system role' }),
            { message: 'Role must be a valid array' }
        )
        .optional(),
    version: z.string({ message: 'Version must be a valid string' }).optional(),
});

export async function createUserAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('createUserAction', async (span) => {
            try {
                const session = await verifySession();

                if (!session) {
                    return {
                        errors: [
                            'Authenication is required to perform this action',
                        ],
                    };
                }

                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const response = await fetch(
                    `${process.env.services__api__https__0}/user`,
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${session.accessToken}`,
                        },
                        body: JSON.stringify(request.data),
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
