CREATE TABLE [ProgramaGestao].[ItemCatalogo] (
    [itemCatalogoId]        UNIQUEIDENTIFIER NOT NULL,
    [titulo]                VARCHAR (250)    NOT NULL,
    [calculoTempoId]        INT              NOT NULL,
    [permiteRemoto]         BIT              NOT NULL,
    [tempoPresencial]       NUMERIC (4, 1)   NULL,
    [tempoRemoto]           NUMERIC (4, 1)   NULL,
    [descricao]             VARCHAR (2000)   NULL,
    [complexidade]          VARCHAR (20)     NULL,
    [definicaoComplexidade] VARCHAR (200)    NULL,
    [entregasEsperadas]     VARCHAR (200)    NULL,
    PRIMARY KEY CLUSTERED ([itemCatalogoId] ASC),
    CONSTRAINT [FK_ItemCatalogo_CalculoTempo] FOREIGN KEY ([calculoTempoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);





