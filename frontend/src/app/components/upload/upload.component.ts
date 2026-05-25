import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload.component.html',
  styleUrl: './upload.component.scss'
})
export class UploadComponent {

  @Output()
  uploaded = new EventEmitter<any>();
  uploading = false;

  selectedFile?: File;

  constructor(private apiService: ApiService) {}

  onFileSelected(event: any) {
    this.selectedFile = event.target.files[0];
  }

  upload() {

    if (!this.selectedFile) {
      return;
    }

    this.uploading = true;

    this.apiService.upload(this.selectedFile)
      .subscribe(response => {

        this.uploading = false;
        
        this.uploaded.emit(response);

        alert('Upload successful');
      });
  }
}