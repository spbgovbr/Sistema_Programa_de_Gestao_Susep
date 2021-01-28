CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio] (
    [planoTrabalhoAtividadeCriterioId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoAtividadeId]         UNIQUEIDENTIFIER NOT NULL,
    [criterioId]                       INT              NOT NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoAtividadeCriterioId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoCriterioAtividade_Criterio] FOREIGN KEY ([criterioId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]),
    CONSTRAINT [FK_PlanoTrabalhoCriterioAtividade_PlanoTrabalhoAtividade] FOREIGN KEY ([planoTrabalhoAtividadeId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId])
);

