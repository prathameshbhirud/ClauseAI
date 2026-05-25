export interface Citation {
  pageNumber: number;
  content: string;
}

export interface AskResponse {
  answer: string;
  citations: Citation[];
}