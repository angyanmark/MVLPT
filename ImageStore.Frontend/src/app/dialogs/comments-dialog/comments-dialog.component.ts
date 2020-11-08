import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentDto, ImageClient } from 'src/app/api/app.generated';
import { CredentialsService } from 'src/app/core/auth/credentials.service';
import { ModalService } from 'src/app/core/modal/modal.service';
import { SnackbarService } from 'src/app/core/snackbar/snackbar.service';

@Component({
  selector: 'app-comments-dialog',
  templateUrl: './comments-dialog.component.html',
  styleUrls: ['./comments-dialog.component.scss']
})
export class CommentsDialogComponent implements OnInit {

  image_id: number;
  comments: CommentDto[] = [];
  comment: string = '';

  constructor(
    private modalService: ModalService,
    private credentialsService: CredentialsService,
    private imageClient: ImageClient,
    private snackbarService: SnackbarService,
    public dialogRef: MatDialogRef<CommentsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {
    this.image_id = data.id;
  }

  ngOnInit(): void {
    this.getComments();
  }

  getComments(): void {
    this.imageClient.getComments(this.image_id).subscribe(
      (r) => (this.comments = r),
      (error) => this.snackbarService.openError(error.detail)
    );
  }

  onDeleteComment(id: number): void {
    this.modalService
      .alert('Are you sure you want to delete the comment?', 'Delete comment', true, 'Delete', 'No')
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.imageClient.deleteComment(id).subscribe(
            () => {
              this.getComments();
              this.snackbarService.openSuccess('Delete successful');
            },
            (error) => this.snackbarService.openError(error.detail)
          );
        }
      });
  }

  postComment(comment: string): void {
    this.imageClient.postComment(this.image_id, comment).subscribe(
      (r) => {
        this.comments.push(r);
        this.snackbarService.openSuccess('Comment successful');
      },
      (error) => {
        this.snackbarService.openError(error.detail);
      }
    );
  }

  onComment(): void {
    if (this.comment.trim()) {
      this.postComment(this.comment);
      this.comment = '';
    }
  }
}
