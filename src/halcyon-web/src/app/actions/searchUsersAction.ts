'use server';

import { trace } from '@opentelemetry/api';
import { z } from 'zod';

enum UserSort {
    EMAIL_ADDRESS_ASC = 'EMAIL_ADDRESS_ASC',
    EMAIL_ADDRESS_DESC = 'EMAIL_ADDRESS_DESC',
    NAME_ASC = 'NAME_ASC',
    NAME_DESC = 'NAME_DESC',
}

const actionSchema = z.object({
    search: z.string({ message: 'Search must be a valid string' }).optional(),
    page: z
        .number({ message: 'Page must be a valid number' })
        .min(1, 'Page must be greater than zero')
        .optional(),
    size: z
        .number({ message: 'Size must be a valid number' })
        .min(1, 'Size must be greater than zero')
        .max(50, 'Size must be less than 50')
        .optional(),
    sort: z
        .nativeEnum(UserSort, { message: 'Sort must be a valid string' })
        .optional(),
});

export async function searchUsersAction(data: unknown) {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('searchUsersAction', async (span) => {
            try {
                const request = actionSchema.safeParse(data);

                if (!request.success) {
                    return {
                        errors: request.error.flatten().fieldErrors,
                    };
                }

                const params = Object.entries(request)
                    .map((pair) => pair.map(encodeURIComponent).join('='))
                    .join('&');

                const response = await fetch(
                    `${process.env.services__api__https__0}/user?${params}`,
                    {
                        method: 'GET',
                        headers: {
                            Authorization: `Bearer ${process.env.API_TOKEN}`,
                        },
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
