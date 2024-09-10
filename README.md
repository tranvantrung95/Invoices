

![Invoicika](https://i.imgur.com/8AF7yiL.png)

# **Invoicika**

Invoicika is an advanced invoice management system built with Angular 16, ASP.NET Core Web API 6.0, and SQL Server. This system provides full-fledged invoicing capabilities, including unlimited invoice creation, customer management, PDF generation, and more. The frontend is built with Angular and utilizes NG-Zorro UI components for a modern, responsive design.

## Features

- **Create Unlimited Invoices**: Generate invoices without any restrictions.
- **Email Invoice**: Send invoices directly to customers via email.
- **PDF Generation**: Export invoices as PDFs for easy sharing and storage.
- **Customer Management**: Add, edit, and manage customers with multiple shipping addresses.
- **Authentication & Roles**: Secure login and role-based access control.
- **Customer Product Management**: Add products linked to customers for quick access.
- **Sign Up & Profile Management**: Users can sign up and manage their profile.
- **Image Upload**: Upload and manage profile pictures and invoice logos.
- **VAT Management**: Handle VAT for customer invoices.
- **Database**: Built to work with SQL Server.

## Technologies Used

- **Frontend**: Angular with NG-Zorro (Responsive UI components).
- **Backend**: ASP.NET Core Web API 6.0 (Robust and scalable API layer).
- **Database**: SQL Server (Code First Migration).

## Getting Started

### Prerequisites

- **Node.js** (For Angular)
- **SQL Server** (For database)
- **.NET 6.0 SDK** (For ASP.NET Core)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/codebangla/invoicika.git
   cd invoicika
   ```

2. **Frontend (Angular)**:
   ```bash
   cd frontend
   npm install
   ng serve
   ```

3. **Backend (ASP.NET Core)**:
   ```bash
   cd backend
   dotnet restore
   ```

4. **Database Setup (SQL Server)**:

   - Ensure SQL Server is installed and running.
   - Update the connection strings in `appsettings.json` to configure your SQL Server database.
   
     Example:
     ```json
     "ConnectionStrings": {
       "SqlServerConnection": "Your SQL Server connection string"
     }
     ```

5. **Apply Migrations**:
   ```bash
   dotnet ef database update --context ApplicationDbContext -- --provider SqlServer
   ```

### Running the Application

1. Run the **Angular frontend**:
   ```bash
   ng serve
   ```

2. Run the **ASP.NET Core backend**:
   ```bash
   dotnet run
   ```

3. Open your browser and navigate to `http://localhost:4200` for the frontend.

## Usage

- **Create Invoice**: Navigate to the "Invoices" section to generate new invoices.
- **Manage Customers**: Add, edit, and manage customer details, including multiple shipping addresses.
- **Email Invoices**: Email invoices directly to customers.
- **Generate PDFs**: Export invoices as PDF documents.
- **User Authentication**: Users can sign up, log in, and manage their profiles with role-based access control.
- **Image Upload**: Upload profile pictures and invoice logos from the user profile settings or the invoice creation page.

## Contributing

We welcome contributions! Please submit a pull request or report issues. Ensure your code follows the project guidelines and passes all tests before submission.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

Let me know if you'd like any further modifications!