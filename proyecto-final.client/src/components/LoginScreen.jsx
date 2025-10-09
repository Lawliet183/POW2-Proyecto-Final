import { useState } from 'react';
import Cookies from 'js-cookie';
import { styled } from 'styled-components';

function LoginScreen({ onLoginSuccess, api, devServerUrl }) {
  const [statusCode, setStatusCode] = useState(0);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [passwordHash, setPasswordHash] = useState("");

  const user = {
    name: name,
    email: email,
    passwordHash: passwordHash,
    //role: "admin"
  }


  async function handleSignUp() {
    const httpResponse = await api(`${devServerUrl}/api/register`, { method: 'POST', body: JSON.stringify(user) });

    setStatusCode(httpResponse.status);

    //if (httpResponse.status == 204 || httpResponse.status == 201) {
    //  onLoginSuccess();
    //}

    //if (!httpResponse.ok) {
    //  return;
    //}
  }

  async function handleLogIn() {
    const httpResponse = await api(`${devServerUrl}/api/login`, { method: 'POST', body: JSON.stringify(user) });

    setStatusCode(httpResponse.status);

    const data = await httpResponse.json();

    if (data.status === "error") {
      return;
    }

    const isLoggedIn = Cookies.get('isLoggedIn');

    if (!isLoggedIn) {
      Cookies.set('isLoggedIn', 'true', { expires: 1, sameSite: 'Strict' });
      Cookies.set('userID', data.user_id, { expires: 1, sameSite: 'Strict' });
    }

    if (data.role === "admin") {
      Cookies.set('role', 'admin', { expires: 1, sameSite: 'Strict' });
    } else {
      Cookies.set('role', 'respondent', { expires: 1, sameSite: 'Strict' });
    }

    onLoginSuccess();

    //if (httpResponse.status === 200) {
    //  const isLoggedIn = Cookies.get('isLoggedIn');

    //  if (!isLoggedIn) {
    //    Cookies.set('isLoggedIn', 'true', { expires: 1, sameSite: 'Strict' });
    //    //Cookies.set('isAdmin', 'false', { expires: 1, sameSite: 'Strict' });
    //  }

    //  onLoginSuccess();
    //}
  }

  function handleNameChange(e) {
    setName(e.target.value);
  }

  function handleEmailChange(e) {
    setEmail(e.target.value);
  }

  function handlePasswordHashChange(e) {
    setPasswordHash(e.target.value);
  }


  return (
    <div>
      <Card>
        <div>
          <h1>Iniciar sesión</h1>
        </div>

        <div>
          <label>Nombre</label><br />
          <input type="text" onChange={handleNameChange} />
        </div>

        <div>
          <label>Correo</label><br />
          <input type="email" onChange={handleEmailChange} />
        </div>

        <div>
          <label>Contraseña</label><br />
          <input type="password" onChange={handlePasswordHashChange} />
        </div>

        <Button $primary onClick={handleLogIn}>Iniciar sesión</Button>
        <Button onClick={handleSignUp}>Registrarse</Button>

        {(statusCode === 201 || statusCode === 204)
          && <p>Registrado exitosamente!</p>
        }
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
  background: ${props => props.$primary ? "#eee" : "transparent"};
  color: ${props => props.$primary && "#111"};
  display: flex;
  flex: auto;
  justify-content: center;
  margin: 0.5em 0;



  @media (prefers-color-scheme: light) {
    background: ${props => props.$primary ? "#111" : "transparent"};
    color: ${props => props.$primary && "#eee"};
  }
`;


export default LoginScreen;