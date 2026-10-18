import type { ReactNode } from 'react';
import { UserInfo } from '../UserInfo/UserInfo';
import styles from './AppShell.module.css';

type AppShellProps = {
    children: ReactNode;
};

export default function AppShell({ children }: AppShellProps) {
    return (
        <div className={styles.container}>
            <div className={styles.backgroundFull} />
            <header className={styles.header}>
                <div style={{ display: 'flex', alignItems: 'center', gap: 12 }}>
                    <div className={styles.brand}>
                        <img className={styles.brandIcon} src="/favicon.svg" alt="" />
                        Pen and Paper
                    </div>
                </div>
                <div className={styles.headerRight}>
                    <UserInfo />
                </div>
            </header>

            <main className={styles.contentArea}>{children}</main>
        </div>
    );
}
