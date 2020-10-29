import { AuthenticationGuard } from './core/auth/authentication.guard';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { HomeComponent } from './home/home.component';

export const APP_ROUTES = [
  {
    path: '',
    canActivate: [AuthenticationGuard],
    component: HomeComponent,
  },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: '**', redirectTo: '/' },
];
