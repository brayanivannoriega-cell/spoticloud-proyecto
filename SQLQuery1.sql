-- =========================================================================================
-- PROYECTO: SPOTICLOUD WEB PLAYER (MASTER SCRIPT)
-- DESCRIPCIÓN: Creación de BD, Tablas, Restricciones, Procedimientos Almacenados y Consultas
-- =========================================================================================

-- =========================================================================================
-- FASE 0: CREACIÓN DE LA BASE DE DATOS Y ESQUEMA (DDL)
-- =========================================================================================

-- 1. Creación de la base de datos desde cero
CREATE DATABASE SpotiCloud;
GO

-- 2. Fijar el contexto estricto de ejecución
USE SpotiCloud;
GO

-- 3. Definición de Tablas y Restricciones (Integridad Referencial)

-- Tabla de Usuarios
CREATE TABLE Usuarios (
    IdUsuario       INT IDENTITY(1,1) PRIMARY KEY,
    Nombre          NVARCHAR(100) NOT NULL,
    Email           NVARCHAR(150) NOT NULL UNIQUE,
    Contrasena      NVARCHAR(255) NOT NULL,
    FechaRegistro   DATETIME DEFAULT GETDATE()
);
GO

-- Tabla de Artistas
CREATE TABLE Artistas (
    IdArtista       INT IDENTITY(1,1) PRIMARY KEY,
    Nombre          NVARCHAR(100) NOT NULL,
    Genero          NVARCHAR(50) NOT NULL
);
GO

-- Tabla de Canciones (Relación 1:N con Artistas)
CREATE TABLE Canciones (
    IdCancion        INT IDENTITY(1,1) PRIMARY KEY,
    Titulo           NVARCHAR(150) NOT NULL,
    IdArtista        INT NOT NULL,
    DuracionSegundos INT NOT NULL,
    FechaLanzamiento DATE NOT NULL,
    Genero           NVARCHAR(50) DEFAULT 'Desconocido',
    CONSTRAINT FK_Canciones_Artistas FOREIGN KEY (IdArtista) REFERENCES Artistas(IdArtista) ON DELETE CASCADE
);
GO

-- Tabla de Playlists (Relación 1:N con Usuarios)
CREATE TABLE Playlists (
    IdPlaylist      INT IDENTITY(1,1) PRIMARY KEY,
    Nombre          NVARCHAR(100) NOT NULL,
    IdUsuario       INT NOT NULL,
    FechaCreacion   DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_Playlists_Usuarios FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario) ON DELETE CASCADE
);
GO

-- Tabla Puente: PlaylistCanciones (Relación N:M entre Playlists y Canciones)
CREATE TABLE PlaylistCanciones (
    IdPlaylist      INT NOT NULL,
    IdCancion       INT NOT NULL,
    PRIMARY KEY (IdPlaylist, IdCancion),
    CONSTRAINT FK_PC_Playlists FOREIGN KEY (IdPlaylist) REFERENCES Playlists(IdPlaylist) ON DELETE CASCADE,
    CONSTRAINT FK_PC_Canciones FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion) ON DELETE CASCADE
);
GO

-- Tabla de Favoritos (Relación N:M entre Usuario y Canción para el sistema de Likes)
CREATE TABLE Favoritos (
    IdUsuario       INT NOT NULL,
    IdCancion       INT NOT NULL,
    FechaAgregado   DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (IdUsuario, IdCancion),
    CONSTRAINT FK_Fav_Usuario FOREIGN KEY (IdUsuario) REFERENCES Usuarios(IdUsuario) ON DELETE CASCADE,
    CONSTRAINT FK_Fav_Cancion FOREIGN KEY (IdCancion) REFERENCES Canciones(IdCancion) ON DELETE CASCADE
);
GO



-- =========================================================================================
-- FASE 1 - PARTE B: 10 CONSULTAS PURAS (SELECT, INNER JOIN, WHERE, ORDER BY)
-- Exclusivas para análisis y lectura de datos complejos en SQL Server.
-- =========================================================================================

-- Query 1: Obtener todas las canciones (mostrando su nuevo campo Género) de un artista de Rock.
SELECT 
    c.Titulo, 
    c.Genero AS GeneroCancion, 
    c.DuracionSegundos, 
    a.Nombre AS Artista 
FROM Canciones c 
INNER JOIN Artistas a ON c.IdArtista = a.IdArtista 
WHERE a.Genero = 'Rock' 
ORDER BY c.Titulo ASC;
GO

-- Query 2: Obtener las Playlists creadas por un usuario con un dominio de correo específico.
SELECT 
    p.Nombre AS Playlist, 
    p.FechaCreacion, 
    u.Nombre AS Creador 
FROM Playlists p 
INNER JOIN Usuarios u ON p.IdUsuario = u.IdUsuario 
WHERE u.Email LIKE '%@spoticloud%' 
ORDER BY p.FechaCreacion DESC;
GO

-- Query 3: Obtener todas las canciones dentro de una playlist específica (Ej: ID = 1).
SELECT 
    c.Titulo, 
    a.Nombre AS Artista 
FROM Canciones c 
INNER JOIN PlaylistCanciones pc ON c.IdCancion = pc.IdCancion 
INNER JOIN Artistas a ON c.IdArtista = a.IdArtista
WHERE pc.IdPlaylist = 1 
ORDER BY c.Titulo ASC;
GO

-- Query 4: Obtener usuarios que crearon playlists después del año 2024.
SELECT 
    u.Nombre, 
    u.Email, 
    p.Nombre AS Playlist 
FROM Usuarios u 
INNER JOIN Playlists p ON u.IdUsuario = p.IdUsuario 
WHERE p.FechaCreacion > '2024-12-31' 
ORDER BY u.Nombre ASC;
GO

-- Query 5: Obtener los artistas que tienen canciones con una duración mayor a 3 minutos (180 seg).
SELECT DISTINCT 
    a.Nombre, 
    a.Genero 
FROM Artistas a 
INNER JOIN Canciones c ON a.IdArtista = c.IdArtista 
WHERE c.DuracionSegundos > 180 
ORDER BY a.Nombre DESC;
GO

-- Query 6: Obtener playlists que contienen canciones de un género en particular (Ej: Pop).
SELECT DISTINCT 
    p.Nombre AS Playlist, 
    u.Nombre AS UsuarioCreador 
FROM Playlists p 
INNER JOIN PlaylistCanciones pc ON p.IdPlaylist = pc.IdPlaylist 
INNER JOIN Canciones c ON pc.IdCancion = c.IdCancion 
INNER JOIN Usuarios u ON p.IdUsuario = u.IdUsuario
WHERE c.Genero = 'Pop' 
ORDER BY p.Nombre ASC;
GO

-- Query 7: Obtener canciones lanzadas después del año 2020 de artistas cuyo nombre contiene 'The'.
SELECT 
    c.Titulo, 
    c.FechaLanzamiento, 
    a.Nombre AS Artista 
FROM Canciones c 
INNER JOIN Artistas a ON c.IdArtista = a.IdArtista 
WHERE a.Nombre LIKE '%The%' 
  AND c.FechaLanzamiento >= '2020-01-01' 
ORDER BY c.FechaLanzamiento DESC;
GO

-- Query 8: Obtener artistas que pertenecen a una playlist creada por un usuario específico (Ej ID=1).
SELECT DISTINCT 
    a.Nombre AS Artista, 
    a.Genero 
FROM Artistas a 
INNER JOIN Canciones c ON a.IdArtista = c.IdArtista 
INNER JOIN PlaylistCanciones pc ON c.IdCancion = pc.IdCancion 
INNER JOIN Playlists p ON pc.IdPlaylist = p.IdPlaylist 
WHERE p.IdUsuario = 1 
ORDER BY a.Nombre ASC;
GO

-- Query 9: Todas las canciones almacenadas en playlists creadas en el año actual.
SELECT 
    c.Titulo, 
    p.Nombre AS Playlist, 
    c.Genero 
FROM Canciones c 
INNER JOIN PlaylistCanciones pc ON c.IdCancion = pc.IdCancion 
INNER JOIN Playlists p ON pc.IdPlaylist = p.IdPlaylist 
WHERE YEAR(p.FechaCreacion) = YEAR(GETDATE()) 
ORDER BY c.Titulo ASC;
GO

-- Query 10: Obtener en qué playlists se encuentra una canción específica (buscada por título).
SELECT 
    p.Nombre AS Playlist, 
    u.Nombre AS Propietario 
FROM Playlists p 
INNER JOIN PlaylistCanciones pc ON p.IdPlaylist = pc.IdPlaylist 
INNER JOIN Canciones c ON pc.IdCancion = c.IdCancion 
INNER JOIN Usuarios u ON p.IdUsuario = u.IdUsuario
WHERE c.Titulo LIKE '%Bohemian%' 
ORDER BY p.Nombre ASC;
GO
