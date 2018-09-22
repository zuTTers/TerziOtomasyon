USE [KTOtomasyon]
GO

/****** Object:  View [dbo].[vLastTotalOrder]    Script Date: 23.9.2018 02:14:10 ******/
DROP VIEW [dbo].[vLastTotalOrder]
GO

/****** Object:  View [dbo].[vLastTotalOrder]    Script Date: 23.9.2018 02:14:10 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO




CREATE VIEW [dbo].[vLastTotalOrder]
AS
  select ISNULL(ROW_NUMBER() OVER(ORDER BY SipMiktar),0) Sira
		 ,SipMiktar
		 ,SipTutar 
  from vTotalOrder 
  where Yil = YEAR(GETDATE()) 
    and Ay = MONTH(GETDATE()) 
	and Hafta = DATEPART(WW,GETDATE())
GO


