USE [BazeProjekat]
GO
/****** Object:  Trigger [dbo].[NullNum]    Script Date: 6/2/2021 9:06:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[NullNum]
ON [dbo].[Let]
FOR INSERT
AS 
BEGIN
	DECLARE @varchar AS varchar(50);
	DECLARE @Var INT
	SET @Var =  [dbo].[fnVratiNullBr]()
	IF(@Var > 2)
	BEGIN
		SET @varchar = 'jos ' + CAST(@Var AS VARCHAR) + ' NULL vrednosti'
		RAISERROR(@varchar,16,1)
	END
END
/************************** ******************************************/
USE [BazeProjekat]
GO
/****** Object:  Trigger [dbo].[GRESKA]    Script Date: 6/2/2021 9:06:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TRIGGER [dbo].[GRESKA]
ON [dbo].[Radnik]
INSTEAD OF INSERT
AS 
BEGIN
	RAISERROR('IMAMO DOVOLJNO ZAPOSLENIH',16,1)
END
/************************** ******************************************/
USE [BazeProjekat]
GO
/****** Object:  StoredProcedure [dbo].[spPilotAvion]    Script Date: 6/2/2021 9:07:29 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER PROCEDURE  [dbo].[spPilotAvion]
AS
	SELECT * FROM dbo.Pilot LEFT JOIN dbo.Upravljati 
		ON dbo.Pilot.JMBG=dbo.Upravljati.JMBG LEFT JOIN dbo.Avion 
		ON dbo.Upravljati.AID = dbo.Avion.AID
/************************** ******************************************/
USE [BazeProjekat]
GO
/****** Object:  UserDefinedFunction [dbo].[fnVratiNullBr]    Script Date: 6/2/2021 9:08:12 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER FUNCTION [dbo].[fnVratiNullBr]
	(

	)
RETURNS INT
AS
BEGIN 
	DECLARE @Cursor INT
	SET @Cursor = 0

	WHILE @Cursor < (SELECT COUNT(*) FROM dbo.Let WHERE JMBG_FK IS NULL)
	BEGIN
		SET @Cursor = @Cursor + 1
	END
	RETURN @Cursor
END
