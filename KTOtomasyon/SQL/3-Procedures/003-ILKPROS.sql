USE [KTOtomasyon]
GO

/****** Object:  StoredProcedure [dbo].[ILKPROS]    Script Date: 23.9.2018 02:17:54 ******/
DROP PROCEDURE [dbo].[ILKPROS]
GO

/****** Object:  StoredProcedure [dbo].[ILKPROS]    Script Date: 23.9.2018 02:17:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Zübeyir Koçalioðlu
-- Create date: 19.03.2018
-- Description:	Ýlk prosedürüm.
-- =============================================

--exec ILKPROS '01-01-2016', '01-01-2019'
CREATE PROCEDURE [dbo].[ILKPROS]
(
	@firstodate datetime,
	@lastodate datetime
)
AS
BEGIN
	SET NOCOUNT ON;
		
	SELECT SUM(ORDET.TotalPrice) Toplam FROM OrderDetail ORDET 
	INNER JOIN Orders ORS ON ORS.Order_Id = ORDET.Order_Id 

	WHERE ORS.OrderDate <= @lastodate AND ORS.OrderDate > @firstodate

	-- SELECT Price,Quantity,TotalPrice FROM OrderDetail
END
GO


