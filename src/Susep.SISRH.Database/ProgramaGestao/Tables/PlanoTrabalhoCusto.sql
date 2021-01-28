CREATE TABLE [ProgramaGestao].[PlanoTrabalhoCusto] (
    [planoTrabalhoCustoId]  UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]       UNIQUEIDENTIFIER NOT NULL,
    [valor]                 NUMERIC (9, 2)   NOT NULL,
    [descricao]             VARCHAR (100)    NOT NULL,
    [planoTrabalhoObjetoId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_PlanoTrabalhoCusto] PRIMARY KEY CLUSTERED ([planoTrabalhoCustoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoCusto_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId]),
    CONSTRAINT [FK_PlanoTrabalhoCusto_PlanoTrabalhoObjeto] FOREIGN KEY ([planoTrabalhoObjetoId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId])
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Descrição do custo.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoCusto', @level2type = N'COLUMN', @level2name = N'descricao';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Valor do custo.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoCusto', @level2type = N'COLUMN', @level2name = N'valor';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave estrangeira para a tabela PlanoTrabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoCusto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Chave primária da tabela PlanoTrabalhoCusto.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoCusto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoCustoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tabela de custos associados ao PlanoTrabalho', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoCusto';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela Objeto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoCusto', @level2type = N'COLUMN', @level2name = N'planoTrabalhoObjetoId';

