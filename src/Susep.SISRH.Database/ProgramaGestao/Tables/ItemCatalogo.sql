CREATE TABLE [ProgramaGestao].[ItemCatalogo] (
    [itemCatalogoId]        UNIQUEIDENTIFIER NOT NULL,
    [titulo]                VARCHAR (500)    NOT NULL,
    [calculoTempoId]        INT              NOT NULL,
    [permiteRemoto]         BIT              NOT NULL,
    [tempoPresencial]       NUMERIC (4, 1)   NULL,
    [tempoRemoto]           NUMERIC (4, 1)   NULL,
    [descricao]             VARCHAR (2000)   NULL,
    [complexidade]          VARCHAR (20)     NULL,
    [definicaoComplexidade] VARCHAR (200)    NULL,
    [entregasEsperadas]     VARCHAR (2000)   NULL,
    PRIMARY KEY CLUSTERED ([itemCatalogoId] ASC),
    CONSTRAINT [FK_ItemCatalogo_CalculoTempo] FOREIGN KEY ([calculoTempoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Entregas esperadas pela atividade', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogo', @level2type = N'COLUMN', @level2name = N'entregasEsperadas';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Definição de como a atividade deve ser avaliada para ser enquadrada na complexidade', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogo', @level2type = N'COLUMN', @level2name = N'definicaoComplexidade';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Título da complexidade do item de catálogo', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'ItemCatalogo', @level2type = N'COLUMN', @level2name = N'complexidade';

