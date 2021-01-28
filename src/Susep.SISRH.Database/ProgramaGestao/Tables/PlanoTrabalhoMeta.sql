CREATE TABLE [ProgramaGestao].[PlanoTrabalhoMeta] (
    [planoTrabalhoMetaId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoId]     UNIQUEIDENTIFIER NOT NULL,
    [meta]                VARCHAR (250)    NOT NULL,
    [indicador]           VARCHAR (250)    NOT NULL,
    [descricao]           VARCHAR (2000)   NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoMetaId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoMeta_PlanoTrabalho] FOREIGN KEY ([planoTrabalhoId]) REFERENCES [ProgramaGestao].[PlanoTrabalho] ([planoTrabalhoId])
);





