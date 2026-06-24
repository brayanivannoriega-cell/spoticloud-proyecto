-- SP 1: Alta de un nuevo usuario
CREATE OR ALTER PROCEDURE sp_InsertarUsuario
    @Nombre     NVARCHAR(100),
    @Email      NVARCHAR(150),
    @Contrasena NVARCHAR(255)
AS
BEGIN
    INSERT INTO Usuarios (Nombre, Email, Contrasena) 
    VALUES (@Nombre, @Email, @Contrasena);
END;
GO

-- SP 2: Cambio (Actualización) de email de un usuario
CREATE OR ALTER PROCEDURE sp_ActualizarEmailUsuario
    @IdUsuario  INT,
    @NuevoEmail NVARCHAR(150)
AS
BEGIN
    UPDATE Usuarios 
    SET Email = @NuevoEmail 
    WHERE IdUsuario = @IdUsuario;
END;
GO

-- SP 3: Baja (Eliminación) de un usuario (Eliminará sus playlists y favoritos en cascada)
CREATE OR ALTER PROCEDURE sp_EliminarUsuario
    @IdUsuario INT
AS
BEGIN
    DELETE FROM Usuarios 
    WHERE IdUsuario = @IdUsuario;
END;
GO

-- SP 4: Alta de un nuevo artista
CREATE OR ALTER PROCEDURE sp_InsertarArtista
    @Nombre NVARCHAR(100),
    @Genero NVARCHAR(50)
AS
BEGIN
    INSERT INTO Artistas (Nombre, Genero) 
    VALUES (@Nombre, @Genero);
END;
GO

-- SP 5: Cambio (Actualización) de género musical de un artista
CREATE OR ALTER PROCEDURE sp_ActualizarGeneroArtista
    @IdArtista   INT,
    @NuevoGenero NVARCHAR(50)
AS
BEGIN
    UPDATE Artistas 
    SET Genero = @NuevoGenero 
    WHERE IdArtista = @IdArtista;
END;
GO

-- SP 6: Baja (Eliminación) de un artista (Eliminará sus canciones en cascada)
CREATE OR ALTER PROCEDURE sp_EliminarArtista
    @IdArtista INT
AS
BEGIN
    DELETE FROM Artistas 
    WHERE IdArtista = @IdArtista;
END;
GO

-- SP 7: Alta de una nueva canción
CREATE OR ALTER PROCEDURE sp_InsertarCancion
    @Titulo             NVARCHAR(150),
    @IdArtista          INT,
    @DuracionSegundos   INT,
    @FechaLanzamiento   DATE,
    @Genero             NVARCHAR(50)
AS
BEGIN
    INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) 
    VALUES (@Titulo, @IdArtista, @DuracionSegundos, @FechaLanzamiento, @Genero);
END;
GO

-- SP 8: Baja (Eliminación) de Canción en general
CREATE OR ALTER PROCEDURE sp_EliminarCancion
    @IdCancion INT
AS
BEGIN
    DELETE FROM Canciones 
    WHERE IdCancion = @IdCancion;
END;
GO

-- SP 9: Alta de una nueva playlist vacía
CREATE OR ALTER PROCEDURE sp_InsertarPlaylist
    @Nombre    NVARCHAR(100),
    @IdUsuario INT
AS
BEGIN
    INSERT INTO Playlists (Nombre, IdUsuario) 
    VALUES (@Nombre, @IdUsuario);
END;
GO

-- SP 10: Baja (Eliminación) de Playlist completa
CREATE OR ALTER PROCEDURE sp_EliminarPlaylist
    @IdPlaylist INT
AS
BEGIN
    DELETE FROM Playlists 
    WHERE IdPlaylist = @IdPlaylist;
END;
GO

-- SP 11: Añadir canción a una playlist (Tabla Puente)
CREATE OR ALTER PROCEDURE sp_AgregarCancionPlaylist
    @IdPlaylist INT,
    @IdCancion  INT
AS
BEGIN
    INSERT INTO PlaylistCanciones (IdPlaylist, IdCancion) 
    VALUES (@IdPlaylist, @IdCancion);
END;
GO

-- SP 12: Quitar canción de una playlist (Tabla Puente)
CREATE OR ALTER PROCEDURE sp_QuitarCancionPlaylist
    @IdPlaylist INT,
    @IdCancion  INT
AS
BEGIN
    DELETE FROM PlaylistCanciones 
    WHERE IdPlaylist = @IdPlaylist AND IdCancion = @IdCancion;
END;
GO

-- SP 13: Dar Like (Agregar a Favoritos)
CREATE OR ALTER PROCEDURE sp_AgregarFavorito
    @IdUsuario INT,
    @IdCancion INT
AS
BEGIN
    IF NOT EXISTS (SELECT 1 FROM Favoritos WHERE IdUsuario = @IdUsuario AND IdCancion = @IdCancion)
    BEGIN
        INSERT INTO Favoritos (IdUsuario, IdCancion) 
        VALUES (@IdUsuario, @IdCancion);
    END
END;
GO

-- SP 14: Quitar Like (Remover de Favoritos)
CREATE OR ALTER PROCEDURE sp_QuitarFavorito
    @IdUsuario INT,
    @IdCancion INT
AS
BEGIN
    DELETE FROM Favoritos 
    WHERE IdUsuario = @IdUsuario AND IdCancion = @IdCancion;
END;
GO