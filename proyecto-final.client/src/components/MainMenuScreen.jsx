import Cookies from 'js-cookie';


function MainMenuScreen({ onLogOut, onSurveyLoad }) {
  function handleLogOut() {
    Cookies.remove('isLoggedIn', { sameSite: 'Strict' });
    onLogOut();
  }


  return (
    <div>
      <p>Iniciado sesion exitosamente!</p>
      <button onClick={() => onSurveyLoad()}>Llenar encuesta</button><br />
      <button onClick={handleLogOut}>Cerrar sesion</button>
    </div>
  );
}

export default MainMenuScreen;