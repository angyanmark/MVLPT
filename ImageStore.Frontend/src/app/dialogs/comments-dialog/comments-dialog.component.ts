import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CommentDto, ImageClient } from 'src/app/api/app.generated';
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
    private imageClient: ImageClient,
    private snackbarService: SnackbarService,
    public dialogRef: MatDialogRef<CommentsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data) {
    this.image_id = data.id;
  }

  ngOnInit(): void {
    this.getComments();
  }

  getComments() {
    this.imageClient.getComments(this.image_id).subscribe(
      (r) => (this.comments = r),
      (error) => this.snackbarService.openError(error.detail)
    );
  }

  postComment(comment: string) {
    this.imageClient.postComment(this.image_id, comment).subscribe(
      () => (this.getComments()),
      (error) => this.snackbarService.openError(error.detail)
    );
  }

  onComment() {
    this.postComment(this.comment);
    this.comment = '';
  }
}
