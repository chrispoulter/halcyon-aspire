'use server';

import { trace } from '@opentelemetry/api';

export async function getApiHealthAction() {
    return await trace
        .getTracer('halcyon-web')
        .startActiveSpan('getApiHealth', async (span) => {
            try {
                const response = await fetch(
                    `${process.env.services__api__https__0}/health`
                );
                return await response.text();
            } finally {
                span.end();
            }
        });
}
