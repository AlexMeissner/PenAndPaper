import type { ReactNode } from 'react';
import { UserInfo } from '../UserInfo/UserInfo';
import styles from './HomeTemplate.module.css';

type HomeTemplateProperties = {
    children: ReactNode;
};

export default function HomeTemplate({ children }: HomeTemplateProperties) {
    return (
        <div className={styles.content}>            
            <header className={styles.header}>
                <div className={styles.brand}>
                        <img className={styles.brandIcon} src="/favicon.svg" alt="" />
                        Pen and Paper
                    </div>
                <UserInfo />
            </header>

            <main className={styles.contentArea}>{children}</main>
        </div>
    );
}
