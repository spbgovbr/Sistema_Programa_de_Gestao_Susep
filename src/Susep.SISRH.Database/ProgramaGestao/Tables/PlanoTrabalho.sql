CREATE TABLE [ProgramaGestao].[PlanoTrabalho] (
    [planoTrabalhoId]      UNIQUEIDENTIFIER NOT NULL,
    [unidadeId]            BIGINT           NOT NULL,
    [dataInicio]           DATE             NOT NULL,
    [dataFim]              DATE             NOT NULL,
    [situacaoId]           INT              NOT NULL,
    [avaliacaoId]          UNIQUEIDENTIFIER NULL,
    [tempoComparecimento]  INT              NULL,
    [totalServidoresSetor] INT              NULL,
    [tempoFaseHabilitacao] INT              NULL,
    [termoAceite]          VARCHAR (MAX)    NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoId] ASC),
    CONSTRAINT [FK_PlanoTrabalho_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId]),
    CONSTRAINT [FK_PlatoTrabalho_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);

















