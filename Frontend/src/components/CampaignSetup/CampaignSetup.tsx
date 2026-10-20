import { useEffect, useRef, useState, type ChangeEvent, type DragEvent } from 'react';
import { useNavigate } from 'react-router-dom';
import theme from '../../Theme.module.css';
import HomeTemplate from '../HomeTemplate/HomeTemplate';
import styles from './CampaignSetup.module.css';

type Player = {
    id: string;
    name: string;
    role: string;
    initials: string;
    isGameMaster?: boolean;
};

type CampaignRule = {
    id: string;
    name: string;
    description: string;
    symbol: string;
};

const players: Player[] = [
    { id: 'alex-carter', name: 'Alex Carter', role: 'Game Master', initials: 'AC', isGameMaster: true },
    { id: 'morgan-reed', name: 'Morgan Reed', role: 'Player', initials: 'MR' },
    { id: 'taylor-smith', name: 'Taylor Smith', role: 'Player', initials: 'TS' },
    { id: 'dana-brooks', name: 'Dana Brooks', role: 'Player', initials: 'DB' },
    { id: 'taylor-kim', name: 'Taylor Kim', role: 'Player', initials: 'TK' },
    { id: 'jordan-ellis', name: 'Jordan Ellis', role: 'Player', initials: 'JE' },
    { id: 'casey-morgan', name: 'Casey Morgan', role: 'Player', initials: 'CM' },
    { id: 'riley-patel', name: 'Riley Patel', role: 'Player', initials: 'RP' },
    { id: 'avery-chen', name: 'Avery Chen', role: 'Player', initials: 'AC' },
    { id: 'sam-rivera', name: 'Sam Rivera', role: 'Player', initials: 'SR' },
];

const campaignRules: CampaignRule[] = [
    { id: 'dnd-5e', name: 'D&D 5e', description: "The world's most popular roleplaying game.", symbol: '◇' },
    { id: 'pathfinder-2e', name: 'Pathfinder 2e', description: 'Deep rules. Rich character options.', symbol: '✥' },
    { id: 'call-of-cthulhu', name: 'Call of Cthulhu', description: 'Horror and investigation.', symbol: '✹' },
    { id: 'starfinder', name: 'Starfinder', description: 'Sci-fi adventures in the far reaches.', symbol: '✧' },
    { id: 'savage-worlds', name: 'Savage Worlds', description: 'Fast, furious, and flexible.', symbol: '☼' },
];

function PlayerList({
    title,
    description,
    players: listedPlayers,
    search,
    onSearchChange,
    onDropPlayer,
    onMovePlayer,
    emptyMessage,
}: {
    title: string;
    description: string;
    players: Player[];
    search: string;
    onSearchChange: (value: string) => void;
    onDropPlayer: (event: DragEvent<HTMLDivElement>) => void;
    onMovePlayer: (player: Player) => void;
    emptyMessage: string;
}) {
    const filteredPlayers = listedPlayers.filter((player) =>
        player.name.toLowerCase().includes(search.toLowerCase()),
    );

    return (
        <section className={styles.playerSection}>
            <div className={styles.listHeading}>
                <span className={styles.peopleIcon} aria-hidden="true">♟</span>
                <h2>{title}</h2>
                <span className={styles.playerCount}>{listedPlayers.length}</span>
            </div>
            <p className={styles.listDescription}>{description}</p>
            <label className={styles.searchBox}>
                <span aria-hidden="true">⌕</span>
                <span className={styles.visuallyHidden}>Search {title.toLowerCase()}</span>
                <input
                    type="search"
                    placeholder={`Search ${title.toLowerCase()}...`}
                    value={search}
                    onChange={(event) => onSearchChange(event.target.value)}
                />
            </label>
            <div
                className={styles.playerList}
                onDragOver={(event) => event.preventDefault()}
                onDrop={onDropPlayer}
                aria-label={`${title}. Drag players here to move them.`}
            >
                {filteredPlayers.length > 0 ? filteredPlayers.map((player) => (
                    <div
                        className={`${styles.playerRow} ${player.isGameMaster ? styles.gameMasterRow : ''}`}
                        key={player.id}
                        draggable={!player.isGameMaster}
                        onDragStart={(event) => {
                            event.dataTransfer.setData('text/plain', player.id);
                            event.dataTransfer.effectAllowed = 'move';
                        }}
                    >
                        <span className={styles.dragHandle} aria-hidden="true">⠿</span>
                        <span className={styles.avatar} aria-hidden="true">{player.initials}</span>
                        <span className={styles.playerInfo}>
                            <span className={styles.playerName}>{player.name}</span>
                            <span className={styles.playerRole}>{player.role}</span>
                        </span>
                        {player.isGameMaster ? (
                            <span className={styles.masterMark} aria-label="Game master">♛</span>
                        ) : (
                            <button
                                type="button"
                                className={styles.moveButton}
                                aria-label={`${title === 'Available Users' ? 'Add' : 'Remove'} ${player.name} ${title === 'Available Users' ? 'to' : 'from'} campaign`}
                                onClick={() => onMovePlayer(player)}
                            >
                                {title === 'Available Users' ? '+' : '×'}
                            </button>
                        )}
                    </div>
                )) : (
                    <p className={styles.emptyList}>{search ? 'No players match your search.' : emptyMessage}</p>
                )}
            </div>
        </section>
    );
}

export default function CampaignSetup() {
    const navigate = useNavigate();
    const imageInputRef = useRef<HTMLInputElement>(null);
    const [campaignName, setCampaignName] = useState('The Lost Kingdom');
    const [imagePreview, setImagePreview] = useState<string | null>(null);
    const [selectedRules, setSelectedRules] = useState<string[]>(['dnd-5e', 'pathfinder-2e', 'call-of-cthulhu']);
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
        if (player.isGameMaster) {
            return;
        }

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

    const handleDrop = (event: DragEvent<HTMLDivElement>, toCampaign: boolean) => {
        event.preventDefault();
        const player = players.find((candidate) => candidate.id === event.dataTransfer.getData('text/plain'));
        if (player) {
            movePlayer(player, toCampaign);
        }
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
                <header className={styles.pageHeading}>
                    <span className={styles.compassRose} aria-hidden="true" />
                    <div>
                        <h1>Create a New Campaign</h1>
                        <p>Bring your story to life. Add a campaign name, upload an image, set the rules, and invite your players.</p>
                    </div>
                </header>

                <div className={styles.setupGrid}>
                    <section className={styles.campaignDetails}>
                        <label className={styles.fieldLabel} htmlFor="campaign-name">
                            <span aria-hidden="true">⚑</span> Campaign Name
                        </label>
                        <input
                            className={styles.nameInput}
                            id="campaign-name"
                            type="text"
                            value={campaignName}
                            onChange={(event) => setCampaignName(event.target.value)}
                            placeholder="Enter a campaign name"
                        />
                        <p className={styles.helperText}>Choose a memorable name for your campaign.</p>

                        <h2 className={styles.fieldLabel}><span aria-hidden="true">▧</span> Campaign Image</h2>
                        <input
                            ref={imageInputRef}
                            className={styles.fileInput}
                            type="file"
                            accept="image/*"
                            aria-label="Choose a campaign image"
                            onChange={handleImageChange}
                        />
                        <button
                            type="button"
                            className={`${styles.imageButton} ${imagePreview ? styles.hasImage : ''}`}
                            style={imagePreview ? { backgroundImage: `url("${imagePreview}")` } : undefined}
                            onClick={() => imageInputRef.current?.click()}
                            aria-label={imagePreview ? 'Replace campaign image' : 'Choose campaign image'}
                        >
                            {!imagePreview && (
                                <span className={styles.imagePlaceholder}>
                                    <span className={styles.imagePlaceholderIcon} aria-hidden="true">＋</span>
                                    <span>Click to add a campaign image</span>
                                </span>
                            )}
                        </button>
                    </section>

                    <section className={styles.rulesSection}>
                        <h2 className={styles.fieldLabel}><span aria-hidden="true">▤</span> Campaign Rules</h2>
                        <p className={styles.helperText}>Select the ruleset(s) used in this campaign. Multiple rules can be active.</p>
                        <div className={styles.rulesList}>
                            {campaignRules.map((rule) => (
                                <label className={styles.ruleRow} key={rule.id}>
                                    <input
                                        type="checkbox"
                                        checked={selectedRules.includes(rule.id)}
                                        onChange={() => toggleRule(rule.id)}
                                    />
                                    <span className={styles.ruleSymbol} aria-hidden="true">{rule.symbol}</span>
                                    <span className={styles.ruleInfo}>
                                        <span className={styles.ruleName}>{rule.name}</span>
                                        <span className={styles.ruleDescription}>{rule.description}</span>
                                    </span>
                                </label>
                            ))}
                        </div>
                    </section>
                </div>

                <div className={styles.playersGrid}>
                    <PlayerList
                        title="Available Users"
                        description="Drag and drop players between the lists to add or remove them."
                        players={availablePlayers}
                        search={availableSearch}
                        onSearchChange={setAvailableSearch}
                        onDropPlayer={(event) => handleDrop(event, false)}
                        onMovePlayer={(player) => movePlayer(player, true)}
                        emptyMessage="Everyone is in this campaign."
                    />
                    <PlayerList
                        title="Players in Campaign"
                        description="Players currently invited to this campaign."
                        players={campaignPlayers}
                        search={campaignSearch}
                        onSearchChange={setCampaignSearch}
                        onDropPlayer={(event) => handleDrop(event, true)}
                        onMovePlayer={(player) => movePlayer(player, false)}
                        emptyMessage="Drop players here to invite them."
                    />
                </div>

                <footer className={styles.actions}>
                    <button type="button" className={theme.buttonAccent} onClick={() => navigate('/')}>
                        <span aria-hidden="true">＋</span> Create Campaign
                    </button>
                    <button type="button" className={theme.buttonSecondary} onClick={() => navigate('/')}>
                        Cancel
                    </button>
                </footer>
            </main>
        </HomeTemplate>
    );
}
