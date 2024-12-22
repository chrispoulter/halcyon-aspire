import 'server-only';

import { cache } from 'react';
import { cookies } from 'next/headers';
import { redirect, unauthorized } from 'next/navigation';
import { decrypt } from '@/lib/session';

export const verifySession = cache(async (roles?: string[]) => {
    const cookie = (await cookies()).get('session')?.value;
    const session = await decrypt(cookie);

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
