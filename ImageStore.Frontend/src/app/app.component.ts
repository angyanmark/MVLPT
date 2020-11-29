import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from './core/auth/authentication.service';
import { CredentialsService } from './core/auth/credentials.service';
import { Roles } from './core/models/constants';
import { ModalService } from './core/modal/modal.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  title = 'ImageStore';

  readonly roles = Roles;

  constructor(
    private authenticationService: AuthenticationService,
    private modalService: ModalService,
    public credentialsService: CredentialsService,
    private router: Router,
  ) {}

  logout(): void {
    this.modalService
      .alert('Are you sure you want to log out?', 'Logout', true, 'Yes', 'No')
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.authenticationService.logout();
          this.router.navigate(['login']);
        }
      });
  }
}
