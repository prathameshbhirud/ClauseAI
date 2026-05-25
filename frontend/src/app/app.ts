import { Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChangeDetectorRef } from '@angular/core';
import { interval, Subject } from 'rxjs';
import { switchMap, takeUntil, filter, take } from 'rxjs/operators';

import { UploadComponent } from './components/upload/upload.component';
import { ChatComponent } from './components/chat/chat.component';
import { ApiService } from './services/api.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    UploadComponent,
    ChatComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class AppComponent implements OnDestroy {

  private destroy$ = new Subject<void>();

  uploadedDocument?: any;
  processing = false;

  constructor(
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
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

        this.cdr.detectChanges();
      });
  }

  ngOnDestroy(): void {

    this.destroy$.next();

    this.destroy$.complete();
  }
}