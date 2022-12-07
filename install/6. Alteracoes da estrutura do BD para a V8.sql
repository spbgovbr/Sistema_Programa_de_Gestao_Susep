

GO
PRINT N'Altering Table [dbo].[CatalogoDominio]...';


GO
ALTER TABLE [dbo].[CatalogoDominio] ALTER COLUMN [descricao] VARCHAR (500) NOT NULL;




GO
PRINT N'Altering Table [ProgramaGestao].[PactoTrabalho]...';


GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho]
    ADD [tipoFrequenciaTeletrabalhoParcialId]         INT NULL,
        [quantidadeDiasFrequenciaTeletrabalhoParcial] INT NULL;


GO
PRINT N'Altering Table [ProgramaGestao].[PactoTrabalhoAtividade]...';


GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade]
    ADD [responsavelAvaliacao] VARCHAR (50)  NULL,
        [dataAvaliacao]        DATETIME2 (0) NULL;





GO
PRINT N'Creating Table [ProgramaGestao].[AgendamentoPresencial]...';


GO
CREATE TABLE [ProgramaGestao].[AgendamentoPresencial] (
    [agendamentoPresencialId] UNIQUEIDENTIFIER NOT NULL,
    [pessoaId]                BIGINT           NOT NULL,
    [dataAgendada]            DATE             NOT NULL,
    CONSTRAINT [PK_AgendamentoPresencial] PRIMARY KEY CLUSTERED ([agendamentoPresencialId] ASC)
);


GO
PRINT N'Creating Table [ProgramaGestao].[PactoTrabalhoDeclaracao]...';


GO
CREATE TABLE [ProgramaGestao].[PactoTrabalhoDeclaracao] (
    [pactoTrabalhoDeclaracaoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]           UNIQUEIDENTIFIER NOT NULL,
    [declaracaoId]              INT              NOT NULL,
    [dataExibicao]              DATETIME2 (7)    NULL,
    [aceita]                    BIT              NULL,
    [responsavelRegistro]       VARCHAR (50)     NOT NULL,
    [dataRegistro]              DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoDeclaracaoId] ASC)
);


GO
PRINT N'Creating Table [ProgramaGestao].[PactoTrabalhoInformacao]...';


GO
CREATE TABLE [ProgramaGestao].[PactoTrabalhoInformacao] (
    [pactoTrabalhoInformacaoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]           UNIQUEIDENTIFIER NOT NULL,
    [informacao]                VARCHAR (2000)   NOT NULL,
    [dataExibicao]              DATETIME2 (7)    NULL,
    [aceita]                    BIT              NULL,
    [responsavelRegistro]       VARCHAR (50)     NOT NULL,
    [dataRegistro]              DATETIME2 (7)    NOT NULL,
    PRIMARY KEY CLUSTERED ([pactoTrabalhoInformacaoId] ASC)
);



GO
PRINT N'Creating Foreign Key [ProgramaGestao].[FK_AgendamentoPresencia_Pessoa]...';


GO
ALTER TABLE [ProgramaGestao].[AgendamentoPresencial] WITH NOCHECK
    ADD CONSTRAINT [FK_AgendamentoPresencia_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId]);


GO
PRINT N'Creating Foreign Key [ProgramaGestao].[FK_PactoTrabalhoDeclaracao_PactoTrabalho]...';


GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoDeclaracao] WITH NOCHECK
    ADD CONSTRAINT [FK_PactoTrabalhoDeclaracao_PactoTrabalho] FOREIGN KEY ([pactoTrabalhoId]) REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId]);


GO
PRINT N'Creating Foreign Key [ProgramaGestao].[FK_PactoTrabalhoInformacao_PactoTrabalho]...';


GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoInformacao] WITH NOCHECK
    ADD CONSTRAINT [FK_PactoTrabalhoInformacao_PactoTrabalho] FOREIGN KEY ([pactoTrabalhoId]) REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId]);


GO
PRINT N'Creating Foreign Key [ProgramaGestao].[FK_PactoTrabalho_TipoFrequenciaTeletrabalhoParcial]...';


GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho] WITH NOCHECK
    ADD CONSTRAINT [FK_PactoTrabalho_TipoFrequenciaTeletrabalhoParcial] FOREIGN KEY ([tipoFrequenciaTeletrabalhoParcialId]) REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId]);


GO
PRINT N'Refreshing View [ProgramaGestao].[VW_AssuntoChaveCompleta]...';


GO
EXECUTE sp_refreshsqlmodule N'[ProgramaGestao].[VW_AssuntoChaveCompleta]';


GO
PRINT N'Altering View [dbo].[VW_UnidadeSiglaCompleta]...';


GO


ALTER VIEW [dbo].[VW_UnidadeSiglaCompleta]
as
WITH cte
AS
(
	SELECT	u.unidadeId, u.undNivel, u.undSigla as undSiglaCompleta, u.undSigla, u.Email, u.undCodigoSIORG, u.pessoaIdChefe, u.pessoaIdChefeSubstituto
	FROM	Unidade as u
	WHERE	u.unidadeIdPai is null and u.situacaoUnidadeId = 1
	UNION ALL
	SELECT	u.unidadeId, u.undNivel, cast(cte.undSiglaCompleta+'/'+u.undSigla as varchar(50)), u.undSigla, u.Email, u.undCodigoSIORG, u.pessoaIdChefe, u.pessoaIdChefeSubstituto
	FROM	Unidade as u
	INNER JOIN cte ON u.unidadeIdPai = cte.unidadeId
)
SELECT und.unidadeId, und.undSigla, und.undDescricao, und.unidadeIdPai, und.tipoUnidadeId,
und.situacaoUnidadeId, und.ufId, und.undNivel, und.tipoFuncaoUnidadeId, cte.undSiglaCompleta, und.Email, und.undCodigoSIORG
, und.pessoaIdChefe, und.pessoaIdChefeSubstituto
FROM cte 
INNER JOIN unidade und on cte.unidadeId = und.unidadeId
GO
PRINT N'Creating Extended Property [dbo].[Pessoa].[cargaHoraria].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Define a carga horária de trabalho de uma pessoa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pessoa', @level2type = N'COLUMN', @level2name = N'cargaHoraria';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[AgendamentoPresencial].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tabela que registra os agendamentos para comparcimento presencial realizados por servidores', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[AgendamentoPresencial].[agendamentoPresencialId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador único do agendamento de comparecimento', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial', @level2type = N'COLUMN', @level2name = N'agendamentoPresencialId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[AgendamentoPresencial].[pessoaId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador da pessoa para a qual o agendamento foi realizado', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial', @level2type = N'COLUMN', @level2name = N'pessoaId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[AgendamentoPresencial].[dataAgendada].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data agendada para comparecimento presencial', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial', @level2type = N'COLUMN', @level2name = N'dataAgendada';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra informações adicionais ao plano de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[pactoTrabalhoDeclaracaoId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra declarações feitas pelos usuários dentro do plano de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoDeclaracaoId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[pactoTrabalhoId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o plano de trabalho da declaração.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[declaracaoId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o ID da declaração.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'declaracaoId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[dataExibicao].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a data em que a declaração foi exibida ao usuário.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'dataExibicao';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[aceita].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra se a declaração foi aceita.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'aceita';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[responsavelRegistro].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o responsável por aceitar a declaração.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'responsavelRegistro';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoDeclaracao].[dataRegistro].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a data em que a declaração foi aceita.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoDeclaracao', @level2type = N'COLUMN', @level2name = N'dataRegistro';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoInformacao].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra informações adicionais ao plano de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoInformacao].[pactoTrabalhoInformacaoId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Exibe o ID do registro da informação.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoInformacaoId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoInformacao].[pactoTrabalhoId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Associa a informação a um pacto de trabalho.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'pactoTrabalhoId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoInformacao].[informacao].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a informação adicionada.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'informacao';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoInformacao].[responsavelRegistro].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o responsável pela informação.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'responsavelRegistro';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoInformacao].[dataRegistro].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a data da informação.', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoInformacao', @level2type = N'COLUMN', @level2name = N'dataRegistro';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalho].[quantidadeDiasFrequenciaTeletrabalhoParcial].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra a quantidade de dias que o servidor em teletrabalho parcial terá que se apresentar presencialmente', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalho', @level2type = N'COLUMN', @level2name = N'quantidadeDiasFrequenciaTeletrabalhoParcial';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalho].[tipoFrequenciaTeletrabalhoParcialId].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o tipo de frequência ao qual o servidor em teletrabalho parcial terá que se submeter', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalho', @level2type = N'COLUMN', @level2name = N'tipoFrequenciaTeletrabalhoParcialId';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoAtividade].[dataAvaliacao].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data da avaliação da atividade', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoAtividade', @level2type = N'COLUMN', @level2name = N'dataAvaliacao';


GO
PRINT N'Creating Extended Property [ProgramaGestao].[PactoTrabalhoAtividade].[responsavelAvaliacao].[MS_Description]...';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Responsável pela avaliação da atividade', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'PactoTrabalhoAtividade', @level2type = N'COLUMN', @level2name = N'responsavelAvaliacao';


GO
PRINT N'Checking existing data against newly created constraints';


GO
PRINT N'Update complete.';


GO




insert into CatalogoDominio values (10101, 'TipoFrequenciaTeletrabalhoParcial', 'Indefinida', 1)
insert into CatalogoDominio values (10102, 'TipoFrequenciaTeletrabalhoParcial', 'Semanal', 1)
insert into CatalogoDominio values (10103, 'TipoFrequenciaTeletrabalhoParcial', 'Quinzenal', 1)
insert into CatalogoDominio values (10104, 'TipoFrequenciaTeletrabalhoParcial', 'Mensal', 1)
																				