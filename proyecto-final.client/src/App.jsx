import { useEffect, useState } from 'react';
import Cookies from 'js-cookie';

import LoginScreen from '@/components/LoginScreen';
import MainMenuScreen from '@/components/MainMenuScreen';

import './App.css';


const api = (path, options = {}) =>
  fetch(path, { credentials: 'same-origin', headers: { 'Content-Type': 'application/json' }, ...options });


function App() {
  const [currentScreen, setCurrentScreen] = useState('login');


  useEffect(() => {
    const isLoggedIn = Cookies.get('isLoggedIn');
    if (isLoggedIn) {
      setCurrentScreen('main-menu');
    }
  }, []);


  function handleLoginSuccess() {
    setCurrentScreen('main-menu');
  }


  let content;
  switch (currentScreen) {
    case 'login': {
      content = <LoginScreen onLoginSuccess={handleLoginSuccess} api={api} />;
      break;
    }
    case 'main-menu': {
      content = <MainMenuScreen />;
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

    //const [forecasts, setForecasts] = useState();

    //useEffect(() => {
    //    populateWeatherData();
    //}, []);

    //const contents = forecasts === undefined
    //    ? <p><em>Loading... Please refresh once the ASP.NET backend has started. See <a href="https://aka.ms/jspsintegrationreact">https://aka.ms/jspsintegrationreact</a> for more details.</em></p>
    //    : <table className="table table-striped" aria-labelledby="tableLabel">
    //        <thead>
    //            <tr>
    //                <th>Date</th>
    //                <th>Temp. (C)</th>
    //                <th>Temp. (F)</th>
    //                <th>Summary</th>
    //            </tr>
    //        </thead>
    //        <tbody>
    //            {forecasts.map(forecast =>
    //                <tr key={forecast.date}>
    //                    <td>{forecast.date}</td>
    //                    <td>{forecast.temperatureC}</td>
    //                    <td>{forecast.temperatureF}</td>
    //                    <td>{forecast.summary}</td>
    //                </tr>
    //            )}
    //        </tbody>
    //    </table>;

    //return (
    //    <div>
    //        <h1 id="tableLabel">Weather forecast</h1>
    //        <p>This component demonstrates fetching data from the server.</p>
    //        {contents}
    //    </div>
    //);
    
    //async function populateWeatherData() {
    //    const response = await fetch('test');
    //    if (response.ok) {
    //        console.log('Response: ', response);

    //        const data = await response.json();
    //        setForecasts(data);
    //    }
    //}
}

export default App;