IF (NOT EXISTS (SELECT *  FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_SCHEMA = 'dbo' AND  TABLE_NAME = 'vm_voucher_rules'))
BEGIN
	CREATE TABLE [dbo].[vm_voucher_rules] (
		[Id]									INT IDENTITY (1, 1) NOT NULL,
		[VoucherId]							    INT NOT NULL,
		[ProductId]							    INT NULL,
		[Color]									NVARCHAR (50) NULL,		
		[MaximumPrice]						    DECIMAL NULL,		
		PRIMARY KEY CLUSTERED ([Id] ASC),
		FOREIGN KEY (VoucherId) REFERENCES vm_voucher(Id),
		FOREIGN KEY (ProductId) REFERENCES vm_product(Id)
	);
END
GO
