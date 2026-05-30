import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ChangeDetectorRef } from '@angular/core';
import { ElementRef, ViewChild } from '@angular/core';

import { ApiService } from '../../services/api.service';
import { AskResponse } from '../../models/ask-response.model';
import { ChatMessage } from '../../models/chat-message.model';

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

  @ViewChild('messagesContainer') messagesContainer?: ElementRef;

  question = '';

  response?: AskResponse;

  loading = false;

  messages: ChatMessage[] = [];

  constructor(
    private apiService: ApiService,
    private cdr: ChangeDetectorRef
) {}

  ask() {

    if (!this.question || !this.documentId) {
      return;
    }

    const userQuestion = this.question;

    this.messages.push({
      role: 'user',
      content: userQuestion
    });

    this.question = '';

    const assistantMessage: ChatMessage = {
      role: 'assistant',
      content: ''
    };

    this.messages.push(assistantMessage);

    this.loading = true;

    this.apiService.askQuestion(
      this.documentId,
      userQuestion)
      .subscribe({

        next: (result) => {

          console.log(result);

          assistantMessage.content =
            result.answer;

          assistantMessage.citations =
            result.citations;

          this.messages = [...this.messages];

          console.log(this.messages);

          this.loading = false;

          this.scrollToBottom();

          this.cdr.detectChanges();
        },

        error: (error) => {

          console.error(error);

          assistantMessage.content =
            'Error generating answer.';

          this.loading = false;

          this.cdr.detectChanges();
        }
      });
  }

  scrollToBottom() {

    setTimeout(() => {

      if (this.messagesContainer) {

        this.messagesContainer.nativeElement
          .scrollTop =
            this.messagesContainer.nativeElement
              .scrollHeight;
      }

    });
  }
}