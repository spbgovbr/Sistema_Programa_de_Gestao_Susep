CREATE TABLE [ProgramaGestao].[PlanoTrabalhoAtividadeItem] (
    [planoTrabalhoAtividadeItemId] UNIQUEIDENTIFIER NOT NULL,
    [planoTrabalhoAtividadeId]     UNIQUEIDENTIFIER NOT NULL,
    [itemCatalogoId]               UNIQUEIDENTIFIER NOT NULL,
    PRIMARY KEY CLUSTERED ([planoTrabalhoAtividadeItemId] ASC),
    CONSTRAINT [FK_PlanoTrabalhoItemAtividade_ItemCatalogo] FOREIGN KEY ([itemCatalogoId]) REFERENCES [ProgramaGestao].[ItemCatalogo] ([itemCatalogoId]),
    CONSTRAINT [FK_PlanoTrabalhoItemAtividade_PlanoTrabalhoAtividade] FOREIGN KEY ([planoTrabalhoAtividadeId]) REFERENCES [ProgramaGestao].[PlanoTrabalhoAtividade] ([planoTrabalhoAtividadeId])
);

