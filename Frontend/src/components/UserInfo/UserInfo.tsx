import { useAuthenticationContext } from '../../authentication/useAuthenticationContext';
import styles from './UserInfo.module.css';

export function UserInfo() {
    const { user } = useAuthenticationContext();

    return (
        <div className={styles.userInfo}>
            {user.picture && <img src={user.picture} alt="" width={32} height={32} referrerPolicy="no-referrer" />}
            <span>{user.name ?? user.email}</span>
        </div>
    );
}
