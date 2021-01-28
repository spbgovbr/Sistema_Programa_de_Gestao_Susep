CREATE TABLE [dbo].[Pessoa] (
    [pessoaId]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [pesNome]           VARCHAR (250) NOT NULL,
    [unidadeId]         BIGINT        NOT NULL,
    [CargaHoraria]      INT           NOT NULL,
    [tipoFuncaoId]      BIGINT        NULL,
    [pesCPF]            VARCHAR (15)  NULL,
    [pesEmail]          VARCHAR (150) NULL,
    [pesMatriculaSiape] VARCHAR (12)  NULL,
    PRIMARY KEY CLUSTERED ([pessoaId] ASC),
    CONSTRAINT [FK_Pessoa_TipoFuncao] FOREIGN KEY ([tipoFuncaoId]) REFERENCES [dbo].[TipoFuncao] ([tipoFuncaoId])
);









