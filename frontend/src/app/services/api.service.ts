import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl = 'http://localhost:5119/api/documents';

  constructor(private http: HttpClient) {}

  upload(file: File): Observable<any> {
    const formData = new FormData();

    formData.append('file', file);

    return this.http.post(
      `${this.baseUrl}/upload`,
      formData
    );
  }

  askQuestion(
    documentId: string,
    question: string
  ): Observable<any> {

    return this.http.post(
      `${this.baseUrl}/${documentId}/ask`,
      {
        question,
        topK: 5
      });
  }

  getStatus(documentId: string) {
    return this.http.get(
    `${this.baseUrl}/${documentId}/status`);
  }

  async askQuestionStream(
    documentId: string,
    question: string,
    onChunk: (chunk: string) => void
  ): Promise<void> {

    const response = await fetch(
      `${this.baseUrl}/${documentId}/ask-stream`,
      {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          question,
          topK: 1 // running locally, so set topK = 1
        })
      });

    if (!response.body) {
      return;
    }

    const reader = response.body.getReader();

    const decoder = new TextDecoder();

    while (true) {

      const { done, value } =
        await reader.read();

      if (done) {
        break;
      }

      const chunk =
        decoder.decode(value);

      onChunk(chunk);
    }
  }
}