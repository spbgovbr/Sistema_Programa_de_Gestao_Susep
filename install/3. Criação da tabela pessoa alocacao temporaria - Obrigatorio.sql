
CREATE TABLE [dbo].[PessoaAlocacaoTemporaria](
	[pessoaAlocacaoTemporariaId] [uniqueidentifier] NOT NULL,
	[pessoaId] [bigint] NOT NULL,
	[unidadeId] [bigint] NOT NULL,
	[dataInicio] [date] NOT NULL,
	[dataFim] [date] NULL,
 CONSTRAINT [PK_PessoaAlocacaoTemporaria] PRIMARY KEY CLUSTERED 
(
	[pessoaAlocacaoTemporariaId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[PessoaAlocacaoTemporaria]  WITH CHECK ADD  CONSTRAINT [FK_PessoaAlocacaoTemporaria_Pessoa] FOREIGN KEY([pessoaId])
REFERENCES [dbo].[Pessoa] ([pessoaId])
GO
ALTER TABLE [dbo].[PessoaAlocacaoTemporaria]  WITH CHECK ADD  CONSTRAINT [FK_PessoaAlocacaoTemporaria_Unidade] FOREIGN KEY([unidadeId])
REFERENCES [dbo].[Unidade] ([unidadeId])
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PessoaAlocacaoTemporaria', @level2type=N'COLUMN',
@level2name=N'pessoaAlocacaoTemporariaId', @value=N'Chave da tabela de alocação temporária'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PessoaAlocacaoTemporaria', @level2type=N'COLUMN',
@level2name=N'pessoaId', @value=N'Pessoa que foi alocada temporariamente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PessoaAlocacaoTemporaria', @level2type=N'COLUMN',
@level2name=N'unidadeId', @value=N'Unidade da alocação temporária'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PessoaAlocacaoTemporaria', @level2type=N'COLUMN',
@level2name=N'dataInicio', @value=N'Data em que se iniciou a alocação temporária'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PessoaAlocacaoTemporaria', @level2type=N'COLUMN',
@level2name=N'dataFim', @value=N'Data em que se encerrou a alocação temporária'
GO
