CREATE TABLE [ProgramaGestao].[PlanoTrabalhoReuniao] (
    [planoTrabalhoReuniaoId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]        UNIQUEIDENTIFIER NOT NULL,
    [data]                   DATETIME         NOT NULL,
    [titulo]                 VARCHAR (250)    NOT NULL,
    [descricao]              VARCHAR (250)    NULL,
    [planoTrabalhoObjetoId]  UNIQUEIDENTIFIER NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoReuniaoId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoReuniao_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId]),
    CONSTRAINT [FK_PlanoTrabalhoReuniao_PlanoTrabalhoObjeto] FOREIGN KEY ([planoTrabalhoObjetoId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId])
);








GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave estrangeira para a tabela Objeto', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PlanoTrabalhoReuniao', @level2type = N'COLUMN', @level2name = N'planoTrabalhoObjetoId';

