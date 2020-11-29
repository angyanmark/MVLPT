import { Injectable } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AlertModalComponent } from './alert-modal/alert-modal.component';

@Injectable({
  providedIn: 'root',
})
export class ModalService {
  constructor(private dialog: MatDialog) {}

  alert = (
    message: string,
    title: string = '',
    cancellable: boolean = false,
    confirmText: string = 'Ok',
    cancelText: string = 'Cancel',
  ) => {
    const dialogRef = this.dialog.open(AlertModalComponent, {
      width: '500px',
      disableClose: true,
    });
    dialogRef.componentInstance.message = message;
    dialogRef.componentInstance.title = title;
    dialogRef.componentInstance.confirmText = confirmText;
    dialogRef.componentInstance.cancelText = cancelText;
    dialogRef.componentInstance.cancellable = cancellable;
    return dialogRef;
  };
}
