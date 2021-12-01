import { Injectable } from '@angular/core';

import { Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';
import { DataService } from '../../../shared/services/data.service';
import { ConfigurationService } from '../../../shared/services/configuration.service';
import { ApplicationResult } from '../../../shared/models/application-result.model';
import { IDadosPaginados } from '../../../shared/models/pagination.model';
import { IPlanoTrabalho, IPlanoTrabalhoAtividade, IPlanoTrabalhoMeta, IPlanoTrabalhoReuniao, IPlanoTrabalhoHistorico, IPlanoTrabalhoAtividadeCandidato, IPlanoTrabalhoPessoaModalidade, IPlanoTrabalhoCusto, IPlanoTrabalhoEmpresa, IPlanoTrabalhoObjeto } from '../models/plano-trabalho.model';
import { IPlanoTrabalhoPesquisa } from '../models/plano-trabalho.pesquisa.model';
import { IPactoTrabalho, IPactoTrabalhoObjeto } from '../models/pacto-trabalho.model';
import { IDadosCombo } from '../../../shared/models/dados-combo.model';
import { Guid } from 'src/app/shared/helpers/guid.helper';

@Injectable()
export class PlanoTrabalhoDataService {

  constructor(
    private service: DataService,
    private configuration: ConfigurationService) { }

  ObterPagina(dadosBusca: IPlanoTrabalhoPesquisa): Observable<ApplicationResult<IDadosPaginados<IPlanoTrabalho>>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const params = this.service.toQueryParams(dadosBusca);
    const url = `${baseURI}planotrabalho?${params}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPlano(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterTermoAceite(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/termoAceite`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterAtual(): Observable<ApplicationResult<IPlanoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/atual`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterHabilitacao(): Observable<ApplicationResult<IPlanoTrabalho>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/habilitacao`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  Cadastrar(dados: IPlanoTrabalho): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  Alterar(dados: IPlanoTrabalho): Observable<ApplicationResult<string>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarFase(planoTrabalhoId: string, situacaoId: number, justificativa?: string, aprovados?: string[], deserto = false): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/situacao`;

    return this.service.put(url, { situacaoId: situacaoId, observacoes: justificativa, aprovados: aprovados, deserto: deserto }).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterHistorico(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoHistorico[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/historico`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterAtividades(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoAtividade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/atividades`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarAtividade(dados: IPlanoTrabalhoAtividade): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/atividades`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarAtividade(dados: IPlanoTrabalhoAtividade): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/atividades/${dados.planoTrabalhoAtividadeId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirAtividade(planoTrabalhoId: string, planoTrabalhoAtividadeId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/atividades/${planoTrabalhoAtividadeId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterCandidatos(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoAtividadeCandidato[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/atividades/candidato`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterCandidatosPorAtividade(planoTrabalhoId: string, planoTrabalhoAtividadeId: string): Observable<ApplicationResult<IPlanoTrabalhoAtividadeCandidato[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/atividades/${planoTrabalhoAtividadeId}/candidato`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  Candidatar(dados: IPlanoTrabalhoAtividade): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/atividades/${dados.planoTrabalhoAtividadeId}/candidato`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AtualizarCandidatura(dados: IPlanoTrabalhoAtividadeCandidato): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/atividades/${dados.planoTrabalhoAtividadeId}/candidato/${dados.planoTrabalhoAtividadeCandidatoId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterPessoasModalidades(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoPessoaModalidade[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/pessoasModalidadesExecucao`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterMetas(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoMeta[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/metas`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarMeta(dados: IPlanoTrabalhoMeta): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/metas`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarMeta(dados: IPlanoTrabalhoMeta): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/metas/${dados.planoTrabalhoMetaId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirMeta(planoTrabalhoId: string, planoTrabalhoMetaId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/metas/${planoTrabalhoMetaId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }



  ObterReunioes(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoReuniao[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/reunioes`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarReuniao(dados: IPlanoTrabalhoReuniao): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/reunioes`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarReuniao(dados: IPlanoTrabalhoReuniao): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/reunioes/${dados.planoTrabalhoReuniaoId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirReuniao(planoTrabalhoId: string, planoTrabalhoReuniaoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/reunioes/${planoTrabalhoReuniaoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }



  ObterCustos(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoCusto[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/custos`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarCusto(dados: IPlanoTrabalhoCusto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/custos`;
    if (dados.valor)
      dados.valor = dados.valor.toString().replace(/\D/, ',');
    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarCusto(dados: IPlanoTrabalhoCusto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/custos/${dados.planoTrabalhoCustoId}`;
    if (dados.valor)
      dados.valor = dados.valor.toString().replace(/\D/, ',');
    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirCusto(planoTrabalhoId: string, planoTrabalhoReuniaoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/custos/${planoTrabalhoReuniaoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }



  private empresas: IPlanoTrabalhoEmpresa[] = [
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 1' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 2' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 3' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 4' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 5' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 6' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 7' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 8' }, 
    { planoTrabalhoId: Guid.newGuid(), planoTrabalhoEmpresaId: Guid.newGuid(), nome: 'Empresa 9' }, 
];

  ObterEmpresas(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoEmpresa[]>> {
    /* IMPLEMENTAR */
    /*
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/empresas`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
    */
    return of({
      retorno: this.empresas.filter((v, i) => i >= 0 && i < 5)
    });
  }

  ObterEmpresassPorTexto(valor: string): Observable<ApplicationResult<IPlanoTrabalhoEmpresa[]>> {
    // IMPLEMENTAR
    return of({
      retorno: this.empresas.filter((v, i) => i >= 5 && i < 9)
    });
  }

  ObterEmpresaPorId(id: Guid): Observable<ApplicationResult<IPlanoTrabalhoEmpresa>> {
    // IMPLEMENTAR
    return of({
      retorno: this.empresas.find(e => e.planoTrabalhoEmpresaId === id)
    });
  }



  ObterPactosTrabalho(planoTrabalhoId: string): Observable<ApplicationResult<IPactoTrabalho[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/pactosTrabalho`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }


  ObterObjetos(planoTrabalhoId: string): Observable<ApplicationResult<IPlanoTrabalhoObjeto[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/objetos`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterObjetosAssociadosOuNaoAAtividadeDoPacto(planoTrabalhoId: string, pactoTrabalhoAtividadeId?: string): Observable<ApplicationResult<IPactoTrabalhoObjeto[]>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const dados = {pactoTrabalhoAtividadeId: pactoTrabalhoAtividadeId}
    const params = this.service.toQueryParams(dados);
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/objetosassociadosounaoatividadepacto?${params}`;

    return this.service.get(url, true, 2, false, false).pipe(map((response: any) => {
      return response;
    }));
  }

  ObterObjeto(planoTrabalhoObjetoId: string): Observable<ApplicationResult<IPlanoTrabalhoObjeto>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/objetos/${planoTrabalhoObjetoId}`;

    return this.service.get(url).pipe(map((response: any) => {
      return response;
    }));
  }

  ExcluirObjeto(planoTrabalhoId: string, planoTrabalhoObjetoId: string): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${planoTrabalhoId}/objetos/${planoTrabalhoObjetoId}`;

    return this.service.delete(url).pipe(map((response: any) => {
      return response;
    }));
  }

  CadastrarObjeto(dados: IPlanoTrabalhoObjeto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/objetos`;

    return this.service.post(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

  AlterarObjeto(dados: IPlanoTrabalhoObjeto): Observable<ApplicationResult<boolean>> {
    const baseURI = this.configuration.getApiGatewayUrl();
    const url = `${baseURI}planotrabalho/${dados.planoTrabalhoId}/objetos/${dados.planoTrabalhoObjetoId}`;

    return this.service.put(url, dados).pipe(map((response: any) => {
      return response;
    }));
  }

}
