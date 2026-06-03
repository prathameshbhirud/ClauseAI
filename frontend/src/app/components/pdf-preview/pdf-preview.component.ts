import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgxExtendedPdfViewerModule } from 'ngx-extended-pdf-viewer';

import { PdfStateService } from '../../services/pdf-state.service';

@Component({
  selector: 'app-pdf-preview',
  standalone: true,
  imports: [
    CommonModule,
    NgxExtendedPdfViewerModule
  ],
  templateUrl: './pdf-preview.component.html',
  styleUrl: './pdf-preview.component.scss'
})
export class PdfPreviewComponent {

  page = 1;

  pdfUrl = '';

  constructor(private pdfState: PdfStateService)
  {
    this.pdfState.selectedPage
      .subscribe(page => {
        this.page = page;
      });

    this.pdfState.pdfUrl
      .subscribe(url => {
        this.pdfUrl = url;
      });
  }
}