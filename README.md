# ClauseAI

AI-powered Insurance Policy Q&A Assistant built using Angular, ASP.NET Core (.NET 10), Semantic Kernel, Ollama, and PostgreSQL + pgvector.

ClauseAI allows users to upload insurance policy PDFs and ask natural language questions about coverage, exclusions, waiting periods, co-pay clauses, claim limits, and more.

The system uses Retrieval-Augmented Generation (RAG) to provide grounded answers with citations directly from uploaded policy documents.



# ClauseAI — Updated Architecture Diagram

```text
╔══════════════════════════════════════════════════════════════════════╗
║                            CLAUSEAI                                ║
║        AI-Powered Insurance Policy Q&A Assistant                   ║
╚══════════════════════════════════════════════════════════════════════╝


                         ┌────────────────────────┐
                         │    Angular Frontend   │
                         │────────────────────────│
                         │ • PDF Upload UI       │
                         │ • Chat Interface      │
                         │ • Citations Viewer    │
                         │ • Streaming Responses │
                         │ • Auth Screens        │
                         └──────────┬────────────┘
                                    │
                             HTTPS / REST
                                    │
                                    ▼

              ┌───────────────────────────────────────┐
              │ ASP.NET Core API (.NET 10)            │
              │───────────────────────────────────────│
              │ • JWT Authentication                  │
              │ • Upload APIs                         │
              │ • Chat APIs                           │
              │ • Semantic Kernel                     │
              │ • RAG Orchestration                   │
              │ • Background Jobs                     │
              │ • Prompt Management                   │
              │ • Streaming Responses                 │
              │ • Logging & Monitoring                │
              └───────────────┬───────────────────────┘
                              │
         ┌────────────────────┼────────────────────┐
         │                    │                    │
         ▼                    ▼                    ▼

┌────────────────┐  ┌────────────────────┐  ┌────────────────────┐
│ Local Storage  │  │ PostgreSQL +       │  │      Ollama        │
│────────────────│  │ pgvector           │  │────────────────────│
│ • Store PDFs   │  │────────────────────│  │ • gemma3:4b        │
│ • OCR Outputs  │  │ • Document Chunks │  │ • phi3:mini        │
│ • Metadata     │  │ • Embeddings      │  │ • nomic-embed-text │
│                │  │ • Citations       │  │ • Local Inference  │
└────────────────┘  │ • Chat History    │  └────────────────────┘
                    └────────────────────┘


═══════════════════════════════════════════════════════════════════════
                    DOCUMENT INGESTION FLOW
═══════════════════════════════════════════════════════════════════════

   User Uploads Insurance Policy PDF
                    │
                    ▼
        Angular Upload Component
                    │
                    ▼
      ASP.NET Core Upload Endpoint
                    │
                    ▼
      Store PDF in Local Storage
                    │
                    ▼
        Background Processing Job
                    │
                    ▼
         Extract Text using PdfPig
                    │
                    ▼
       OCR Fallback (Optional Later)
                    │
                    ▼
         Chunk Text into Sections
                    │
                    ▼
      Generate Embeddings (Ollama)
                    │
                    ▼
   Store Chunks + Vectors in pgvector


═══════════════════════════════════════════════════════════════════════
                     QUESTION ANSWERING FLOW
═══════════════════════════════════════════════════════════════════════

          User Asks Question
                    │
                    ▼
         Angular Chat Component
                    │
                    ▼
         ASP.NET Core Chat API
                    │
                    ▼
    Generate Question Embedding
                    │
                    ▼
     Vector Similarity Search
                    │
                    ▼
      Retrieve Relevant Chunks
                    │
                    ▼
   Semantic Kernel Prompt Pipeline
                    │
                    ▼
       Ollama Response Generation
                    │
                    ▼
    Grounded Answer + Citations
                    │
                    ▼
      Streaming Response to UI


═══════════════════════════════════════════════════════════════════════
                         SECURITY LAYER
═══════════════════════════════════════════════════════════════════════

• JWT Authentication
• HTTPS Only
• User-Level Data Isolation
• PDF File Validation
• Prompt Grounding
• Citation Enforcement
• Rate Limiting


═══════════════════════════════════════════════════════════════════════
                         FUTURE EXTENSIONS
═══════════════════════════════════════════════════════════════════════

• OCR Support
• Multi-Document Chat
• Policy Comparison
• Claim Assistance
• WhatsApp Integration
• Voice AI
• Self-Correcting RAG
• Agentic Workflows
```
