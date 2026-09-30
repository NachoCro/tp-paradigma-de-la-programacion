USE [master]
GO

IF DB_ID(N'TPSocios') IS NOT NULL
BEGIN
    ALTER DATABASE [TPSocios] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [TPSocios];
END
GO

CREATE DATABASE [TPSocios];
GO

USE [TPSocios]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET DATEFORMAT ymd
GO

IF OBJECT_ID(N'dbo.Socios', N'U') IS NOT NULL
    DROP TABLE dbo.Socios;
GO

CREATE TABLE [dbo].[Socios](
    [IdSocio]         [int] IDENTITY(1,1) NOT NULL,
    [LegajoSocio]     [nvarchar](6)   NOT NULL,
    [Apellido]        [nvarchar](50)  NOT NULL,
    [Nombre]          [nvarchar](50)  NOT NULL,
    [Email]           [nvarchar](100) NOT NULL,
    [FechaNacimiento] [date]          NOT NULL,
    [CuotaMensual]    [decimal](8,2)  NOT NULL,
    [TipoSocio]       [nvarchar](20)  NOT NULL,
    [Activo]          [bit]           NOT NULL,
 CONSTRAINT [PK_Socios] PRIMARY KEY CLUSTERED
(
    [IdSocio] ASC
) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF,
        ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Socios] ADD CONSTRAINT [DF_Socios_LegajoSocio]
    DEFAULT (N'S') FOR [LegajoSocio];
GO
ALTER TABLE [dbo].[Socios] ADD CONSTRAINT [DF_Socios_Activo]
    DEFAULT ((1)) FOR [Activo];
GO
ALTER TABLE [dbo].[Socios] ADD CONSTRAINT [CK_Socios_TipoSocio]
    CHECK ([TipoSocio] IN (N'Menor', N'Mayor', N'Jubilado', N'Familiar'));
GO
ALTER TABLE [dbo].[Socios] ADD CONSTRAINT [CK_Socios_CuotaMensual]
    CHECK ([CuotaMensual] > 0);
GO
ALTER TABLE [dbo].[Socios] ADD CONSTRAINT [CK_Socios_FechaNacimiento]
    CHECK ([FechaNacimiento] <= CAST(GETDATE() AS date));
GO
CREATE NONCLUSTERED INDEX [IX_Socios_LegajoSocio]
    ON [dbo].[Socios] ([LegajoSocio] ASC);
GO

-- Los IdSocio se escriben de forma explícita para que los datos de ejemplo
-- siempre tengan los mismos identificadores. Por eso hay que habilitar
-- IDENTITY_INSERT: si no, SQL Server rechaza el INSERT sobre una columna IDENTITY.
SET IDENTITY_INSERT [dbo].[Socios] ON;
GO

INSERT [dbo].[Socios]
    ([IdSocio], [LegajoSocio], [Apellido], [Nombre], [Email],
     [FechaNacimiento], [CuotaMensual], [TipoSocio], [Activo])
VALUES
    (1, N'S-0001', N'García',      N'Juan',    N'juan.garcia@email.com',      CAST(N'1985-03-15' AS date), CAST(12500.00 AS decimal(8,2)), N'Mayor',    1),
    (2, N'S-0002', N'Fernández',   N'María',   N'maria.fernandez@email.com',  CAST(N'1990-07-22' AS date), CAST(12500.00 AS decimal(8,2)), N'Mayor',    1),
    (3, N'F-0001', N'López',       N'Carlos',  N'carlos.lopez@email.com',     CAST(N'2015-11-08' AS date), CAST( 7000.00 AS decimal(8,2)), N'Menor',    1),
    (4, N'A-0214', N'Martínez',    N'Ana',     N'ana.martinez@email.com',     CAST(N'1954-01-30' AS date), CAST( 5000.00 AS decimal(8,2)), N'Jubilado', 1),
(5, N'S-0005', N'Rodríguez',   N'Pedro',  N'pedro.rodriguez@email.com',  CAST(N'1978-09-18' AS date), CAST(12500.00 AS decimal(8,2)), N'Mayor',    0);
GO

SET IDENTITY_INSERT [dbo].[Socios] OFF;
GO

PRINT N'Base TPSocios creada correctamente.';
GO
