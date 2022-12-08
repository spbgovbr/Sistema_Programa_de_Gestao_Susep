CREATE TABLE [dbo].[TipoFuncaoUnidade] (
    [tipoFuncaoUnidadeId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [tfuDescricao]        VARCHAR (150) NOT NULL,
    CONSTRAINT [PK_TipoFuncaoUnidade] PRIMARY KEY CLUSTERED ([tipoFuncaoUnidadeId] ASC)
);

