-- San Judas: creación inicial, sin claves foráneas ni restricciones CHECK.
-- Ejecutar manualmente en SQL Server. No elimina datos ni tablas existentes.
-- Las relaciones se validan en el backend mediante DGDbContext y servicios.
IF DB_ID(N'DBSistemaEmpleados') IS NULL
    EXEC(N'CREATE DATABASE SanJudas');
GO
USE SanJudas;
GO

IF OBJECT_ID(N'Alumnos', N'U') IS NULL
BEGIN
CREATE TABLE Alumnos (
    IdAlumno INT IDENTITY(1,1) PRIMARY KEY,
    NombresAlumno NVARCHAR(100) NOT NULL,
    ApellidosAlumno NVARCHAR(100) NOT NULL,
    CodigoAlumno NVARCHAR(30) NOT NULL UNIQUE,
    FechaNacimiento DATE NOT NULL,
    Genero NVARCHAR(20) NULL,
    TieneAdecuacionCurricular BIT NOT NULL
);
END;
GO

IF OBJECT_ID(N'AlumnosResponsables', N'U') IS NULL
BEGIN
CREATE TABLE AlumnosResponsables (
    IdAlumnoResponsable INT IDENTITY(1,1) PRIMARY KEY,
    IdAlumno INT NOT NULL,
    IdResponsable INT NOT NULL,
    EstadoParentesco INT NOT NULL
);
END;
GO

IF OBJECT_ID(N'CiclosEscolares', N'U') IS NULL
BEGIN
CREATE TABLE CiclosEscolares (
    IdCicloEscolar INT IDENTITY(1,1) PRIMARY KEY,
    Anio INT NOT NULL,
    Cerrado BIT NOT NULL
);
END;
GO

IF OBJECT_ID(N'CategoriasDocumentos', N'U') IS NULL
BEGIN
CREATE TABLE CategoriasDocumentos (
    IdCategoriaDocumento INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL
);
END;
GO

IF OBJECT_ID(N'Documentos', N'U') IS NULL
BEGIN
CREATE TABLE Documentos (
    IdDocumento INT IDENTITY(1,1) PRIMARY KEY,
    Titulo NVARCHAR(200) NOT NULL,
    EstadoTipoDocumento INT NOT NULL,
    IdAlumno INT NULL,
    IdResponsable INT NULL,
    IdCategoriaDocumento INT NULL,
    FechaDocumento DATE NOT NULL,
    FechaCargaUtc DATETIME2 NOT NULL,
    EsPlantilla BIT NOT NULL,
    RutaRelativa NVARCHAR(200) NOT NULL,
    NombreOriginal NVARCHAR(255) NOT NULL,
    TipoContenido NVARCHAR(100) NOT NULL,
    TamanoBytes BIGINT NOT NULL
);
END;
GO

IF OBJECT_ID(N'Grados', N'U') IS NULL
BEGIN
CREATE TABLE Grados (
    IdGrado INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(80) NOT NULL,
    EdadReferencia INT NULL
);
END;
GO

IF OBJECT_ID(N'GradosAreas', N'U') IS NULL
BEGIN
CREATE TABLE GradosAreas (
    IdGradoArea INT IDENTITY(1,1) PRIMARY KEY,
    IdGrado INT NOT NULL,
    IdArea INT NOT NULL,
    Orden INT NOT NULL
);
END;
GO

IF OBJECT_ID(N'Grupos', N'U') IS NULL
BEGIN
CREATE TABLE Grupos (
    IdGrupo INT IDENTITY(1,1) PRIMARY KEY,
    IdGrado INT NOT NULL,
    IdCicloEscolar INT NOT NULL,
    EstadoSeccion INT NOT NULL,
    CupoMaximo INT NOT NULL
);
END;
GO

IF OBJECT_ID(N'Inscripciones', N'U') IS NULL
BEGIN
CREATE TABLE Inscripciones (
    IdInscripcion INT IDENTITY(1,1) PRIMARY KEY,
    IdAlumno INT NOT NULL,
    IdGrupo INT NOT NULL,
    IdCicloEscolar INT NOT NULL,
    FechaInscripcion DATE NOT NULL,
    IdEncargado INT NOT NULL,
    TelefonoEncargado NVARCHAR(25) NOT NULL
);
END;
GO

IF OBJECT_ID(N'Areas', N'U') IS NULL
BEGIN
CREATE TABLE Areas (
    IdArea INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(150) NOT NULL
);
END;
GO

IF OBJECT_ID(N'Calificaciones', N'U') IS NULL
BEGIN
CREATE TABLE Calificaciones (
    IdCalificacion INT IDENTITY(1,1) PRIMARY KEY,
    IdInscripcion INT NOT NULL,
    IdArea INT NOT NULL,
    EstadoBimestre INT NOT NULL,
    Nota DECIMAL(5,2) NOT NULL
);
END;
GO

IF OBJECT_ID(N'Responsables', N'U') IS NULL
BEGIN
CREATE TABLE Responsables (
    IdResponsable INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto NVARCHAR(200) NOT NULL,
    FechaNacimiento DATE NULL,
    Dpi NVARCHAR(13) NULL,
    Telefono NVARCHAR(25) NOT NULL
);
END;
GO

-- Campo adicional de la ficha; también permite incorporarlo a una tabla existente.
IF COL_LENGTH('Alumnos','Genero') IS NULL
    ALTER TABLE Alumnos ADD Genero NVARCHAR(20) NULL;
GO
-- Catálogos de la escuela. No se agregan alumnos, responsables ni notas de ejemplo.
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Kínder')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Kínder',5);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Párvulos')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Párvulos',6);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Primero primaria')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Primero primaria',NULL);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Segundo primaria')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Segundo primaria',NULL);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Tercero primaria')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Tercero primaria',NULL);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Cuarto primaria')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Cuarto primaria',NULL);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Quinto primaria')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Quinto primaria',NULL);
IF NOT EXISTS (SELECT 1 FROM Grados WHERE Nombre=N'Sexto primaria')
    INSERT INTO Grados (Nombre,EdadReferencia) VALUES (N'Sexto primaria',NULL);
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO Areas (Nombre) VALUES (N'Comunicación y Lenguaje L-1');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO Areas (Nombre) VALUES (N'Comunicación y Lenguaje L-2 (POQOMAM)');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO Areas (Nombre) VALUES (N'Comunicación y Lenguaje L-3 (INGLÉS)');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Matemáticas')
    INSERT INTO Areas (Nombre) VALUES (N'Matemáticas');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Medio natural y social')
    INSERT INTO Areas (Nombre) VALUES (N'Medio natural y social');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Expresión artística')
    INSERT INTO Areas (Nombre) VALUES (N'Expresión artística');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Productividad y Desarrollo')
    INSERT INTO Areas (Nombre) VALUES (N'Productividad y Desarrollo');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Educación Física')
    INSERT INTO Areas (Nombre) VALUES (N'Educación Física');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Ciencias Sociales')
    INSERT INTO Areas (Nombre) VALUES (N'Ciencias Sociales');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Ciencias Naturales y Tecnología')
    INSERT INTO Areas (Nombre) VALUES (N'Ciencias Naturales y Tecnología');
IF NOT EXISTS (SELECT 1 FROM Areas WHERE Nombre=N'Formación ciudadana')
    INSERT INTO Areas (Nombre) VALUES (N'Formación ciudadana');
IF NOT EXISTS (SELECT 1 FROM CategoriasDocumentos WHERE Nombre=N'Actas')
    INSERT INTO CategoriasDocumentos (Nombre) VALUES (N'Actas');
IF NOT EXISTS (SELECT 1 FROM CategoriasDocumentos WHERE Nombre=N'Cartas')
    INSERT INTO CategoriasDocumentos (Nombre) VALUES (N'Cartas');
IF NOT EXISTS (SELECT 1 FROM CategoriasDocumentos WHERE Nombre=N'Donativos')
    INSERT INTO CategoriasDocumentos (Nombre) VALUES (N'Donativos');
IF NOT EXISTS (SELECT 1 FROM CategoriasDocumentos WHERE Nombre=N'Entrega de víveres')
    INSERT INTO CategoriasDocumentos (Nombre) VALUES (N'Entrega de víveres');
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,1 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,2 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,3 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Matemáticas')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,4 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Matemáticas';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Medio natural y social')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,5 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Medio natural y social';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Expresión artística')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,6 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Expresión artística';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Productividad y Desarrollo')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,7 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Productividad y Desarrollo';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Educación Física')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,8 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Primero primaria' AND a.Nombre=N'Educación Física';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,1 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,2 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,3 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Matemáticas')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,4 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Matemáticas';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Medio natural y social')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,5 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Medio natural y social';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Expresión artística')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,6 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Expresión artística';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Productividad y Desarrollo')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,7 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Productividad y Desarrollo';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Educación Física')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,8 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Segundo primaria' AND a.Nombre=N'Educación Física';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,1 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,2 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,3 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Matemáticas')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,4 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Matemáticas';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Medio natural y social')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,5 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Medio natural y social';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Expresión artística')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,6 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Expresión artística';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Productividad y Desarrollo')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,7 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Productividad y Desarrollo';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Educación Física')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,8 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Tercero primaria' AND a.Nombre=N'Educación Física';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,1 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,2 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,3 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Matemáticas')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,4 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Matemáticas';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Ciencias Sociales')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,5 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Ciencias Sociales';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Ciencias Naturales y Tecnología')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,6 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Ciencias Naturales y Tecnología';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Expresión artística')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,7 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Expresión artística';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Formación ciudadana')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,8 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Formación ciudadana';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Productividad y Desarrollo')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,9 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Productividad y Desarrollo';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Educación Física')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,10 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Cuarto primaria' AND a.Nombre=N'Educación Física';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,1 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,2 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,3 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Matemáticas')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,4 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Matemáticas';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Ciencias Sociales')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,5 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Ciencias Sociales';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Ciencias Naturales y Tecnología')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,6 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Ciencias Naturales y Tecnología';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Expresión artística')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,7 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Expresión artística';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Formación ciudadana')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,8 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Formación ciudadana';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Productividad y Desarrollo')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,9 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Productividad y Desarrollo';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Educación Física')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,10 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Quinto primaria' AND a.Nombre=N'Educación Física';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,1 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-1';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,2 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-2 (POQOMAM)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,3 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Comunicación y Lenguaje L-3 (INGLÉS)';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Matemáticas')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,4 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Matemáticas';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Ciencias Sociales')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,5 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Ciencias Sociales';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Ciencias Naturales y Tecnología')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,6 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Ciencias Naturales y Tecnología';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Expresión artística')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,7 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Expresión artística';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Formación ciudadana')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,8 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Formación ciudadana';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Productividad y Desarrollo')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,9 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Productividad y Desarrollo';
IF NOT EXISTS (SELECT 1 FROM GradosAreas ga JOIN Grados g ON g.IdGrado=ga.IdGrado JOIN Areas a ON a.IdArea=ga.IdArea WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Educación Física')
    INSERT INTO GradosAreas (IdGrado,IdArea,Orden) SELECT g.IdGrado,a.IdArea,10 FROM Grados g CROSS JOIN Areas a WHERE g.Nombre=N'Sexto primaria' AND a.Nombre=N'Educación Física';
GO
