'use server';

import { createSession } from '@/lib/session';
import { trace } from '@opentelemetry/api';
import { z } from 'zod';

const actionSchema = z.object({
    emailAddress: z
        .string({ message: 'Email Address must be a valid string' })
        .min(1, 'Email Address is a required field')
        .email('Email Address must be a valid email'),
    password: z
        .string({ message: 'Password must be a valid string' })
        .min(1, 'Password is a required field'),
});

export type LoginResponse = {
    accessToken: string;
};

export async function loginAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('loginAction', async (span) => {
            try {
                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const response = await fetch(
                    `${process.env.services__api__https__0}/account/login`,
                    {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
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

                const result = (await response.json()) as LoginResponse;

                await createSession(result.accessToken);

                return result;
            } finally {
                span.end();
            }
        });
}
