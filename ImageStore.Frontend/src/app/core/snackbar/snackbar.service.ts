import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  constructor(private snackbar: MatSnackBar) {}

  openError(message: string) {
    if (message) {
      this.snackbar.open(message, 'close', {
        duration: 3000,
        panelClass: ['red-snackbar'],
      });
    } else {
      this.snackbar.open('Error', 'close', {
        duration: 3000,
        panelClass: ['red-snackbar'],
      });
    }
  }

  openSuccess(message: string) {
    if (message) {
      this.snackbar.open(message, 'close', {
        duration: 3000,
        panelClass: ['green-snackbar'],
      });
    } else {
      this.snackbar.open('Success', 'close', {
        duration: 3000,
        panelClass: ['green-snackbar'],
      });
    }
  }
}
