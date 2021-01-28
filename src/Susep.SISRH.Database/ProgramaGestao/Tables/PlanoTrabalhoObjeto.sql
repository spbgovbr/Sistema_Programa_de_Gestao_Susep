CREATE TABLE [ProgramaGestao].[PlanoTrabalhoObjeto] (
    [planoTrabalhoObjetoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]       UNIQUEIDENTIFIER NOT NULL,
    [objetoId]              UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_PlanoTrabalhoObjeto] PRIMARY KEY CLUSTERED ([planoTrabalhoObjetoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoObjeto_Objeto] FOREIGN KEY ([objetoId]) REFERENCES [ProgramaGestao].[Objeto] ([objetoId]),
    CONSTRAINT [FK_PlanoTrabalhoObjeto_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId]),
    CONSTRAINT [UQ_PlanoTrabalho_Objeto] UNIQUE NONCLUSTERED ([planoTrabalhoId] ASC, [objetoId] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela Objeto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjeto', @level2type = N'COLUMN', @level2name = N'objetoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela PlanoTrabalho', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjeto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela PlanoTrabalhoObjeto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjeto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoObjetoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Tabela de relacionamento entre PlanoTrabalho e Objeto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoObjeto';

