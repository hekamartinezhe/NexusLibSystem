# 📚 NexusLibrarySystem

Library management system developed in WPF (.NET) with a clean design, L-shaped navigation, and ADO.NET-based backend logic.

---

## 📁 Project Structure

```
NexusLibrarySystem/
│
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── NexusLibrarySystem.csproj
├── packages.config
│
├── Styles/
│   └── Theme.xaml
│
├── Views/
│   ├── LoginPage.xaml
│   ├── DashboardPage.xaml
│   ├── BooksPage.xaml
│   ├── UsersPage.xaml
│   ├── ProfilePage.xaml
│   ├── BookDetailPage.xaml
│   └── LoansPage.xaml
│
├── Models/
│   ├── User.cs
│   ├── Book.cs
│   ├── Loan.cs
│   ├── Major.cs
│   └── Category.cs
│
├── Database.cs
│
├── bin/Debug/
└── obj/Debug/
```

---

## 🧠 System Logic Overview

### A) UI (`Views/`)

- Each user screen is a `Page.xaml` file.
- Pages are loaded dynamically inside a `Frame` within `MainWindow.xaml` to avoid pop-ups.

```csharp
MainFrame.Navigate(new Views.BooksPage());
```

---

### B) Models (`Models/`)

- Represent database tables.
- Plain classes used to carry data throughout the app.

---

### C) Data Access (`Database.cs`)

- Direct communication with SQL Server using ADO.NET.
- Static methods such as:

```csharp
public static List<Book> GetAllBooks();
public static void AddBook(Book newBook);
```

---

## 🎨 Styling (`Styles/Theme.xaml`)

- Reusable styles: colors, sizes, borders, etc.
- Automatically loaded from `App.xaml`.

---

## 🧭 L-Shaped Navigation

- Left vertical menu with icon + text buttons.
- Top bar with title, user info, and logout.
- Main content loads in central area.

---

## 🔐 User Roles

| Role    | Main Permissions         |
|---------|--------------------------|
| Admin   | CRUD, Dashboard, Users   |
| Student | View, Loans, Profile     |

UI adapts based on role:

```csharp
if (user.Role != "Admin")
    UsersButton.Visibility = Visibility.Collapsed;
```

---

## 📄 Included Pages

- `LoginPage`: User login.
- `DashboardPage`: Admin summary.
- `BooksPage`: Book list and management.
- `UsersPage`: Registered users view.
- `ProfilePage`: Edit personal profile.
- `BookDetailPage`: View book details.
- `LoansPage`: Loan history.

---

## 🌀 Git Best Practices

### Useful Commands

```bash
git status
git add .
git commit -m "Implemented main views and L-shaped navigation; added base styles and structure for backend logic"
git push origin main
```

### Recommendations

- Run `git pull origin main` before working.
- Use `git push` after making changes.
- Do not delete folders like `Styles`, even if they appear empty.

---

## 🛠 Technologies

- .NET WPF
- C#
- XAML
- ADO.NET