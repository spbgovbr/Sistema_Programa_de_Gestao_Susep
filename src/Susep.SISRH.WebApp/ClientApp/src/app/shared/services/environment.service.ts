import { Injectable, OnInit } from '@angular/core';
import { environment } from './../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EnvironmentService implements OnInit {

  public identityUrl = '';
  public apiGatewayUrl = '';
  public modo = '';
  public client: IClientConfiguration = {};

  constructor() {}

  ngOnInit(): void {
    console.log(environment)
    this.identityUrl = environment.identityUrl;
    this.apiGatewayUrl = environment.apiGatewayUrl;
    this.modo = environment.modo;
    this.client = environment.client;
  }

}

export interface IClientConfiguration
{
  id?: string,
  secret?: string,
  scope?: string

}
