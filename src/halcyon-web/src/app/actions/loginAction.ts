'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';

const schema = z.object({
    emailAddress: z
        .string({ message: 'Email Address is a required field' })
        .min(1, 'Email Address is a required field')
        .email('Email Address must be a valid email'),
    password: z
        .string({ message: 'Password is a required field' })
        .min(1, 'Password is a required field'),
});

export async function loginAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('forgotPasswordAction', async (span) => {
            try {
                const request = schema.safeParse(data);

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

                return await response.json();
            } finally {
                span.end();
            }
        });
}
