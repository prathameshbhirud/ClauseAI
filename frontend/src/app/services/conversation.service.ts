import { Injectable } from '@angular/core';

import { HttpClient }
from '@angular/common/http';

import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ConversationService {

  constructor(private http: HttpClient)
  {}

  getConversations(documentId: string)
  {
    return this.http.get<any[]>(
      `${environment.apiUrl}/conversations/${documentId}`);
  }

  getConversationMessages(conversationId: string)
  {
    return this.http.get<any[]>(
      `${environment.apiUrl}/conversations/messages/${conversationId}`);
  }
}