import { useState } from 'react';
import Cookies from 'js-cookie';

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

    if (httpResponse.status == 200) {
      const isLoggedIn = Cookies.get('isLoggedIn');

      if (!isLoggedIn) {
        Cookies.set('isLoggedIn', 'true', { expires: 1, sameSite: 'Strict' });
      }

      onLoginSuccess();
    }
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

      <p>Status code: {statusCode}</p>

      <button onClick={handleLogIn}>Iniciar sesión</button>
      <button onClick={handleSignUp}>Registrarse</button>

      { (statusCode == 201 || statusCode == 204)
        && <p>Registrado exitosamente!</p>
      }
    </div>
  );
}


export default LoginScreen;