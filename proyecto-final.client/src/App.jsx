import { useEffect, useState } from 'react';
import Cookies from 'js-cookie';

import LoadingScreen from '@/components/LoadingScreen';
import LoginScreen from '@/components/LoginScreen';
import MainMenuScreen from '@/components/MainMenuScreen';
import SurveyScreen from '@/components/SurveyScreen';
import SurveyAnsweredScreen from '@/components/SurveyAnsweredScreen';
import AnswersScreen from '@/components/AnswersScreen';

import '@/App.css';


const api = (path, options = {}) =>
  fetch(path, { credentials: 'same-origin', headers: { 'Content-Type': 'application/json' }, ...options });

const devServerUrl = import.meta.env.VITE_DEV_SERVER_URL;


function App() {
  const [currentScreen, setCurrentScreen] = useState('loading');


  useEffect(() => {
    async function initialPing() {
      await api(`${devServerUrl}/api/ping`);

      const isLoggedIn = Cookies.get('isLoggedIn');
      if (isLoggedIn) {
        setCurrentScreen('main-menu');
      } else {
        setCurrentScreen('login');
      }
    }

    initialPing();
  }, []);


  function handleLoginSuccess() {
    setCurrentScreen('main-menu');
  }

  function handleLogOut() {
    setCurrentScreen('login');
  }

  function handleSurveyLoad() {
    setCurrentScreen('survey');
  }

  function handleAnswersSubmitted() {
    setCurrentScreen('survey-answered');
  }

  function handleReturnToMainMenu() {
    setCurrentScreen('main-menu');
  }

  function handleAnswersLoad() {
    setCurrentScreen('answers');
  }


  let content;
  switch (currentScreen) {
    case 'loading': {
      content = <LoadingScreen />
      break;
    }
    case 'login': {
      content = <LoginScreen onLoginSuccess={handleLoginSuccess} api={api} devServerUrl={devServerUrl} />;
      break;
    }
    case 'main-menu': {
      content = <MainMenuScreen onLogOut={handleLogOut} onSurveyLoad={handleSurveyLoad} onAnswersLoad={handleAnswersLoad} />;
      break;
    }
    case 'survey': {
      content = <SurveyScreen onAnswersSubmitted={handleAnswersSubmitted} api={api} devServerUrl={devServerUrl} />
      break;
    }
    case 'survey-answered': {
      content = <SurveyAnsweredScreen onReturnToMainMenu={handleReturnToMainMenu} />
      break;
    }
    case 'answers': {
      content = <AnswersScreen onReturnToMainMenu={handleReturnToMainMenu} api={api} devServerUrl={devServerUrl} />
      break;
    }
    default: {
      content = null;
      break;
    }
  }


  return (
    <>
      {content}
    </>
  );
}


export default App;