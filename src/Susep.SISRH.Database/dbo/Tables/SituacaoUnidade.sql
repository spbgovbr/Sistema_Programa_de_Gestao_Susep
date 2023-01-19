CREATE TABLE [dbo].[SituacaoUnidade] (
    [situacaoUnidadeId] BIGINT       NOT NULL,
    [sunDescricao]      VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SituacaoUnidade] PRIMARY KEY CLUSTERED ([situacaoUnidadeId] ASC),
    CONSTRAINT [UQ_SituacaoUnidade_sunDescricao] UNIQUE NONCLUSTERED ([sunDescricao] ASC)
);

