CREATE TABLE [ProgramaGestao].[PactoTrabalho] (
    [pactoTrabalhoId]          UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]          UNIQUEIDENTIFIER NOT NULL,
    [unidadeId]                BIGINT           NOT NULL,
    [pessoaId]                 BIGINT           NOT NULL,
    [dataInicio]               DATE             NOT NULL,
    [dataFim]                  DATE             NOT NULL,
    [formaExecucaoId]          INT              NOT NULL,
    [situacaoId]               INT              NOT NULL,
    [tempoComparecimento]      INT              NULL,
    [cargaHorariaDiaria]       INT              NOT NULL,
    [percentualExecucao]       NUMERIC (10, 2)  NULL,
    [relacaoPrevistoRealizado] NUMERIC (10, 2)  NULL,
    [avaliacaoId]              UNIQUEIDENTIFIER NULL,
    [tempoTotalDisponivel]     INT              NOT NULL,
    [termoAceite]              VARCHAR (MAX)    NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoId] ASC),
    CONSTRAINT [FK_PactoTrabalho_FormaExecucao] FOREIGN KEY ([formaExecucaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PactoTrabalho_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId]),
    CONSTRAINT [FK_PactoTrabalho_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId]),
    CONSTRAINT [FK_PactoTrabalho_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PactoTrabalho_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId])
);























