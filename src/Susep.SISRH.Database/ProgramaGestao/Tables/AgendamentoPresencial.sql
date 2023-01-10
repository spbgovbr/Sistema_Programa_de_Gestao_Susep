CREATE TABLE [ProgramaGestao].[AgendamentoPresencial] (
    [agendamentoPresencialId] UNIQUEIDENTIFIER NOT NULL,
    [pessoaId]                BIGINT           NOT NULL,
    [dataAgendada]            DATE             NOT NULL,
    CONSTRAINT [PK_AgendamentoPresencial] PRIMARY KEY CLUSTERED ([agendamentoPresencialId] ASC),
    CONSTRAINT [FK_AgendamentoPresencia_Pessoa] FOREIGN KEY ([pessoaId]) REFERENCES [dbo].[Pessoa] ([pessoaId])
);


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Data agendada para comparecimento presencial', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial', @level2type = N'COLUMN', @level2name = N'dataAgendada';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador da pessoa para a qual o agendamento foi realizado', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial', @level2type = N'COLUMN', @level2name = N'pessoaId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Identificador único do agendamento de comparecimento', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial', @level2type = N'COLUMN', @level2name = N'agendamentoPresencialId';


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'Tabela que registra os agendamentos para comparcimento presencial realizados por servidores', @level0type = N'SCHEMA', @level0name = N'ProgramaGestao', @level1type = N'TABLE', @level1name = N'AgendamentoPresencial';

