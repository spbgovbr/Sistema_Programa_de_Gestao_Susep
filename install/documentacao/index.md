# Manual de instalação do Sistema SUSEP

## Introdução

Esse manual visa detalhar os procedimentos a serem seguidos para instalar e configurar o **Sistema de Gestão de Pessoas por Resultados**, aqui chamado **SGPR**, ferramenta de apoio tecnológico para acompanhamento e controle do cumprimento de metas e alcance de resultados desenvolvida pela Superintendência de Seguros Privados – Susep nos termos da [Instrução Normativa Nº 65, de 30 de julho de 2020](https://www.in.gov.br/en/web/dou/-/instrucao-normativa-n-65-de-30-de-julho-de-2020-269669395).

Esse é um documento técnico destinado às áreas de tecnologia dos órgãos que visam adotar o sistema da Susep como a ferramenta de gestão do teletrabalho. Detalha tecnologias, padrões e pacotes que devem ser implantados para ter o sistema funcionando dentro dos ambientes de cada órgão.

## Base de dados

O sistema foi desenvolvido utilizando o banco de dados [Microsoft SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads) com a ferramenta ORM da Microsoft [Entity Framework](https://docs.microsoft.com/pt-br/ef/) Core nas funções de persistência e com SQL ANSI nas funções de consulta a dados. 

Em teoria, essa configuração permite que diferentes tecnologias de bancos de dados sejam utilizadas. Entretanto, a equipe de tecnologia da Susep garante a compatibilidade e realizou testes apenas com o Microsoft SQL Server.

### Executar script de criação da base

O primeiro passo para instalar o sistema é criar a estrutura do banco de dados da aplicação. O script `Criação da estrutura do banco de dados.sql` executa todos os passos necessários para criar os objetos de banco. 

Além disso, o pacote `Susep.SISRH.Database` tem toda a estrutura do banco em formato do SQL Server Data Tools (SSDT).

Ressalta-se que o sistema é um dos módulos do sistema que gerencia o pessoal dentro da Susep. Por isso, existem dois schemas na base: o `dbo` com as bases gerais do sistema de gestão de pessoas e o `ProgramaGestao` com as bases específicas do módulo do programa de gestão.

### Executar script para popular a tabela catálogo domínio

Após criadas as estruturas de banco de dados, é necessário fazer a inserção dos dados de domínio da aplicação. O script `Inserir dados de domínio.sql` cadastra em banco de dados todas as informações de domínio, incluindo unidades federativas, necessárias para o sistema funcionar.


## Importação de usuários

Uma vez criada a estrutura de banco de dados, é necessário fazer a importação dos dados do órgão no SIAPE para a estrutura do sistema. 

Os perfis do sistema serão derivados da estrutura real do órgão. Então, servidores que não tem função de chefia no órgão terão habilitadas apenas funções de acompanhamento dos seus próprios planos de trabalho. Servidores que tem função de chefia terão acesso aos planos de trabalho das suas respectivas equipes e terão acesso às funções de programas de gestão (cadastro, seleção, avaliação, etc). Servidores que trabalham na área de indicadores poderão, além de acompanhar seus próprios planos de trabalho, cadastrar as listas de atividades do órgão e dos demais setores. 

A ferramenta que fará a carga e manterá os dados atualizados a partir do SIAPE ainda está em fase de construção pela Susep. Os órgãos podem, entretanto, realizar uma carga de dados a partir do seu sistema de recursos humanos para a base do SGPR. As tabelas que devem ser populadas são: `Unidade`, `Pessoa` e `Feriado`.

O script `Inserir dados de teste.sql` traz um exemplo dessa carga com dados fictícios (CPFs gerados aleatoriamente e unidades da estrutura da Susep) e serve para permitir o acesso e a validação do sistema. Assim que o sistema entrar em produção, os dados da tabela `Pessoa` cadastrados por esse script devem ser apagados da base de dados.

### Configurar gestores do sistema

Em determinadas situações como, por exemplo, no caso fictício em que o titular da unidade está de férias e o substituto de licença, pode ser necessário ter pessoas no órgão com acesso total ao sistema para evitar que o trabalho do setor fique parado. Para suprir tal necessidade, existe o perfil Gestor com controle total da ferramenta.

O cadastro de gestores do sistema é feito na tabela CatalogoDominio. Basta inserir um registro nessa tabela com a coluna `classificação` preenchida com o valor `GestorSistema` e a coluna `descrição` preenchida com o id da pessoa que terá perfil de gestor.

## Instalação

Nesse passo a base de dados já deverá estar criada e populada com as informações necessárias para o sistema funcionar. Passa-se, então, à configuração dos componentes da aplicação.

O sistema segue modelos de arquitetura de microserviços e utiliza estruturas separadas para o front e para o back end. Por isso, é necessário configurar diferentes pacotes para a aplicação funcionar. 

A publicação deve ser feita em algum servidor de aplicação. Na Susep, o [Internet Information Services (IIS)](https://pt.wikipedia.org/wiki/Internet_Information_Services) é o servidor utilizado e a implantação é feita em dois servidores diferentes, seguindo a seguinte estrutura: 

1. Servidor web, aberto para a internet, em que ficam publicados o gateway (pasta gateway) e o frontend (pasta app). Na Susep, utiliza-se a seguinte estrutura de pastas:
    * SISGP
        * APP
        * Gateway
2. Servidor web, acessível apenas por meio da máquina do gateway, em que fica publicada a API). Na Susep, utiliza-se a seguinte estrutura de pastas:
    * SISGP
        * API

As pastas app, gateway e api devem ser convertidas em aplicações no IIS e sugere-se que rodem sob um mesmo Application Pool.

Os nomes e as estruturas das pastas poderão variar de acordo com a conveniência do Órgão, mas será importante saber o caminho para cada uma delas.

Para facilitar o entendimento do manual, iremos considerar que os seguintes caminhos foram configurados:

1. APP: https://sitedoorgao/sisgp/app
1. Gateway: https://sitedoorgao/sisgp/gateway
1. API: https://caminhointernoservicos/sisgp/api

### Backend

O backend concentra as regras de negócio e a interação da aplicação com o banco de dados. É separado em dois pacotes:

#### API
A API é onde estão as regras de negócio, funcionalidades, operações de persistência e de consulta aos dados do sistema. 

Alguns métodos só funcionarão quando invocadas por meio do gateway. Entretanto, qualquer pessoa com algum conhecimento de chamadas HTTP conseguirá executar chamadas a alguns métodos da API. Por isso, recomenda-se que esse pacote fique em um ambiente acessível apenas por meio da máquina onde está hospedado o gateway. 

Na pasta api citada acima, deve ser copiado o conteúdo da pasta `Susep.SISRH.WebApi`.

##### Parametrização

Com os arquivos copiados na pasta, deve ser feita a configuração da API, conforme passos abaixo.

###### `web.config`

Faz configurações específicas da aplicação no IIS. 

A parametrização que deve ser observada é a chave `ASPNETCORE_ENVIRONMENT`. Nela, deve ser definido o ambiente em que a aplicação está publicada e o sistema irá procurar pelos demais arquivos de configuração com o nome do ambiente. 

Exemplo: caso a variável seja definida como **Homolog**, o sistema vai procurar pelo arquivo _Settings/appsettings.**Homolog**.json_. 

Essa definição é obrigatória.

###### `Settings/appsettings.json`

É onde são definidos os parâmetros gerais da API como servidor SMTP e demais configurações de e-mail, servidor LDAP e demais configurações do LDAP, etc.

Os próprios arquivos já trazem textos que ajudam a entender o que deve ser preenchido em cada campo. 

###### `Settings/connectionstrings.json`

É onde fica definida a conexão com o banco de dados. 

Usa padrões amplamente utilizados de conexões com banco de dados. Os campos que devem ser alterados estão marcados no arquivo do pacote. 

##### Verificação da instalação

Uma vez instalado e configurado, recomenda-se verificar se a instalação foi feita corretamente acessando a seguinte URL (atentar que o endereço em negrito abaixo é o adotado como caminho para a API): 

[**https://caminhointernoservicos/sisgp/api**/api/v1/dominio/ModalidadeExecucao](https://caminhointernoservicos/sisgp/api/api/v1/dominio/ModalidadeExecucao)

Esse acesso deve retornar um JSON com as modalidades de execução cadastradas na tabela catálogo domínio.

#### Gateway

O gateway adiciona camadas de segurança, cache e outros recursos à API. É essencial para o funcionamento do sistema, para a prevenção de ataques e outros problemas.

Na pasta gateway citada acima, deve ser copiado o conteúdo da pasta `Susep.SISRH.ApiGateway`.

##### Parametrização

Com os arquivos copiados na pasta, deve ser feita a configuração da API, conforme passos abaixo.

###### `web.config`
Faz configurações específicas da aplicação no IIS. 

A parametrização que deve ser observada é a chave `ASPNETCORE_ENVIRONMENT`. Nela, deve ser definido o ambiente em que a aplicação está publicada e o sistema irá procurar pelos demais arquivos de configuração com o nome do ambiente. 

Exemplo: caso a variável seja definida como **Homolog**, o sistema vai procurar pelo arquivo _Settings/appsettings.**Homolog**.json_. 

Essa definição é obrigatória.

###### `Settings/appsettings.json`

É onde são definidos os parâmetros gerais do gateway como a forma como é feita a autenticação e o controle de segurança. 

Pode parecer estranho, mas o gateway controla os acessos externos à API e a API é a responsável por armazenar quais clientes podem acessar o gateway. 

Existe uma espécie de referência cruzada no controle de autenticação, mas funciona corretamente. Essa foi uma forma encontrada pela Susep de evitar que fosse necessário publicar mais um pacote para controlar a autenticação do sistema.

Deve ser alterada a chave authority para apontar para o endereço da API. No exemplo dado nesse manual, o caminho é https://caminhointernoservicos/sisgp/api/

##### `Settings/ocelot.json`

É onde são configuradas as rotas do gateway, respectivos controles de acesso, funcionalidades de cache e outros recursos. 

Deve ser alterados os seguintes campos:

1. Chave `BaseUrl` (no fim do arquivo): Alterar de https://sitedoorgao/sisgp/gateway para o caminho do gateway na sua organização
2. Campos `Host`, `Port`, `DownstreamScheme` e `DownstreamPathTemplate` em todas as entradas do arquivo. 
    > Observe o caminho usado para validação da instalação da API: **https://caminhointernoservicos/sisgp/api**/api/v1/dominio/ModalidadeExecucao
    1. **DownstreamScheme**: `https`
    1. **Host**: `caminhointernoservicos`
    1. **DownstreamPathTemplate**: /sisgp/api/api/v1/dominio/ModalidadeExecucao 
        * esse é o valor para a rota /dominio/ModalidadeExecucao
    1. **Port**: `80` (porta padrão)
    * Esses parâmetros devem ser alterados para que a url formada por eles seja equivalente ao caminho da API no seu órgão (usado para validar a instalação da API). 
    * Deve ser alterada somente a parte em negrito da URL de exemplo acima. Os demais valores que variam entre as rotas devem ser mantidos conforme enviado no arquivo contido no pacote. 

#### `Settings/ocelot.json`

Uma vez instalado e configurado, recomenda-se verificar se a instalação foi feita corretamente acessando a seguinte URL (atentar que o endereço em negrito abaixo é o adotado como caminho para o gateway): 

**https://sitedoorgao/sisgp/gateway**/dominio/ModalidadeExecucao

Esse acesso deve retornar um JSON com as modalidades de execução cadastradas na tabela catálogo domínio (mesmo retorno da validação da instalação pela API).

### Frontend

O frontend é a camada de apresentação do sistema. Construída em angular, teoricamente, pode ser publicada em qualquer servidor de aplicação que rode node.js. Na Susep, utiliza-se, também, aplicações no IIS.

Na pasta app citada acima, deve ser copiado o conteúdo da pasta `Susep.SISRH.WebApp`.

#### Parametrização

Com os arquivos copiados na pasta, deve ser feita a configuração da API, conforme passos abaixo.

##### `ClientApp\dist\env.js`

Nos dois pontos em que aparece, deve ser alterado de https://sitedoorgao/sisgp/gateway para o caminho do gateway no seu órgão. 

##### `ClientApp\dist\Index.html`

A tag base deve ser alterada para refletir o caminho em que a aplicação está publicada entro do site do seu órgão. No caso de exemplo desse manual em que a acesso à aplicação é feito por meio do caminho https://sitedoorgao/sisgp/app, a tag deve ter o valor `/sisgp/app/`.


##### `web.config`
A tag action do tipo Rewrite deve ser alterada para refletir o caminho em que a aplicação está publicada entro do site do seu órgão, conforme configuração feita anteriormente para o arquivo index.html.

## Observações e comentários
Após a construção do sistema, com a publicação da IN 65, houve uma mudança de nomenclatura das entidades. Antes, o sistema chamava de _plano de trabalho_ o que hoje é chamado de _programa de gestão_ e chamava de _pacto de trabalho_ o que hoje é chamado de _plano de trabalho_. Logo, caso seja necessário fazer alguma manutenção ou verificar alguma informação em banco de dados, deve-se levar em conta essa mudança de nomes. 

A solução proposta pela Susep possui controles apenas por unidade federativa e não por cidade. Essa é uma característica da Susep que só tem representação em capitais. Essa restrição pode impactar algumas verificações automáticas de feriados que acontecem em determinadas cidades de uma unidade federativa e não acontecem em outras.

É essencial que o LDAP do órgão registre ou o CPF ou o e-mail cadastrados para a pessoa no banco de dados. É por meio desses campos (qualquer um isoladamente ou os dois) que será feito o link entre o LDAP e a tabela Pessoa para identificar o usuário autenticado e seu respectivo perfil.

Momentaneamente está sendo disponibilizado os pacotes para publicação do sistema. Assim que os fontes forem publicados, poderão ser alteradas configurações como, por exemplo, o secret do cliente para acesso ao gateway.
