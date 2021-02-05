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

Pronto, a aplicação está acessível no endereço http://localhost.

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

1. Configurar um servidor LDAP de testes para possibilitar uma homologação do sistema sem precisar configurar manualmente esse passo (criar um docker-compose específico)
2. Evitar que o scripts sqls sejam executados mais de uma vez
3. Configurar adequadamente o volume do banco
4. Deixar banco opcional (criar um docker-compose específico)

#### Outras informações

Caso você deseje fazer o build local ao invés de utilizar a imagem preparada:
```
docker build -f docker/Dockerfile -t susep .
```