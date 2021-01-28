export interface IConfiguration {

  identityUrl: string,
  apiGatewayUrl: string,
  modo: TipoModo,
  clientId: string,
  clientSecret: string,
  userAuthScope: string,
  clientAuthScope: string,
  signalrHubUrl: string,

}

export type TipoModo = 'normal' | 'avancado';
