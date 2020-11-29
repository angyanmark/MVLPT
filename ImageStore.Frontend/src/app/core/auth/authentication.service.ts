import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';

import { map } from 'rxjs/operators';
import * as jwtDecode from 'jwt-decode';
import { SnackbarService } from 'src/app/core/snackbar/snackbar.service';
import { Router } from '@angular/router';
import { Token } from '../models/token';
import { AccountClient, LoginDto, RegisterDto } from '../../api/app.generated';
import { Credentials, CredentialsService } from './credentials.service';

export interface LoginContext {
  username: string;
  password: string;
  remember?: boolean;
}

@Injectable()
export class AuthenticationService {
  constructor(
    private credentialsService: CredentialsService,
    private accountClient: AccountClient,
    private snackbarService: SnackbarService,
    private router: Router,
  ) {}

  /**
   * Authenticates the user.
   * @param context The login parameters.
   * @return The user credentials.
   */
  login(context: LoginContext): Observable<Credentials> {
    return this.accountClient
      .login({
        username: context.username,
        password: context.password,
      } as LoginDto)
      .pipe(
        map((res) => {
          const tokenData = jwtDecode<Token>(res.token);
          const data = {
            username: context.username,
            token: res.token,
            roles: tokenData.role,
            email: tokenData.email,
            id: tokenData.nameid,
          } as Credentials;
          this.credentialsService.setCredentials(data, context.remember);
          return data;
        }),
      );
  }

  register(dto: RegisterDto): Observable<void> {
    this.accountClient.register(dto).subscribe(
      (r) => {
        this.router.navigateByUrl('/login');
        this.snackbarService.openSuccess('Registration successful');
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      },
    );
    return of();
  }

  /**
   * Logs out the user and clear credentials.
   */
  // tslint:disable-next-line:typedef
  logout() {
    this.credentialsService.setCredentials();
    this.snackbarService.openSuccess('Logout successful');
  }
}
