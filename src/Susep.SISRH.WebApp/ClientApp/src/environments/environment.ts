export const environment = {
  production: false,
  identityUrl: "http://homolog2.susep.gov.br/safe/autenticacao",
  apiGatewayUrl: "http://homolog2.susep.gov.br/safe/sisrhapig/api/v1/",
  modo: 'normal', // 'avancado',
  valorPadraoTempoComparecimento: null,
  valorPadraoTermosUso: null,
  formaParticipacaoPlanoTrabalho: null,
  apiKeyGoogleVisio: "",
  client: {
    id: "SISRH.Web",
    secret: '', //"secret",
    scope: "SISRH.API"
  },
};
