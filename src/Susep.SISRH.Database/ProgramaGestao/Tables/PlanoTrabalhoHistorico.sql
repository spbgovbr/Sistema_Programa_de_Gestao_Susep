CREATE TABLE [ProgramaGestao].[PlanoTrabalhoHistorico] (
    [planoTrabalhoHistoricoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]          UNIQUEIDENTIFIER NOT NULL,
    [situacaoId]               INT              NOT NULL,
    [observacoes]              VARCHAR (2000)   NULL,
    [responsavelOperacao]      VARCHAR (50)     NOT NULL,
    [DataOperacao]             DATETIME         NOT NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoHistoricoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoHistorico_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId]),
    CONSTRAINT [FK_PlanoTrabalhoHistorico_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);





