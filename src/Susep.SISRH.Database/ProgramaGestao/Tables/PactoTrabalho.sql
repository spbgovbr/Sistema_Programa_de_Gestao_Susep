CREATE TABLE [ProgramaGestao].[PactoTrabalho] (
    [pactoTrabalhoId]                             UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]                             UNIQUEIDENTIFIER NOT NULL,
    [unidadeId]                                   BIGINT           NOT NULL,
    [pessoaId]                                    BIGINT           NOT NULL,
    [dataInicio]                                  DATE             NOT NULL,
    [dataFim]                                     DATE             NOT NULL,
    [formaExecucaoId]                             INT              NOT NULL,
    [situacaoId]                                  INT              NOT NULL,
    [tempoComparecimento]                         INT              NULL,
    [cargaHorariaDiaria]                          INT              NOT NULL,
    [percentualExecucao]                          NUMERIC (10, 2)  NULL,
    [relacaoPrevistoRealizado]                    NUMERIC (10, 2)  NULL,
    [avaliacaoId]                                 UNIQUEIDENTIFIER NULL,
    [tempoTotalDisponivel]                        INT              NOT NULL,
    [termoAceite]                                 VARCHAR (MAX)    NULL,
    [tipoFrequenciaTeletrabalhoParcialId]         INT              NULL,
    [quantidadeDiasFrequenciaTeletrabalhoParcial] INT              NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoId] ASC),
    CONSTRAINT [FK_PactoTrabalho_FormaExecucao] FOREIGN KEY ([formaExecucaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PactoTrabalho_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId]),
    CONSTRAINT [FK_PactoTrabalho_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId]),
    CONSTRAINT [FK_PactoTrabalho_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PactoTrabalho_TipoFrequenciaTeletrabalhoParcial] FOREIGN KEY ([tipoFrequenciaTeletrabalhoParcialId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PactoTrabalho_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId])
);


























GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Termo de aceite assinado pelo servidor ao aceitar plano de trabalho', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalho', @level2type = N'COLUMN', @level2name = N'termoAceite';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o tipo de frequência ao qual o servidor em teletrabalho parcial terá que se submeter', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalho', @level2type = N'COLUMN', @level2name = N'tipoFrequenciaTeletrabalhoParcialId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a quantidade de dias que o servidor em teletrabalho parcial terá que se apresentar presencialmente', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalho', @level2type = N'COLUMN', @level2name = N'quantidadeDiasFrequenciaTeletrabalhoParcial';

