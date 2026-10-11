import { useAuthenticationContext } from '../authentication/useAuthenticationContext';

export function UserInfo() {
    const { user, logout } = useAuthenticationContext();

    return (
        <div className="user-info">
            {user.picture && <img src={user.picture} alt="" width={32} height={32} referrerPolicy="no-referrer" />}
            <span>{user.name ?? user.email}</span>
            <button type="button" onClick={logout}>Logout</button>
        </div>
    );
}
