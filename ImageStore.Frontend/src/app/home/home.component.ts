import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ImageClient, ImageDto } from '../api/app.generated';
import { CredentialsService } from '../core/auth/credentials.service';
import { ModalService } from '../core/modal/modal.service';
import { SnackbarService } from '../core/snackbar/snackbar.service';
import { CommentsDialogComponent } from '../dialogs/comments-dialog/comments-dialog.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
})
export class HomeComponent implements OnInit {
  searchValue: String = '';
  images: ImageDto[] = [];
  filteredImages: ImageDto[] = [];

  constructor(
    private modalService: ModalService,
    private credentialsService: CredentialsService,
    private imageClient: ImageClient,
    private snackbarService: SnackbarService,
    public dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.getImages();
  }

  getImages(): void {
    this.imageClient.getImages().subscribe(
      (r) => { this.images = r; this.filteredImages = this.images },
      (error) => this.snackbarService.openError(error.detail)
    );
  }

  onSearch(value: String): void {
    this.searchValue = value;
    this.filteredImages = this.images.filter(i => i.fileName.toLowerCase().includes(this.searchValue.toLowerCase()));
  }

  openCommentsDialog(id: number): void {
    this.dialog.open(CommentsDialogComponent, {
      width: '720px',
      data: { id: id }
    });
  }

  onDeleteImage(id: number): void {
    this.modalService
      .alert('Are you sure you want to delete the image?', 'Delete image', true, 'Delete', 'No')
      .afterClosed()
      .subscribe((r) => {
        if (r) {
          this.imageClient.deleteImage(id).subscribe(
            () => {
              this.getImages();
              this.snackbarService.openSuccess('Delete successful');
            },
            (error) => this.snackbarService.openError(error.detail)
          );
        }
      });
  }

  uploadFile(files): void {
    if (files.length !== 0) {
      let file = files[0] as File;
      let fileParameter = {
        fileName: file.name,
        data: file,
      };

      this.imageClient.uploadImage(fileParameter).subscribe(
        (r) => {
          this.images.push(r);
          this.snackbarService.openSuccess('Upload successful');
        },
        (error) => {
          this.snackbarService.openError(error.detail);
        }
      );
    }
  }
}
