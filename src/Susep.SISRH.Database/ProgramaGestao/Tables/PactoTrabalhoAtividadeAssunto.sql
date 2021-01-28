CREATE TABLE [ProgramaGestao].[PactoTrabalhoAtividadeAssunto] (
    [pactoTrabalhoAtividadeAssuntoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoAtividadeId]        UNIQUEIDENTIFIER NOT NULL,
    [assuntoId]                       UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PactoTrabalhoAtividadeAssunto] PRIMARY KEY CLUSTERED ([pactoTrabalhoAtividadeAssuntoId] ASC),
    CONSTRAINT [FK_PactoTrabalhoAtividadeAssunto_Assunto] FOREIGN KEY ([assuntoId]) REFERENCES [ProgramaGestao].[Assunto] ([assuntoId]),
    CONSTRAINT [FK_PactoTrabalhoAtividadeAssunto_PactoTrabalhoAtividade] FOREIGN KEY ([pactoTrabalhoAtividadeId]) REFERENCES [ProgramaGestao].[PactoTrabalhoAtividade] ([pactoTrabalhoAtividadeId]),
    CONSTRAINT [UK_PactoTrabalhoAtividadeAssunto_Assunto_PactoTrabalhoAtividade] UNIQUE NONCLUSTERED ([assuntoId] ASC, [pactoTrabalhoAtividadeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela Assunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoAtividadeAssunto', @level2type = N'COLUMN', @level2name = N'assuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela PactoTrabalhoAtividade.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoAtividadeAssunto', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoAtividadeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela PactoTrabalhoAtividadeAssunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoAtividadeAssunto', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoAtividadeAssuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tabela de assuntos associados às atividades do PactoTrabalho', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoAtividadeAssunto';

