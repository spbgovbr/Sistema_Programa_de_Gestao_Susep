export interface IConfiguration {

  identityUrl: string,
  apiGatewayUrl: string,
  modo: TipoModo,
  valorPadraoTempoComparecimento: number,
  valorPadraoTermosUso: string,
  clientId: string,
  clientSecret: string,
  userAuthScope: string,
  clientAuthScope: string,
  signalrHubUrl: string,

}

export type TipoModo = 'normal' | 'avancado';
