import 'server-only';

import { cache } from 'react';
import { cookies } from 'next/headers';
import { redirect, unauthorized } from 'next/navigation';
import { SessionPayload } from '@/lib/definitions';
import { decrypt } from '@/lib/session';

export const getSession = cache(async () => {
    const cookie = (await cookies()).get('session')?.value;
    return (await decrypt(cookie)) as SessionPayload | undefined;
});

export const verifySession = cache(async (roles?: string[]) => {
    const session = await getSession();

    if (!session?.accessToken) {
        redirect('/account/login?dal=1');
    }

    if (!roles) {
        return session;
    }

    if (!roles.some((value) => session.roles?.includes(value))) {
        return unauthorized();
    }

    return session;
});
