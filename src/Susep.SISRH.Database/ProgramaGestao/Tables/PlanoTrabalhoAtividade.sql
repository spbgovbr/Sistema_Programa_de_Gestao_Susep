CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividade] (
    [planoTrabalhoAtividadeId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]          UNIQUEIDENTIFIER NOT NULL,
    [modalidadeExecucaoId]     INT              NOT NULL,
    [quantidadeColaboradores]  INT              NOT NULL,
    [descricao]                VARCHAR (2000)   NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoAtividadeId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoAtividade_ModalidadeExecucao] FOREIGN KEY ([modalidadeExecucaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PlanoTrabalhoAtividade_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
);







