import { createContext } from 'react';

export interface User {
    name: string | null;
    email: string | null;
    picture: string | null;
}

export interface AuthContextValue {
    user: User;
    logout: () => Promise<void>;
}

export const AuthContext = createContext<AuthContextValue | undefined>(undefined);
