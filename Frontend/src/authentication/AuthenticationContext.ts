import { createContext } from 'react';

export interface User {
    name: string | null;
    email: string | null;
    picture: string | null;
}

export interface AuthenticationContextValue {
    user: User;
    logout: () => Promise<void>;
}

export const AuthenticationContext = createContext<AuthenticationContextValue | undefined>(undefined);
