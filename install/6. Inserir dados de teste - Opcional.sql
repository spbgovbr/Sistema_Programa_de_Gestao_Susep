
/*** Limpeza das tabelas que receberão dados de teste ***/

DELETE FROM [dbo].[Pessoa]
DELETE FROM [dbo].[Unidade]
DELETE FROM [dbo].[TipoFuncao]
GO

/*** EXEMPLO DE REGISTRO DE TIPOS DE FUNÇÃO ***/
   
INSERT INTO [dbo].[TipoFuncao] ([tipoFuncaoId],[tfnDescricao],[tfnCodigoFuncao],[tfnIndicadorChefia]) 
   VALUES (2, 'Diretor', '101.5', 1)
INSERT INTO [dbo].[TipoFuncao] ([tipoFuncaoId],[tfnDescricao],[tfnCodigoFuncao],[tfnIndicadorChefia]) 
   VALUES (3, 'Coordenador-Geral', '101.4', 1)
INSERT INTO [dbo].[TipoFuncao] ([tipoFuncaoId],[tfnDescricao],[tfnCodigoFuncao],[tfnIndicadorChefia]) 
   VALUES (4, 'Coordenador', '101.3', 1)
GO
   
/*** EXEMPLO DE REGISTRO DE UNIDADES ***/
/*** Os IDs das unidades serão utilizados para o registro de pessoas na sequência ***/
   
DECLARE @SUSEP INT
DECLARE @DEAFI INT
DECLARE @DETIC INT
DECLARE @COGEP INT
DECLARE @COGET INT
DECLARE @ASDEN INT
DECLARE @COPROJ INT
DECLARE @COARQ INT
DECLARE @USUARIOGESTOR INT

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('SUSEP', 'Superintendência de Seguros Privados', NULL, 13, 1, 'RJ', 1, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @SUSEP = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('DEAFI', 'Departamento de Administração e Finanças', @SUSEP, 2, 1, 'RJ', 2, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @DEAFI = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('DETIC', 'Departamento de tecnologia da informação', @SUSEP, 2, 1, 'RJ', 2, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @DETIC = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('COGEP', 'Coordenação de Gestão e Desenvolvimento de Pessoal', @DEAFI, 4, 1, 'RJ', 3, 1, 'susep.rj@susep.gov.br', '', null, null)
SET @COGEP = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('COGET', 'Coordenação de Apoio à Gestão Estratégica', @DEAFI, 4, 1, 'RJ', 3, 5, 'susep.rj@susep.gov.br', '', null, null)
SET @COGET = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('ASDEN', 'Assessoria de Desenvolvimento de Sistemas', @DETIC, 3, 1, 'RJ', 3, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @ASDEN = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto])
   VALUES ('COPROJ', 'Coordenação de Projetos de Tecnologia', @ASDEN, 4, 1, 'RJ', 4, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @COPROJ = @@IDENTITY

INSERT INTO [dbo].[Unidade] ([undSigla],[undDescricao],[unidadeIdPai],[tipoUnidadeId],[situacaoUnidadeId],[ufId],[undNivel],[tipoFuncaoUnidadeId],[Email],[undCodigoSIORG],[pessoaIdChefe],[pessoaIdChefeSubstituto]) 
   VALUES ('COARQ', 'Departamento de tecnologia da informação', @ASDEN, 4, 1, 'RJ', 4, NULL, 'susep.rj@susep.gov.br', '', null, null)
SET @COARQ = @@IDENTITY

/*** EXEMPLO DE REGISTRO DE PESSOAS ***/
    
/*** Esta pessoa será designada como usuário gestor na tabela CatalogoDominio ***/
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário Gestor', '08056275029', getdate(), '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
SET @USUARIOGESTOR = @@IDENTITY
DELETE FROM [dbo].[CatalogoDominio] WHERE classificacao = 'GestorSistema'
INSERT INTO [dbo].[CatalogoDominio] VALUES(10001, 'GestorSistema', @USUARIOGESTOR, 1)

/*** Pessoas sem função associada ***/
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria])
   VALUES ('Usuário Servidor', '08152972541', getdate(), '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário Servidor 1', '59516301002', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COARQ, NULL, 8)
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário Servidor 2', '18761704091',  getdate(), '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COARQ, NULL, 8)
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário Servidor 3', '07721701007', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria])
   VALUES ('Usuário Servidor 4', '51884275087', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COPROJ, NULL, 8)
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário COGET', '43321040565', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @COGET, 4, 8)
   
/*** Pessoas com função associada e respectiva atualização na tabela de Unidades ***/
INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria])
   VALUES ('Usuário Coordenador', '25715446597', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR',1, 1,  @COPROJ, 2, 8)
update [dbo].[Unidade] set pessoaIdChefe = @@IDENTITY where unidadeid = @COPROJ

INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário CG', '95387502500', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @ASDEN, 3, 8)
update [dbo].[Unidade] set pessoaIdChefe = @@IDENTITY where unidadeid = @ASDEN

INSERT INTO [dbo].[Pessoa] ([pesNome],[pesCPF],[pesDataNascimento],[pesMatriculaSiape],[pesEmail],[situacaoPessoaId],[tipoVinculoId],[unidadeId],[tipoFuncaoId],[cargaHoraria]) 
   VALUES ('Usuário Diretor', '39178470510', getdate(),  '111', 'EMAILPESSOA@ORGAO.GOV.BR', 1, 1, @DETIC, NULL, 8)
update [dbo].[Unidade] set pessoaIdChefe = @@IDENTITY where unidadeid = @DETIC

GO
    
/*** EXEMPLO DE REGISTROS PARA A TABELA DE FERIADOS ***/
/**** Datas no formato YYYYMMDD. Caso ocorra erro, considere refisar o formato  ****/
   
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200101', 1,'Confraternização universal', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200120', 1,'São Sebastião', 'RJ')
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200421', 1,'Tiradentes', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200423', 1,'São Jorge', 'RJ')
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200501', 1,'Dia do Trabalho', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200624', 1,'São João', 'BA')
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200907', 1,'Independência do Brasil', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201012', 1,'Nossa Senhora Aparecida', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201102', 1,'Finados', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201115', 1,'Proclamação da República', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201120', 1,'Dia da consciência negra', 'RJ')
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201120', 1,'Dia da consciência negra', 'SP')
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201225', 1,'Natal', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200224', 0,'Carnaval', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200225', 0,'Carnaval', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200226', 0,'Sem Expediente', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200410', 0,'Sexta-feira Santa', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20200611', 0,'Corpus Christi', NULL)
INSERT INTO [dbo].[Feriado] ([ferData],[ferFixo],[ferDescricao],[ufId]) VALUES ('20201028', 0,'Dia do Servidor Público', NULL)
GO
