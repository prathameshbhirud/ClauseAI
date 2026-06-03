import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class PdfStateService {

  selectedPage = new BehaviorSubject<number>(1);

  pdfUrl = new BehaviorSubject<string>('');
}