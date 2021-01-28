CREATE TABLE [ProgramaGestao].[Assunto] (
    [assuntoId]    UNIQUEIDENTIFIER NOT NULL,
    [valor]        VARCHAR (100)    NOT NULL,
    [assuntoPaiId] UNIQUEIDENTIFIER NULL,
    [ativo]        BIT              NOT NULL,
    CONSTRAINT [PK_Assunto] PRIMARY KEY CLUSTERED ([assuntoId] ASC),
    CONSTRAINT [FK_Assunto_AssuntoPai] FOREIGN KEY ([assuntoPaiId]) REFERENCES [ProgramaGestao].[Assunto] ([assuntoId]),
    CONSTRAINT [UQ_Valor] UNIQUE NONCLUSTERED ([valor] ASC)
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Indica se o assunto encontra-se ativo', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Assunto', @level2type = N'COLUMN', @level2name = N'ativo';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Assunto pai', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Assunto', @level2type = N'COLUMN', @level2name = N'assuntoPaiId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Campo valor', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Assunto', @level2type = N'COLUMN', @level2name = N'valor';


GO


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = 'Armazena os assuntos.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'Assunto';

