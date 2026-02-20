# TodoCLI

A simple **command-line Todo application** built with **.NET 9**. This project demonstrates **layered architecture**, **exception handling**, **atomic file operations**, **logging**, and **CLI-based UX**.  

It’s designed to showcase **clean architecture principles**, **separation of concerns**, and **production-ready patterns** suitable for beginner-to-junior backend developer portfolios.

## Inspiration

This project was inspired by the [Task Tracker roadmap on Roadmap.sh](https://roadmap.sh/projects/task-tracker)

---

## Table of Contents

- [Features](#features)  
- [Architecture](#architecture)  
- [Project Structure](#project-structure)  
- [Commands](#commands)  
- [Error Handling & Logging](#error-handling--logging)  
- [Technical Highlights](#technical-highlights)   

---

## Features

- Add, update, delete, and retrieve todos.  
- Filter todos by status (`todo`, `inprogress`, `paused`, `done`).  
- Mark todos with specific statuses.  
- Atomic file operations to prevent data corruption.  
- Layered design: **UI → Application → Infrastructure**.  
- Exception handling and user-friendly responses.  
- Logger system for technical debugging.  
- Data persisted in a JSON file (`data.json`).  

---

## Architecture

The project follows a **clean, layered architecture**:

1. **UI Layer (`UI`)**  
   - Handles CLI input and output.  
   - Validates commands and arguments.  
   - Calls `Processor` methods.  
   - Displays user-friendly messages.

2. **Application Layer (`Processor`)**  
   - Orchestrates application logic.  
   - Handles exceptions from the infrastructure layer.  
   - Converts exceptions to user-readable responses (`ReadResponse`, `WriteResponse`).

3. **Infrastructure Layer (`DataAccess`)**  
   - Reads/writes JSON data.  
   - Performs atomic file operations with temporary files.  
   - Logs exceptions with full technical context.  

4. **Logger (`Logger`)**  
   - Writes exceptions to a log file (`logs.txt`).  
   - Captures stack traces, timestamps, and message context.  

5. **Models (`Models`)**  
   - `Todo` class with properties: `Id`, `Description`, `Status`, `CreatedAt`, `UpdatedAt`.  
   - `Status` enum: `todo`, `inprogress`, `paused`, `done`.  

---

## Project Structure
```
.
├── data.json               # Persistent data store
├── TodoCLI                 # Main project
│   ├── Application
│   │   ├── Processor.cs
│   │   └── Responses
│   │       ├── ReadResponse.cs
│   │       └── WriteResponse.cs
│   ├── Infrastructure
│   │   ├── Contracts/IDataAccess.cs
│   │   └── DataAccess.cs
│   ├── Logger/Logger.cs
│   ├── Models/Todo.cs
│   └── UI/UI.cs
└── Todo.sln
```

---

## Commands

```bash
# Add a new todo
todo create "Buy groceries"

# Update todo description
todo update 1 "Buy groceries and cook dinner"

# Delete todo
todo delete 1

# Mark todo status
todo mark-inprogress 1
todo mark-done 1
todo mark-pause 1
todo mark-todo 1

# List all todos
todo list

# List todos by status
todo list todo
todo list inprogress
todo list done

# Get a specific todo
todo get 1
```
## Error Handling & Logging

- All file-related exceptions (`FileNotFoundException`, `JsonException`) are **logged in `logs.txt`**.  
- UI layer **never sees raw exceptions** — only user-friendly messages.  
- Processor **maps technical exceptions** to readable responses.  

**Example Mapping:**

| Exception               | User Message                  | Logged Info             |
|-------------------------|-------------------------------|------------------------|
| FileNotFoundException   | "data file not found."        | Full stack trace       |
| JsonException           | "data file is corrupted."     | Full stack trace       |
| Any other Exception     | "Operation failed."           | Full stack trace       |

- Logger includes **timestamp**, **exception type**, and **message** for debugging purposes.

## Technical Highlights

- **Atomic file writes** using `.tmp` file + `File.Move()` to prevent data loss.
- `FirstOrDefault` returns **reference**, allowing direct updates to objects inside a collection.
- **Immutable vs mutable design**: `Todo.Id` and `CreatedAt` are `init`-only to prevent accidental mutation.
- **Layered separation** allows unit testing and future expansion (e.g., database integration).
- **CLI-first UX** with argument validation and helpful feedback.