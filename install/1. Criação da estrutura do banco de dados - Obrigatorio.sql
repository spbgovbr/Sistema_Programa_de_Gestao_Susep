CREATE SCHEMA [ProgramaGestao]
GO

CREATE TABLE [dbo].[CatalogoDominio](
	[catalogoDominioId] [int] NOT NULL,
	[classificacao] [varchar](50) NOT NULL,
	[descricao] [varchar](100) NOT NULL,
	[ativo] [bit] NOT NULL,
 CONSTRAINT [PK_CatalogoDominio] PRIMARY KEY CLUSTERED 
(
	[catalogoDominioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Feriado]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Feriado](
	[feriadoId] [bigint] IDENTITY(1,1) NOT NULL,
	[ferData] [datetime] NOT NULL,
	[ferFixo] [bit] NOT NULL,
	[ferDescricao] [varchar](50) NOT NULL,
	[ufId] [varchar](2) NULL,
 CONSTRAINT [PK_Feriado] PRIMARY KEY CLUSTERED 
(
	[feriadoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Pessoa]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Pessoa](
	[pessoaId] [bigint] IDENTITY(1,1) NOT NULL,
	[pesNome] [varchar](150) NOT NULL,
	[pesCPF] [varchar](15) NOT NULL,
	[pesDataNascimento] [datetime] NOT NULL,
	[pesMatriculaSiape] [varchar](12) NULL,
	[pesEmail] [varchar](150) NULL,
	[unidadeId] [bigint] NOT NULL,
	[tipoFuncaoId] [bigint] NULL,
	[cargaHoraria] [int] NULL,
 CONSTRAINT [PK_Pessoa] PRIMARY KEY CLUSTERED 
(
	[pessoaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Pessoa_pesCPF] UNIQUE NONCLUSTERED 
(
	[pesCPF] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TipoFuncao]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TipoFuncao](
	[tipoFuncaoId] [bigint] NOT NULL,
	[tfnDescricao] [varchar](150) NOT NULL,
	[tfnCodigoFuncao] [varchar](5) NULL,
	[tfnIndicadorChefia] [bit] NULL,
 CONSTRAINT [PK_TipoFuncao] PRIMARY KEY CLUSTERED 
(
	[tipoFuncaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_TipoFuncao_tfnDescricao] UNIQUE NONCLUSTERED 
(
	[tfnDescricao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UF]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UF](
	[ufId] [varchar](2) NOT NULL,
	[ufDescricao] [varchar](50) NOT NULL,
 CONSTRAINT [PK_UnidadeFederativa] PRIMARY KEY CLUSTERED 
(
	[ufId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_UnidadeFederativa_ufDescricao] UNIQUE NONCLUSTERED 
(
	[ufDescricao] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Unidade]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Unidade](
	[unidadeId] [bigint] IDENTITY(1,1) NOT NULL,
	[undSigla] [varchar](50) NOT NULL,
	[undDescricao] [varchar](150) NOT NULL,
	[unidadeIdPai] [bigint] NULL,
	[tipoUnidadeId] [bigint] NOT NULL,
	[situacaoUnidadeId] [bigint] NOT NULL,
	[ufId] [varchar](2) NOT NULL,
	[undNivel] [smallint] NULL,
	[tipoFuncaoUnidadeId] [bigint] NULL,
	[Email] [varchar](150) NULL,
	[undCodigoSIORG] [int] NOT NULL,
 CONSTRAINT [PK_Unidade] PRIMARY KEY CLUSTERED 
(
	[unidadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[Assunto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[Assunto](
	[assuntoId] [uniqueidentifier] NOT NULL,
	[chave] [varchar](10) NOT NULL,
	[valor] [varchar](100) NOT NULL,
	[assuntoPaiId] [uniqueidentifier] NULL,
	[ativo] [bit] NOT NULL,
 CONSTRAINT [PK_Assunto] PRIMARY KEY CLUSTERED 
(
	[assuntoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Chave] UNIQUE NONCLUSTERED 
(
	[chave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[Catalogo]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[Catalogo](
	[catalogoId] [uniqueidentifier] NOT NULL,
	[unidadeId] [bigint] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[catalogoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[CatalogoItemCatalogo]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[CatalogoItemCatalogo](
	[catalogoItemCatalogoId] [uniqueidentifier] NOT NULL,
	[catalogoId] [uniqueidentifier] NOT NULL,
	[itemCatalogoId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[catalogoItemCatalogoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[ItemCatalogo]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[ItemCatalogo](
	[itemCatalogoId] [uniqueidentifier] NOT NULL,
	[titulo] [varchar](250) NOT NULL,
	[calculoTempoId] [int] NOT NULL,
	[permiteRemoto] [bit] NOT NULL,
	[tempoPresencial] [numeric](4, 1) NULL,
	[tempoRemoto] [numeric](4, 1) NULL,
	[descricao] [varchar](2000) NULL,
	[complexidade] [varchar](20) NULL,
	[definicaoComplexidade] [varchar](200) NULL,
	[entregasEsperadas] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[itemCatalogoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[ItemCatalogoAssunto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[ItemCatalogoAssunto](
	[itemCatalogoAssuntoId] [uniqueidentifier] NOT NULL,
	[assuntoId] [uniqueidentifier] NOT NULL,
	[itemCatalogoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Item_Catalogo_Assunto] PRIMARY KEY CLUSTERED 
(
	[itemCatalogoAssuntoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_ItemCatalogoAssunto_Assunto_ItemCatalogo] UNIQUE NONCLUSTERED 
(
	[assuntoId] ASC,
	[itemCatalogoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[Objeto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[Objeto](
	[objetoId] [uniqueidentifier] NOT NULL,
	[descricao] [varchar](100) NOT NULL,
	[tipo] [int] NOT NULL,
	[chave] [varchar](20) NOT NULL,
	[ativo] [bit] NOT NULL,
 CONSTRAINT [PK_Objeto] PRIMARY KEY CLUSTERED 
(
	[objetoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_Objeto_Chave] UNIQUE NONCLUSTERED 
(
	[chave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PactoAtividadePlanoObjeto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PactoAtividadePlanoObjeto](
	[pactoAtividadePlanoObjetoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoObjetoId] [uniqueidentifier] NOT NULL,
	[pactoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PactoAtividadePlanoObjeto] PRIMARY KEY CLUSTERED 
(
	[pactoAtividadePlanoObjetoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_PlanoObjeto_PactoAtividade] UNIQUE NONCLUSTERED 
(
	[planoTrabalhoObjetoId] ASC,
	[pactoTrabalhoAtividadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PactoTrabalho]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PactoTrabalho](
	[pactoTrabalhoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[unidadeId] [bigint] NOT NULL,
	[pessoaId] [bigint] NOT NULL,
	[dataInicio] [date] NOT NULL,
	[dataFim] [date] NOT NULL,
	[formaExecucaoId] [int] NOT NULL,
	[situacaoId] [int] NOT NULL,
	[tempoComparecimento] [int] NULL,
	[cargaHorariaDiaria] [int] NOT NULL,
	[percentualExecucao] [numeric](10, 2) NULL,
	[relacaoPrevistoRealizado] [numeric](10, 2) NULL,
	[avaliacaoId] [uniqueidentifier] NULL,
	[tempoTotalDisponivel] [int] NOT NULL,
	[termoAceite] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[pactoTrabalhoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PactoTrabalhoAtividade]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PactoTrabalhoAtividade](
	[pactoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
	[pactoTrabalhoId] [uniqueidentifier] NOT NULL,
	[itemCatalogoId] [uniqueidentifier] NOT NULL,
	[quantidade] [int] NOT NULL,
	[tempoPrevistoPorItem] [numeric](4, 1) NOT NULL,
	[tempoPrevistoTotal] [numeric](4, 1) NOT NULL,
	[dataInicio] [datetime] NULL,
	[dataFim] [datetime] NULL,
	[tempoRealizado] [numeric](4, 1) NULL,
	[situacaoId] [int] NOT NULL,
	[descricao] [varchar](2000) NULL,
	[tempoHomologado] [numeric](4, 1) NULL,
	[nota] [numeric](4, 2) NULL,
	[justificativa] [varchar](200) NULL,
	[consideracoesConclusao] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[pactoTrabalhoAtividadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PactoTrabalhoAtividadeAssunto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PactoTrabalhoAtividadeAssunto](
	[pactoTrabalhoAtividadeAssuntoId] [uniqueidentifier] NOT NULL,
	[pactoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
	[assuntoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PactoTrabalhoAtividadeAssunto] PRIMARY KEY CLUSTERED 
(
	[pactoTrabalhoAtividadeAssuntoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_PactoTrabalhoAtividadeAssunto_Assunto_PactoTrabalhoAtividade] UNIQUE NONCLUSTERED 
(
	[assuntoId] ASC,
	[pactoTrabalhoAtividadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PactoTrabalhoHistorico]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PactoTrabalhoHistorico](
	[pactoTrabalhoHistoricoId] [uniqueidentifier] NOT NULL,
	[pactoTrabalhoId] [uniqueidentifier] NOT NULL,
	[situacaoId] [int] NOT NULL,
	[observacoes] [varchar](2000) NULL,
	[responsavelOperacao] [varchar](50) NOT NULL,
	[DataOperacao] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[pactoTrabalhoHistoricoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PactoTrabalhoSolicitacao]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PactoTrabalhoSolicitacao](
	[pactoTrabalhoSolicitacaoId] [uniqueidentifier] NOT NULL,
	[pactoTrabalhoId] [uniqueidentifier] NOT NULL,
	[tipoSolicitacaoId] [int] NOT NULL,
	[dataSolicitacao] [datetime] NOT NULL,
	[solicitante] [varchar](50) NOT NULL,
	[dadosSolicitacao] [varchar](2000) NOT NULL,
	[observacoesSolicitante] [varchar](2000) NULL,
	[analisado] [bit] NOT NULL,
	[dataAnalise] [datetime] NULL,
	[analista] [varchar](50) NULL,
	[aprovado] [bit] NULL,
	[observacoesAnalista] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[pactoTrabalhoSolicitacaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PessoaModalidadeExecucao]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PessoaModalidadeExecucao](
	[pessoaModalidadeExecucaoId] [uniqueidentifier] NOT NULL,
	[pessoaId] [bigint] NOT NULL,
	[modalidadeExecucaoId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[pessoaModalidadeExecucaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalho]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalho](
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[unidadeId] [bigint] NOT NULL,
	[dataInicio] [date] NOT NULL,
	[dataFim] [date] NOT NULL,
	[situacaoId] [int] NOT NULL,
	[avaliacaoId] [uniqueidentifier] NULL,
	[tempoComparecimento] [int] NULL,
	[totalServidoresSetor] [int] NULL,
	[tempoFaseHabilitacao] [int] NULL,
	[termoAceite] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoAtividade]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividade](
	[planoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[modalidadeExecucaoId] [int] NOT NULL,
	[quantidadeColaboradores] [int] NOT NULL,
	[descricao] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoAtividadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto](
	[planoTrabalhoAtividadeAssuntoId] [uniqueidentifier] NOT NULL,
	[assuntoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PlanoTrabalhoAtividadeAssunto] PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoAtividadeAssuntoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UK_PlanoTrabalhoAtividadeAssunto_Assunto_PlanoTrabalhoAtividade] UNIQUE NONCLUSTERED 
(
	[assuntoId] ASC,
	[planoTrabalhoAtividadeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato](
	[planoTrabalhoAtividadeCandidatoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
	[pessoaId] [bigint] NOT NULL,
	[situacaoId] [int] NOT NULL,
	[termoAceite] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoAtividadeCandidatoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico](
	[planoTrabalhoAtividadeCandidatoHistoricoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoAtividadeCandidatoId] [uniqueidentifier] NOT NULL,
	[situacaoId] [int] NOT NULL,
	[data] [datetime] NOT NULL,
	[descricao] [varchar](2000) NULL,
	[responsavelOperacao] [varchar](25) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoAtividadeCandidatoHistoricoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio](
	[planoTrabalhoAtividadeCriterioId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
	[criterioId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoAtividadeCriterioId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoAtividadeItem]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeItem](
	[planoTrabalhoAtividadeItemId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoAtividadeId] [uniqueidentifier] NOT NULL,
	[itemCatalogoId] [uniqueidentifier] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoAtividadeItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoCusto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoCusto](
	[planoTrabalhoCustoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[valor] [numeric](9, 2) NOT NULL,
	[descricao] [varchar](100) NOT NULL,
	[planoTrabalhoObjetoId] [uniqueidentifier] NULL,
 CONSTRAINT [PK_PlanoTrabalhoCusto] PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoCustoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoHistorico]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoHistorico](
	[planoTrabalhoHistoricoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[situacaoId] [int] NOT NULL,
	[observacoes] [varchar](2000) NULL,
	[responsavelOperacao] [varchar](50) NOT NULL,
	[DataOperacao] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoHistoricoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoMeta]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoMeta](
	[planoTrabalhoMetaId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[meta] [varchar](250) NOT NULL,
	[indicador] [varchar](250) NOT NULL,
	[descricao] [varchar](2000) NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoMetaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoObjeto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoObjeto](
	[planoTrabalhoObjetoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[objetoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PlanoTrabalhoObjeto] PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoObjetoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_PlanoTrabalho_Objeto] UNIQUE NONCLUSTERED 
(
	[planoTrabalhoId] ASC,
	[objetoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoObjetoAssunto]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoObjetoAssunto](
	[planoTrabalhoObjetoAssuntoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoObjetoId] [uniqueidentifier] NOT NULL,
	[assuntoId] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_PlanoTrabalhoObjetoAssunto] PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoObjetoAssuntoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_PlanoTrabalhoObjeto_Assunto] UNIQUE NONCLUSTERED 
(
	[planoTrabalhoObjetoId] ASC,
	[assuntoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [ProgramaGestao].[PlanoTrabalhoReuniao]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [ProgramaGestao].[PlanoTrabalhoReuniao](
	[planoTrabalhoReuniaoId] [uniqueidentifier] NOT NULL,
	[planoTrabalhoId] [uniqueidentifier] NOT NULL,
	[data] [datetime] NOT NULL,
	[titulo] [varchar](250) NOT NULL,
	[descricao] [varchar](250) NULL,
	[planoTrabalhoObjetoId] [uniqueidentifier] NULL,
PRIMARY KEY CLUSTERED 
(
	[planoTrabalhoReuniaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [ProgramaGestao].[UnidadeModalidadeExecucao]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ProgramaGestao].[UnidadeModalidadeExecucao](
	[unidadeModalidadeExecucaoId] [uniqueidentifier] NOT NULL,
	[unidadeId] [bigint] NOT NULL,
	[modalidadeExecucaoId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[unidadeModalidadeExecucaoId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  View [dbo].[VW_UnidadeSiglaCompleta]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [dbo].[VW_UnidadeSiglaCompleta]
as
WITH cte
AS
(
	SELECT	u.unidadeId, u.undNivel, u.undSigla as undSiglaCompleta, u.undSigla, u.Email, u.undCodigoSIORG
	FROM	Unidade as u
	WHERE	u.unidadeIdPai is null and u.situacaoUnidadeId = 1
	UNION ALL
	SELECT	u.unidadeId, u.undNivel, cast(cte.undSiglaCompleta+'/'+u.undSigla as varchar(50)), u.undSigla, u.Email, u.undCodigoSIORG
	FROM	Unidade as u
	INNER JOIN cte ON u.unidadeIdPai = cte.unidadeId
)
SELECT und.unidadeId, und.undSigla, und.undDescricao, und.unidadeIdPai, und.tipoUnidadeId,
und.situacaoUnidadeId, und.ufId, und.undNivel, und.tipoFuncaoUnidadeId, cte.undSiglaCompleta, und.Email, und.undCodigoSIORG
FROM cte 
INNER JOIN unidade und on cte.unidadeId = und.unidadeId

GO
/****** Object:  View [ProgramaGestao].[VW_AssuntoChaveCompleta]    Script Date: 13/11/2020 16:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Remover campo chave da view VW_AssuntoChaveCompleta
CREATE VIEW [ProgramaGestao].[VW_AssuntoChaveCompleta]
AS
	WITH cte_assunto AS (

		SELECT 
			assuntoId, 
			valor, 
			assuntoPaiId, 
			ativo, 
			CAST(valor AS varchar(200)) AS hierarquia, 
			1 as nivel
		FROM ProgramaGestao.Assunto
		WHERE assuntoPaiId IS  NULL

		UNION ALL

		SELECT 
			filho.assuntoId, 
			filho.valor, 
			filho.assuntoPaiId, 
			filho.ativo, 
			CAST(CONCAT(pai.hierarquia, '/', filho.valor) AS VARCHAR(200)) AS hierarquia, 
			pai.nivel + 1 AS nivel
		FROM ProgramaGestao.Assunto filho
		JOIN cte_assunto pai ON filho.assuntoPaiId = pai.assuntoId

	)
	SELECT *
	FROM cte_assunto;

GO
ALTER TABLE [dbo].[TipoFuncao] ADD  CONSTRAINT [DF__TipoFuncao_tfnIndicadorChefia]  DEFAULT ((0)) FOR [tfnIndicadorChefia]
GO
ALTER TABLE [dbo].[Feriado]  WITH CHECK ADD  CONSTRAINT [FK_Feriado_UF] FOREIGN KEY([ufId])
REFERENCES [dbo].[UF] ([ufId])
GO
ALTER TABLE [dbo].[Feriado] CHECK CONSTRAINT [FK_Feriado_UF]
GO

ALTER TABLE [dbo].[Pessoa]  WITH CHECK ADD  CONSTRAINT [FK_Pessoa_TipoFuncao] FOREIGN KEY([tipoFuncaoId])
REFERENCES [dbo].[TipoFuncao] ([tipoFuncaoId])
GO
ALTER TABLE [dbo].[Pessoa] CHECK CONSTRAINT [FK_Pessoa_TipoFuncao]
GO
ALTER TABLE [dbo].[Pessoa]  WITH CHECK ADD  CONSTRAINT [FK_Pessoa_Unidade] FOREIGN KEY([unidadeId])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO
ALTER TABLE [dbo].[Pessoa] CHECK CONSTRAINT [FK_Pessoa_Unidade]
GO
ALTER TABLE [dbo].[Unidade]  WITH CHECK ADD  CONSTRAINT [FK_Unidade_UF] FOREIGN KEY([ufId])
REFERENCES [dbo].[UF] ([ufId])
GO
ALTER TABLE [dbo].[Unidade] CHECK CONSTRAINT [FK_Unidade_UF]
GO
ALTER TABLE [dbo].[Unidade]  WITH CHECK ADD  CONSTRAINT [FK_Unidade_UnidadePai] FOREIGN KEY([unidadeIdPai])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO
ALTER TABLE [dbo].[Unidade] CHECK CONSTRAINT [FK_Unidade_UnidadePai]
GO
ALTER TABLE [ProgramaGestao].[Assunto]  WITH CHECK ADD  CONSTRAINT [FK_Assunto_AssuntoPai] FOREIGN KEY([assuntoPaiId])
REFERENCES [ProgramaGestao].[Assunto] ([assuntoId])
GO
ALTER TABLE [ProgramaGestao].[Assunto] CHECK CONSTRAINT [FK_Assunto_AssuntoPai]
GO
ALTER TABLE [ProgramaGestao].[Catalogo]  WITH CHECK ADD  CONSTRAINT [FK_Catalogo_Unidade] FOREIGN KEY([unidadeId])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO
ALTER TABLE [ProgramaGestao].[Catalogo] CHECK CONSTRAINT [FK_Catalogo_Unidade]
GO
ALTER TABLE [ProgramaGestao].[CatalogoItemCatalogo]  WITH CHECK ADD  CONSTRAINT [FK_CatalogoItemCatalogo_Catalogo] FOREIGN KEY([catalogoId])
REFERENCES [ProgramaGestao].[Catalogo] ([catalogoId])
GO
ALTER TABLE [ProgramaGestao].[CatalogoItemCatalogo] CHECK CONSTRAINT [FK_CatalogoItemCatalogo_Catalogo]
GO
ALTER TABLE [ProgramaGestao].[CatalogoItemCatalogo]  WITH CHECK ADD  CONSTRAINT [FK_CatalogoItemCatalogo_ItemCatalogo] FOREIGN KEY([itemCatalogoId])
REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId])
GO
ALTER TABLE [ProgramaGestao].[CatalogoItemCatalogo] CHECK CONSTRAINT [FK_CatalogoItemCatalogo_ItemCatalogo]
GO
ALTER TABLE [ProgramaGestao].[ItemCatalogo]  WITH CHECK ADD  CONSTRAINT [FK_ItemCatalogo_CalculoTempo] FOREIGN KEY([calculoTempoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[ItemCatalogo] CHECK CONSTRAINT [FK_ItemCatalogo_CalculoTempo]
GO
ALTER TABLE [ProgramaGestao].[ItemCatalogoAssunto]  WITH CHECK ADD  CONSTRAINT [FK_ItemCatalogoAssunto_Assunto] FOREIGN KEY([assuntoId])
REFERENCES [ProgramaGestao].[Assunto] ([assuntoId])
GO
ALTER TABLE [ProgramaGestao].[ItemCatalogoAssunto] CHECK CONSTRAINT [FK_ItemCatalogoAssunto_Assunto]
GO
ALTER TABLE [ProgramaGestao].[ItemCatalogoAssunto]  WITH CHECK ADD  CONSTRAINT [FK_ItemCatalogoAssunto_ItemCatalogo] FOREIGN KEY([itemCatalogoId])
REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId])
GO
ALTER TABLE [ProgramaGestao].[ItemCatalogoAssunto] CHECK CONSTRAINT [FK_ItemCatalogoAssunto_ItemCatalogo]
GO
ALTER TABLE [ProgramaGestao].[PactoAtividadePlanoObjeto]  WITH CHECK ADD  CONSTRAINT [FK_PactoAtividadePlanoObjeto_PactoTrabalhoAtividade] FOREIGN KEY([pactoTrabalhoAtividadeId])
REFERENCES [ProgramaGestao].[PactoTrabalhoAtividade] ([pactoTrabalhoAtividadeId])
GO
ALTER TABLE [ProgramaGestao].[PactoAtividadePlanoObjeto] CHECK CONSTRAINT [FK_PactoAtividadePlanoObjeto_PactoTrabalhoAtividade]
GO
ALTER TABLE [ProgramaGestao].[PactoAtividadePlanoObjeto]  WITH CHECK ADD  CONSTRAINT [FK_PactoAtividadePlanoObjeto_PlanoTrabalhoObjeto] FOREIGN KEY([planoTrabalhoObjetoId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId])
GO
ALTER TABLE [ProgramaGestao].[PactoAtividadePlanoObjeto] CHECK CONSTRAINT [FK_PactoAtividadePlanoObjeto_PlanoTrabalhoObjeto]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalho_FormaExecucao] FOREIGN KEY([formaExecucaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho] CHECK CONSTRAINT [FK_PactoTrabalho_FormaExecucao]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalho_Pessoa] FOREIGN KEY([pessoaId])
REFERENCES [dbo].[Pessoa] ([pessoaId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho] CHECK CONSTRAINT [FK_PactoTrabalho_Pessoa]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalho_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho] CHECK CONSTRAINT [FK_PactoTrabalho_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalho_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho] CHECK CONSTRAINT [FK_PactoTrabalho_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalho_Unidade] FOREIGN KEY([unidadeId])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalho] CHECK CONSTRAINT [FK_PactoTrabalho_Unidade]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoAtividade_ItemCatalogo] FOREIGN KEY([itemCatalogoId])
REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade] CHECK CONSTRAINT [FK_PactoTrabalhoAtividade_ItemCatalogo]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoAtividade_PactoTrabalho] FOREIGN KEY([pactoTrabalhoId])
REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade] CHECK CONSTRAINT [FK_PactoTrabalhoAtividade_PactoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoAtividade_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividade] CHECK CONSTRAINT [FK_PactoTrabalhoAtividade_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividadeAssunto]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoAtividadeAssunto_Assunto] FOREIGN KEY([assuntoId])
REFERENCES [ProgramaGestao].[Assunto] ([assuntoId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividadeAssunto] CHECK CONSTRAINT [FK_PactoTrabalhoAtividadeAssunto_Assunto]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividadeAssunto]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoAtividadeAssunto_PactoTrabalhoAtividade] FOREIGN KEY([pactoTrabalhoAtividadeId])
REFERENCES [ProgramaGestao].[PactoTrabalhoAtividade] ([pactoTrabalhoAtividadeId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoAtividadeAssunto] CHECK CONSTRAINT [FK_PactoTrabalhoAtividadeAssunto_PactoTrabalhoAtividade]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoHistorico_PactoTrabalho] FOREIGN KEY([pactoTrabalhoId])
REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoHistorico] CHECK CONSTRAINT [FK_PactoTrabalhoHistorico_PactoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoHistorico_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoHistorico] CHECK CONSTRAINT [FK_PactoTrabalhoHistorico_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoSolicitacao]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoSolicitacao_PactoTrabalho] FOREIGN KEY([pactoTrabalhoId])
REFERENCES [ProgramaGestao].[PactoTrabalho] ([pactoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoSolicitacao] CHECK CONSTRAINT [FK_PactoTrabalhoSolicitacao_PactoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoSolicitacao]  WITH CHECK ADD  CONSTRAINT [FK_PactoTrabalhoSolicitacao_TipoSolicitacao] FOREIGN KEY([tipoSolicitacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PactoTrabalhoSolicitacao] CHECK CONSTRAINT [FK_PactoTrabalhoSolicitacao_TipoSolicitacao]
GO
ALTER TABLE [ProgramaGestao].[PessoaModalidadeExecucao]  WITH CHECK ADD  CONSTRAINT [FK_PessoaModalidadeExecucao_ModalidadeExecucao] FOREIGN KEY([modalidadeExecucaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PessoaModalidadeExecucao] CHECK CONSTRAINT [FK_PessoaModalidadeExecucao_ModalidadeExecucao]
GO
ALTER TABLE [ProgramaGestao].[PessoaModalidadeExecucao]  WITH CHECK ADD  CONSTRAINT [FK_PessoaModalidadeExecucao_Pessoa] FOREIGN KEY([pessoaId])
REFERENCES [dbo].[Pessoa] ([pessoaId])
GO
ALTER TABLE [ProgramaGestao].[PessoaModalidadeExecucao] CHECK CONSTRAINT [FK_PessoaModalidadeExecucao_Pessoa]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalho_Unidade] FOREIGN KEY([unidadeId])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalho] CHECK CONSTRAINT [FK_PlanoTrabalho_Unidade]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalho]  WITH CHECK ADD  CONSTRAINT [FK_PlatoTrabalho_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalho] CHECK CONSTRAINT [FK_PlatoTrabalho_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividade_ModalidadeExecucao] FOREIGN KEY([modalidadeExecucaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividade] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividade_ModalidadeExecucao]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividade]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividade_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividade] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividade_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeAssunto_Assunto] FOREIGN KEY([assuntoId])
REFERENCES [ProgramaGestao].[Assunto] ([assuntoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeAssunto_Assunto]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeAssunto_PlanoTrabalhoAtividade] FOREIGN KEY([planoTrabalhoAtividadeId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeAssunto] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeAssunto_PlanoTrabalhoAtividade]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_Pessoa] FOREIGN KEY([pessoaId])
REFERENCES [dbo].[Pessoa] ([pessoaId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_Pessoa]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_PlanoTrabalhoAtividade] FOREIGN KEY([planoTrabalhoAtividadeId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_PlanoTrabalhoAtividade]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidato_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidatoHistorico_PlanoTrabalhoAtividadeCandidato] FOREIGN KEY([planoTrabalhoAtividadeCandidatoId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividadeCandidato] ([planoTrabalhoAtividadeCandidatoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidatoHistorico_PlanoTrabalhoAtividadeCandidato]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidatoHistorico_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCandidatoHistorico] CHECK CONSTRAINT [FK_PlanoTrabalhoAtividadeCandidatoHistorico_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoCriterioAtividade_Criterio] FOREIGN KEY([criterioId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio] CHECK CONSTRAINT [FK_PlanoTrabalhoCriterioAtividade_Criterio]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoCriterioAtividade_PlanoTrabalhoAtividade] FOREIGN KEY([planoTrabalhoAtividadeId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeCriterio] CHECK CONSTRAINT [FK_PlanoTrabalhoCriterioAtividade_PlanoTrabalhoAtividade]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeItem]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoItemAtividade_ItemCatalogo] FOREIGN KEY([itemCatalogoId])
REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeItem] CHECK CONSTRAINT [FK_PlanoTrabalhoItemAtividade_ItemCatalogo]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeItem]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoItemAtividade_PlanoTrabalhoAtividade] FOREIGN KEY([planoTrabalhoAtividadeId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeItem] CHECK CONSTRAINT [FK_PlanoTrabalhoItemAtividade_PlanoTrabalhoAtividade]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoCusto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoCusto_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoCusto] CHECK CONSTRAINT [FK_PlanoTrabalhoCusto_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoCusto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoCusto_PlanoTrabalhoObjeto] FOREIGN KEY([planoTrabalhoObjetoId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoCusto] CHECK CONSTRAINT [FK_PlanoTrabalhoCusto_PlanoTrabalhoObjeto]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoHistorico_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoHistorico] CHECK CONSTRAINT [FK_PlanoTrabalhoHistorico_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoHistorico]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoHistorico_Situacao] FOREIGN KEY([situacaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoHistorico] CHECK CONSTRAINT [FK_PlanoTrabalhoHistorico_Situacao]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoMeta]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoMeta_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoMeta] CHECK CONSTRAINT [FK_PlanoTrabalhoMeta_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjeto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoObjeto_Objeto] FOREIGN KEY([objetoId])
REFERENCES [ProgramaGestao].[Objeto] ([objetoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjeto] CHECK CONSTRAINT [FK_PlanoTrabalhoObjeto_Objeto]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjeto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoObjeto_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjeto] CHECK CONSTRAINT [FK_PlanoTrabalhoObjeto_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjetoAssunto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoObjetoAssunto_Assunto] FOREIGN KEY([assuntoId])
REFERENCES [ProgramaGestao].[Assunto] ([assuntoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjetoAssunto] CHECK CONSTRAINT [FK_PlanoTrabalhoObjetoAssunto_Assunto]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjetoAssunto]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoObjetoAssunto_PlanoTrabalhoObjeto] FOREIGN KEY([planoTrabalhoObjetoId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoObjetoAssunto] CHECK CONSTRAINT [FK_PlanoTrabalhoObjetoAssunto_PlanoTrabalhoObjeto]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoReuniao]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoReuniao_PlanoTrabalho] FOREIGN KEY([planoTrabalhoId])
REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoReuniao] CHECK CONSTRAINT [FK_PlanoTrabalhoReuniao_PlanoTrabalho]
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoReuniao]  WITH CHECK ADD  CONSTRAINT [FK_PlanoTrabalhoReuniao_PlanoTrabalhoObjeto] FOREIGN KEY([planoTrabalhoObjetoId])
REFERENCES [ProgramaGestao].[PlanoTrabalhoObjeto] ([planoTrabalhoObjetoId])
GO
ALTER TABLE [ProgramaGestao].[PlanoTrabalhoReuniao] CHECK CONSTRAINT [FK_PlanoTrabalhoReuniao_PlanoTrabalhoObjeto]
GO
ALTER TABLE [ProgramaGestao].[UnidadeModalidadeExecucao]  WITH CHECK ADD  CONSTRAINT [FK_UnidadeModalidadeExecucao_ModalidadeExecucao] FOREIGN KEY([modalidadeExecucaoId])
REFERENCES [dbo].[CatalogoDominio] ([catalogoDominioId])
GO
ALTER TABLE [ProgramaGestao].[UnidadeModalidadeExecucao] CHECK CONSTRAINT [FK_UnidadeModalidadeExecucao_ModalidadeExecucao]
GO
ALTER TABLE [ProgramaGestao].[UnidadeModalidadeExecucao]  WITH CHECK ADD  CONSTRAINT [FK_UnidadeModalidadeExecucao_Unidade] FOREIGN KEY([unidadeId])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO
ALTER TABLE [ProgramaGestao].[UnidadeModalidadeExecucao] CHECK CONSTRAINT [FK_UnidadeModalidadeExecucao_Unidade]
GO
CREATE NONCLUSTERED INDEX IX_Unidade_unidadeIdPai  
    ON Unidade (unidadeIdPai);   
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Código da unidade no SIORG' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Unidade', @level2type=N'COLUMN',@level2name=N'undCodigoSIORG'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo chave' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Assunto', @level2type=N'COLUMN',@level2name=N'chave'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo valor' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Assunto', @level2type=N'COLUMN',@level2name=N'valor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Assunto pai' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Assunto', @level2type=N'COLUMN',@level2name=N'assuntoPaiId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se o assunto encontra-se ativo' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Assunto', @level2type=N'COLUMN',@level2name=N'ativo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Armazena os assuntos.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Assunto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Título da complexidade do item de catálogo' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogo', @level2type=N'COLUMN',@level2name=N'complexidade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Definição de como a atividade deve ser avaliada para ser enquadrada na complexidade' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogo', @level2type=N'COLUMN',@level2name=N'definicaoComplexidade'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Entregas esperadas pela atividade' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogo', @level2type=N'COLUMN',@level2name=N'entregasEsperadas'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela ItemCatalogoAssunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogoAssunto', @level2type=N'COLUMN',@level2name=N'itemCatalogoAssuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Assunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogoAssunto', @level2type=N'COLUMN',@level2name=N'assuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela ItemCatalogo.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogoAssunto', @level2type=N'COLUMN',@level2name=N'itemCatalogoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de associação entre ItemCatalogo e Assunto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'ItemCatalogoAssunto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo descrição' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Objeto', @level2type=N'COLUMN',@level2name=N'descricao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo tipo' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Objeto', @level2type=N'COLUMN',@level2name=N'tipo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo chave' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Objeto', @level2type=N'COLUMN',@level2name=N'chave'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se o objeto encontra-se ativo' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Objeto', @level2type=N'COLUMN',@level2name=N'ativo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Armazena os objetos.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'Objeto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela PactoAtividadePlanoObjeto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoAtividadePlanoObjeto', @level2type=N'COLUMN',@level2name=N'pactoAtividadePlanoObjetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PlanoTrabalhoObjeto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoAtividadePlanoObjeto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoObjetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PactoTrabalhoAtividade' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoAtividadePlanoObjeto', @level2type=N'COLUMN',@level2name=N'pactoTrabalhoAtividadeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de relacionamento entre PlanoTrabalhoObjeto e PactoTrabalhoAtividade.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoAtividadePlanoObjeto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Termo de aceite assinado pelo servidor ao aceitar plano de trabalho' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalho', @level2type=N'COLUMN',@level2name=N'termoAceite'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tempo homologado para a realização da atividade.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividade', @level2type=N'COLUMN',@level2name=N'tempoHomologado'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nota da avaliação após a conclusão da atividade.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividade', @level2type=N'COLUMN',@level2name=N'nota'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Justificativa para a nota de avaliação baixa.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividade', @level2type=N'COLUMN',@level2name=N'justificativa'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Detalhes que o servidor fornece sobre a atividade ao concluir sua execução.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividade', @level2type=N'COLUMN',@level2name=N'consideracoesConclusao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela PactoTrabalhoAtividadeAssunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividadeAssunto', @level2type=N'COLUMN',@level2name=N'pactoTrabalhoAtividadeAssuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PactoTrabalhoAtividade.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividadeAssunto', @level2type=N'COLUMN',@level2name=N'pactoTrabalhoAtividadeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Assunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividadeAssunto', @level2type=N'COLUMN',@level2name=N'assuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de assuntos associados às atividades do PactoTrabalho' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PactoTrabalhoAtividadeAssunto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Termo de aceite a ser assinado pelos servidores nesse programa de gestão' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalho', @level2type=N'COLUMN',@level2name=N'termoAceite'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela PlanoTrabalhoAtividadeAssunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoAtividadeAssunto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoAtividadeAssuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Assunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoAtividadeAssunto', @level2type=N'COLUMN',@level2name=N'assuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PlanoTrabalhoAtividade.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoAtividadeAssunto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoAtividadeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de associação entre PlanoTrabalhoAtividade e Assunto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoAtividadeAssunto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Termo de aceite assinado pelo servidor ao se candidatar a vaga' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoAtividadeCandidato', @level2type=N'COLUMN',@level2name=N'termoAceite'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela PlanoTrabalhoCusto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoCusto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoCustoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PlanoTrabalho.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoCusto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valor do custo.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoCusto', @level2type=N'COLUMN',@level2name=N'valor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrição do custo.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoCusto', @level2type=N'COLUMN',@level2name=N'descricao'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Objeto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoCusto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoObjetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de custos associados ao PlanoTrabalho' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoCusto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela PlanoTrabalhoObjeto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjeto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoObjetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PlanoTrabalho' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjeto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Objeto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjeto', @level2type=N'COLUMN',@level2name=N'objetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de relacionamento entre PlanoTrabalho e Objeto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjeto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave primária da tabela PlanoTrabalhoObjetoAssunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjetoAssunto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoObjetoAssuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela PlanoTrabalhoObjeto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjetoAssunto', @level2type=N'COLUMN',@level2name=N'planoTrabalhoObjetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Assunto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjetoAssunto', @level2type=N'COLUMN',@level2name=N'assuntoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabela de relacionamento entre PlanoTrabalhoObjeto e Assunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoObjetoAssunto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Chave estrangeira para a tabela Objeto' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'TABLE',@level1name=N'PlanoTrabalhoReuniao', @level2type=N'COLUMN',@level2name=N'planoTrabalhoObjetoId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Campo valor' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'VIEW',@level1name=N'VW_AssuntoChaveCompleta', @level2type=N'COLUMN',@level2name=N'valor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Assunto pai' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'VIEW',@level1name=N'VW_AssuntoChaveCompleta', @level2type=N'COLUMN',@level2name=N'assuntoPaiId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indica se o assunto encontra-se ativo' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'VIEW',@level1name=N'VW_AssuntoChaveCompleta', @level2type=N'COLUMN',@level2name=N'ativo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Exibe as chaves de forma hierarquica.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'VIEW',@level1name=N'VW_AssuntoChaveCompleta', @level2type=N'COLUMN',@level2name=N'hierarquia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Exibe o nível hierarquico do assunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'VIEW',@level1name=N'VW_AssuntoChaveCompleta', @level2type=N'COLUMN',@level2name=N'nivel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'View para exibir dados hierarquicos da tabela Assunto.' , @level0type=N'SCHEMA',@level0name=N'ProgramaGestao', @level1type=N'VIEW',@level1name=N'VW_AssuntoChaveCompleta'
GO
