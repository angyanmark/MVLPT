import { Component, OnInit } from '@angular/core';
import { ImageClient, ImageDto } from '../api/app.generated';
import { SnackbarService } from '../core/snackbar/snackbar.service';

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
    private imageClient: ImageClient,
    private snackbarService: SnackbarService
  ) { }

  ngOnInit(): void {
    this.imageClient.getImages().subscribe(
      (r) => { this.images = r; this.filteredImages = this.images},
      (error) => this.snackbarService.openError(error.detail)
    );
  }

  onSearch(value: String) {
    this.searchValue = value;
    this.filteredImages = this.images.filter(i => i.fileName.toLowerCase().includes(this.searchValue.toLowerCase()));
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
