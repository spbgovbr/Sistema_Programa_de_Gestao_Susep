CREATE TABLE [ProgramaGestao].[PlanoTrabalhoObjetoAssunto] (
    [planoTrabalhoObjetoAssuntoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoObjetoId]        UNIQUEIDENTIFIER NOT NULL,
    [assuntoId]                    UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PlanoTrabalhoObjetoAssunto] PRIMARY KEY CLUSTERED ([planoTrabalhoObjetoAssuntoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoObjetoAssunto_Assunto] FOREIGN KEY ([assuntoId]) REFERENCES [ProgramaGestao].[Assunto] ([assuntoId]),
    CONSTRAINT [FK_PlanoTrabalhoObjetoAssunto_PlanoTrabalhoObjeto] FOREIGN KEY ([planoTrabalhoObjetoId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId]),
    CONSTRAINT [UQ_PlanoTrabalhoObjeto_Assunto] UNIQUE NONCLUSTERED ([planoTrabalhoObjetoId] ASC, [assuntoId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela Assunto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjetoAssunto', @level2type = N'COLUMN', @level2name = N'assuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela PlanoTrabalhoObjeto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjetoAssunto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoObjetoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela PlanoTrabalhoObjetoAssunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjetoAssunto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoObjetoAssuntoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Tabela de relacionamento entre PlanoTrabalhoObjeto e Assunto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjetoAssunto';

