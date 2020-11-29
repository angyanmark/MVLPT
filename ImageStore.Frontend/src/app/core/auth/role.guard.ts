import { Injectable } from '@angular/core';
import {
  Router,
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  CanLoad,
  Route,
  CanActivateChild,
} from '@angular/router';
import { ModalService } from '../modal/modal.service';

import { CredentialsService } from './credentials.service';
import { allRoles } from '../models/constants';

@Injectable({
  providedIn: 'root', // ADDED providedIn root here.
})
export class RoleGuard implements CanActivate, CanActivateChild, CanLoad {
  constructor(
    private router: Router,
    private credentialsService: CredentialsService,
    private modalService: ModalService,
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivateItem(route, state);
  }

  canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    return this.canActivateItem(childRoute, state);
  }

  canLoad(route: Route): boolean {
    // Role
    if (!this.checkUserRoleForRoute(route)) {
      return false;
    }

    return true;
  }

  private canActivateItem(
    routeSnapshot: ActivatedRouteSnapshot,
    state: RouterStateSnapshot,
  ): boolean {
    // Role
    if (!this.checkUserRoleForRoute(routeSnapshot.routeConfig)) {
      this.modalService.alert('Access forbidden', 'Warning');
      this.router.navigate(['/'], { replaceUrl: true });
      return false;
    }

    return true;
  }

  private checkUserRoleForRoute(route: Route) {
    const allowedRoles =
      route.data && route.data.allowedRoles && route.data.allowedRoles.length
        ? route.data.allowedRoles
        : allRoles();

    let allowed = false;
    allowedRoles.forEach((role: string) => {
      if (this.credentialsService.isInRole(role)) {
        allowed = true;
      }
    });

    return allowed;
  }
}
