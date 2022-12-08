CREATE TABLE [dbo].[SituacaoPessoa] (
    [situacaoPessoaId] BIGINT       NOT NULL,
    [spsDescricao]     VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_SituacaoPessoa] PRIMARY KEY CLUSTERED ([situacaoPessoaId] ASC),
    CONSTRAINT [UQ_SituacaoPessoa_spsDescricao] UNIQUE NONCLUSTERED ([spsDescricao] ASC)
);

