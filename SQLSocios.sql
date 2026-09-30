USE [TPSocios]
GO
/****** Object:  Table [dbo].[Socios]    Script Date: 25/09/2026 15:39:46 ******/
SET ANSI_NULLS ON
SET DATEFORMAT ymd
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Socios](
	[IdSocio] [int] IDENTITY(1,1) NOT NULL,
	[LegajoSocio] [nvarchar](6) NOT NULL,
	[Apellido] [nvarchar](50) NOT NULL,
	[Nombre] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[FechaNacimiento] [date] NOT NULL,
	[CuotaMensual] [decimal](8, 2) NOT NULL,
	[TipoSocio] [nvarchar](20) NOT NULL,
	[Activo] [bit] NOT NULL,
 CONSTRAINT [PK_Socios] PRIMARY KEY CLUSTERED 
(
	[IdSocio] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[Socios] ON 
GO
INSERT [dbo].[Socios] ([IdSocio], [LegajoSocio], [Apellido], [Nombre], [Email], [FechaNacimiento], [CuotaMensual], [TipoSocio], [Activo]) VALUES (1, N'S-0001', N'García', N'Juan', N'juan.garcia@email.com', CAST(N'1985-03-15' AS Date), CAST(12500.00 AS Decimal(8, 2)), N'Mayor', 1)
GO
INSERT [dbo].[Socios] ([IdSocio], [LegajoSocio], [Apellido], [Nombre], [Email], [FechaNacimiento], [CuotaMensual], [TipoSocio], [Activo]) VALUES (2, N'S-0002', N'Fernández', N'María', N'maria.fernandez@email.com', CAST(N'1990-07-22' AS Date), CAST(12500.00 AS Decimal(8, 2)), N'Mayor', 1)
GO
INSERT [dbo].[Socios] ([IdSocio], [LegajoSocio], [Apellido], [Nombre], [Email], [FechaNacimiento], [CuotaMensual], [TipoSocio], [Activo]) VALUES (3, N'F-0001', N'López', N'Carlos', N'carlos.lopez@email.com', CAST(N'2015-11-08' AS Date), CAST(7000.00 AS Decimal(8, 2)), N'Menor', 1)
GO
INSERT [dbo].[Socios] ([IdSocio], [LegajoSocio], [Apellido], [Nombre], [Email], [FechaNacimiento], [CuotaMensual], [TipoSocio], [Activo]) VALUES (4, N'A-0214', N'Martínez', N'Ana', N'ana.martinez@email.com', CAST(N'1954-01-30' AS Date), CAST(5000.00 AS Decimal(8, 2)), N'Jubilado', 1)
GO
INSERT [dbo].[Socios] ([IdSocio], [LegajoSocio], [Apellido], [Nombre], [Email], [FechaNacimiento], [CuotaMensual], [TipoSocio], [Activo]) VALUES (5, N'S-0005', N'Rodríguez', N'Pedro', N'pedro.rodriguez@email.com', CAST(N'1978-09-18' AS Date), CAST(12500.00 AS Decimal(8, 2)), N'Mayor', 0)
GO
SET IDENTITY_INSERT [dbo].[Socios] OFF
GO
