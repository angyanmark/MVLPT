import { AuthenticationService } from '../core/auth/authentication.service';
import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { Router } from '@angular/router';
import { SnackbarService } from 'src/app/core/snackbar/snackbar.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  loginInvalid: boolean;
  hidePassword: boolean;

  constructor(
    private fb: FormBuilder,
    private authenticationService: AuthenticationService,
    private router: Router,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    this.loginInvalid = false;
    this.hidePassword = true;

    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  onSubmit(): void {
    this.loginInvalid = false;
    if (this.form.valid) {
      try {
        this.authenticationService
          .login(this.form.value)
          .pipe(
            finalize(() => {
              this.form.markAsPristine();
            })
          )
          .subscribe(
            () => {
              this.router.navigate(['']);
              this.snackbarService.openSuccess('Login successful');
            },
            (error) => {
              this.snackbarService.openError(error.detail);
            }
          );
      } catch (err) {
        this.loginInvalid = true;
      }
    }
  }
}
