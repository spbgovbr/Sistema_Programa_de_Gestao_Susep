CREATE TABLE [dbo].[Unidade] (
    [unidadeId]           BIGINT        IDENTITY (1, 1) NOT NULL,
    [undSigla]            VARCHAR (50)  NOT NULL,
    [undDescricao]        VARCHAR (150) NOT NULL,
    [unidadeIdPai]        BIGINT        NULL,
    [tipoUnidadeId]       BIGINT        NULL,
    [situacaoUnidadeId]   BIGINT        NOT NULL,
    [ufId]                VARCHAR (2)   NULL,
    [undNivel]            SMALLINT      NULL,
    [tipoFuncaoUnidadeId] BIGINT        NULL,
    [Email]               VARCHAR (150) NULL,
    PRIMARY KEY CLUSTERED ([unidadeId] ASC)
);



