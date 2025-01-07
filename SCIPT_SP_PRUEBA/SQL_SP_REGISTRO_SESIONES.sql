CREATE PROCEDURE sp_RegistrarSesion
    @UsuariosIdUsuario INT,
    @FechaIngreso DATETIME,
    @FechaCierre DATETIME = NULL
AS
BEGIN
    INSERT INTO Sessions (UsuariosIdUsuario, FechaIngreso, FechaCierre)
    VALUES (@UsuariosIdUsuario, @FechaIngreso, @FechaCierre);
END

GO

EXEC sp_helptext 'sp_RegistrarSesion';


