# Deal Management System

## Overview

The **Deal Management System** is a full-stack application that helps manage deals and other related data. It consists of:

- **Frontend**: Built with **React**.
- **Backend**: Built with **.NET 8.0** using ASP.NET Core and Entity Framework.
- **Database**: Uses **SQL Server** for storing the data.

## Features

- User authentication and role-based authorization (Admin, User, SuperAdmin).
- Deal management (Create, Update, Delete, View).
- Hotel and itinerary management.
- File media management.

---

## Prerequisites

Before running the application, ensure you have the following installed:

### System Requirements

- **Windows**, **Mac**, or **Linux** operating system.
- **Docker** (if you wish to run the application in Docker containers).

### Required Tools

- **.NET 8.0 SDK**: For building and running the backend.
  - [Download .NET SDK 8.0](https://dotnet.microsoft.com/download)
  
- **Node.js**: For running the frontend (React).
  - [Download Node.js](https://nodejs.org/)
  
- **SQL Server**: You can use a local SQL Server instance or any remote server.
  - [Download SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

- **Visual Studio Code** or **Visual Studio**: For editing the code.
  - [Download Visual Studio Code](https://code.visualstudio.com/)
  - [Download Visual Studio](https://visualstudio.microsoft.com/)

---

## Running the Application Locally (Without Docker)

### 1. Clone the Repository

Clone the repository to your local machine:

```bash
git clone <repository-url>
cd DealManagementSystem-FullStack
