CREATE TABLE [dbo].[Unidade] (
    [unidadeId]               BIGINT        IDENTITY (1, 1) NOT NULL,
    [undSigla]                VARCHAR (50)  NOT NULL,
    [undDescricao]            VARCHAR (150) NOT NULL,
    [unidadeIdPai]            BIGINT        NULL,
    [tipoUnidadeId]           BIGINT        NOT NULL,
    [situacaoUnidadeId]       BIGINT        NOT NULL,
    [ufId]                    VARCHAR (2)   NOT NULL,
    [undNivel]                SMALLINT      NULL,
    [tipoFuncaoUnidadeId]     BIGINT        NULL,
    [Email]                   VARCHAR (150) NULL,
    [undCodigoSIORG]          INT           NULL,
    [pessoaIdChefe]           BIGINT        NULL,
    [pessoaIdChefeSubstituto] BIGINT        NULL,
    CONSTRAINT [PK_Unidade] PRIMARY KEY CLUSTERED ([unidadeId] ASC),
    CONSTRAINT [FK_Unidade_SituacaoUnidade] FOREIGN KEY ([situacaoUnidadeId]) REFERENCES [dbo].[SituacaoUnidade] ([situacaoUnidadeId]),
    CONSTRAINT [FK_Unidade_TipoUnidade] FOREIGN KEY ([tipoUnidadeId]) REFERENCES [dbo].[TipoUnidade] ([tipoUnidadeId]),
    CONSTRAINT [FK_Unidade_UF] FOREIGN KEY ([ufId]) REFERENCES [dbo].[UF] ([ufId]),
    CONSTRAINT [FK_Unidade_UnidadePai] FOREIGN KEY ([unidadeIdPai]) REFERENCES [dbo].[Unidade] ([unidadeId])
);






GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Código da unidade no SIORG', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unidade', @level2type = N'COLUMN', @level2name = N'undCodigoSIORG';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o ID da pessoa que é o chefe substituto da unidade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unidade', @level2type = N'COLUMN', @level2name = N'pessoaIdChefeSubstituto';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Registra o ID da pessoa que é o chefe da unidade', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'Unidade', @level2type = N'COLUMN', @level2name = N'pessoaIdChefe';

