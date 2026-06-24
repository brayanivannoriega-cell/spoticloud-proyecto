# 🎵 SpotiCloud Web Player

Aplicación de escritorio desarrollada en **VB.NET** que simula un reproductor de música estilo Spotify, con persistencia de datos en **SQL Server**. El proyecto cuenta con un panel para usuarios finales (exploración por género, búsqueda en vivo y sistema de favoritos) y un panel de administración independiente para la gestión completa de la base de datos.

## ✨ Características

**Panel de Usuario**
- Inicio de sesión y registro de nuevas cuentas.
- Exploración de canciones organizadas por género, mostradas en tarjetas con degradado de color.
- Buscador en vivo (*live search*) que filtra por Artista, Género o Canción mientras se escribe, usando comodines (`LIKE %...%`) en SQL.
- Sistema de "Me gusta" (favoritos): agregar y quitar canciones de una lista personal de favoritos por usuario.

**Panel de Administración**
- Gestión de Artistas: alta, actualización de género y eliminación.
- Gestión de Canciones: alta (con título, artista, duración, fecha de lanzamiento y género) y eliminación.
- Gestión de Playlists: creación, eliminación, y manejo de la relación canción-playlist (agregar/quitar canciones de una playlist específica).
- Ordenamiento de tablas (Artistas A-Z/Z-A, Playlists por fecha).

## 🛠️ Tecnologías

- **VB.NET** (Windows Forms) — interfaz construida de forma programática (sin diseñador visual, controles creados en código).
- **SQL Server** — motor de base de datos.
- **ADO.NET** (`System.Data.SqlClient`) — conexión y ejecución de consultas/procedimientos almacenados.
- Procedimientos almacenados para las operaciones CRUD principales (alta, baja y actualización de Usuarios, Artistas, Canciones, Playlists y Favoritos).

## 🗃️ Modelo de Datos

La base de datos `SpotiCloud` está compuesta por las siguientes tablas:

| Tabla | Descripción |
|---|---|
| `Usuarios` | Cuentas registradas (nombre, email, contraseña, fecha de registro). |
| `Artistas` | Catálogo de artistas y su género musical. |
| `Canciones` | Canciones, vinculadas a un artista (relación 1:N). |
| `Playlists` | Listas de reproducción creadas por un usuario (relación 1:N). |
| `PlaylistCanciones` | Tabla puente para la relación N:M entre Playlists y Canciones. |
| `Favoritos` | Tabla puente para la relación N:M entre Usuarios y Canciones (sistema de likes). |

Todas las relaciones cuentan con integridad referencial mediante `FOREIGN KEY` y borrado en cascada (`ON DELETE CASCADE`).

## 🗂️ Estructura del Proyecto

```
spoti/
├── PROYECTO/              # Solución de Visual Studio (VB.NET)
│   └── PROYECTO/
│       ├── Form1.vb       # Login, registro y panel de administración (frmAdmin)
│       ├── frmAdmin (en Form1.vb)  # Panel de gestión para administradores
│       └── ...
├── SQLQuery1.sql           # Script maestro: creación de BD, tablas y consultas de análisis
├── SQLQuery3.sql           # Script de poblamiento masivo (datos de prueba)
├── SQLQuery4.sql           # Procedimientos almacenados (CRUD)
└── .gitignore
```

## ⚙️ Instalación y Configuración

1. **Base de datos:** ejecuta en orden los scripts `.sql` sobre una instancia de SQL Server (Express o completo): primero el de creación de base de datos y tablas, después el de poblamiento de datos, y por último el de procedimientos almacenados.
2. **Cadena de conexión:** el proyecto se conecta por defecto a una instancia local llamada `SQLEXPRESS` mediante autenticación integrada de Windows:
   ```vb
   Data Source=.\SQLEXPRESS;Initial Catalog=SpotiCloud;Integrated Security=True
   ```
   Si tu instancia de SQL Server tiene otro nombre, deberás actualizar esta cadena en el módulo `DatabaseConnection`.
3. **Ejecutar:** abre la solución en Visual Studio y ejecuta el proyecto (F5).

## ⚠️ Notas y Limitaciones

Este proyecto fue desarrollado con fines académicos, por lo que tiene algunas simplificaciones intencionales que no se recomendarían en un entorno de producción:

- Las contraseñas de los usuarios se almacenan y comparan en **texto plano**, sin cifrado ni hash.
- El acceso de administrador usa credenciales fijas en el código (`admin` / `admin`) en lugar de un sistema de roles real.
- El manejo de errores en varias operaciones es básico (bloques `Try/Catch` vacíos en algunos casos).

## 👥 Autores

- **Iván Noriega** — Estudiante de Ingeniería en Telecomunicaciones, Sistemas y Electrónica, FES Cuautitlán (UNAM).
- **Ricardo Galeana**
