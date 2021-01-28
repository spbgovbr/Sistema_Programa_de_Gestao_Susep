CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto] (
    [planoTrabalhoAtividadeAssuntoId] UNIQUEIDENTIFIER NOT NULL,
    [assuntoId]                       UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoAtividadeId]        UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PlanoTrabalhoAtividadeAssunto] PRIMARY KEY CLUSTERED ([planoTrabalhoAtividadeAssuntoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeAssunto_Assunto] FOREIGN KEY ([assuntoId]) REFERENCES [ProgramaGestao].[Assunto] ([assuntoId]),
    CONSTRAINT [FK_PlanoTrabalhoAtividadeAssunto_PlanoTrabalhoAtividade] FOREIGN KEY ([planoTrabalhoAtividadeId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId]),
    CONSTRAINT [UK_PlanoTrabalhoAtividadeAssunto_Assunto_PlanoTrabalhoAtividade] UNIQUE NONCLUSTERED ([assuntoId] ASC, [planoTrabalhoAtividadeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela PlanoTrabalhoAtividade.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoAtividadeAssunto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoAtividadeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela Assunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoAtividadeAssunto', @level2type = N'COLUMN', @level2name = N'assuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela PlanoTrabalhoAtividadeAssunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoAtividadeAssunto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoAtividadeAssuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tabela de associação entre PlanoTrabalhoAtividade e Assunto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoAtividadeAssunto';

