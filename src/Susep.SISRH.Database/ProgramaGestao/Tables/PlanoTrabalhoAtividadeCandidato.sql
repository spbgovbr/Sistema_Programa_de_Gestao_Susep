CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] (
    [planoTrabalhoAtividadeCandidatoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoAtividadeId]          UNIQUEIDENTIFIER NOT NULL,
    [pessoaId]                          BIGINT           NOT NULL,
    [situacaoId]                        INT              NOT NULL,
    [termoAceite]                       VARCHAR (MAX)    NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoAtividadeCandidatoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId]),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_PlanoTrabalhoAtividade] FOREIGN KEY ([planoTrabalhoAtividadeId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId]),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_Situacao] FOREIGN KEY ([situacaoId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
);



