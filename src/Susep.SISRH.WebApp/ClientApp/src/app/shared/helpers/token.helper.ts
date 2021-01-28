import { UrlHelper } from "./url.encoder.helper";


/*
 * Classe utilitária que implementa as funcionalidades de leitura de tokens
*/

export class TokenHelper {
  static getDataFromToken(token: any) {
    let data = {};
    if (typeof token !== 'undefined') {
      let encoded = token.split('.')[1];
      data = JSON.parse(UrlHelper.urlBase64Decode(encoded));
    }

    return data;
  }
}
