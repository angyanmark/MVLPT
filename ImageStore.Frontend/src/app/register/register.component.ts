import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../core/auth/authentication.service';
import { finalize } from 'rxjs/operators';
import { RegisterDto } from 'src/app/api/app.generated';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  form: FormGroup;
  loginInvalid: boolean;
  formSubmitAttempt: boolean;

  constructor(
    private fb: FormBuilder,
    private authenticationService: AuthenticationService
  ) {}

  ngOnInit(): void {
    this.loginInvalid = false;

    this.form = this.fb.group({
      username: [
        '',
        [Validators.required, Validators.pattern('^[a-zA-Z0-9]*$')],
      ],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  onSubmit(): void {
    this.loginInvalid = false;
    this.formSubmitAttempt = false;

    console.log(this.form);

    if (this.form.valid) {
      try {
        this.authenticationService
          .register({
            userName: this.form.get('username').value,
            email: this.form.get('email').value,
            password: this.form.get('password').value,
          } as RegisterDto)
          .pipe(
            finalize(() => {
              this.form.markAsPristine();
            })
          )
          .subscribe();
      } catch (err) {
        this.loginInvalid = true;
      }
    } else {
      this.formSubmitAttempt = true;
    }
  }
}
