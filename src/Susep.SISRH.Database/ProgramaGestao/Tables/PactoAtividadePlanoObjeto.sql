CREATE TABLE [ProgramaGestao].[PactoAtividadePlanoObjeto] (
    [pactoAtividadePlanoObjetoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoObjetoId]       UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoAtividadeId]    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PactoAtividadePlanoObjeto] PRIMARY KEY CLUSTERED ([pactoAtividadePlanoObjetoId] ASC),
    CONSTRAINT [FK_PactoAtividadePlanoObjeto_PactoTrabalhoAtividade] FOREIGN KEY ([pactoTrabalhoAtividadeId]) REFERENCES [ProgramaGestao].[PactoTrabalhoAtividade] ([pactoTrabalhoAtividadeId]),
    CONSTRAINT [FK_PactoAtividadePlanoObjeto_PlanoTrabalhoObjeto] FOREIGN KEY ([planoTrabalhoObjetoId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId]),
    CONSTRAINT [UQ_PlanoObjeto_PactoAtividade] UNIQUE NONCLUSTERED ([planoTrabalhoObjetoId] ASC, [pactoTrabalhoAtividadeId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela PactoTrabalhoAtividade', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoAtividadePlanoObjeto', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoAtividadeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela PlanoTrabalhoObjeto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoAtividadePlanoObjeto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoObjetoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela PactoAtividadePlanoObjeto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoAtividadePlanoObjeto', @level2type = N'COLUMN', @level2name = N'pactoAtividadePlanoObjetoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Tabela de relacionamento entre PlanoTrabalhoObjeto e PactoTrabalhoAtividade.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoAtividadePlanoObjeto';

