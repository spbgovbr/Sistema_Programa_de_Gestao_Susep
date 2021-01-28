import { Component, Input, OnInit } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { PerfilEnum } from '../../../modules/programa-gestao/enums/perfil.enum';
import { IPerfilUsuario, IUsuario } from '../../models/perfil-usuario.model';
import { ApplicationStateService } from '../../services/application.state.service';

@Component({
  selector: 'secure-input',
  templateUrl: './secure-input.component.html'
})
export class SecureInputComponent implements OnInit {

  @Input() perfis: number[];
  @Input() unidade: BehaviorSubject<number>;
  @Input() servidor: BehaviorSubject<number>;

  temPerfil: boolean;
  somenteVerificacaoGeral = true;
  perfilUsuario: IUsuario;

  constructor(private applicationStateService: ApplicationStateService) { }  

  ngOnInit() {    
    this.applicationStateService.perfilUsuario.subscribe(perfis => {
      this.perfilUsuario = perfis;
      this.carregarPermissoes();
    });
  }

  carregarPermissoes() {
    if (this.perfilUsuario) {
      if (this.perfilUsuario.perfis.filter(p => p.perfil === PerfilEnum.Gestor || p.perfil === PerfilEnum.Administrador).length > 0) {
        this.temPerfil = true;
      }
      else {

        this.temPerfil = this.verificarPermissoes(this.perfilUsuario.perfis);

        if (this.unidade)
          this.unidade.subscribe(u => this.verificarPermissaoUnidade());

        if (this.servidor)
          this.servidor.subscribe(u => this.verificarPermissaoServidor());
      }
    }
  }

  limparSeApenasGeral() {
    if (this.somenteVerificacaoGeral)
      this.temPerfil = false;
    this.somenteVerificacaoGeral = false;
  }

  verificarPermissaoServidor() {
    this.limparSeApenasGeral();
    //Se for o próprio servidor, vê se a ação pode ser acessada pelo perfil servidor
    if (this.perfilUsuario.pessoaId === this.servidor.value &&
      this.perfilUsuario.perfis.filter(pu => pu.perfil === PerfilEnum.Servidor).length > 0) {
      this.temPerfil = true;
    }
  }

  verificarPermissaoUnidade() {
    this.limparSeApenasGeral();

    //Obtém todos os perfis de gestão do usuário na unidade desejada
    const perfisUnidade = this.perfilUsuario.perfis.filter(pu =>
      (pu.perfil === PerfilEnum.Servidor ||
       pu.perfil === PerfilEnum.ChefeUnidade ||
       pu.perfil === PerfilEnum.CoordenadorGeral ||
       pu.perfil === PerfilEnum.Diretor) &&
      pu.unidades.filter(u => u === this.unidade.value).length > 0);

    this.temPerfil = this.verificarPermissoes(perfisUnidade);
  }

  verificarPermissoes(perfisVerificar: IPerfilUsuario[]): boolean {
    let achouPerfil = false;
    this.perfis.forEach(pi => {
      perfisVerificar.forEach(pu => {
        if (pi === pu.perfil) {
          achouPerfil = true;
        }
      })
    });
    return achouPerfil;
  }

}
