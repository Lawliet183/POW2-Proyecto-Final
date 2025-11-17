# Proyecto Final POW2

Aplicaci車n web desarrollada con **.NET (C#)** en el backend y **React** en el frontend.  
El backend utiliza **Entity Framework Core** con el proveedor **Pomelo.EntityFrameworkCore.MySql** para conectarse a una base de datos **MySQL**.

---

## ?? Requisitos previos

- **Sistema operativo:** Windows 10/11 (recomendado)  
- **IDE:** Visual Studio 2022 con la carga de trabajo ASP.NET y desarrollo web  
- **SDK:** .NET 6.0 SDK o superior  
- **Node.js:** v16+ (LTS recomendado)  
- **Base de datos:** MySQL Server (local o remoto)  
- **Control de versiones:** Git para clonar el repositorio

---

## ?? Instalaci車n

### 1. Clonar el repositorio
```bash
git clone https://github.com/Lawliet183/POW2-Proyecto-Final.git
cd POW2-Proyecto-Final
```

---

### 2. Configuraci車n del Backend (Proyecto-Final.Server)

- Abrir la soluci車n en Visual Studio:
  - **Proyecto-Final.sln**
- Establecer **Proyecto-Final.Server** como proyecto de inicio.

- Restaurar paquetes NuGet:
```bash
dotnet restore
```

- Configurar la cadena de conexi車n en `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=proyecto_encuesta;User=root;Password=tu_password;"
}
```

- Ajusta **Server**, **Database**, **User** y **Password** seg迆n tu entorno MySQL.

- Aplicar migraciones de Entity Framework Core:
```bash
dotnet ef database update
```

- Ejecutar el servidor:
```bash
dotnet run

El backend quedar芍 disponible en `https://localhost:7268` o `http://localhost:5192`.

---

### 3. Configuraci車n del Frontend (proyecto-final.client)

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

El frontend quedar芍 disponible en `https://localhost:57565/`.

## ? Verificaci車n

- Abre `https://localhost:57565/` en tu navegador.  
- El cliente React debe conectarse al backend en `https://localhost:7268`.  
- Prueba las funcionalidades b芍sicas para confirmar la conexi車n con la base de datos MySQL.

## ?? Usuarios preexistentes en la base de datos

### Administrador
- **Usuario:** Liam  
- **Correo:** liam@gmail.com  
- **Contrase?a:** 123qwe  

### Usuario normal
- **Usuario:** Francisco  
- **Correo:** francisco@gmail.com  
- **Contrase?a:** 123qwe  

---

## ?? Problemas comunes

- **Error de conexi車n a MySQL** ↙ Revisa la cadena de conexi車n en `appsettings.json`.  
- **Migraciones no aplicadas** ↙ Ejecuta `dotnet ef database update`.  
- **CORS bloquea el frontend** ↙ Configura `UseCors` en `Program.cs` permitiendo `https://localhost:57565/`.  
- **Certificado HTTPS en desarrollo falla** ↙ Si ocurre, usa `http://localhost:5192` en lugar de `https://localhost:7268`.

---

## ??? Tecnolog赤as utilizadas

- **Backend:** .NET 6, Entity Framework Core, Pomelo MySQL Provider  
- **Frontend:** React, JavaScript, CSS  
- **Base de datos:** MySQL  
- **Herramientas:** Visual Studio 2022, Node.js, Git

