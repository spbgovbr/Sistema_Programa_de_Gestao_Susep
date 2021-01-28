import { Pipe, PipeTransform } from "@angular/core";
import { IChaveDescricao } from "./input-autocomplete-async.component";

@Pipe({
  name: 'filtrarJaEscolhidos', 
  pure: false
})
export class FiltrarItensJaEscolhidosPipe implements PipeTransform {
  transform(items: IChaveDescricao[], chavesJaEscolhidas: any[]): IChaveDescricao[] {
    if (!items || !chavesJaEscolhidas || !chavesJaEscolhidas.length) return items;
    return items.filter(item => !chavesJaEscolhidas.includes(item.chave));
  }
}
  