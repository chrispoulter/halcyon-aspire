'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';

const actionSchema = z.object({
    version: z.string({ message: 'Version must be a valid string' }).optional(),
});

export async function deleteAccountAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('deleteAccountAction', async (span) => {
            try {
                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const response = await fetch(
                    `${process.env.services__api__https__0}/profile`,
                    {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json',
                            Authorization: `Bearer ${process.env.API_TOKEN}`,
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
