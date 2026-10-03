import { useCallback, useEffect, useMemo, useState, type ReactNode } from 'react';
import { AuthContext, type User } from './AuthContext';
import { redirectToLogin } from './api';

type AuthState =
    | { status: 'loading' }
    | { status: 'authenticated'; user: User }
    | { status: 'redirecting' }
    | { status: 'error'; message: string };

export function AuthProvider({ children }: { children: ReactNode }) {
    const [state, setState] = useState<AuthState>({ status: 'loading' });

    useEffect(() => {
        let cancelled = false;

        (async () => {
            try {
                const response = await fetch('/account/me', { credentials: 'same-origin' });
                if (cancelled) {
                    return;
                }

                if (response.ok) {
                    setState({ status: 'authenticated', user: await response.json() });
                } else if (response.status === 401) {
                    setState({ status: 'redirecting' });
                    redirectToLogin();
                } else {
                    setState({ status: 'error', message: `Unexpected response (${response.status}).` });
                }
            } catch {
                if (!cancelled) {
                    setState({ status: 'error', message: 'The server is not reachable.' });
                }
            }
        })();

        return () => {
            cancelled = true;
        };
    }, []);

    const logout = useCallback(async () => {
        await fetch('/account/logout', {
            method: 'POST',
            headers: { 'X-CSRF': '1' },
            credentials: 'same-origin'
        });
        setState({ status: 'redirecting' });
        window.location.assign('/');
    }, []);

    const value = useMemo(
        () => (state.status === 'authenticated' ? { user: state.user, logout } : undefined),
        [state, logout]);

    if (state.status === 'error') {
        return <p>Authentication failed: {state.message}</p>;
    }

    if (value === undefined) {
        return <p><em>Signing in...</em></p>;
    }

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}
