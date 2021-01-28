CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico] (
    [planoTrabalhoAtividadeCandidatoHistoricoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoAtividadeCandidatoId]          UNIQUEIDENTIFIER NOT NULL,
    [situacaoId]                                 INT              NOT NULL,
    [data]                                       DATETIME         NOT NULL,
    [descricao]                                  VARCHAR (2000)   NULL,
    [responsavelOperacao]                        VARCHAR (25)     NOT NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoAtividadeCandidatoHistoricoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidatoHistorico_PlanoTrabalhoAtividadeCandidato] FOREIGN KEY ([planoTrabalhoAtividadeCandidatoId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] ([planoTrabalhoAtividadeCandidatoId]),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidatoHistorico_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);





