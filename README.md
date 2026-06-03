# ClauseAI

ClauseAI is a production-style AI-powered Insurance Policy Intelligence Platform built using Angular, ASP.NET Core (.NET 10), Ollama, PostgreSQL + pgvector, OCR, and Retrieval-Augmented Generation (RAG).

The platform allows users to upload insurance policy PDFs and ask natural language questions about:

* Waiting periods
* Coverage conditions
* Exclusions
* Co-pay clauses
* Room rent limits
* Maternity benefits
* Claim restrictions
* Policy terms

ClauseAI combines semantic retrieval, hybrid search, OCR fallback, and grounded LLM responses with citations directly from uploaded insurance documents.

The system supports:

* Modern conversational AI chat UX
* PDF preview with clickable citations
* OCR for scanned insurance documents
* pgvector semantic retrieval
* Hybrid vector + keyword search
* Conversation history persistence
* Background document ingestion
* Local AI inference using Ollama
* Production-ready architecture patterns


# Features

## AI & Retrieval

* Retrieval-Augmented Generation (RAG)
* pgvector semantic search
* Hybrid vector + keyword retrieval
* Grounded responses with citations
* Ollama local inference
* OCR fallback for scanned PDFs

## Document Intelligence

* Insurance PDF upload
* Text extraction using PdfPig
* OCR using Tesseract
* Intelligent chunking pipeline
* Citation-aware responses
* Clickable citation navigation
* Embedded PDF preview

## Frontend

* Modern Angular UI
* ChatGPT-style conversational UX
* Conversation history sidebar
* Responsive layout
* Real-time chat interface
* PDF preview panel

## Backend

* ASP.NET Core (.NET 10)
* Background ingestion jobs
* PostgreSQL + pgvector
* REST APIs
* Async processing pipeline
* Modular architecture

## Production Engineering

* Docker-ready architecture
* Environment-based configuration
* OpenAI/Ollama extensibility
* Clean Architecture principles
* Conversation persistence
* Scalable retrieval pipeline




# ClauseAI — Updated Architecture Diagram

# Updated Architecture

```text
╔══════════════════════════════════════════════════════════════════════╗
║                            CLAUSEAI                                ║
║         AI-Powered Insurance Policy Intelligence Platform          ║
╚══════════════════════════════════════════════════════════════════════╝


 ┌─────────────────────────────────────────────────────────────────┐
 │                     ANGULAR FRONTEND                           │
 │─────────────────────────────────────────────────────────────────│
 │ • Modern Conversational Chat UI                                │
 │ • Insurance PDF Upload                                         │
 │ • Conversation History Sidebar                                 │
 │ • Clickable Citations                                          │
 │ • Embedded PDF Preview                                         │
 │ • Real-Time Processing Status                                  │
 └───────────────────────────┬─────────────────────────────────────┘
                             │
                         HTTPS / REST
                             │
                             ▼

 ┌─────────────────────────────────────────────────────────────────┐
 │                 ASP.NET CORE API (.NET 10)                     │
 │─────────────────────────────────────────────────────────────────│
 │ • Upload APIs                                                  │
 │ • Chat APIs                                                    │
 │ • Streaming Endpoints                                          │
 │ • RAG Orchestration                                            │
 │ • Conversation Persistence                                     │
 │ • Hybrid Search                                                │
 │ • OCR Pipeline                                                 │
 │ • Background Processing                                        │
 │ • Citation Generation                                          │
 └───────────────┬───────────────────────┬─────────────────────────┘
                 │                       │
                 ▼                       ▼

 ┌────────────────────────┐    ┌──────────────────────────────┐
 │ PostgreSQL + pgvector  │    │           Ollama             │
 │────────────────────────│    │──────────────────────────────│
 │ • Documents            │    │ • phi3:mini                 │
 │ • Chunks               │    │ • nomic-embed-text          │
 │ • Embeddings           │    │ • Local LLM Inference       │
 │ • Conversation History │    │                             │
 │ • Citations            │    └──────────────────────────────┘
 │ • Metadata             │
 └──────────────┬─────────┘
                │
                ▼

 ┌──────────────────────────────────────────────────────────────┐
 │                    DOCUMENT PIPELINE                         │
 │──────────────────────────────────────────────────────────────│
 │ Upload PDF                                                   │
 │      ↓                                                       │
 │ Store File                                                   │
 │      ↓                                                       │
 │ Background Processing                                        │
 │      ↓                                                       │
 │ PdfPig Text Extraction                                       │
 │      ↓                                                       │
 │ OCR Fallback (Tesseract)                                    │
 │      ↓                                                       │
 │ Intelligent Chunking                                         │
 │      ↓                                                       │
 │ Embedding Generation                                         │
 │      ↓                                                       │
 │ pgvector Storage                                             │
 └──────────────────────────────────────────────────────────────┘


═══════════════════════════════════════════════════════════════════════
                         QUESTION ANSWERING FLOW
═══════════════════════════════════════════════════════════════════════

User Question
      ↓
Conversation Context
      ↓
Hybrid Retrieval
(Vector + Keyword Search)
      ↓
Relevant Chunks
      ↓
Prompt Construction
      ↓
Ollama LLM
      ↓
Grounded Answer
      ↓
Citations
      ↓
PDF Navigation
```


# Tech Stack

| Layer            | Technology              |
| ---------------- | ----------------------- |
| Frontend         | Angular                 |
| Backend          | ASP.NET Core (.NET 10)  |
| Database         | PostgreSQL              |
| Vector DB        | pgvector                |
| LLM Runtime      | Ollama                  |
| Chat Model       | phi3:mini               |
| Embedding Model  | nomic-embed-text        |
| OCR              | Tesseract               |
| PDF Extraction   | PdfPig                  |
| PDF Viewer       | ngx-extended-pdf-viewer |
| Background Jobs  | Hangfire                |
| ORM              | Entity Framework Core   |
| Containerization | Docker                  |


# Current Implementation Status

## Completed

* PDF upload pipeline
* OCR fallback support
* pgvector semantic retrieval
* Hybrid search
* Angular conversational UI
* Streaming AI responses
* Clickable citations
* Embedded PDF preview
* Conversation persistence
* Background ingestion
* Local Ollama integration

## Planned

* Multi-document chat
* Authentication
* User workspaces
* OpenAI fallback provider
* Cloud deployment
* Query rewriting
* Reranking models
* Multi-user collaboration


# How it works?

## Landing Page
![Landing Page](https://github.com/prathameshbhirud/ClauseAI/blob/main/images/Landing_Page.PNG)

## Uploading Policy Document
![Uploading Policy Document](https://github.com/prathameshbhirud/ClauseAI/blob/main/images/Uploading_Policy_Document.PNG)

## Chat Mode + PDF Preview Mode
![Chat Mode + PDF Preview Mode](https://github.com/prathameshbhirud/ClauseAI/blob/main/images/Chat_Mode_%2B_PDF_Preview_Mode.PNG)

## Result of Chat with CItations
![Result of Chat with CItations](https://github.com/prathameshbhirud/ClauseAI/blob/main/images/Results_of_Chat_with_Citation.PNG)

## Clickable Citation
![Clickable Citation](https://github.com/prathameshbhirud/ClauseAI/blob/main/images/Clickable_Citation.PNG)