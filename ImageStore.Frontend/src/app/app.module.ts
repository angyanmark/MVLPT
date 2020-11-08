import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatInputModule } from '@angular/material/input';
import { MatDialogModule } from '@angular/material/dialog';
import { MatMenuModule } from '@angular/material/menu';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { API_BASE_URL } from './api/app.generated';
import { APP_ROUTES } from './app.routes';
import { ModalService } from './core/modal/modal.service';
import { CredentialsService } from './core/auth/credentials.service';
import { AuthenticationService } from './core/auth/authentication.service';
import { SnackbarService } from './core/snackbar/snackbar.service';
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component';
import { HomeComponent } from './home/home.component';
import { AlertModalComponent } from './core/modal/alert-modal/alert-modal.component';
import { CommentsDialogComponent } from './dialogs/comments-dialog/comments-dialog.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    HomeComponent,
    AlertModalComponent,
    CommentsDialogComponent,
  ],
  imports: [
    RouterModule.forRoot(APP_ROUTES),
    BrowserModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    MatToolbarModule,
    MatIconModule,
    MatCardModule,
    HttpClientModule,
    MatFormFieldModule,
    MatTooltipModule,
    MatCardModule,
    FormsModule,
    ReactiveFormsModule,
    MatListModule,
    MatInputModule,
    MatButtonModule,
    MatDialogModule,
    MatMenuModule,
    MatSnackBarModule,
  ],
  providers: [
    CredentialsService,
    AuthenticationService,
    ModalService,
    {
      provide: API_BASE_URL, // TODO templatet átírni vagy az openapi leírót
      useValue: ' ', // openapi leíróban van szerver felsorolva, és a generált kliens e nélkül rossz helyre hívna
    },
    SnackbarService,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
