# SUSEP - Docker

* **Versão atual:** 1.8
* Versão 1.7: https://github.com/SrMouraSilva/Sistema_Programa_de_Gestao_Susep/tree/v1.7

É possível subir a aplicação por meio do [Docker](https://www.docker.com/). Dentre as vantagens estão:
1. A ausência da necessidade de uma configuração do IIS;
1. Uma menor intervenção manual para a realização das configurações;
1. A ausência da necessidade de possuir licenças para o Windows Server para rodar a aplicação, tendo em vista que as imagens foram configuradas utilizando Microsoft suporta oficialmente;
1. A ausência da obrigatoriedade de configurar um servidor SQL Server para ambiente de homologação. O docker-compose utiliza o SQL Server 2019 para Linux, oficialmente suportado para a Microsoft, no modo de avaliação. Atente-se que a utilização em produção exige uma licença válida, mas a configuração disponibilizada pemite a realização de testes e da homologação do sistema.
1. A possibilidade de configuração por variáveis de ambiente no docker-compose no lugar de alterar arquivos `.json`.

**Observações:**

* ~~Para possibilitar a execução em ambiente docker (imagens linux/amd64), realizamos uma troca do plugin de logs `Eventlog` para o `Serilog` por este ser multiplataforma;~~ Mudanças incluídas no código oficial.
* O processo de criação de imagem compila o código disponibilizado. Entretanto, são utilizadas algumas `dlls` previamente compiladas;
* Deve ser possível utilizar a mesma solução em servidores Windows, caso ele esteja configurado para rodar imagens Linux. Porém, o desempenho possivelmente será ligeiramente inferior do que se configurar diretamente com IIS. Não foi testado;
  * A Microsoft também disponibiliza imagens docker nativas para Windows. Porém, até o momento, não foram geradas imagens nativas.
* A aplicação provavelmente deve funcionar em máquinas Mac, mas também não foi testado.


## Configurando a aplicação

Em uma máquina que tenha o [Docker](https://docs.docker.com/engine/install/) e o [docker-compose](https://docs.docker.com/compose/install/) instalados, baixe o código. Esse passo pode ser via git
```bash
git clone https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep.git
git checkout docker-codigo-fonte
```
ou baixando diretamente o código pelo link:
* <https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/archive/docker-codigo-fonte.zip>

Após baixar, acesse a pasta do projeto pelo terminal
```bash
cd Sistema_Programa_de_Gestao_Susep
```

Por fim, execute o seguinte comando para subir a aplicação
```bash
docker compose -f docker/docker-compose.yml up -d
```
E o seguinte comando para subir o banco de dados de homologação
```bash
# Obs: Execute DEPOIS do docker/docker-compose.yml
docker compose -f docker/docker-compose.sqlserver-homologacao.yml up -d
```

Pronto, a aplicação está acessível no endereço http://localhost. Porém você não irá conseguir se logar se não configurar o LDAP (veja abaixo) se não inserir as pessoas na tabela de pessoas.

### Configurações

Após alterar uma configuração, execute
```bash
docker compose -f docker/docker-compose.yml down
docker compose -f docker/docker-compose.yml up -d
```

#### Verificando se deu certo

Execute o seguinte comando
```bash
docker compose -f docker/docker-compose.yml ps -a
```
Os 4 containers devem estar ativos.
```
        Name                      Command               State                                          Ports                                        
---------------------------------------------------------------------------
docker_api-gateway_1   dotnet Susep.SISRH.ApiGate ...   Up
docker_traefik_1       /entrypoint.sh --providers ...   Up      80/tcp, ...
docker_web-api_1       dotnet Susep.SISRH.WebApi.dll    Up
docker_web-app_1       dotnet Susep.SISRH.WebApp.dll    Up
```

Caso nenhum dos três relacionados ao dotnet subirem (`web-app`, `web-api`, `gateway`), provavelmente o usuário **dos containers** não tem permissão o suficiente para subir o processo e utilizar a porta 80. Esse erro foi detectado no CentOs, mas não ocorre no Debian e nem no Ubuntu. A forma que sabemos até o momento de "contornar" esse problema é dizer que o usuário root que irá rodar esse processo. Altere o docker-compose adicionando o seguinte:
```diff
web-api:
    image: ghcr.io/srmourasilva/sistema_programa_de_gestao_susep/sgd:latest
+    user: 0:0
...
api-gateway:
    image: ghcr.io/srmourasilva/sistema_programa_de_gestao_susep/sgd:latest
+    user: 0:0
...
web-app:
    image: ghcr.io/srmourasilva/sistema_programa_de_gestao_susep/sgd:latest
+    user: 0:0
```
Atente-se que o yml é sensível a identação e que foi utilizado espaço como identação.

#### Configurar Servidor de email

Acesse o arquivo `docker/docker-compose.yml` e edite as seguintes linhas conforme sua necessidade
```yaml
      # Configurações de e-mail - Exemplo: Ministério da Economia
      - emailOptions__EmailRemetente=no-reply@me.gov.br
      - emailOptions__NomeRemetente=Programa de Gestão - ME
      - emailOptions__SmtpServer=smtp.me.gov.br
      - emailOptions__Port=25
```

#### Configurar Servidor ldap

Acesse o arquivo `docker/docker-compose.yml` e edite as seguintes linhas
```yml
      # LDAP
      # -> URL do Servidor LDAP
      - ldapOptions__Configurations__0__Url=
      # -> Porta do Servidor LDAP
      - ldapOptions__Configurations__0__Port=389
      # -> DN do usuário de serviço que será utilizado para autenticar no LDAP"
      - ldapOptions__Configurations__0__BindDN=CN=Fulano de tal,CN=Users,DC=orgao
      # -> Senha do usuário de serviço que será utilizado para autenticar no LDAP
      - ldapOptions__Configurations__0__BindPassword=
      # -> DC que será utilizado para chegar à base de usuários no LDAP
      - ldapOptions__Configurations__0__SearchBaseDC=CN=Users,DC=orgao
      # -> Consulta a ser aplicada no LDAP para encontrar os usuários
      - ldapOptions__Configurations__0__SearchFilter=(&(objectClass=user)(objectClass=person)(sAMAccountName={0}))
      # -> Campo do LDAP em que será encontrado o CPF do usuário
      - ldapOptions__Configurations__0__CpfAttributeFilter=
      # -> Campo do LDAP em que será encontrado o e-mail do usuário
      - ldapOptions__Configurations__0__EmailAttributeFilter=
```

**Obs:** Note que é possível definir mais de uma configuração. Basta copiar as linhas e trocar `__n__` por `__n+1__` nas linhas novas (ex: `__0__` -> `__1__`).

#### Observações

* O login só ocorrerá adequadamente caso exista um usuário na tabela `[dbo].[Pessoa]` com o CPF e o email igual ao usuário do LDAP;
* Caso seja consultado uma pessoa que não exista na base do LDAP, o `api-gateway` retornará um erro `500` e nada será exibido para o usuário pelo `web-app`. No response você poderá ver uma mensagem como `System.Threading.Tasks.TaskCanceledException: A task was canceled.`;
* Por algum motivo desconhecido, em alguns casos é necessário pressionar o botão de `Entrar` duas vezes;
* A aplicação possui [usuários de teste](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep#valida%C3%A7%C3%A3o-da-instala%C3%A7%C3%A3o-3%C2%AA-etapa) para simplificar o processo de homologação. Você pode ver [a lista completa dos usuários que não necessitam de senha](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/97892e1/src/Susep.SISRH.Application/Auth/ResourceOwnerPasswordValidator.cs#L45-L54). Caso utilize em produção, não execute o script `install/4. Inserir dados de teste - Opcional.sql`. Caso não sejam retirados, estes usuários poderão serem utilizados por pessoas má-intensionadas como [backdook](https://pt.wikipedia.org/wiki/Backdoor).

#### Configurar Acesso ao Banco de Dados

Caso você queira utilizar um servidor de banco de dados SQL Server com a devida licença, será necessário alterar a configuração do banco.
Acesse o arquivo ``docker/docker-compose.yml` e edite as seguintes linhas:
```yaml
      # Configurações de banco de dados
      - ConnectionStrings__DefaultConnection=Server=db,1433;Database=master;User Id=sa;Password=P1ssw@rd;
```

#### Configurar Script de inserção de dados no Banco de Dados

**Obs:** Este passo só é necessário caso você utilize o banco de dados configurado no `docker-compose.sqlserver-homologacao.yml`. Caso você decida utilizar outro banco de dados (por exemplo, um banco SQL Server com licença), naturalmente você deverá executar todos os scripts do banco definidos como obrigatório.

Edite o arquivo `install/4. Inserir dados de teste - Opcional.sql`, conforme desejado.

Caso não seja detectada a existência do banco `programa_gestao`, ele será criado e os scripts `.sql` serão executados, conforme definido em `docker-compose.sqlserver-homologacao.yml`. Os registros do banco são persistidos na pasta `docker/volume` por meio de volume docker ([bind mount](https://docs.docker.com/storage/bind-mounts/)). Caso seja detectado a existência do banco `programa_gestao`, nenhum script será executado, mesmo que você adicione um novo script sql na lista de volumes.

Se por ventura você editar os arquivos `sql`, eles somente serão automaticamente executados caso você exclua a pasta `docker/volume/mssql/`. Entretanto, isto acarretará na perda de todos os dados. Caso você venha de uma versão anterior (a partir da 1.7), e queira manter os dados que existiam anteriormente, execute os eventuais novos scripts em sql manualmente.

## Trabalhos futuros

* [ ] Configurar um servidor LDAP de testes para possibilitar uma homologação do sistema sem precisar configurar manualmente esse passo (criar um docker-compose específico);
* [ ] Gerar imagens nativas para Windows por Github Actions;
* [ ] Gerar um build do código para indicar quando um código enviado para o gitlab estiver incompleto.

## Outras informações

Caso você deseje fazer o build local ao invés de utilizar a imagem preparada:
```bash
docker build -f docker/Dockerfile -t susep .
```
Lembre-se de alterar o `docker-compose.yml` para utilizar `susep` no lugar da imagem oficial.