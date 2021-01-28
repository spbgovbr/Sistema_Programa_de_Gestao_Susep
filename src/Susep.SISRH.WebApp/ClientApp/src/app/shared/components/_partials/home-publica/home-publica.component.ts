import { Component, OnInit } from '@angular/core';
import { SecurityService } from '../../../services/security.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home-publica',
  templateUrl: './home-publica.component.html',
  styleUrls: ['./home-publica.component.css']
})
export class HomePublicaComponent implements OnInit {  

  constructor(private securityService: SecurityService) {}

  ngOnInit() { }

  autenticar() {
    this.securityService.goToAuthentication();
  }
  
}
