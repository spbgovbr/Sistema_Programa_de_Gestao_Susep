CREATE TABLE [dbo].[UF] (
    [ufId]        VARCHAR (2)  NOT NULL,
    [ufDescricao] VARCHAR (50) NOT NULL,
    CONSTRAINT [PK_UnidadeFederativa] PRIMARY KEY CLUSTERED ([ufId] ASC),
    CONSTRAINT [UQ_UnidadeFederativa_ufDescricao] UNIQUE NONCLUSTERED ([ufDescricao] ASC)
);

