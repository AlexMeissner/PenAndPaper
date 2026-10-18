import { BrowserRouter, Route, Routes } from 'react-router-dom';
import styles from './App.module.css';
import CampaignSetup from './components/CampaignSetup/CampaignSetup';
import Landing from './components/Landing/Landing';

function App() {
  return (
    <div className={styles.appRoot}>
      <BrowserRouter>
        <Routes>
          <Route path="/" element={<Landing />} />
          <Route path="/campaign-setup" element={<CampaignSetup />} />
          <Route path="/campaign-setup/:id" element={<CampaignSetup />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
}

export default App;
