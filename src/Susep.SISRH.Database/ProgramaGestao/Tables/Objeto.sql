CREATE TABLE [ProgramaGestao].[Objeto] (
    [objetoId]  UNIQUEIDENTIFIER NOT NULL,
    [descricao] VARCHAR (100)    NOT NULL,
    [tipo]      INT              NOT NULL,
    [chave]     VARCHAR (20)     NOT NULL,
    [ativo]     BIT              NOT NULL,
    CONSTRAINT [PK_Objeto] PRIMARY KEY CLUSTERED ([objetoId] ASC),
    CONSTRAINT [UQ_Objeto_Chave] UNIQUE NONCLUSTERED ([chave] ASC)
);




GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Campo chave', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Objeto', @level2type = N'COLUMN', @level2name = N'chave';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campo tipo', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Objeto', @level2type = N'COLUMN', @level2name = N'tipo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campo descrição', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Objeto', @level2type = N'COLUMN', @level2name = N'descricao';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Armazena os objetos.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Objeto';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica se o objeto encontra-se ativo', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Objeto', @level2type = N'COLUMN', @level2name = N'ativo';

