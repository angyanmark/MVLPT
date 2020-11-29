import { Component, OnInit, ViewChild } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { finalize } from 'rxjs/operators';
import { RegisterDto } from 'src/app/api/app.generated';
import { AuthenticationService } from '../core/auth/authentication.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  form: FormGroup;

  hidePassword: boolean;

  constructor(private fb: FormBuilder, private authenticationService: AuthenticationService) {}

  ngOnInit(): void {
    this.hidePassword = true;

    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.pattern('^[a-zA-Z0-9]*$')]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
  }

  onSubmit(): void {
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
            }),
          )
          .subscribe();
      } catch (err) {}
    }
  }
}
