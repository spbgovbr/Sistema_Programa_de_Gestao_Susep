CREATE TABLE [ProgramaGestao].[PactoTrabalhoInformacao] (
    [pactoTrabalhoInformacaoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]           UNIQUEIDENTIFIER NOT NULL,
    [informacao]                VARCHAR (2000)   NOT NULL,
    [dataExibicao]              DATETIME2 (7)    NULL,
    [aceita]                    BIT              NULL,
    [responsavelRegistro]       VARCHAR (50)     NOT NULL,
    [dataRegistro]              DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoInformacaoId] ASC),
    CONSTRAINT [FK_PactoTrabalhoInformacao_PactoTrabalho] FOREIGN KEY ([pactoTrabalhoId]) REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a data da informação.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'dataRegistro';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o responsável pela informação.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'responsavelRegistro';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a informação adicionada.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'informacao';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Associa a informação a um pacto de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Exibe o ID do registro da informação.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoInformacaoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra informações adicionais ao plano de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao';

