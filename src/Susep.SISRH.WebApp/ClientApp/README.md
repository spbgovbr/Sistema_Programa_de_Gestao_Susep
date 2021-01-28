# Susep.SISRH.WebApp

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 6.0.0.

## Development server

Run `ng serve` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `dist/` directory. Use the `--prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).

## Usar a api de serviço mockada para o backend

Para usar a api mockada, instale o json-server, globalmente `npm install -g json-server`. Instruções de uso em [Json Server](https://www.npmjs.com/package/json-server)

Os serviços mockados devem ser colocados dentro do arquivo `mocked-server-data.json`, na raiz da aplicação web (pasta ClientApp). O formato deve ser o seguinte: 

```
{
    "servico1": dados1,
    "servico2": dados2
}
```

Ex.:

```
{
    "dias-semana": [
        { "ordem": 1, "descricao": "domingo" },
        { "ordem": 2, "descricao": "segunda-feira" },
        { "ordem": 3, "descricao": "terça-feira" },
        { "ordem": 4, "descricao": "quarta-feira" },
        { "ordem": 5, "descricao": "quinta-feira" },
        { "ordem": 6, "descricao": "sexta-feira" },
        { "ordem": 7, "descricao": "sabado" }
    ]
}
```

Abra um terminal na raiz da aplicação e execute o comando `json-server --watch --routes mocked-server-routes.json mocked-server-data.json`. A porta default é 3000. Para subir o servidor na porta 3004, por exemplo, basta adicionar `--port 3004` ao comando. O arquivo `mocked-server-data.json` contém os dados e o arquivo `mocked-server-routes.json` contém as rotas customizadas.

Recuperamos os dados disponíveis no exemplo acima através da url `http://localhost:3000/dias-semana`.

