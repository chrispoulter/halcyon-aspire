'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';

const actionSchema = z.object({
    id: z
        .string({ message: 'Id is a required field' })
        .min(1, 'Id is a required field')
        .uuid('Id must be a valid UUID'),
    version: z.string({ message: 'Version must be a string' }),
});

export async function lockUserAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('lockUserAction', async (span) => {
            try {
                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const { id, ...rest } = request.data;

                const response = await fetch(
                    `${process.env.services__api__https__0}/user/${id}/lock`,
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
