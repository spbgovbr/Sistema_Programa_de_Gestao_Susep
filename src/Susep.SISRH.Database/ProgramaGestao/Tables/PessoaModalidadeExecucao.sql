CREATE TABLE [ProgramaGestao].[PessoaModalidadeExecucao] (
    [pessoaModalidadeExecucaoId] UNIQUEIDENTIFIER NOT NULL,
    [pessoaId]                   BIGINT           NOT NULL,
    [modalidadeExecucaoId]       INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([pessoaModalidadeExecucaoId] ASC),
    CONSTRAINT [FK_PessoaModalidadeExecucao_ModalidadeExecucao] FOREIGN KEY ([modalidadeExecucaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PessoaModalidadeExecucao_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId])
);



