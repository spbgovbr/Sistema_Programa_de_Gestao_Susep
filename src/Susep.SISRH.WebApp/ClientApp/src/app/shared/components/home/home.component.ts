import { Component, OnInit } from '@angular/core';
import { ApplicationStateService } from '../../services/application.state.service';
import { Router } from '@angular/router';

@Component({
  selector: 'corretores-home',
  templateUrl: './home.component.html'
})
export class HomeComponent implements OnInit {

  isAuthenticated: boolean;

  constructor(
    private applicationState: ApplicationStateService,
    private router: Router
  ) { }

  ngOnInit() {
    this.applicationState.isAuthenticated.subscribe(value => {
      this.isAuthenticated = value;
      if (this.isAuthenticated) {
        this.router.navigateByUrl('/dashboard');
      }
      else {
        this.router.navigateByUrl('/login');
      }
    });
  }

}
