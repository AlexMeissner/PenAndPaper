import { useContext } from 'react';
import { AuthenticationContext, type AuthenticationContextValue } from './AuthenticationContext';

export function useAuthenticationContext(): AuthenticationContextValue {
    const context = useContext(AuthenticationContext);
    if (context === undefined) {
        throw new Error('useAuthenticationContext must be used within an AuthenticationProvider.');
    }
    return context;
}
