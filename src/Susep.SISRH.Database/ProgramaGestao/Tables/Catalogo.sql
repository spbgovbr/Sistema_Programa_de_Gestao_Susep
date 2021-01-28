CREATE TABLE [ProgramaGestao].[Catalogo] (
    [catalogoId] UNIQUEIDENTIFIER NOT NULL,
    [unidadeId]  BIGINT           NOT NULL,
    PRIMARY KEY CLUSTERED ([catalogoId] ASC),
    CONSTRAINT [FK_Catalogo_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId])
);

