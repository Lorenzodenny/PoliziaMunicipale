ReadME 

- Importante il Data Base

Aprire SQL, click destro su Database, fare "importa applicazioni livello dati", clickare avanti, sfoglia e cercare il file PoliziaMunicipale.bacpac, fare avanti fino alla fine, godetevi il DB!

nome File DataBase DbPolizia.bacpac




Altrimenti

Tabelle 

Anagrafica

USE [PoliziaMunicipale]
GO

/****** Object:  Table [dbo].[Anagrafica]    Script Date: 01/03/2024 16:15:47 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Anagrafica](
	[IDAnagrafica] [int] IDENTITY(1,1) NOT NULL,
	[Cognome] [varchar](50) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Indirizzo] [varchar](50) NOT NULL,
	[Citta] [varchar](50) NOT NULL,
	[CAP] [nchar](5) NOT NULL,
	[Cod_Fisc] [nchar](16) NOT NULL,
 CONSTRAINT [PK_Anagrafica] PRIMARY KEY CLUSTERED 
(
	[IDAnagrafica] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO




Verbale

USE [PoliziaMunicipale]
GO

/****** Object:  Table [dbo].[Verbale]    Script Date: 01/03/2024 16:16:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Verbale](
	[IDVerbale] [int] IDENTITY(1,1) NOT NULL,
	[DataViolazione] [date] NOT NULL,
	[IndirizzoViolazione] [varchar](50) NOT NULL,
	[Nominativo_Agente] [varchar](50) NOT NULL,
	[DataTrascrizioneVerbale] [date] NOT NULL,
	[Importo] [money] NOT NULL,
	[DecurtamentoPunti] [int] NOT NULL,
	[IDAnagrafica] [int] NOT NULL,
	[IDViolazione] [int] NOT NULL,
 CONSTRAINT [PK_Verbale] PRIMARY KEY CLUSTERED 
(
	[IDVerbale] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Verbale]  WITH CHECK ADD  CONSTRAINT [FK_Verbale_Anagrafica1] FOREIGN KEY([IDAnagrafica])
REFERENCES [dbo].[Anagrafica] ([IDAnagrafica])
GO

ALTER TABLE [dbo].[Verbale] CHECK CONSTRAINT [FK_Verbale_Anagrafica1]
GO

ALTER TABLE [dbo].[Verbale]  WITH CHECK ADD  CONSTRAINT [FK_Verbale_TipoViolazione] FOREIGN KEY([IDViolazione])
REFERENCES [dbo].[Violazione] ([IDViolazione])
GO

ALTER TABLE [dbo].[Verbale] CHECK CONSTRAINT [FK_Verbale_TipoViolazione]
GO


Violazione

USE [PoliziaMunicipale]
GO

/****** Object:  Table [dbo].[Violazione]    Script Date: 01/03/2024 16:16:50 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Violazione](
	[IDViolazione] [int] IDENTITY(1,1) NOT NULL,
	[Descrizione] [varchar](255) NOT NULL,
 CONSTRAINT [PK_TipoViolazione] PRIMARY KEY CLUSTERED 
(
	[IDViolazione] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO



