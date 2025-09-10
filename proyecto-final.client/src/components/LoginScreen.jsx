import { useState } from 'react';

function LoginScreen() {
  const [isLoaded, setIsLoaded] = useState(false);
  const [statusCode, setStatusCode] = useState(0);
  const [responseData, setResponseData] = useState("");

  const devServerUrl = import.meta.env.VITE_DEV_SERVER_URL;

  //const api = (path, options = {}) =>
  //  fetch(path, { credentials: 'include', headers: { 'Content-Type': 'application/json' }, ...options });

  async function LogIn() {
    

    const user = {
      name: "Liam",
      email: "liam@google.com",
      passwordhash: "123qwe",
      role: "admin"
    }

    const httpResponse = await fetch(`${devServerUrl}/api/register`, { method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(user) });

    setStatusCode(httpResponse.status);

    if (!httpResponse.ok) {
      return;
    }

    let data = "";
    try {
      data = await httpResponse.json();
    } catch (err) {
      setIsLoaded(false);
    }

    if (!data.error) {
      setIsLoaded(true);

      console.log(data);

      setResponseData(data);
    }
  }

  return (
    <div>
      <p>is Loaded?: {isLoaded ? 'yes' : 'no'}</p>
      <p>Status code: {statusCode}</p>
      <button onClick={LogIn}>Iniciar sesion</button>
    </div>
  );
}

export default LoginScreen;