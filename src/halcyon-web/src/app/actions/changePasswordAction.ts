'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';

const actionSchema = z.object({
    currentPassword: z
        .string({ message: 'Current Password is a required field' })
        .min(1, 'Current Password is a required field'),
    newPassword: z
        .string({ message: 'New Password is a required field' })
        .min(8, 'New Password must be at least 8 characters')
        .max(50, 'New Password must be no more than 50 characters'),
});

export async function changePasswordAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('changePasswordAction', async (span) => {
            try {
                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const response = await fetch(
                    `${process.env.services__api__https__0}/profile/change-password`,
                    {
                        method: 'PUT',
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
