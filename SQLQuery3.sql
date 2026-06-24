-- =============================================================================
-- SCRIPT DE POBLAMIENTO MASIVO (DATA SEEDING) - SPOTICLOUD
-- 10 Artistas y 100 Canciones (10 por artista)
-- =============================================================================
USE SpotiCloud;
GO

DECLARE @IdActual INT;

-- ==========================================
-- 1. FUERZA REGIDA
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Fuerza Regida', 'Corridos Tumbados');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('TQM', @IdActual, 157, '2023-05-19', 'Corridos Tumbados'),
('Sabor Fresa', @IdActual, 165, '2023-06-22', 'Corridos Tumbados'),
('Igualito a Mi Apá', @IdActual, 166, '2022-12-30', 'Corridos Tumbados'),
('Ch y la Pizza', @IdActual, 134, '2022-12-01', 'Corridos Tumbados'),
('Harley Quinn', @IdActual, 144, '2023-10-20', 'Corridos Tumbados'),
('Ex Agresivo', @IdActual, 172, '2023-10-20', 'Regional Mexicano'),
('Qué Onda', @IdActual, 191, '2023-08-30', 'Corridos Tumbados'),
('CRAZYZ', @IdActual, 168, '2023-10-20', 'Regional Mexicano'),
('Billete Grande', @IdActual, 170, '2022-12-30', 'Corridos Tumbados'),
('Tu Name', @IdActual, 158, '2024-02-15', 'Regional Mexicano');

-- ==========================================
-- 2. PESO PLUMA
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Peso Pluma', 'Corridos Tumbados');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('Ella Baila Sola', @IdActual, 165, '2023-03-17', 'Corridos Tumbados'),
('Lady Gaga', @IdActual, 212, '2023-06-22', 'Corridos Tumbados'),
('PRC', @IdActual, 184, '2023-01-23', 'Corridos Tumbados'),
('AMG', @IdActual, 174, '2022-11-24', 'Corridos Tumbados'),
('Tulum', @IdActual, 212, '2023-06-29', 'Reggaeton'),
('La Bebe - Remix', @IdActual, 234, '2023-03-17', 'Reggaeton'),
('Rubicon', @IdActual, 198, '2023-06-22', 'Corridos Tumbados'),
('Nueva Vida', @IdActual, 191, '2023-06-22', 'Regional Mexicano'),
('Rosa Pastel', @IdActual, 201, '2023-04-20', 'Corridos Tumbados'),
('Bye', @IdActual, 212, '2023-05-26', 'Corridos Tumbados');

-- ==========================================
-- 3. TRAVIS SCOTT
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Travis Scott', 'Hip-Hop');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('SICKO MODE', @IdActual, 312, '2018-08-03', 'Hip-Hop'),
('goosebumps', @IdActual, 243, '2016-09-02', 'Hip-Hop'),
('HIGHEST IN THE ROOM', @IdActual, 175, '2019-10-04', 'Hip-Hop'),
('FE!N', @IdActual, 191, '2023-07-28', 'Hip-Hop'),
('STARGAZING', @IdActual, 270, '2018-08-03', 'Hip-Hop'),
('BUTTERFLY EFFECT', @IdActual, 190, '2017-05-15', 'Hip-Hop'),
('MELTDOWN', @IdActual, 246, '2023-07-28', 'Hip-Hop'),
('TELEKINESIS', @IdActual, 353, '2023-07-28', 'Hip-Hop'),
('90210', @IdActual, 339, '2015-09-04', 'Hip-Hop'),
('I KNOW ?', @IdActual, 211, '2023-07-28', 'Hip-Hop');

-- ==========================================
-- 4. BAD BUNNY
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Bad Bunny', 'Reggaeton');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('Tití Me Preguntó', @IdActual, 243, '2022-05-06', 'Reggaeton'),
('Me Porto Bonito', @IdActual, 178, '2022-05-06', 'Reggaeton'),
('Ojitos Lindos', @IdActual, 258, '2022-05-06', 'Pop Latino'),
('MONACO', @IdActual, 267, '2023-10-13', 'Trap Latino'),
('DÁKITI', @IdActual, 205, '2020-10-30', 'Reggaeton'),
('Yonaguni', @IdActual, 206, '2021-06-04', 'Reggaeton'),
('Callaíta', @IdActual, 250, '2019-05-31', 'Reggaeton'),
('Efecto', @IdActual, 213, '2022-05-06', 'Reggaeton'),
('Vete', @IdActual, 192, '2019-11-21', 'Reggaeton'),
('Soy Peor', @IdActual, 258, '2016-12-30', 'Trap Latino');

-- ==========================================
-- 5. JOSÉ JOSÉ
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('José José', 'Balada');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('El Triste', @IdActual, 253, '1970-03-15', 'Balada'),
('Almohada', @IdActual, 214, '1978-01-01', 'Balada'),
('Amar y Querer', @IdActual, 230, '1977-01-01', 'Balada'),
('Gavilán o Paloma', @IdActual, 251, '1977-01-01', 'Balada'),
('La Nave del Olvido', @IdActual, 215, '1970-01-01', 'Balada'),
('Lo Pasado, Pasado', @IdActual, 244, '1978-01-01', 'Balada'),
('Preso', @IdActual, 222, '1981-01-01', 'Balada'),
('Si Me Dejas Ahora', @IdActual, 280, '1979-01-01', 'Balada'),
('Volcán', @IdActual, 290, '1978-01-01', 'Balada'),
('Desesperado', @IdActual, 216, '1986-01-01', 'Balada');

-- ==========================================
-- 6. VALENTÍN ELIZALDE
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Valentín Elizalde', 'Regional Mexicano');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('Vete Ya', @IdActual, 153, '2003-01-01', 'Banda'),
('Soy Así', @IdActual, 192, '2005-01-01', 'Banda'),
('A Mis Enemigos', @IdActual, 160, '2006-01-01', 'Banda'),
('Ebrio de Amor', @IdActual, 185, '2005-01-01', 'Banda'),
('Te Quiero Así', @IdActual, 165, '2004-01-01', 'Banda'),
('Lobo Domesticado', @IdActual, 169, '2006-01-01', 'Banda'),
('Cómo Me Duele', @IdActual, 192, '2006-01-01', 'Banda'),
('Y Se Parece A Ti', @IdActual, 210, '2003-01-01', 'Banda'),
('Vencedor', @IdActual, 195, '2006-01-01', 'Banda'),
('La Papa', @IdActual, 140, '2004-01-01', 'Banda');

-- ==========================================
-- 7. CALIBRE 50
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Calibre 50', 'Regional Mexicano');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('A La Antiguita', @IdActual, 169, '2021-02-12', 'Norteño'),
('Siempre Te Voy A Querer', @IdActual, 203, '2016-08-19', 'Norteño'),
('Si Te Pudiera Mentir', @IdActual, 219, '2021-08-20', 'Norteño'),
('Javier El De Los Llanos', @IdActual, 214, '2014-10-21', 'Norteño'),
('Amor Del Bueno', @IdActual, 195, '2016-08-19', 'Norteño'),
('Contigo', @IdActual, 184, '2015-08-14', 'Norteño'),
('El Inmigrante', @IdActual, 201, '2013-10-01', 'Norteño'),
('Ni Que Estuvieras Tan Buena', @IdActual, 165, '2013-10-01', 'Norteño'),
('Simplemente Gracias', @IdActual, 181, '2019-03-15', 'Norteño'),
('Bohemio Loco', @IdActual, 192, '2013-10-01', 'Norteño');

-- ==========================================
-- 8. GERARDO ORTIZ
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Gerardo Ortiz', 'Regional Mexicano');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('Tranquilito', @IdActual, 150, '2021-03-26', 'Corridos'),
('Mujer De Piedra', @IdActual, 222, '2014-04-08', 'Regional Mexicano'),
('Quién Se Anima', @IdActual, 196, '2014-04-08', 'Corridos'),
('Amor Confuso', @IdActual, 200, '2012-09-25', 'Regional Mexicano'),
('Cara A La Muerte', @IdActual, 235, '2011-02-15', 'Corridos'),
('El Cholo', @IdActual, 188, '2015-10-09', 'Corridos'),
('Eres Una Niña', @IdActual, 203, '2014-04-08', 'Regional Mexicano'),
('Perdóname', @IdActual, 195, '2015-10-09', 'Regional Mexicano'),
('Dámaso', @IdActual, 189, '2012-09-25', 'Corridos'),
('Aerolínea Carrillo', @IdActual, 195, '2015-10-09', 'Corridos');

-- ==========================================
-- 9. ÁLVARO DÍAZ
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Álvaro Díaz', 'Hip-Hop Latino');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('Problemón', @IdActual, 196, '2021-10-28', 'Hip-Hop Latino'),
('Llori Pari', @IdActual, 205, '2021-10-28', 'Pop Latino'),
('Babysita', @IdActual, 184, '2021-10-28', 'Urbano Latino'),
('Gatas', @IdActual, 195, '2021-10-28', 'Urbano Latino'),
('Lentito', @IdActual, 180, '2022-09-15', 'Hip-Hop Latino'),
('Majos', @IdActual, 165, '2023-04-12', 'Trap Latino'),
('Yoko', @IdActual, 210, '2023-05-04', 'Pop Latino'),
('1000 Canciones', @IdActual, 222, '2023-08-11', 'Hip-Hop Latino'),
('Fatal Fantassy', @IdActual, 198, '2023-11-03', 'Trap Latino'),
('Reina Pepiada', @IdActual, 191, '2021-10-28', 'Hip-Hop Latino');

-- ==========================================
-- 10. TITO DOBLE P
-- ==========================================
INSERT INTO Artistas (Nombre, Genero) VALUES ('Tito Doble P', 'Corridos Tumbados');
SET @IdActual = SCOPE_IDENTITY();

INSERT INTO Canciones (Titulo, IdArtista, DuracionSegundos, FechaLanzamiento, Genero) VALUES 
('La 701', @IdActual, 155, '2023-12-01', 'Corridos Tumbados'),
('Gavilán II', @IdActual, 175, '2023-06-22', 'Corridos Tumbados'),
('El Gavilán', @IdActual, 180, '2022-10-21', 'Corridos Tumbados'),
('Linda', @IdActual, 160, '2024-01-15', 'Regional Mexicano'),
('Dembow Bélico', @IdActual, 150, '2023-08-25', 'Corridos Tumbados'),
('La People', @IdActual, 163, '2023-06-22', 'Corridos Tumbados'),
('La People II', @IdActual, 165, '2024-03-21', 'Corridos Tumbados'),
('El Bélico', @IdActual, 142, '2023-05-15', 'Corridos Tumbados'),
('PRC (En Vivo)', @IdActual, 195, '2023-11-10', 'Corridos Tumbados'),
('AMG (En Vivo)', @IdActual, 188, '2023-11-10', 'Corridos Tumbados');

GO