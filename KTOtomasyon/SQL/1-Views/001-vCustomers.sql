USE [KTOtomasyon]
GO

/****** Object:  View [dbo].[vCustomers]    Script Date: 23.9.2018 02:13:38 ******/
DROP VIEW [dbo].[vCustomers]
GO

/****** Object:  View [dbo].[vCustomers]    Script Date: 23.9.2018 02:13:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[vCustomers]
AS
SELECT  O.CustomerName,
		O.PhoneNumber 
FROM Orders O
GROUP BY O.Order_Id,O.PhoneNumber,O.CustomerName 
GO


