CREATE TABLE [ProgramaGestao].[PactoTrabalhoSolicitacao] (
    [pactoTrabalhoSolicitacaoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]            UNIQUEIDENTIFIER NOT NULL,
    [tipoSolicitacaoId]          INT              NOT NULL,
    [dataSolicitacao]            DATETIME         NOT NULL,
    [solicitante]                VARCHAR (50)     NOT NULL,
    [dadosSolicitacao]           VARCHAR (2000)   NOT NULL,
    [observacoesSolicitante]     VARCHAR (2000)   NULL,
    [analisado]                  BIT              NOT NULL,
    [dataAnalise]                DATETIME         NULL,
    [analista]                   VARCHAR (50)     NULL,
    [aprovado]                   BIT              NULL,
    [observacoesAnalista]        VARCHAR (2000)   NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoSolicitacaoId] ASC),
    CONSTRAINT [FK_PactoTrabalhoSolicitacao_PactoTrabalho] FOREIGN KEY ([pactoTrabalhoId]) REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId]),
    CONSTRAINT [FK_PactoTrabalhoSolicitacao_TipoSolicitacao] FOREIGN KEY ([tipoSolicitacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);

