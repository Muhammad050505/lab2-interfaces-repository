```mermaid
erDiagram
    AUTHOR ||--o{ BOOK : "пишет"
    AUTHOR {
        int Id PK
        string Name
    }
    BOOK {
        int Id PK
        string Title
        int Year
        int AuthorId FK
    }
```
