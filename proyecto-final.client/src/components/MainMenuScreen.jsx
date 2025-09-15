import Cookies from 'js-cookie';


function MainMenuScreen({ onLogOut, onSurveyLoad, onAnswersLoad }) {
  function handleLogOut() {
    Cookies.remove('isLoggedIn', { sameSite: 'Strict' });
    Cookies.remove('role', { sameSite: 'Strict' });

    onLogOut();
  }


  return (
    <div>
      <p>Iniciado sesion exitosamente!</p>
      <button onClick={onSurveyLoad}>Llenar encuesta</button><br />
      <button onClick={handleLogOut}>Cerrar sesion</button>
      {Cookies.get('role') === 'admin' && 
        <button onClick={onAnswersLoad}>Mirar respuestas</button>
      }
    </div>
  );
}

export default MainMenuScreen;