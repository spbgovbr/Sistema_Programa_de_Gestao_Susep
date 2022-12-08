CREATE TABLE [ProgramaGestao].[PactoTrabalhoDeclaracao] (
    [pactoTrabalhoDeclaracaoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]           UNIQUEIDENTIFIER NOT NULL,
    [declaracaoId]              INT              NOT NULL,
    [dataExibicao]              DATETIME2 (7)    NULL,
    [aceita]                    BIT              NULL,
    [responsavelRegistro]       VARCHAR (50)     NOT NULL,
    [dataRegistro]              DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoDeclaracaoId] ASC),
    CONSTRAINT [FK_PactoTrabalhoDeclaracao_PactoTrabalho] FOREIGN KEY ([pactoTrabalhoId]) REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a data em que a declaração foi aceita.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'dataRegistro';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o responsável por aceitar a declaração.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'responsavelRegistro';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra se a declaração foi aceita.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'aceita';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a data em que a declaração foi exibida ao usuário.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'dataExibicao';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o ID da declaração.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'declaracaoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o plano de trabalho da declaração.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra declarações feitas pelos usuários dentro do plano de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoDeclaracaoId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra informações adicionais ao plano de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao';

