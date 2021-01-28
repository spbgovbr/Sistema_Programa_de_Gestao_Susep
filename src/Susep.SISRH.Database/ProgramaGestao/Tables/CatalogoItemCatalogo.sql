CREATE TABLE [ProgramaGestao].[CatalogoItemCatalogo] (
    [catalogoItemCatalogoId] UNIQUEIDENTIFIER NOT NULL,
    [catalogoId]             UNIQUEIDENTIFIER NOT NULL,
    [itemCatalogoId]         UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([catalogoItemCatalogoId] ASC),
    CONSTRAINT [FK_CatalogoItemCatalogo_Catalogo] FOREIGN KEY ([catalogoId]) REFERENCES [ProgramaGestao].[Catalogo] ([catalogoId]),
    CONSTRAINT [FK_CatalogoItemCatalogo_ItemCatalogo] FOREIGN KEY ([itemCatalogoId]) REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId])
);

