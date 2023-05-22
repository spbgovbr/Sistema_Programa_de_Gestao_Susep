import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SecurityService } from '../../services/security.service';

@Component({
  selector: 'login',
  templateUrl: './login.component.html',  
})
export class LoginComponent implements OnInit {

  form: FormGroup;
  passwordType = 'password';

  loginError: string;

  constructor(
    public router: Router,
    private formBuilder: FormBuilder,
    private securityService: SecurityService) {
  }

  ngOnInit() {

    this.form = this.formBuilder.group({
      username: ['', [Validators.required]],
      password: ['', [Validators.required]]
    }, { updateOn: 'change' });
  }

  toggleMostrarPassword() {
    this.passwordType = this.passwordType === 'password' ? 'text' : 'password';
  }

  logar() {
    const username = this.form.get('username').value;
    const password = this.form.get('password').value;

    this.loginError = '';

    this.securityService.authenticate(username, password)
      .subscribe(ret => {
        if (ret.token_type === 'Invalid Credentials')
          this.loginError = 'Usuário e senha não conferem';
        else if (ret.token_type !== 'Bearer')
          this.loginError = ret.token_type;
    });
  }

  keyup() {
    this.form.get('username').updateValueAndValidity();
    this.form.get('password').updateValueAndValidity();
  }
  
  onPressEnter() {
    this.form.markAllAsTouched();
  }
}
