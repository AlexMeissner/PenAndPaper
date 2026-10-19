import { BrowserRouter, Route, Routes } from 'react-router-dom';
import CampaignSetup from './components/CampaignSetup/CampaignSetup';
import Landing from './components/Landing/Landing';

function App() {
  return (
    <BrowserRouter>
        <Routes>
          <Route path="/" element={<Landing />} />
          <Route path="/campaign-setup" element={<CampaignSetup />} />
          <Route path="/campaign-setup/:id" element={<CampaignSetup />} />
        </Routes>
      </BrowserRouter>
  );
}

export default App;
