import Cookies from 'js-cookie';
import { styled } from 'styled-components';


function MainMenuScreen({ onLogOut, onSurveyLoad, onAnswersLoad }) {
  function handleLogOut() {
    Cookies.remove('isLoggedIn', { sameSite: 'Strict' });
    Cookies.remove('userID', { sameSite: 'Strict' });
    Cookies.remove('role', { sameSite: 'Strict' });

    onLogOut();
  }


  return (
    <div>
      <Card>
        <h2>Iniciado sesion exitosamente!</h2>
        <Button onClick={onSurveyLoad}>Llenar encuesta</Button>
        {Cookies.get('role') === 'admin' &&
          <Button $admin onClick={onAnswersLoad}>Mirar respuestas</Button>
        }
        <Button $logout onClick={handleLogOut}>Cerrar sesion</Button>
      </Card>
    </div>
  );
}


const Card = styled.div`
  display: flex;
  flex-direction: column;
  border-radius: 5px;
  background: #444;
  color: white;
  padding: 1em;

  @media (prefers-color-scheme: light) {
    background: #ddd;
    color: #333;
  }
`;

const Button = styled.button`
  border-radius: 0px;
  border: 1px solid;
  background: ${props => {
    if (props.$logout)
      return "#c04";

    if (props.$admin)
      return "#c0c";

    return "#185BA2";
  }};

  color: #eee;
  display: flex;
  flex: auto;
  justify-content: center;
  margin: 0.5em 0;
`;


export default MainMenuScreen;