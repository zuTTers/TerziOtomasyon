USE [KTOtomasyon]
GO

/****** Object:  View [dbo].[vOrders]    Script Date: 23.9.2018 02:15:02 ******/
DROP VIEW [dbo].[vOrders]
GO

/****** Object:  View [dbo].[vOrders]    Script Date: 23.9.2018 02:15:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO





CREATE View [dbo].[vOrders]
as
SELECT Order_Id, CustomerName, PhoneNumber,Debt,Addition,OrderDate,IsDelivered,Discount,(Select sum(TotalPrice) from OrderDetail d where d.Order_Id = Orders.Order_Id) as NetTotal,((Select sum(TotalPrice) from OrderDetail d where d.Order_Id = Orders.Order_Id)-(Select sum(TotalPrice) from OrderDetail d where d.Order_Id = Orders.Order_Id)*Discount/100) TotalPrice
FROM dbo.Orders 
WHERE Orders.IsDeleted = '0'
GO


