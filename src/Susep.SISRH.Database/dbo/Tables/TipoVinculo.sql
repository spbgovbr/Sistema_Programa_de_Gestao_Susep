CREATE TABLE [dbo].[TipoVinculo] (
    [tipoVinculoId] BIGINT        NOT NULL,
    [tvnDescricao]  VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_TipoVinculo] PRIMARY KEY CLUSTERED ([tipoVinculoId] ASC),
    CONSTRAINT [UQ_TipoVinculo_tvnDescricao] UNIQUE NONCLUSTERED ([tvnDescricao] ASC)
);

