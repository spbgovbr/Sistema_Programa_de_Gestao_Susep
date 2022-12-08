CREATE TABLE [dbo].[CatalogoDominio] (
    [catalogoDominioId] INT           NOT NULL,
    [classificacao]     VARCHAR (50)  NOT NULL,
    [descricao]         VARCHAR (500) NOT NULL,
    [ativo]             BIT           NOT NULL,
    CONSTRAINT [PK_CatalogoDominio] PRIMARY KEY CLUSTERED ([catalogoDominioId] ASC)
);



