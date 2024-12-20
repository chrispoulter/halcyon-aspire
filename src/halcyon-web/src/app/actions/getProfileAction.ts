'use server';

import { trace } from '@opentelemetry/api';

export async function getProfileAction() {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('getProfileAction', async (span) => {
            try {
                const response = await fetch(
                    `${process.env.services__api__https__0}/profile`,
                    {
                        headers: {
                            Authorization: `Bearer ${process.env.API_TOKEN}`,
                        },
                    }
                );
                return await response.json();
            } finally {
                span.end();
            }
        });
}
