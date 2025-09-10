import { useState } from 'react';


const api = (path, options = {}) =>
  fetch(path, { credentials: 'same-origin', headers: { 'Content-Type': 'application/json' }, ...options });


function LoginScreen() {
  const [statusCode, setStatusCode] = useState(0);
  const [name, setName] = useState("");
  const [email, setEmail] = useState("");
  const [passwordHash, setPasswordHash] = useState("");

  const devServerUrl = import.meta.env.VITE_DEV_SERVER_URL;

  const user = {
    name: name,
    email: email,
    passwordHash: passwordHash,
    //role: "admin"
  }

  async function SignUp() {
    const httpResponse = await api(`${devServerUrl}/api/register`, { method: 'POST', body: JSON.stringify(user) });

    setStatusCode(httpResponse.status);

    //if (!httpResponse.ok) {
    //  return;
    //}
  }

  async function LogIn() {
    const httpResponse = await api(`${devServerUrl}/api/login`, { method: 'POST', body: JSON.stringify(user) });

    setStatusCode(httpResponse.status);
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
      <button onClick={LogIn}>Iniciar sesión</button>
      <button onClick={SignUp}>Registrarse</button>
    </div>
  );
}


export default LoginScreen;