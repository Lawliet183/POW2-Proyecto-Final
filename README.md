# Proyecto Final POW2

Aplicacion web desarrollada con **.NET (C#)** en el backend y **React** en el frontend.  
El backend utiliza **Entity Framework Core** con el proveedor **Pomelo.EntityFrameworkCore.MySql** para conectarse a una base de datos **MySQL**.

---

## Requisitos previos

- **Sistema operativo:** Windows 10/11 (recomendado)  
- **IDE:** Visual Studio 2022 con la carga de trabajo ASP.NET y desarrollo web  
- **SDK:** .NET 6.0 SDK o superior  
- **Node.js:** v16+ (LTS recomendado)  
- **Base de datos:** MySQL Server (local o remoto)  
- **Control de versiones:** Git para clonar el repositorio

---

## Instalacion

### 1. Clonar el repositorio
```bash
git clone https://github.com/Lawliet183/POW2-Proyecto-Final.git
cd POW2-Proyecto-Final
```

---

### 2. Configuracion del Backend (Proyecto-Final.Server)

- Abrir la solucion en Visual Studio:
  - **Proyecto-Final.sln**
- Establecer **Proyecto-Final.Server** como proyecto de inicio.

- Restaurar paquetes NuGet:
```bash
dotnet restore
```

- Configurar la cadena de conexion en `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=proyecto_encuesta;User=root;Password=tu_password;"
}
```

- Ajusta **Server**, **Database**, **User** y **Password** seg¨²n tu entorno MySQL.

- Aplicar migraciones de Entity Framework Core:
```bash
dotnet ef database update
```

- Ejecutar el servidor:
```bash
dotnet run

El backend quedar¨¢ disponible en `https://localhost:7268` o `http://localhost:5192`.

---

### 3. Configuracion del Frontend (proyecto-final.client)

- Instalar dependencias:
```bash
cd proyecto-final.client
npm install
```

- Configurar la URL del backend en `.env`:
```env
VITE_DEV_SERVER_URL="https://localhost:7268"
```

- Ejecutar el cliente:
```bash
npm start
```

El frontend quedar¨¢ disponible en `https://localhost:57565/`.

## Verificacion

- Abre `https://localhost:57565/` en tu navegador.  
- El cliente React debe conectarse al backend en `https://localhost:7268`.  
- Prueba las funcionalidades b¨¢sicas para confirmar la conexion con la base de datos MySQL.

## Usuarios preexistentes en la base de datos

### Administrador
- **Usuario:** Liam  
- **Correo:** liam@gmail.com  
- **Contrase?a:** 123qwe  

### Usuario normal
- **Usuario:** Francisco  
- **Correo:** francisco@gmail.com  
- **Contrase?a:** 123qwe  

---

## Problemas comunes

- **Error de conexion a MySQL** ¡ú Revisa la cadena de conexion en `appsettings.json`.  
- **Migraciones no aplicadas** ¡ú Ejecuta `dotnet ef database update`.  
- **CORS bloquea el frontend** ¡ú Configura `UseCors` en `Program.cs` permitiendo `https://localhost:57565/`.  
- **Certificado HTTPS en desarrollo falla** ¡ú Si ocurre, usa `http://localhost:5192` en lugar de `https://localhost:7268`.

---

##? Tecnolog¨ªas utilizadas

- **Backend:** .NET 6, Entity Framework Core, Pomelo MySQL Provider  
- **Frontend:** React, JavaScript, CSS  
- **Base de datos:** MySQL  
- **Herramientas:** Visual Studio 2022, Node.js, Git

