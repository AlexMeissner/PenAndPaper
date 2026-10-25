import styles from './PlayerList.module.css';

export type Player = {
    id: string;
    name: string;
    initials: string;
    picture: string;
};

type PlayerListProps = {
    title: string;
    players: Player[];
    search: string;
    onSearchChange: (value: string) => void;
    onMovePlayer: (player: Player) => void;
    movement: 'add' | 'remove';
    emptyMessage: string;
};

export default function PlayerList({
    title,
    players: listedPlayers,
    search,
    onSearchChange,
    onMovePlayer,
    movement,
    emptyMessage,
}: PlayerListProps) {
    const filteredPlayers = listedPlayers
        .filter((player) => player.name.toLowerCase().includes(search.toLowerCase()))
        .sort((first, second) => first.name.localeCompare(second.name));

    return (
        <section>
            <div className={styles.header}>
                <svg className={styles.userIcon} viewBox="0 0 640 640">
                    <path d="M320 312C386.3 312 440 258.3 440 192C440 125.7 386.3 72 320 72C253.7 72 200 125.7 200 192C200 258.3 253.7 312 320 312zM290.3 368C191.8 368 112 447.8 112 546.3C112 562.7 125.3 576 141.7 576L498.3 576C514.7 576 528 562.7 528 546.3C528 447.8 448.2 368 349.7 368L290.3 368z" />
                </svg>
                <h4>{title}</h4>
                <span className={styles.userCount}>{listedPlayers.length}</span>
            </div>
            <label className={styles.searchBox}>
                <input
                    type="search"
                    placeholder={`Durchsuche ${title}...`}
                    value={search}
                    onChange={(event) => onSearchChange(event.target.value)}
                />
            </label>
            <div className={styles.players}>
                {filteredPlayers.length > 0 ? filteredPlayers.map((player) => (
                    <button
                        type="button"
                        className={`${styles.player} ${movement === 'add' ? styles.playerAdd : styles.playerRemove}`}
                        key={player.id}
                        onClick={() => onMovePlayer(player)}
                    >
                        <img className={styles.avatar} src={player.picture} alt={player.initials} referrerPolicy="no-referrer" />
                        <span className={styles.playerName}>{player.name}</span>
                    </button>
                )) : (
                    <p className={styles.playersEmpty}>{search ? 'No players match your search.' : emptyMessage}</p>
                )}
            </div>
        </section>
    );
}
