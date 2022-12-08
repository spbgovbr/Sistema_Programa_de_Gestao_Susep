CREATE TABLE [dbo].[PessoaAlocacaoTemporaria] (
    [pessoaAlocacaoTemporariaId] UNIQUEIDENTIFIER NOT NULL,
    [pessoaId]                   BIGINT           NOT NULL,
    [unidadeId]                  BIGINT           NOT NULL,
    [dataInicio]                 DATE             NOT NULL,
    [dataFim]                    DATE             NULL,
    CONSTRAINT [PK_PessoaAlocacaoTemporaria] PRIMARY KEY CLUSTERED ([pessoaAlocacaoTemporariaId] ASC),
    CONSTRAINT [FK_PessoaAlocacaoTemporaria_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId]),
    CONSTRAINT [FK_PessoaAlocacaoTemporaria_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data em que se encerrou a alocação temporária', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PessoaAlocacaoTemporaria', @level2type = N'COLUMN', @level2name = N'dataFim';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data em que se iniciou a alocação temporária', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PessoaAlocacaoTemporaria', @level2type = N'COLUMN', @level2name = N'dataInicio';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Unidade da alocação temporária', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PessoaAlocacaoTemporaria', @level2type = N'COLUMN', @level2name = N'unidadeId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Pessoa que foi alocada temporariamente', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PessoaAlocacaoTemporaria', @level2type = N'COLUMN', @level2name = N'pessoaId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Chave da tabela de alocação temporária', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'PessoaAlocacaoTemporaria', @level2type = N'COLUMN', @level2name = N'pessoaAlocacaoTemporariaId';

