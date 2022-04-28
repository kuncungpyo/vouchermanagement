IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'vm_voucher'))
BEGIN
	CREATE TABLE [dbo].[vm_voucher] (
		[Id]									INT IDENTITY (1, 1) NOT NULL,
		[Code]									NVARCHAR (50) NOT NULL,		
		[Discount]								DECIMAL NOT NULL,	
		[DiscountType]							NVARCHAR (10) NOT NULL,
		[ExpiredDate]							DATETIME2(7) NOT NULL,	
		[Status]								NVARCHAR (10) NOT NULL,
		[LastUsedDate]							DATETIME2(7) NOT NULL
		PRIMARY KEY CLUSTERED ([Id] ASC)
	);
END
GO
