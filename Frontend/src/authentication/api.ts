export class UnauthorizedError extends Error {
    constructor() {
        super('Unauthorized');
        this.name = 'UnauthorizedError';
    }
}

export function redirectToLogin(): void {
    const returnUrl = window.location.href;
    window.location.assign(`/account/login?returnUrl=${encodeURIComponent(returnUrl)}`);
}

export async function apiFetch(input: string, init: RequestInit = {}): Promise<Response> {
    const headers = new Headers(init.headers);
    headers.set('X-CSRF', '1');

    const response = await fetch(input, {
        ...init,
        headers,
        credentials: 'same-origin'
    });

    if (response.status === 401) {
        redirectToLogin();
        throw new UnauthorizedError();
    }

    return response;
}
