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






# Project Structure

```text
ClauseAI/
│
├── backend/
│   ├── ClauseAI.Api/
│   ├── ClauseAI.Application/
│   ├── ClauseAI.Domain/
│   └── ClauseAI.Infrastructure/
│
├── frontend-angular/
│
├── storage/
│
├── docker/
│
└── README.md
```

---

# Prerequisites

Before running ClauseAI locally, install the following:

| Software       | Version         |
| -------------- | --------------- |
| Node.js        | 20+             |
| Angular CLI    | Latest          |
| .NET SDK       | .NET 10 Preview |
| PostgreSQL     | 16+             |
| Docker Desktop | Optional        |
| Ollama         | Latest          |
| Git            | Latest          |

---

# Required Ollama Models

Install required models:

```bash
ollama pull phi3:mini
ollama pull nomic-embed-text
```

Verify:

```bash
ollama list
```

Expected:

```text
phi3:mini
nomic-embed-text
```

---

# Install Tesseract OCR

Required for scanned/image-based PDF support.

Windows installer:

https://github.com/UB-Mannheim/tesseract/wiki

Verify installation:

```bash
tesseract --version
```

---

# Local Setup Guide

## Step 1 — Clone Repository

```bash
git clone https://github.com/prathameshbhirud/ClauseAI.git

cd ClauseAI
```

---

## Step 2 — Start PostgreSQL

Ensure PostgreSQL is running locally.

Create database:

```sql
CREATE DATABASE clauseai;
```

Enable pgvector:

```sql
CREATE EXTENSION vector;
```

---

## Step 3 — Configure Backend

Navigate to backend:

```bash
cd backend
```

Update:

```text
ClauseAI.Api/appsettings.json
```

Example:

```json
"ConnectionStrings": {
  "DefaultConnection":
    "Host=localhost;Port=5432;Database=clauseai;Username=postgres;Password=yourpassword"
}
```

---

## Step 4 — Install Backend Dependencies

```bash
dotnet restore
```

---

## Step 5 — Run EF Core Migrations

From:

```text
ClauseAI/backend
```

Run:

```bash
dotnet ef database update \
--project ClauseAI.Infrastructure \
--startup-project ClauseAI.Api
```

---

## Step 6 — Copy Tesseract Language Data

Copy:

```text
eng.traineddata
```

into:

```text
backend/ClauseAI.Api/tessdata
```

Final structure:

```text
ClauseAI.Api/
 └── tessdata/
      └── eng.traineddata
```

---

## Step 7 — Start Ollama

Run:

```bash
ollama serve
```

Verify:

```bash
ollama list
```

---

## Step 8 — Start Backend API

From:

```text
ClauseAI/backend
```

Run:

```bash
dotnet run --project ClauseAI.Api
```

Swagger:

```text
http://localhost:5184/swagger
```

---

## Step 9 — Start Angular Frontend

Open NEW terminal.

Navigate:

```bash
cd frontend-angular
```

Install dependencies:

```bash
npm install
```

Start Angular:

```bash
ng serve
```

Frontend URL:

```text
http://localhost:4200
```

---

# Application Startup Order

Start services in this sequence:

1. PostgreSQL
2. Ollama
3. Backend API
4. Angular Frontend

---

# First End-to-End Test

## Upload Sample Insurance PDF

1. Open frontend:

   ```text
   http://localhost:4200
   ```

2. Upload insurance policy PDF

3. Wait for document processing

4. Ask questions such as:

```text
What is the waiting period for cataract surgery?
```

```text
What are maternity exclusions?
```

```text
What is the room rent limit?
```

---
