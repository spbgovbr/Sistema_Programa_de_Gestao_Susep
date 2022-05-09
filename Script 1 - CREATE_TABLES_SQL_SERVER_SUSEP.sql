CREATE TABLE [tb_pessoa]
(
	id_pessoa int NOT NULL IDENTITY(1, 1), 
	ds_nome nvarchar(250) NULL , 
	nr_cpf nvarchar(14) NULL , 
	st_registro nvarchar(25) NULL , 
	CONSTRAINT [PK_TBPESSOA] PRIMARY KEY (id_pessoa)
)
GO

CREATE TABLE [tb_usuario]
(
	id_usuario int NOT NULL IDENTITY(1, 1), 
	id_pessoa int NULL , 
	ds_email nvarchar(100) NULL , 
	co_senha nvarchar(200) NULL , 
	st_registro nvarchar(25) NULL , 
	CONSTRAINT [PK_TBUSUARIO] PRIMARY KEY (id_usuario), 
	CONSTRAINT [FK_TBUSUARIO_PESSOA] FOREIGN KEY (id_pessoa) REFERENCES tb_pessoa (id_pessoa)
)
GO
CREATE 
	NONCLUSTERED INDEX id_pessoa
		ON [tb_usuario] (id_pessoa ASC)
GO

CREATE TABLE [tb_perfil]
(
	id_perfil int NOT NULL IDENTITY(1, 1), 
	ds_perfil nvarchar(100) NULL , 
	ds_detalhe nvarchar(max) NULL, 
	st_registro nvarchar(25) NULL , 
	CONSTRAINT [PK_TBPERFIL] PRIMARY KEY (id_perfil)
)
GO

CREATE TABLE [tb_funcao]
(
	id_funcao int NOT NULL IDENTITY(1, 1), 
	ds_funcao nvarchar(200) NULL , 
	ds_detalhe nvarchar(max) NULL, 
	ds_pagina_acesso nvarchar(200) NULL , 
	st_registro nvarchar(25) NULL , 
	CONSTRAINT [PK_TBFUNCAO] PRIMARY KEY (id_funcao)
)
GO

CREATE TABLE [rl_perfil_funcao]
(
	id_perfil int NOT NULL, 
	id_funcao int NOT NULL, 
	st_alterar nvarchar(25) NULL , 
	st_cadastrar nvarchar(25) NULL , 
	st_excluir nvarchar(25) NULL , 
	st_visualizar nvarchar(25) NULL , 
	st_registro nvarchar(25) NULL , 
	CONSTRAINT [PK_RLPF] PRIMARY KEY (id_funcao, id_perfil), 
	CONSTRAINT [FK_RLPF_PERFIL] FOREIGN KEY (id_perfil) REFERENCES tb_perfil (id_perfil), 
	CONSTRAINT [FK_RLPF_FUNCAO] FOREIGN KEY (id_funcao) REFERENCES tb_funcao (id_funcao)
)
GO
CREATE 
	NONCLUSTERED INDEX id_perfil
		ON [rl_perfil_funcao] (id_perfil ASC)
GO

CREATE TABLE [rl_usuario_perfil]
(
	id_usuario int NOT NULL, 
	id_perfil int NOT NULL, 
	CONSTRAINT [PK_RLUP] PRIMARY KEY (id_usuario, id_perfil), 
	CONSTRAINT [PK_RLUP_USUARIO] FOREIGN KEY (id_usuario) REFERENCES tb_usuario (id_usuario), 
	CONSTRAINT [PK_RLUP_PERFIL] FOREIGN KEY (id_perfil) REFERENCES tb_perfil (id_perfil)
)
GO
CREATE 
	NONCLUSTERED INDEX id_perfil
		ON [rl_usuario_perfil] (id_perfil ASC)
GO

CREATE TABLE [tb_log]
(
	id_log int NOT NULL IDENTITY(1, 1), 
	id_usuario int NULL , 
	ds_acao nvarchar(max) NULL, 
	ds_tipo_acao nvarchar(200) NULL , 
	dt_acao datetime2(0) NULL , 
	ds_nivel_acao nvarchar(200) NULL , 
	CONSTRAINT [PK_TBLOG] PRIMARY KEY (id_log), 
	CONSTRAINT [FK_TBLOG_USUARIO] FOREIGN KEY (id_usuario) REFERENCES tb_usuario (id_usuario)
)
GO
CREATE 
	NONCLUSTERED INDEX id_usuario
		ON [tb_log] (id_usuario ASC)
GO

CREATE TABLE [tb_parametro]
(
	ds_chave nvarchar(50) NOT NULL, 
	ds_valor nvarchar(200) NULL , 
	CONSTRAINT [PK_TBPARAM] PRIMARY KEY (ds_chave)
)
GO

INSERT INTO [tb_pessoa] (ds_nome, nr_cpf, st_registro) 
VALUES ('Usuário de Sistema - API PG.Cade', '000.000.000-00', 'ATIVO')
INSERT INTO [tb_pessoa] (ds_nome, nr_cpf, st_registro) 
VALUES ('Henrique Alves', '111.111.111-11', 'ATIVO')

INSERT INTO [tb_perfil] (ds_perfil, ds_detalhe, st_registro) 
VALUES ('ROLE_ADMIN', 'Administrador do Sistema', 'ATIVO')
INSERT INTO [tb_perfil] (ds_perfil, ds_detalhe, st_registro) 
VALUES ('ROLE_USUARIO', 'Usuário do Sistema', 'ATIVO')
INSERT INTO [tb_perfil] (ds_perfil, ds_detalhe, st_registro) 
VALUES ('ROLE_USUARIO_AVANCADO', 'Usuário Avançado do Sistema', 'ATIVO')

INSERT INTO [tb_usuario] (id_pessoa, ds_email, co_senha, st_registro) 
VALUES (1, 'usapipg@cade.gov.br', NULL, 'ATIVO')
INSERT INTO [tb_usuario] (id_pessoa, ds_email, co_senha, st_registro) 
VALUES (2, 'user@gov.br', '$2a$10$bVH7/Gxnux5yKLvkUjuHheV32HqIzRRgkSG0rFkczuQa/h1NpMH6a', 'ATIVO') /* @t3st3# */

INSERT INTO [rl_usuario_perfil] (id_usuario, id_perfil) VALUES (1, 2)
INSERT INTO [rl_usuario_perfil] (id_usuario, id_perfil) VALUES (2, 1)

INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Painel de Funcionalidades', 'Painel de funcionalidades disponíveis na API PG.Cade.', 'PAINEL_FUNCIONALIDADES', 'ATIVO') 
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Funções', 'Funcionalidade que permite ao usuário manter (pesquisar, cadastrar, alterar e excluir) as funções da API PG.Cade.', 'MANTER_FUNCOES', 'ATIVO')  
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Perfis', 'Funcionalidade que permite ao usuário manter (pesquisar, cadastrar, alterar e excluir) os perfis da API PG.Cade.', 'MANTER_PERFIS', 'ATIVO')
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Usuários', 'Funcionalidade que permite ao usuário manter (pesquisar, cadastrar, alterar e excluir) os usuários da API PG.Cade.', 'MANTER_USUARIOS', 'ATIVO')
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Logs', 'Funcionalidade que permite ao usuário consultar os logs gerados pela API PG.Cade.', 'MANTER_LOGS', 'ATIVO')
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('API PG.Cade - Enviar Planos', 'Funcionalidade da API PG.Cade que permite ao usuário enviar os planos de trabalho cadastrados no Sistema PG.Cade para a Plataforma do Programa de Gestão - PGD do Ministério da Economia (ME).', 'PAINEL_API_PG_CADE', 'ATIVO')
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('API PG.Cade - Planos Enviados', 'Funcionalidade da API PG.Cade que permite ao usuário consultar os planos de trabalho enviados para a Plataforma do Programa de Gestão - PGD do Ministério da Economia (ME).', 'PAINEL_API_PLANOS_ENVIADOS', 'ATIVO')
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Parâmetros do Sistema', 'Funcionalidade que permite ao usuário manter (pesquisar, cadastrar, alterar e excluir) os parâmetros da API PG.Cade.', 'PAINEL_PARAMETROS', 'ATIVO')
INSERT INTO [tb_funcao] (ds_funcao, ds_detalhe, ds_pagina_acesso, st_registro)
VALUES ('Agendamento do Envio de Planos', 'Funcionalidade que permite ao usuário agendar o envio de planos de trabalhos da API PG.Cade.', 'PAINEL_AGENDAMENTO', 'ATIVO')

INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 1, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 2, 'SIM', 'SIM', 'SIM', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 3, 'SIM', 'SIM', 'SIM', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 4, 'SIM', 'SIM', 'SIM', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 5, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 6, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 7, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 8, 'SIM', 'SIM', 'SIM', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (1, 9, 'SIM', 'SIM', 'NAO', 'SIM', 'ATIVO')

INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (2, 1, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (2, 9, 'SIM', 'SIM', 'NAO', 'SIM', 'ATIVO')

INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (3, 1, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (3, 6, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (3, 7, 'NAO', 'NAO', 'NAO', 'SIM', 'ATIVO')
INSERT INTO [rl_perfil_funcao] (id_perfil, id_funcao, st_alterar, st_cadastrar, st_excluir, st_visualizar, st_registro) 
VALUES (3, 9, 'SIM', 'SIM', 'NAO', 'SIM', 'ATIVO')

INSERT INTO [tb_log] (id_usuario, ds_acao, ds_tipo_acao, dt_acao, ds_nivel_acao) 
VALUES (1, '0 0 0 1 * *', 'LOG_CRON_AGENDAMENTO', '2021-06-23', 'NIVEL_USUARIO')

INSERT INTO [tb_parametro] (ds_chave, ds_valor) 
VALUES ('DS_AGENDAMENTO_ENVIO_PLANOS', '0 0 0 * * *')
INSERT INTO [tb_parametro] (ds_chave, ds_valor) 
VALUES ('ST_AGENDAMENTO_ENVIO_PLANOS', 0)
