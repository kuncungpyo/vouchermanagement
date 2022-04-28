IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'vm_product'))
BEGIN
	CREATE TABLE [dbo].[vm_product] (
		[Id]									INT IDENTITY (1, 1) NOT NULL,
		[MerchantId]							INT NOT NULL,	
		[Name]									NVARCHAR (50) NOT NULL,		
		[Price]									DECIMAL NOT NULL,		
		[Color]									NVARCHAR (10) NOT NULL,		
		[Stock]								    INT NOT NULL DEFAULT 0,	
		PRIMARY KEY CLUSTERED ([Id] ASC),
		FOREIGN KEY (MerchantId) REFERENCES vm_merchant(Id)
	);
END
GO
