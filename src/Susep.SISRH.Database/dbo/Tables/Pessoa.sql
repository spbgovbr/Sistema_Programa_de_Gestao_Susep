CREATE TABLE [dbo].[Pessoa] (
    [pessoaId]          BIGINT        IDENTITY (1, 1) NOT NULL,
    [pesNome]           VARCHAR (150) NOT NULL,
    [pesCPF]            VARCHAR (15)  NOT NULL,
    [pesDataNascimento] DATETIME      NOT NULL,
    [pesMatriculaSiape] VARCHAR (12)  NULL,
    [pesEmail]          VARCHAR (150) NULL,
    [situacaoPessoaId]  BIGINT        NOT NULL,
    [tipoVinculoId]     BIGINT        NOT NULL,
    [unidadeId]         BIGINT        NOT NULL,
    [tipoFuncaoId]      BIGINT        NULL,
    [cargoId]           BIGINT        NULL,
    [classePadraoId]    BIGINT        NULL,
    [RGNumero]          VARCHAR (14)  NULL,
    [RGOrgaoExpeditor]  VARCHAR (12)  NULL,
    [RGDataExpedicao]   DATE          NULL,
    [RGUF]              VARCHAR (2)   NULL,
    [cargaHoraria]      INT           NULL,
    CONSTRAINT [PK_Pessoa] PRIMARY KEY CLUSTERED ([pessoaId] ASC),
    CONSTRAINT [FK_Pessoa_Situacao_Pessoa] FOREIGN KEY ([situacaoPessoaId]) REFERENCES [dbo].[SituacaoPessoa] ([situacaoPessoaId]),
    CONSTRAINT [FK_Pessoa_TipoFuncao] FOREIGN KEY ([tipoFuncaoId]) REFERENCES [dbo].[TipoFuncao] ([tipoFuncaoId]),
    CONSTRAINT [FK_Pessoa_TipoVinculo] FOREIGN KEY ([tipoVinculoId]) REFERENCES [dbo].[TipoVinculo] ([tipoVinculoId]),
    CONSTRAINT [FK_Pessoa_Unidade] FOREIGN KEY ([unidadeId]) REFERENCES [dbo].[Unidade] ([unidadeId]),
    CONSTRAINT [UQ_Pessoa_pesCPF] UNIQUE NONCLUSTERED ([pesCPF] ASC)
);












GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Define a carga horária de trabalho de uma pessoa', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Pessoa', @level2type = N'COLUMN', @level2name = N'cargaHoraria';

