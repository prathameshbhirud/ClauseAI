import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';

import { ApiService } from '../../services/api.service';
import { AskResponse } from '../../models/ask-response.model';

@Component({
  selector: 'app-chat',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './chat.component.html',
  styleUrl: './chat.component.scss'
})
export class ChatComponent {

  @Input()
  documentId = '';

  question = '';

  response?: AskResponse;

  loading = false;

  constructor(
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
) {}

  ask() {

    if (!this.question || !this.documentId) {
      return;
    }

    this.loading = true;

    this.response = undefined;

    this.apiService.askQuestion(this.documentId, this.question)
      .subscribe({

        next: (result) => {

          console.log(result);

          this.response = result;

          this.loading = false;

          this.cdr.detectChanges();
        },

        error: (error) => {

          console.error(error);

          this.loading = false;

          this.cdr.detectChanges();
        }
      });
  }
}