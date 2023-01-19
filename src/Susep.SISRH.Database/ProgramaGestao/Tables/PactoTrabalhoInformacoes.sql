CREATE TABLE [ProgramaGestao].[PactoTrabalhoHistorico] (
    [pactoTrabalhoHistoricoId] UNIQUEIDENTIFIER NOT NULL,
    [pactoTrabalhoId]          UNIQUEIDENTIFIER NOT NULL,
    [situacaoId]               INT              NOT NULL,
    [observacoes]              VARCHAR (2000)   NULL,
    [responsavelOperacao]      VARCHAR (50)     NOT NULL,
    [DataOperacao]             DATETIME         NOT NULL,
    ,
    ,
    
);



