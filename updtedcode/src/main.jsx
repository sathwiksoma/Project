import React from 'react'
import ReactDOM from 'react-dom/client'
import { ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import App from './App.jsx'
import './index.css'
import {BrowserRouter} from 'react-router-dom'
import StoreContextProvider from './context/StoreContext.jsx'
import { AuthProvider } from './context/Auth.jsx'
ReactDOM.createRoot(document.getElementById('root')).render(
  <BrowserRouter>
  <AuthProvider>
  <StoreContextProvider>
   <App />
  </StoreContextProvider>
  </AuthProvider>
  <ToastContainer />
  </BrowserRouter>
  
)
