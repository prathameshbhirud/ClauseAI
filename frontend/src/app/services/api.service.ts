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
}