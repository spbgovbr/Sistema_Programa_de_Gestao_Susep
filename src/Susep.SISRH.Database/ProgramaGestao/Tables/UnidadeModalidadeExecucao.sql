CREATE TABLE [ProgramaGestao].[UnidadeModalidadeExecucao] (
    [unidadeModalidadeExecucaoId] UNIQUEIDENTIFIER NOT NULL,
    [unidadeId]                   BIGINT           NOT NULL,
    [modalidadeExecucaoId]        INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([unidadeModalidadeExecucaoId] ASC),
    CONSTRAINT [FK_UnidadeModalidadeExecucao_ModalidadeExecucao] FOREIGN KEY ([modalidadeExecucaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_UnidadeModalidadeExecucao_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId])
);



