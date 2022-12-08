CREATE TABLE [dbo].[TipoUnidade] (
    [tipoUnidadeId] BIGINT       NOT NULL,
    [tudDescricao]  VARCHAR (30) NOT NULL,
    CONSTRAINT [PK_TipoUnidade] PRIMARY KEY CLUSTERED ([tipoUnidadeId] ASC),
    CONSTRAINT [UQ_TipoUnidade_tudDescricao] UNIQUE NONCLUSTERED ([tudDescricao] ASC)
);

