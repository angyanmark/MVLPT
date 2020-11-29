import { Injectable } from '@angular/core';
import { Roles } from '../models/constants';

export interface Credentials {
  username: string;
  roles: string[];
  email: string;
  token: string;
  id: string;
}

const credentialsKey = 'credentials';

/**
 * Provides storage for authentication credentials.
 */
@Injectable()
export class CredentialsService {
  // tslint:disable-next-line:variable-name
  private _credentials: Credentials | null = null;

  constructor() {
    const savedCredentials =
      sessionStorage.getItem(credentialsKey) || localStorage.getItem(credentialsKey);
    if (savedCredentials) {
      this._credentials = JSON.parse(savedCredentials);
    }
  }

  /**
   * Checks is the user is authenticated.
   * @return True if the user is authenticated.
   */
  isAuthenticated(): boolean {
    return !!this.credentials;
  }

  /**
   * Checks if the user is an administator
   * @return True if the user is an administrator
   */
  isAdmin(): boolean {
    return this.isAuthenticated() && this.credentials.roles?.includes(Roles.Admin);
  }

  isInRole(role: string): boolean {
    return this.isAuthenticated() && this.credentials.roles?.includes(role);
  }

  /**
   * Gets the user credentials.
   * @return The user credentials or null if the user is not authenticated.
   */
  get credentials(): Credentials | null {
    return this._credentials;
  }

  /**
   * Sets the user credentials.
   * The credentials may be persisted across sessions by setting the `remember` parameter to true.
   * Otherwise, the credentials are only persisted for the current session.
   * @param credentials The user credentials.
   * @param remember True to remember credentials across sessions.
   */
  // tslint:disable-next-line:typedef
  setCredentials(credentials?: Credentials, remember: boolean = null) {
    this._credentials = credentials || null;

    if (credentials) {
      const storage = remember ? localStorage : sessionStorage;
      storage.setItem(credentialsKey, JSON.stringify(credentials));
    } else {
      sessionStorage.removeItem(credentialsKey);
      localStorage.removeItem(credentialsKey);
    }
  }
}
