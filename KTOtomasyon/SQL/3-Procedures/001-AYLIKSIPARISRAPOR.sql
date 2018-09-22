USE [KTOtomasyon]
GO

/****** Object:  StoredProcedure [dbo].[AYLIKSIPARISRAPOR]    Script Date: 23.9.2018 02:16:48 ******/
DROP PROCEDURE [dbo].[AYLIKSIPARISRAPOR]
GO

/****** Object:  StoredProcedure [dbo].[AYLIKSIPARISRAPOR]    Script Date: 23.9.2018 02:16:48 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zübeyir KOÇALÝOÐLU
-- Create date: 22.03.2018
-- Description:	Aylýk Bazda Sipariþ Tutarý
-- =============================================

-- exec [AYLIKSIPARISRAPOR]

CREATE PROCEDURE [dbo].[AYLIKSIPARISRAPOR]
AS
BEGIN
	SET NOCOUNT ON;
		
	SELECT MONTH(ORS.OrderDate) Ay,Sum(ORDET.TotalPrice) ToplamSatis FROM OrderDetail ORDET 
	INNER JOIN Orders ORS ON ORS.Order_Id = ORDET.Order_Id 

	WHERE ORS.OrderDate BETWEEN '01.01.2018' AND '01.01.2019' 
	group by  MONTH(ORS.OrderDate)

	-- select Sum(ORDET.TotalPrice) Toplam FROM OrderDetail ORDET 
END
GO


