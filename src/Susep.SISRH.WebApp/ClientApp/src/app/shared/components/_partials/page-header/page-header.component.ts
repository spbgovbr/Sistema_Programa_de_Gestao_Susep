import { Component, OnInit, ViewChild } from '@angular/core';
import { ApplicationStateService } from '../../../services/application.state.service';
//import { ListModalService } from '../../../modal/list-modal.service';
import { MatDialogRef } from '@angular/material';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
selector: 'app-page-header',
templateUrl: './page-header.component.html'
})
export class PageHeaderComponent implements OnInit {  

@ViewChild('modalLoading', { static: true }) modalLoading;

isLoading = false;

constructor(
  private modalService: NgbModal,
  private applicationState: ApplicationStateService) { }

  ngOnInit() {
    
    this.applicationState.isLoading.subscribe(isLoading => {
      this.isLoading = isLoading;
      this.modalService.dismissAll();
      if (isLoading) {
        this.modalService.open(this.modalLoading, {
          size: 'sm',
          centered: true,
          windowClass: 'modalLoading',
          backdrop: 'static',
          keyboard: false
        });
      }
    });
  }

  toggleMenu() {
    const menu = document.getElementById('menu-col');
    menu.classList.toggle('d-none');

    const toggleMenuButton = document.getElementById('menu-hamburger-1');
    toggleMenuButton.classList.toggle('fa-bars');
    toggleMenuButton.classList.toggle('fa-times');
  }

}
