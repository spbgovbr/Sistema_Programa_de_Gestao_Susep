export interface IConfiguration {

  identityUrl: string,
  apiGatewayUrl: string,
  modo: TipoModo,
  valorPadraoTempoComparecimento: number,
  valorPadraoTermosUso: string,
  formaParticipacaoPlanoTrabalho: string,
  frequenciaPresencialObrigatoria: boolean,
  clientId: string,
  clientSecret: string,
  userAuthScope: string,
  clientAuthScope: string,
  signalrHubUrl: string,

}

export type TipoModo = 'normal' | 'avancado';
