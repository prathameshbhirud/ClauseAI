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

  async ask() {

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

    await this.apiService.askQuestionStream(
      this.documentId,
      userQuestion,
      (chunk) => {

        this.scrollToBottom();

        assistantMessage.content += chunk;

        this.cdr.detectChanges();
      });

    this.loading = false;

    this.cdr.detectChanges();
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