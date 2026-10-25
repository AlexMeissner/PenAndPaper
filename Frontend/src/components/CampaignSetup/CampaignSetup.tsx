import { useEffect, useRef, useState, type ChangeEvent } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import theme from '../../Theme.module.css';
import HomeTemplate from '../HomeTemplate/HomeTemplate';
import PlayerList, { type Player } from './PlayerList/PlayerList';
import styles from './CampaignSetup.module.css';

type CampaignRule = {
    id: string;
    name: string;
};

const players: Player[] = [
    { id: 'alex-carter', name: 'Alex Carter', initials: 'AC', picture: 'https://i.pravatar.cc/80?img=3' },
    { id: 'morgan-reed', name: 'Morgan Reed', initials: 'MR', picture: 'https://i.pravatar.cc/80?img=12' },
    { id: 'taylor-smith', name: 'Taylor Smith', initials: 'TS', picture: 'https://i.pravatar.cc/80?img=5' },
    { id: 'dana-brooks', name: 'Dana Brooks', initials: 'DB', picture: 'https://i.pravatar.cc/80?img=10' },
    { id: 'taylor-kim', name: 'Taylor Kim', initials: 'TK', picture: 'https://i.pravatar.cc/80?img=8' },
    { id: 'jordan-ellis', name: 'Jordan Ellis', initials: 'JE', picture: 'https://i.pravatar.cc/80?img=6' },
    { id: 'casey-morgan', name: 'Casey Morgan', initials: 'CM', picture: 'https://i.pravatar.cc/80?img=4' },
    { id: 'riley-patel', name: 'Riley Patel', initials: 'RP', picture: 'https://i.pravatar.cc/80?img=9' },
    { id: 'avery-chen', name: 'Avery Chen', initials: 'AC', picture: 'https://i.pravatar.cc/80?img=11' },
    { id: 'sam-rivera', name: 'Sam Rivera', initials: 'SR', picture: 'https://i.pravatar.cc/80?img=13' },
];

const campaignRules: CampaignRule[] = [
    { id: 'spieler-handbuch', name: 'Spieler Handbuch' },
    { id: 'eberron-schmiede-des-magierschmieds', name: 'Eberron: Schmiede des Magierschmieds' },
    { id: 'tashas-kessel-mit-allem', name: 'Tashas Kessel mit Allem' },
    { id: 'revolverheld', name: 'Revolverheld' },
    { id: 'vampire-die-maskerade', name: 'Vampire: Die Maskerade' },
    { id: 'grim-hollow', name: 'Grim Hollow' },
    { id: 'magus', name: 'Magus' },
    { id: 'faustkaempfer', name: 'Faustkämpfer' },
    { id: 'herr-der-ringe', name: 'Herr der Ringe' },
    { id: 'blutjaeger', name: 'Blutjäger' },
    { id: 'hoellenritter', name: 'Höllenritter' },
    { id: 'world-of-warcraft', name: 'World of Warcraft' },
];

export default function CampaignSetup() {
    const navigate = useNavigate();
    const { id } = useParams();
    const imageInputRef = useRef<HTMLInputElement>(null);
    const [imagePreview, setImagePreview] = useState<string | null>(null);
    const [selectedRules, setSelectedRules] = useState<string[]>([
        'spieler-handbuch', 'eberron-schmiede-des-magierschmieds', 'tashas-kessel-mit-allem',
    ]);
    const [campaignPlayerIds, setCampaignPlayerIds] = useState<string[]>([
        'alex-carter', 'morgan-reed', 'taylor-smith', 'dana-brooks',
    ]);
    const [availableSearch, setAvailableSearch] = useState('');
    const [campaignSearch, setCampaignSearch] = useState('');

    useEffect(() => () => {
        if (imagePreview) {
            URL.revokeObjectURL(imagePreview);
        }
    }, [imagePreview]);

    const campaignPlayers = players.filter((player) => campaignPlayerIds.includes(player.id));
    const availablePlayers = players.filter((player) => !campaignPlayerIds.includes(player.id));

    const movePlayer = (player: Player, toCampaign: boolean) => {
        setCampaignPlayerIds((currentIds) => {
            const isInCampaign = currentIds.includes(player.id);
            if (isInCampaign === toCampaign) {
                return currentIds;
            }
            return toCampaign
                ? [...currentIds, player.id]
                : currentIds.filter((id) => id !== player.id);
        });
    };

    const handleImageChange = (event: ChangeEvent<HTMLInputElement>) => {
        const file = event.currentTarget.files?.[0];
        if (file) {
            setImagePreview(URL.createObjectURL(file));
        }
        event.currentTarget.value = '';
    };

    const toggleRule = (ruleId: string) => {
        setSelectedRules((currentRules) => currentRules.includes(ruleId)
            ? currentRules.filter((id) => id !== ruleId)
            : [...currentRules, ruleId]);
    };

    return (
        <HomeTemplate>
            <main className={styles.parchment}>
                <header className={styles.header}>
                    <span className={styles.compassRose} />
                    <div className={styles.pageHeading}>
                        <h1 className={styles.headerHeading}>{id ? 'Bearbeite die Kampagne' : 'Erstelle eine neue Kampagne'}</h1>
                        <p className={styles.headerDescription}>Bring deine Kampagne zum Leben! Wähle einen Namen und ein episches Bild aus, stelle die Regeln ein und lade deine Spieler ein.</p>
                    </div>
                </header>

                <div className={styles.grid}>
                    <section>
                        <h4 className={styles.sectionHeader}>⚑ Name</h4>
                        <input
                            className={styles.nameInput}
                            type="text"
                            placeholder="F&uuml;ge einen Kampagnennamen hinzu"
                        />

                        <h4 className={styles.sectionHeader}>▧ &Uuml;bersichtsbild</h4>
                        <input
                            ref={imageInputRef}
                            className={styles.imageHiddenInput}
                            type="file"
                            accept="image/*"
                            onChange={handleImageChange}
                        />
                        <button
                            type="button"
                            className={`${styles.imageButton} ${imagePreview ? styles.hasImage : ''}`}
                            style={imagePreview ? { backgroundImage: `url("${imagePreview}")` } : undefined}
                            onClick={() => imageInputRef.current?.click()}
                        >
                            {!imagePreview && (
                                <span>+ Hier klicken um ein Bild hinzuzufügen</span>
                            )}
                        </button>
                    </section>

                    <section className={styles.rules}>
                        <h4 className={styles.sectionHeader}>▤ Regeln</h4>
                        <div className={styles.rulesList}>
                            {campaignRules.map((rule) => (
                                <label className={styles.rule} key={rule.id}>
                                    <input
                                        type="checkbox"
                                        checked={selectedRules.includes(rule.id)}
                                        onChange={() => toggleRule(rule.id)}
                                    />
                                    <span className={styles.ruleName}>{rule.name}</span>
                                </label>
                            ))}
                        </div>
                    </section>
                </div>

                <div className={styles.players}>
                    <PlayerList
                        title="Verf&uuml;gbare Spieler"
                        players={availablePlayers}
                        search={availableSearch}
                        onSearchChange={setAvailableSearch}
                        onMovePlayer={(player) => movePlayer(player, true)}
                        movement="add"
                        emptyMessage="Alle Spieler sind bereits in dieser Kampagne."
                    />
                    <PlayerList
                        title="Spieler in dieser Kampagne"
                        players={campaignPlayers}
                        search={campaignSearch}
                        onSearchChange={setCampaignSearch}
                        onMovePlayer={(player) => movePlayer(player, false)}
                        movement="remove"
                        emptyMessage="Es befindet sich noch kein Spieler in dieser Kampagne."
                    />
                </div>

                <footer className={styles.actions}>
                    <button type="button" className={theme.buttonAccent} onClick={() => navigate('/')}>
                        {id ? 'Kampagne speichern' : 'Kampagne erstellen'}
                    </button>
                    <button type="button" className={theme.buttonSecondary} onClick={() => navigate('/')}>
                        Cancel
                    </button>
                </footer>
            </main>
        </HomeTemplate>
    );
}
