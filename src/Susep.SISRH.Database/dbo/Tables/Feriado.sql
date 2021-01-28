CREATE TABLE [dbo].[Feriado] (
    [feriadoId]    BIGINT       IDENTITY (1, 1) NOT NULL,
    [ferData]      DATETIME     NOT NULL,
    [ferFixo]      BIT          NOT NULL,
    [ferDescricao] VARCHAR (50) NOT NULL,
    [ufId]         VARCHAR (2)  NULL,
    CONSTRAINT [PK_Feriado] PRIMARY KEY CLUSTERED ([feriadoId] ASC),
    CONSTRAINT [FK_Feriado_UF] FOREIGN KEY ([ufId]) REFERENCES [dbo].[UF] ([ufId])
);

