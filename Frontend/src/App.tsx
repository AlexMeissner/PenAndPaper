import { BrowserRouter, Route, Routes } from 'react-router-dom';
import CampaignSetup from './components/CampaignSetup/CampaignSetup';
import Campaigns from './components/Campaigns/Campaigns';

function App() {
  return (
    <BrowserRouter>
        <Routes>
          <Route path="/" element={<Campaigns />} />
          <Route path="/campaign-setup" element={<CampaignSetup />} />
          <Route path="/campaign-setup/:id" element={<CampaignSetup />} />
        </Routes>
      </BrowserRouter>
  );
}

export default App;
