Para entender os conceitos e principais funcionalidades, assista à apresentação disponível no seguinte link: https://youtu.be/VU_1TTAMg2Y

# PARCEIROS!

>SUSEP -Superintendência de Seguros Privados
>
>DTI ME -Diretoria de Tecnologia da Informação do Ministério da Economia

# INTRODUÇÃO!

Esse roteiro visa detalhar os procedimentos a serem seguidos para instalar e configurar o Sistema de Programa de Gestão, aqui chamado SISPG, ferramenta de apoio tecnológico para acompanhamento e controle do cumprimento de metas e alcance de resultados nos termos da Instrução Normativa Nº 65, de 30 de julho de 2020. Esse é um documento técnico destinado às áreas de tecnologia dos órgãos que visam adotar o SISPG como ferramenta de gestão do teletrabalho. Detalha tecnologias, padrões e pacotes que devem ser implantados para ter o sistema funcionando dentro dos ambientes de cada órgão.

# LISTA DE RECURSOS MÍNIMOS!

Dimensionamento de recursos, pré-requisitos, dependências, etc.

✔Internet Information Services (IIS) 7.5 ou mais novo

✔Microsoft SQL Server 2012 ou, Microsoft SQL Server 2016 SP2 Express ou, Microsoft SQL Server 2019

✔Microsoft .NET Core 2.2 1 -Windows Server Hosting

✔Microsoft .NET Core Runtime –2.2.1 (x64)

✔Microsoft .NET Core Runtime –2.2.1 (x86)

✔Microsoft .NET Core Runtime –3.1.8 (x64)

✔Microsoft .NET Core Runtime –3.1.8 (x86)

✔Microsoft .NET Core SDK 3.1.403 (x64)

✔Microsoft Visual C++ 2015-2019 Redistributable (x64)

✔Microsoft Visual C++ 2017 Redistributable (x86)

✔Microsoft Windows Desktop Runtime –3.1.9 (x64)

# INFRAESTRUTURA

### Sequência de passos 
>1. Habilitar IIS -> "[Imagem de exemplo](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/main/install/Arquivos%20de%20configura%C3%A7%C3%A3o/Habilita%C3%A7%C3%A3o%20IIS.jpg)".

> Realizar a instalação através do Server Manager (Add roles and features);
> Selecionar o Role Web Server (IIS);
> Selecionar (IIS);
> Em Application Development: ASP.NET

>2. Instalar os arquivos da pasta "[dependências](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/main/install/Arquivos%20de%20configura%C3%A7%C3%A3o/Instala%C3%A7%C3%A3o%20de%20Depend%C3%AAncias%20.NET%20-%20AMBIENTE.txt)".

# DIAGRAMA DE INSTALAÇÃO!

Passa-se, então, à configuração dos componentes da aplicação. O sistema segue modelos de arquitetura de micro serviços e utiliza estruturas separadas para o front-end e para o back-end. Diante disso, faz-se necessário configurar diferentes pacotes para o correto funcionamento da aplicação.

Sequência de instalação sugerida

>| 1ª Etapa |`'Base de Dados'`            |<br/>
>|2ª Etapa |`Back-End `|<br/> API<br/> 
>|3ª Etapa |`Front-End` |<br/>Gateway <br/>APP |


> **Importante:** Sugere-se as etapas acima destacadas para a correta instalação do SISGP.

# BASE DE DADOS!

O sistema foi desenvolvido utilizando o banco de dados Microsoft SQL Server com a ferramenta ORM da Microsoft Entity Framework Core nas funções de persistência e com SQL ANSI nas funções de consulta a dados.Em teoria, essa configuração permite que diferentes tecnologias de bancos de dados sejam utilizadas. Entretanto, a equipe de tecnologia da Susep garante a compatibilidade e realizou testes apenas com o Microsoft SQL Server.

## Sequência de passos (SQL Server)

>1. Criar banco de dados DBSISGP.
>2. Executar o script “[1. Criação da estrutura do banco de dados.sql](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/main/install/1.%20Cria%C3%A7%C3%A3o%20da%20estrutura%20do%20banco%20de%20dados.sql "1. Criação da estrutura do banco de dados.sql")”.
>3. Executar o script “[2. Inserir dados de domínio.sql](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/main/install/2.%20Inserir%20dados%20de%20dom%C3%ADnio.sql "2. Inserir dados de domínio.sql")”.
>4. Se for ambiente de desenvolvimento/homologação,executar o script “[3. Inserir dados de teste.sql](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/main/install/3.%20Inserir%20dados%20de%20teste.sql "3. Inserir dados de teste.sql")”.
>5. Criar um usuário de aplicação com permissões de leitura e escrita.

### Importação de usuários (desenvolvimento/homologação/produção)

Uma vez criada a estrutura de banco de dados, é necessário fazer a importação dos dados do órgão no SIAPE para a estrutura do sistema.

> **TOME NOTA** O script “Inserir dados de teste”traz um exemplo de carga com dados fictícios (CPFs gerados aleatoriamente e unidades da estrutura da Susep), serve para permitir o acesso e a validação do sistema. Assim que o sistema entrar em produção, os dados da tabela **Pessoa** e **Unidade** cadastrados por esse script devem ser apagados da base de dados.

Os perfis do sistema serão derivados da estrutura real do órgão. Desse modo, servidores que não tem função de chefia no órgão recebem valor null na coluna tipoFuncaoIdda tabela Pessoae deste modo terão habilitadas apenas funções de acompanhamento dos seus próprios planos de trabalho, ao passo que servidores com função de chefia poderão ter acesso aos planos de trabalho das suas respectivas equipes e terão acesso às funções de programas de gestão (cadastro, seleção, avaliação,etc). Servidores que trabalham na área de indicadores poderão, além de acompanhar seus próprios planos de trabalho, cadastrar as listas de atividades do órgão e dos demais setores.

# Configurar gestores do sistema (1º Acesso)

Em determinadas situações como, por exemplo, no caso fictício em que o titular da unidade está de férias e o substituto de licença, pode ser necessário ter pessoas no órgão com acesso total ao sistema para evitar que o trabalho do setor fique parado. Para suprir tal necessidade, existe o perfil Gestor com controle total da ferramenta. 

>O cadastro de gestores do sistema é feito na tabela **CatalogoDominio**. Basta inserir um registro nessa tabela com a coluna classificação preenchida com o valor **‘GestorSistema’** e a coluna descrição preenchida com o id da pessoa que terá perfil de gestor.

# APLICAÇÃO!

A publicação deve ser feita em algum servidor de aplicação. Sugere-se o Internet Information Services (IIS) na seguinte estrutura:

>I. Servidor web, acessível apenas por meio da máquina do Gateway, em que fica publicada a API.<br/>
	◦ SISGP
		▪ API
		
>II. Servidor web, aberto para a internet, em que ficam publicados o Gateway (pasta Gateway) e o front-end (pasta APP).<br/>
◦ SISGP
▪ APP
▪ Gateway

As pastas APP, gateway e APIdevem ser convertidas em aplicações no IIS e sugere-se que rodem sob um mesmo Application Pool.

>**TOME NOTA: **Para ambiente de desenvolvimento/homologação, é suficiente apenas servidor interno. Já para produção, faz-se necessário um servidor interno e outro externo, e nesse caso as pastas “Gateway” e “APP” deverão estar no servidor externo enquanto a pasta “API” estará no servidor interno.

As pastas APP, Gateway e API devem ser convertidas em aplicações no IIS e sugere-se que rodem sob um mesmo Application Pool. Os nomes e as estruturas das pastas poderão variar de acordo com a conveniência do Órgão, todavia será importante conhecer o caminho para cada uma delas.
	Para facilitar o entendimento do roteiro, considera-se que os seguintes caminhos foram configurados:

>APP: https://**servidorExterno**/sisgp/app<br/>
Gateway: https://**servidorExterno**/sisgp/gateway<br/>
API: https://**servidorInterno**/sisgp/api

## Sequência de passos (.NET)

>1. Criar um Pool de Aplicativos chamado “sisgp”.
>2. Criar um novo site vinculado ao pool “sisgp” e apontando para o caminho físico da pasta “api”.
>3. Criar um novo site vinculado ao pool “sisgp” e apontando para o caminho físico da pasta “gateway”.
>4. Criar um novo site vinculado ao pool “sisgp” e apontando para o caminho físico da pasta “app”.

# BACK-END!

O back-end concentra as regras de negócio e a interação da aplicação com o banco de dados e é composto por dois pacotes, API e Gateway. 

## API

A API é onde estão as regras de negócio, funcionalidades, operações de persistência e de consulta aos dados do sistema.

>**TOME NOTA:** Alguns métodos só funcionarão quando invocadas por meio do Gateway. Entretanto, com algum conhecimento de chamadas HTTP é possível executar chamadas a alguns métodos da API. Por isso, recomenda-se que esse pacote fique em um ambiente acessível apenas por meio da máquina onde está hospedado o Gateway.

## Sequência de passos (pasta “API”)

>1. No arquivo “web.config”, alterar o valor da variável ASPNETCORE_ENVIRONMENT com um dos valores a seguir, de acordo com o ambiente: Dev, Homolog ou Prod.
>2. Em “Settings/connectionstrings.AMBIENTE.json”, informar os valores para conexão com o banco de dados, sendo “data source” oservidor, “initial catalog” o nome do banco, “User ID” o usuário e “Password” a senha.
>3. Em “Settings/appsettings.AMBIENTE.json”, configurar o servidor de e-mail SMTP e o LDAP para autenticação dos usuários - ESTA ETAPA PODE OCORRER APÓS VALIDAÇÃO DA APLICAÇÃO COM USERS DE TESTE.

### VALIDAÇÃO DA INSTALAÇÃO –1ª ETAPA

Após instalação e configuração da base de dados e API, recomenda-se a validação dessas etapas pela seguinte URL: 
>**URL da Aplicação**/api/api/v1/dominio/ModalidadeExecucao

Se a instalação foi realizada corretamente, deverá retornar um json com as modalidades de trabalho da tabela catálogo domínio.

#### Em caso de erro:

I. Abra um prompt de comando
II. Navegue até a pasta da API e digite o comando

>Dotnet susep.sisrh.webapi.dll

O retorno é o possível erro.

GATEWAY

O Gateway adiciona camadas de segurança, cache e outros recursos à API. É essencial para o funcionamento do sistema, para a prevenção de ataques e outros problemas.

## Sequência de passos (pasta “gateway”)

>1. No arquivo “web.config”, alterar o valor da variável ASPNETCORE_ENVDQRONMENT com um dos valores a seguir, de acordo com o ambiente: Dev, Homolog ou Prod.
>2. Em “Settings/appsettings.**AMBIENTE**.json”, informar a url da API no campo “authority”.
>3. Em “Settings/ocelot.**AMBIENTE**.json”,dentro do objeto “ReRoutes”, substituir todos os campos “Host” e “Port” pelas do servidor da API. Caso a API esteja com https, alterar todos os "DownstreamScheme". Caso a API não esteja na raiz do servidor, completar os caminhos em todos os "DownstreamPathTemplate". Já no final do arquivo substituir o campo "BaseUrl" pela url do gateway.

### VALIDAÇÃO DA INSTALAÇÃO –2ª ETAPA

Após instalação e configuração do Gateway, recomenda-se a validação dessa etapa pela seguinte URL: 

>**URL da aplicação**/gateway/dominio/ModalidadeExecucao

Se a instalação foi realizada corretamente, deverá retornar um json com as modalidades de trabalho da tabela catálogo domínio (mesmo retorno da validação da API).

#### Em caso de erro:

I. Abra um prompt de comando
II. Navegue até a pasta do Gateway e digite o comando

>Dotnet susep.sisrh.webapi.dll

O retorno é o possível erro.

# FRONT-END!

O front-end é a camada de apresentação do sistema. Construída em angular, teoricamente, pode ser publicada em qualquer servidor de aplicação que rode node.js.

## APP

#### ClientApp\dist\env.js
Nos dois pontos em que aparece, deve ser alterado de https://sitedoorgao/sisgp/gateway para o caminho do gateway no seu órgão. 

#### ClientApp\dist\Index.html
A tag base deve ser alterada para refletir o caminho em que a aplicação está publicada entro do site do seu órgão. Exemplo: O acesso à aplicação é feito por meio do caminho https://sitedoorgao/sisgp/app, a tag deve ter o valor /app/.

#### web.config
A tag action do tipo Rewrite deve ser alterada para refletir o caminho em que a aplicação está publicada entro do site do seu órgão, conforme configuração feita anteriormente para o arquivo index.html.

### Sequência de passos (pasta “app”)

>1. No arquivo “ClientApp/dist/env.js”, alterar o valor das variáveis “window.__env.identityUrl” e “window.__env.apiGatewayUrl” para a url do gateway.
>2. No arquivo “ClientApp/dist/index.html”, dentro da tag `<base />`, colocar no href o caminho da publicação da pasta app no servidor.
>3. No arquivo “ClientApp/dist/web.config”, dentro da tag `<action />` do tipo Rewrite, colocar na url o caminho da publicação da pasta app no servidor.
>4. Acessar a url do app e verificar se a instalação foi realizada corretamente. Ela deverá retornar a tela de login do sistema.

### VALIDAÇÃO DA INSTALAÇÃO –3ª ETAPA

Após instalação e configuração da aplicação, recomenda-se a validação dessa etapa acesdo a aplicação com os usuários de teste cadastrados na carga de teste: 

>**URL da aplicação**

#### Usuários:

- sisgp_gestor
- sisgp_cg   
- sisgp_coget          
- sisgp_coordenador       
- sisgp_diretor     
- sisgp_servidor     
- sisgp_servidor1
- sisgp_servidor2
-  sisgp_servidor3
- sisgp_servidor4
- Obs.: Senha para todos os usuários ("qualquer carácter")

> Assim que o sistema entrar em produção, os dados da tabela **Pessoa** e **Unidade** cadastrados por esse script devem ser apagados da base de dados.

### Volte à etapa BACK-END! e vincule o LDAP


### Nota da versão 7 ###
Principais funcionalidades adicionadas:
	Chefe passa a ter acesso a todos os planos dos servidores da sua unidade, mesmo que o plano tenha sido executado em outra unidade
	Adição de tela para a consulta da estrutura hierárquica
	Criação da possibilidade de reabertura de plano
	Adição de botão para marcar/desmarcar todas as atividades ao criar um PGD
	Ajustes gerais de layout

Principais bugs corrigidos:
	Problemas no aceite do plano
	Retirada de possibilidade de abrir mais de um plano de trabalho para o mesmo período
	Ajustes na data de encerramento do plano
	Atualização do tempo total do plano após alterar o periodo
	Retirada da possibilidade de abrir mais de uma solicitação de exclusão da mesma atividade


Arquivos de configuração alterados:
	Gateway:
		Ocelot.json
			* Novas rotas: 
				/pactotrabalho/{pactoTrabalhoid}/reabrir


	API:
		AppSettings.json
			* Configuração dos textos dos e-mails para deixar de ficar hard coded

	
Alterações na estrutura do BD (arquivo '5. Alteracoes da estrutura do BD para a V7.sql'):
	Aumento dos tamanhos dos campos Titulo e Entregas Esperadas da tabela ItemCatalogo
	Adição de campo na tabela PactoTrabalhoAtividade para informar onde (se local ou remoto) a atividade foi executada
	Adição de campos para registrar o chefe e o substituto da unidade

