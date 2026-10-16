import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './Theme.module.css'
import './index.css'
import App from './App.tsx'
import { AuthenticationProvider } from './authentication/AuthenticationProvider.tsx'

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthenticationProvider>
      <App />
    </AuthenticationProvider>
  </StrictMode>,
)
