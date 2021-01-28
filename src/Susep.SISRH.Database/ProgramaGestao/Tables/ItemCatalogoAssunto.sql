CREATE TABLE [ProgramaGestao].[ItemCatalogoAssunto] (
    [itemCatalogoAssuntoId] UNIQUEIDENTIFIER NOT NULL,
    [assuntoId]             UNIQUEIDENTIFIER NOT NULL,
    [itemCatalogoId]        UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Item_Catalogo_Assunto] PRIMARY KEY CLUSTERED ([itemCatalogoAssuntoId] ASC),
    CONSTRAINT [FK_ItemCatalogoAssunto_Assunto] FOREIGN KEY ([assuntoId]) REFERENCES [ProgramaGestao].[Assunto] ([assuntoId]),
    CONSTRAINT [FK_ItemCatalogoAssunto_ItemCatalogo] FOREIGN KEY ([itemCatalogoId]) REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId]),
    CONSTRAINT [UK_ItemCatalogoAssunto_Assunto_ItemCatalogo] UNIQUE NONCLUSTERED ([assuntoId] ASC, [itemCatalogoId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela ItemCatalogo.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogoAssunto', @level2type = N'COLUMN', @level2name = N'itemCatalogoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela Assunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogoAssunto', @level2type = N'COLUMN', @level2name = N'assuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela ItemCatalogoAssunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogoAssunto', @level2type = N'COLUMN', @level2name = N'itemCatalogoAssuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tabela de associação entre ItemCatalogo e Assunto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogoAssunto';

