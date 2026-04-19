# 📋 Task Manager — C# University Course Project

> **Student:** Yura Ivanov  
> **Course:** Programming in C# (.NET MAUI / OOP)  
> **Repository:** C# Lab Works 1–4

---

## 🧩 Project Overview

A full-featured **Task Manager** desktop application built with **.NET MAUI** across 4 iterative lab assignments. The app manages **Projects** and their **Tasks**, with a complete 3-layer architecture, MVVM pattern, real SQLite persistence, and async operations.

---

## 🗂 Lab Structure

| Lab | Topic | Key Deliverable |
|-----|-------|-----------------|
| **Lab 1** | Data Models & Console UI | DBModels, FakeStorage, Repository, Console App |
| **Lab 2** | MAUI UI & Navigation | 3-page MAUI app (Projects → Detail → Task), Shell navigation, DI |
| **Lab 3** | MVVM & 3-Layer Architecture | ViewModels, DTOModels, Service layer, Dependency Inversion |
| **Lab 4** | Final Project | SQLite, full CRUD, async, search/sort/filter, edit forms |

---

## 🏗 Architecture

```
TaskManager.DBModels       ← Storage models + Enums (ProjectType, TaskPriority)
TaskManager.Repository     ← SQLite DatabaseContext + IProjectRepository
TaskManager.DTOModels      ← Data Transfer Objects (list + detail + edit DTOs)
TaskManager.Services       ← IProjectService: converts DBModels → DTOs
Lab01Ivanov (MAUI App)     ← ViewModels + Pages (MVVM pattern)
```

**Dependency flow:**
```
MAUI App → IProjectService → IProjectRepository → SQLite
```
Each layer depends only on the layer below it via interfaces (Dependency Inversion Principle).

---

## ✨ Features

### Projects (Level 1 Entity)
- 📋 List all projects with name, type, description and progress bar
- 🔍 Real-time search / filter by name or description
- 🔃 Sort by: Name ↑↓, Type, Progress ↑↓
- ➕ Add new project
- ✏️ Edit project (name, description, type)
- 🗑 Delete project (swipe left) — cascades and deletes all child tasks

### Tasks (Level 2 Entity)
- 📝 List tasks inside each project with status icon, priority, due date
- 🔍 Search tasks by name
- 🔃 Sort by: Name, Priority, Due Date
- ➕ Add new task
- ✏️ Edit task (all fields: name, description, priority, due date, completed)
- 🗑 Delete task (swipe left)

### Technical Highlights
- ⚡ **Fully async** — all storage operations are `async/await`, UI never freezes
- 🔄 **Loading indicator** shown during every async operation
- 💾 **SQLite persistence** — data survives app restarts
- 🌱 **Auto-seeding** — 3 projects + 12 tasks inserted on first run only
- 🏗 **MVVM** — zero business logic in `.xaml.cs` files
- 💉 **IoC / DI** — all dependencies injected via `Microsoft.Extensions.DependencyInjection`

---

## 🛠 Tech Stack

- **.NET 10 / .NET MAUI** — cross-platform UI (runs on Windows)
- **SQLite** via `sqlite-net-pcl` — embedded database, no installation required
- **C# 13** — async/await, pattern matching, records, LINQ
- **MVVM** — manual `INotifyPropertyChanged` + `IQueryAttributable`
- **Shell Navigation** — single-window, page-based navigation

---

## 🚀 How to Run

1. Open `Lab01Ivanov.slnx` in **Visual Studio 2022/2026**
2. Set startup project to **`Lab01Ivanov`**
3. Select **Windows Machine** as the target
4. Press **F5**

> The SQLite database is created automatically at first launch in the app data directory.  
> No additional software or database setup required.

---

## 📁 Project Structure

```
Lab01Ivanov/
├── Lab01Ivanov/                    # MAUI App (UI Layer)
│   ├── Pages/                      # 5 pages (Projects, Detail, Edit, Task...)
│   ├── ViewModels/                 # 5 ViewModels (MVVM)
│   └── Converters/                 # BoolToYesNo, InverseBool
├── TaskManager.DBModels/           # Storage entities + Enums
├── TaskManager.Repository/         # SQLite + IProjectRepository
├── TaskManager.DTOModels/          # DTOs (list, detail, create, update)
└── TaskManager.Services/           # IProjectService + business logic
```
