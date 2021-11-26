DELETE FROM [dbo].[Pessoa]
DELETE FROM [dbo].[Unidade]
DELETE FROM [dbo].[TipoFuncao]

INSERT INTO [dbo].[TipoFuncao] VALUES (2, 'Diretor', '101.5', 1)
INSERT INTO [dbo].[TipoFuncao] VALUES (3, 'Coordenador-Geral', '101.4', 1)
INSERT INTO [dbo].[TipoFuncao] VALUES (4, 'Coordenador', '101.3', 1)

DECLARE @SUSEP INT
DECLARE @DEAFI INT
DECLARE @DETIC INT
DECLARE @COGEP INT
DECLARE @COGET INT
DECLARE @ASDEN INT
DECLARE @COPROJ INT
DECLARE @COARQ INT
DECLARE @USUARIOGESTOR INT

INSERT INTO [dbo].[Unidade] VALUES ('SUSEP', 'Superintendência de Seguros Privados', NULL, 13, 1, 'RJ', 1, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @SUSEP = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('DEAFI', 'Departamento de Administração e Finanças', @SUSEP, 2, 1, 'RJ', 2, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @DEAFI = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('DETIC', 'Departamento de tecnologia da informação', @SUSEP, 2, 1, 'RJ', 2, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @DETIC = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('COGEP', 'Coordenação de Gestão e Desenvolvimento de Pessoal', @DEAFI, 4, 1, 'RJ', 3, 1, 'susep.rj@susep.gov.br', '', null, null)
SET @COGEP = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('COGET', 'Coordenação de Apoio à Gestão Estratégica', @DEAFI, 4, 1, 'RJ', 3, 5, 'susep.rj@susep.gov.br', '', null, null)
SET @COGET = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('ASDEN', 'Assessoria de Desenvolvimento de Sistemas', @DETIC, 3, 1, 'RJ', 3, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @ASDEN = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('COPROJ', 'Coordenação de Projetos de Tecnologia', @ASDEN, 4, 1, 'RJ', 4, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @COPROJ = @@IDENTITY

INSERT INTO [dbo].[Unidade] VALUES ('COARQ', 'Departamento de tecnologia da informação', @ASDEN, 4, 1, 'RJ', 4, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @COARQ = @@IDENTITY

INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Gestor', '08056275029', getdate(), '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
SET @USUARIOGESTOR = @@IDENTITY


INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Servidor', '08152972541', getdate(), '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Servidor 1', '59516301002', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COARQ, NULL, 8)
INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Servidor 2', '18761704091',  getdate(), '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COARQ, NULL, 8)
INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Servidor 3', '07721701007', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Servidor 4', '51884275087', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Coordenador', '25715446597', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR',1, 1,  @COPROJ, 2, 8)
update [dbo].[Unidade] set pessoaIdChefe = @@IDENTITY where unidadeid = @COPROJ

INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário CG', '95387502500', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @ASDEN, 3, 8)
update [dbo].[Unidade] set pessoaIdChefe = @@IDENTITY where unidadeid = @ASDEN

INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário COGET', '43321040565', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COGET, 4, 8)
INSERT INTO [dbo].[Pessoa] (pesNome, pesCPF, pesDataNascimento, pesMatriculaSiape, pesEmail, situacaoPessoaId, tipoVinculoId, unidadeid, tipoFuncaoId, cargaHoraria) VALUES ('Usuário Diretor', '39178470510', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @DETIC, NULL, 8)
update [dbo].[Unidade] set pessoaIdChefe = @@IDENTITY where unidadeid = @DETIC

DELETE FROM [dbo].[CatalogoDominio] WHERE classificacao = 'GestorSistema'
INSERT INTO [dbo].[CatalogoDominio] VALUES(10001, 'GestorSistema', @USUARIOGESTOR, 1)


INSERT INTO [dbo].[Feriado] VALUES ('2020-1-1', 1,'Confraternização universal', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-1-20', 1,'São Sebastião', 'RJ')
INSERT INTO [dbo].[Feriado] VALUES ('2020-4-21', 1,'Tiradentes', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-4-23', 1,'São Jorge', 'RJ')
INSERT INTO [dbo].[Feriado] VALUES ('2020-5-1', 1,'Dia do Trabalho', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-6-24', 1,'São João', 'BA')
INSERT INTO [dbo].[Feriado] VALUES ('2020-9-7', 1,'Independência do Brasil', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-10-12', 1,'Nossa Senhora Aparecida', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-11-2', 1,'Finados', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-11-15', 1,'Proclamação da República', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-11-20', 1,'Dia da consciência negra', 'RJ')
INSERT INTO [dbo].[Feriado] VALUES ('2020-11-20', 1,'Dia da consciência negra', 'SP')
INSERT INTO [dbo].[Feriado] VALUES ('2020-12-25', 1,'Natal', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-2-24', 0,'Carnaval', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-2-25', 0,'Carnaval', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-2-26', 0,'Sem Expediente', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-4-10', 0,'Sexta-feira Santa', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-6-11', 0,'Corpus Christi', NULL)
INSERT INTO [dbo].[Feriado] VALUES ('2020-10-28', 0,'Dia do Servidor Público', NULL)
