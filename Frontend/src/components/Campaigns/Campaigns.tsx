import React from 'react';
import styles from './Campaigns.module.css';
import theme from '../../Theme.module.css';
import { useNavigate } from 'react-router-dom';
import HomeTemplate from '../HomeTemplate/HomeTemplate';
import CampaignCard, { type CampaignCardData } from './CampaignCard/CampaignCard';

type Campaign = CampaignCardData & {
  id: number;
};

const sampleThumbs = [
  'https://straybrush.wordpress.com/wp-content/uploads/2014/05/enjoying_the_view_by_minionslayer-d70n4z5.png',
  'https://straybrush.wordpress.com/wp-content/uploads/2014/05/fantasy_landscape_concept_by_codedzer0-d6boo7v.jpg',
  'https://novacrystallis.com/wp-content/uploads/2024/06/7jem75wYHnCfwwzB97yIIVnpko.jpg',
  'https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcT2VJGYNlIqqij7Q-5Fgji6wY3R2sq_-ea8NI3lNT50ukUknzWNfT7Mi1Tr&s=10'
];

function makeMockCampaigns(count = 15): Campaign[] {
  const gms = [
    { name: 'Morgan Reed', avatar: 'https://i.pravatar.cc/80?img=12' },
    { name: 'Taylor Kim', avatar: 'https://i.pravatar.cc/80?img=5' },
    { name: 'Jordan Ellis', avatar: 'https://i.pravatar.cc/80?img=6' },
    { name: 'Casey Morgan', avatar: 'https://i.pravatar.cc/80?img=10' },
    { name: 'Riley Patel', avatar: 'https://i.pravatar.cc/80?img=8' },
    { name: 'Alex Carter', avatar: 'https://i.pravatar.cc/80?img=3' }
  ];

  const playersPool = [
    { name: 'Ava', avatar: 'https://i.pravatar.cc/40?img=1' },
    { name: 'Ben', avatar: 'https://i.pravatar.cc/40?img=2' },
    { name: 'Cleo', avatar: 'https://i.pravatar.cc/40?img=4' },
    { name: 'Diego', avatar: 'https://i.pravatar.cc/40?img=6' },
    { name: 'Eve', avatar: 'https://i.pravatar.cc/40?img=7' },
    { name: 'Finn', avatar: 'https://i.pravatar.cc/40?img=9' },
    { name: 'Grace', avatar: 'https://i.pravatar.cc/40?img=11' },
    { name: 'Hugo', avatar: 'https://i.pravatar.cc/40?img=13' }
  ];

  return Array.from({ length: count }).map((_, i) => {
    const gm = gms[i % gms.length];
    const playerCount = i < 4 ? 6 + (i % 3) : 3 + (i % 4);
    const players = Array.from({ length: playerCount }).map((__, idx) => playersPool[(i + idx) % playersPool.length]);
    return {
      id: i + 1,
      title: ['The Lost Kingdom', 'Ashes of Tiamat', 'The Whispering Woods', 'Sails of the Shattered Isles', 'Curse of Blackreach', 'The Verdant Expanse'][i % 6] + (i >= 6 ? ` ${i}` : ''),
      gm,
      players,
      thumbnail: sampleThumbs[i % sampleThumbs.length]
    };
  });
}

const Campaigns: React.FC = () => {
  const campaigns = React.useMemo(() => makeMockCampaigns(15), []);
  const navigate = useNavigate();

  return (
    <HomeTemplate>
      <div className={styles.welcome}>
        <h1 className={styles.welcomeTitle}>Wilkommen zur&uuml;ck, Reisender</h1>
        <div className={styles.welcomeSubtitle}>Neue Welten erwarten dich. Schlie&szlig;e dich einer Gruppe an, hole die W&uuml;rfel aus dem Schrank und erlebe dein n&auml;chstes legend&auml;res Abenteuer.</div>
        <button className={theme.buttonAccent} onClick={() => navigate('/campaign-setup')}>+ Kampagne erstellen</button>
      </div>

      <div className={styles.campaigns}>
        <div className={styles.header}>
            <span className={styles.compassRose} aria-hidden="true" />
            <div className={styles.headerTitle}>Kampagnen</div>
        </div>
        <div className={styles.campaignCards}>
          <div className={styles.grid}>
            {campaigns.map((campaign) => (
              <CampaignCard key={campaign.id} campaign={campaign} />
            ))}
          </div>
        </div>

        <div className={styles.note}>Nicht alle, die umherwandern, sind verloren</div>
      </div>
    </HomeTemplate>
  );
};

export default Campaigns;
