# Sistema Programa de Gestão de Desempenho - PGD

Esse é o Sistema da Superintendência de Seguros Privados (Susep) para a gestão do Programa de Gestão de Desempenho (PGD) em conformidade com a [IN65/2020](https://www.in.gov.br/en/web/dou/-/instrucao-normativa-n-65-de-30-de-julho-de-2020-269669395).

~~Para a correta instalação, consulte o manual de instalação e utilize os arquivos disponibilizados na pasta `install/`~~ (veja a próxima seção).

Para acesso ao código fonte, utilize os arquivos disponibilizados na pasta `src/`

Para entender os conceitos e principais funcionalidades, assista à apresentação [Sistema SUSEP - Programa de Gestão - Teletrabalho](https://youtu.be/VU_1TTAMg2Y).

Mais detalhes:

* https://www.gov.br/servidor/pt-br/assuntos/programa-de-gestao


## Subir aplicação em ambiente Docker

É possível subir a aplicação por meio do [Docker](https://www.docker.com/). Dentre as vantagens estão:
1. A ausência da necessidade de uma configuração do IIS;
1. A simplificação da configuração (a maior parte das configurações já foram feitas);
1. A ausência da necessidade de possuir licenças para o Windows Server para rodar a aplicação, tendo em vista que as imagens foram configuradas utilizando Microsoft suporta oficialmente;
1. A ausência da obrigatoriedade de configurar um servidor SQL Server. O docker-compose utiliza o SQL Server 2019 para Linux, oficialmente suportado para a Microsoft, no modo de avaliação. Atente-se que a utilização em produção exige uma licença válida.

**Observações:**

* Para possibilitar a execução em ambiente docker (imagens linux/amd64), realizamos uma troca do plugin de logs Eventlog para o Serilog, pois o Serilog é multiplataforma. É possível ver todas as mudanças e adições realizadas neste [pull request](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/pull/17);
* Dado que até o momento o código fonte não foi completamente compartilhado, tivemos de utilizar algumas `dlls` já compiladas, disponibilizadas pela SUSEP. Elas foram copiadas da pasta [install/](install/) para a pasta [src/Susep.libs/](src/Susep.libs/);
* Deve ser possível utilizar a mesma solução em servidores Windows, caso ele esteja configurado para rodar imagens Linux. Porém, o desempenho possivelmente será ligeiramente inferior do que se configurar diretamente com IIS. Não foi testado;
  * A microsoft também disponibiliza imagens docker nativas para Windows. Porém a minha versão do SO não é a Professional, então não tive como testar. A geração de imagens com Github actions está nos planos futuros.
* A aplicação provavelmente deve funcionar em máquinas Mac, mas também não foi testado.


### Configurando a aplicação

Em uma máquina que tenha o [Docker](https://docs.docker.com/engine/install/) e o [docker-compose](https://docs.docker.com/compose/install/) instalados, baixe o código. Esse passo pode ser via git
```bash
git clone https://github.com/SrMouraSilva/Sistema_Programa_de_Gestao_Susep.git
git checkout docker-codigo-fonte
```
ou baixando diretamente o código pelo link:
* <https://github.com/SrMouraSilva/Sistema_Programa_de_Gestao_Susep/archive/docker-codigo-fonte.zip>

Após baixar, acesse a pasta do projeto pelo terminal
```bash
cd Sistema_Programa_de_Gestao_Susep
```

Por fim, execute o seguinte comando
```bash
docker-compose -f docker/docker-compose.yml up -d
```

Pronto, a aplicação está acessível no endereço http://localhost. Porém você não irá conseguir se logar se não configurar o LDAP (veja abaixo) e se não inserir as pessoas na tabela de pessoas.

#### Configurações

Após alterar uma configuração, execute
```bash
docker-compose -f docker/docker-compose.yml down
docker-compose -f docker/docker-compose.yml up -d
```

##### Configurar Servidor de email

Acesse o arquivo `docker/api/Settings/appsettings.Homolog.json` e edite as seguintes linhas
```json
"emailOptions": {
  "EmailRemetente": "ENDEREÇO EMAIL REMETENTE SAIDA",
  "NomeRemetente": "NOME REMETENTE SAIDA",
  "SmtpServer": "SERVIDOR SMTP",
  "Port": "PORTA SERVIDOR SMTP"
},
```

##### Configurar Servidor ldap

Acesse o arquivo `docker/api/Settings/appsettings.Homolog.json` e edite as seguintes linhas
```json
"ldapOptions": {
  "Url": "URL SERVIDOR LDAP",
  "Port": 389,
  "BindDN": "DN do usuário de serviço que será utilizado para autenticar no LDAP", //Exemplo: CN=Fulano de tal,CN=Users,DC=orgao
  "BindPassword": "Senha do usuário de serviço que será utilizado para autenticar no LDAP",
  "SearchBaseDC": "DC que será utilizado para chegar à base de usuários no LDAP", //Exemplo: CN=Users,DC=orgao
  "SearchFilter": "Consulta a ser aplicada no LDAP para encontrar os usuários", //Exemplo: (&(objectClass=user)(objectClass=person)(sAMAccountName={0}))
  "CpfAttributeFilter": "Campo do LDAP em que será encontrado o CPF do usuário", 
  "EmailAttributeFilter": "Campo do LDAP em que será encontrado o e-mail do usuário"
}
```

###### Observações

* O login só ocorrerá adequadamente caso exista um usuário na tabela `[dbo].[Pessoa]` com o CPF e o email igual ao usuário do LDAP;
* Caso seja consultado uma pessoa que não exista na base do LDAP, o `api-gateway` retornará um erro `500` e nada será exibido para o usuário pelo `web-app`. No response você poderá ver uma mensagem como `System.Threading.Tasks.TaskCanceledException: A task was canceled.`;
* Por algum motivo desconhecido, em alguns casos é necessário pressionar o botão de `Entrar` duas vezes;
* A aplicação possui um [backdook](https://pt.wikipedia.org/wiki/Backdoor) para simplificar o processo de homologação. Você pode ver [a lista completa dos usuários que não necessitam de senha](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/blob/97892e1/src/Susep.SISRH.Application/Auth/ResourceOwnerPasswordValidator.cs#L45-L54). Caso utilize em produção, **RETIRE ESSES USUÁRIOS DO SCRIPT** `install/3. Inserir dados de teste.sql`. [Desconheço se o compilado disponibilizado pela SUSEP possui também esse backdoor](https://github.com/spbgovbr/Sistema_Programa_de_Gestao_Susep/tree/97892e1/install).

##### Configurar Acesso ao Banco de Dados

Caso você queira utilizar um servidor de banco de dados SQL Server com a devida licença, será necessário alterar a configuração do banco.
Acesse o arquivo `connectionstrings.Homolog.json` e edite as seguintes linhas:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "data source=db;initial catalog=master;User ID=sa;Password=P1ssw@rd;"
  }
}
```

##### Configurar Script de inserção de dados no Banco de Dados

**Obs:** Este passo só é necessário caso você utilize o banco de dados configurado no docker-compose. Caso você decida utilizar outro banco de dados (por exemplo, um banco SQL Server com licença), naturalmente você deverá executar todos os scripts do banco.

Edite o arquivo `install/3. Inserir dados de teste.sql`, conforme desejado.

**Obs:** Quando o banco de dados aplicação sobe, são executados os três arquivos `.sql` automaticamente.

## Trabalhos futuros

1. Configurar um servidor LDAP de testes para possibilitar uma homologação do sistema sem precisar configurar manualmente esse passo (criar um docker-compose específico);
1. Banco de homologação
   1. Configurar adequadamente o volume do banco;
   1. Evitar que o scripts sqls sejam executados mais de uma vez em caso de reinício do banco de dados;
1. Criar docker compose de produção:
   1. Sem banco de dados;
   1. Ajustar configurações para uso em produção para evitar exibir stacktrace  desnecessário para os usuários:
      * Mudar arquivos de configuração de `Homolog` para `Production`;
      * Mudar variáveis de ambiente de `Homolog` para `Production`.
1. Gerar imagens nativas para Windows por Github Actions.

## Outras informações

Caso você deseje fazer o build local ao invés de utilizar a imagem preparada:
```
docker build -f docker/Dockerfile -t susep .
```