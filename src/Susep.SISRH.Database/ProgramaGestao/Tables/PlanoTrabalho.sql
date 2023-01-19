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




















GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Termo de aceite a ser assinado pelos servidores nesse programa de gestão', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalho', @level2type = N'COLUMN', @level2name = N'termoAceite';

