if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_tblCartShippingMethods_tblCartProductCategories]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[tblCartShippingMethods] DROP CONSTRAINT FK_tblCartShippingMethods_tblCartProductCategories;


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblCartProductCategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblCartProductCategories];




CREATE TABLE [dbo].[tblCartProductCategories] (
	[nCatKey] [int] IDENTITY (1, 1) NOT NULL ,
	[cCatSchemaName] [nvarchar] (50)  NULL ,
	[cCatForeignRef] [nvarchar] (50)  NULL ,
	[nCatParentId] [int] NULL ,
	[cCatName] [nvarchar] (255)  NULL ,
	[cCatDescription] [nvarchar] (50)  NULL ,
	[nAuditId] [int] NULL ,
	CONSTRAINT [PK_tblCartProductCategories] PRIMARY KEY  CLUSTERED 
	(
		[nCatKey]
	) WITH  FILLFACTOR = 90  ON [PRIMARY] 
) ON [PRIMARY]



