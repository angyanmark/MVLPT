import { Component, OnInit, Input } from '@angular/core';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-alert-modal',
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.scss'],
})
export class AlertModalComponent {
  @Input() title: string;

  @Input() message: string;

  @Input() confirmText: string;

  @Input() cancelText: string;

  @Input() cancellable: boolean;

  constructor(private dialogRef: MatDialogRef<AlertModalComponent>) {}

  confirm = () => {
    this.dialogRef.close(true);
  };

  cancel = () => {
    this.dialogRef.close(false);
  };
}
