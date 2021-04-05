import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { PerfilEnum } from 'src/app/modules/programa-gestao/enums/perfil.enum';
import { IAssunto } from '../../models/assunto.model';
import { AssuntoDataService } from '../../services/assunto.service';

@Component({
  selector: 'assunto-edicao',
  templateUrl: './assunto-edicao.component.html',
})
export class AssuntoEdicaoComponent implements OnInit {

  PerfilEnum = PerfilEnum;

  form: FormGroup;

  assunto: IAssunto;
  assuntos: IAssunto[];

  constructor(
    public router: Router,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
    private assuntoDataService: AssuntoDataService
  ) { }

  ngOnInit() {

    this.form = this.formBuilder.group({
      valor: ['', [Validators.required, Validators.minLength(5)]],
      assuntoPaiId: [null, []],
      ativo: [null, []]
    });

    this.assuntoDataService.ObterAtivos().subscribe(result => {
      this.assuntos = result.retorno;
    });

    const id = this.route.snapshot.params.id;
    if (id) {
      this.assuntoDataService.ObterPorId(id).subscribe(result => {
        this.assunto = result.retorno;

        this.form.patchValue({
          valor: this.assunto.valor,
          assuntoPaiId: this.assunto.assuntoPaiId,
          ativo: this.assunto.ativo
        });
      });
    }
  }

  salvar() {
    if (this.form.valid) {
      const dados : IAssunto = this.form.value;

      if (this.assunto) {
        dados.assuntoId = this.assunto.assuntoId;
        this.assuntoDataService.AtualizarAssunto(dados).subscribe(result => {
          if (result.retorno) {
            this.router.navigateByUrl('/assunto');
          }
        });
      }
      else {
        this.assuntoDataService.CadastrarAssunto(dados).subscribe(result => {
          if (result.retorno) {
            this.router.navigateByUrl('/assunto');
          }
        });
      }
    }
    else {
      this.getFormValidationErrors(this.form)
    }
  }

  getFormValidationErrors(form) {
    Object.keys(form.controls).forEach(field => {
      const control = form.get(field);
      control.markAsDirty({ onlySelf: true });
    });
  }

}

