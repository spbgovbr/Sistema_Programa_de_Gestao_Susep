CREATE TABLE [ProgramaGestao].[PactoTrabalhoHistorico] (
    [pactoTrabalhoHistoricoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]          UNIQUEIDENTIFIER NOT NULL,
    [situacaoId]               INT              NOT NULL,
    [observacoes]              VARCHAR (2000)   NULL,
    [responsavelOperacao]      VARCHAR (50)     NOT NULL,
    [DataOperacao]             DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoHistoricoId] ASC),
    CONSTRAINT [FK_PactoTrabalhoHistorico_PactoTrabalho] FOREIGN KEY ([pactoTrabalhoId]) REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId]),
    CONSTRAINT [FK_PactoTrabalhoHistorico_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);



