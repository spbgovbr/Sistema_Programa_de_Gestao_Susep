ALTER TABLE ProgramaGestao.ItemCatalogo
ALTER COLUMN [entregasEsperadas]     VARCHAR (2000)    NULL

ALTER TABLE ProgramaGestao.ItemCatalogo
ALTER COLUMN [titulo]                VARCHAR (500)    NOT NULL


ALTER TABLE ProgramaGestao.PactoTrabalhoAtividade
	ADD modalidadeExecucaoId     INT              NULL
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividade', @level2type=N'COLUMN',
@level2name=N'modalidadeExecucaoId', @value=N'Registra a modalidade em que a atividade foi executada'
GO


ALTER TABLE dbo.Unidade
	ADD pessoaIdChefe bigint NULL,
		pessoaIdChefeSubstituto bigint NULL

EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidade', @level2type=N'COLUMN',
@level2name=N'pessoaIdChefe', @value=N'Registra o ID da pessoa que é o chefe da unidade'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidade', @level2type=N'COLUMN',
@level2name=N'pessoaIdChefeSubstituto', @value=N'Registra o ID da pessoa que é o chefe substituto da unidade'
GO



CREATE TABLE [dbo].[SituacaoPessoa](
	[situacaoPessoaId] [bigint] NOT NULL,
	[spsDescricao] [varchar](50) NOT NULL,
 CONSTRAINT [PK_SituacaoPessoa] PRIMARY KEY CLUSTERED 
(
	[situacaoPessoaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO



insert into [dbo].[SituacaoPessoa] values (1 , 'Ativa')
insert into [dbo].[SituacaoPessoa] values (4 , 'Cedida')
insert into [dbo].[SituacaoPessoa] values (5 , 'Desligada')
insert into [dbo].[SituacaoPessoa] values (2 , 'Falecida')
insert into [dbo].[SituacaoPessoa] values (3 , 'Inativa')

GO


CREATE TABLE [dbo].[TipoVinculo](
	[tipoVinculoId] [bigint] NOT NULL,
	[tvnDescricao] [varchar](150) NOT NULL,
 CONSTRAINT [PK_TipoVinculo] PRIMARY KEY CLUSTERED 
(
	[tipoVinculoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_TipoVinculo_tvnDescricao] UNIQUE NONCLUSTERED 
(
	[tvnDescricao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO



ALTER TABLE [dbo].[Pessoa]
	ADD 	[situacaoPessoaId] [bigint] NULL,
		[tipoVinculoId] [bigint] NULL
GO