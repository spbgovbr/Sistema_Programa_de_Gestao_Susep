CREATE TABLE [dbo].[TipoFuncao] (
    [tipoFuncaoId]       BIGINT        NOT NULL,
    [tfnDescricao]       VARCHAR (150) NOT NULL,
    [tfnCodigoFuncao]    VARCHAR (5)   NULL,
    [tfnIndicadorChefia] BIT           CONSTRAINT [DF__TipoFuncao_tfnIndicadorChefia] DEFAULT ((0)) NULL,
    CONSTRAINT [PK_TipoFuncao] PRIMARY KEY CLUSTERED ([tipoFuncaoId] ASC),
    CONSTRAINT [UQ_TipoFuncao_tfnDescricao] UNIQUE NONCLUSTERED ([tfnDescricao] ASC)
);

