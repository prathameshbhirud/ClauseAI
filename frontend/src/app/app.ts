import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { interval, Subject } from 'rxjs';
import { switchMap, takeUntil, filter, take } from 'rxjs/operators';

import { UploadComponent } from './components/upload/upload.component';
import { ChatComponent } from './components/chat/chat.component';
import { ApiService } from './services/api.service';
import { PdfPreviewComponent } from './components/pdf-preview/pdf-preview.component';
import { PdfStateService } from './services/pdf-state.service';
import { ConversationService } from './services/conversation.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    UploadComponent,
    ChatComponent,
    PdfPreviewComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent implements OnDestroy {

  private destroy$ = new Subject<void>();

  uploadedDocument?: any;
  processing = false;

  conversations: any[] = [];

  constructor(
    private apiService: ApiService,
    private cdr: ChangeDetectorRef,
    private pdfState: PdfStateService,
    private conversationService: ConversationService
  ) {}

  onUploaded(document: any) {

    this.uploadedDocument = document;

    this.processing = true;

    interval(3000)
      .pipe(

        switchMap(() =>
          this.apiService.getStatus(document.id)
        ),

        filter((status: any) =>
          status.status === 'Ready'),

        take(1)

      )
      .subscribe(() => {

        this.processing = false;

        this.pdfState.pdfUrl.next(`http://localhost:5119/api/documents/${document.id}/file`);

        this.cdr.detectChanges();
      });
  }

  loadConversations() {
    if (!this.uploadedDocument) {
      return;
    }

    this.conversationService.getConversations(
        this.uploadedDocument.id)
      .subscribe(result => {

        this.conversations = result;
      });
  }

  ngOnDestroy(): void {

    this.destroy$.next();

    this.destroy$.complete();
  }
}