Here's the updated **README.md** file for your project "Invoicika," including the database switch option and image upload for profile and invoice logos:

---

# Invoicika

Invoicika is an advanced invoice management system built with Angular 16, ASP.NET Core Web API 6.0, and MySQL/SQL Server. This system provides full-fledged invoicing capabilities, including unlimited invoice creation, customer management, PDF generation, and more. The frontend is built with Angular and utilizes NG-Zorro UI components for a modern, responsive design.

## Features

- **Create Unlimited Invoices**: Generate invoices without any restrictions.
- **Email Invoice**: Send invoices directly to customers via email.
- **PDF Generation**: Export invoices as PDFs for easy sharing and storage.
- **Customer Management**: Add, edit, and manage customers with multiple shipping addresses.
- **Authentication & Roles**: Secure login and role-based access control.
- **Customer Product Management**: Add products linked to customers for quick access.
- **Sign Up & Profile Management**: Users can sign up and manage their profile.
- **Image Upload**: Upload and manage profile pictures and invoice logos.
- **Database Switch**: Option to choose between SQL Server or MySQL during setup or deployment.

## Technologies Used

- **Frontend**: Angular with NG-Zorro (Responsive UI components).
- **Backend**: ASP.NET Core Web API 6.0 (Robust and scalable API layer).
- **Database**: MySQL / SQL Server (Choose during installation or deployment).
  
## Getting Started

### Prerequisites

- **Node.js** (For Angular)
- **MySQL** or **SQL Server** (For database)
- **.NET 6.0 SDK** (For ASP.NET Core)

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/invoicika.git
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

4. **Database Setup**:

   - Ensure either MySQL or SQL Server is installed and running.
   - Update the connection strings in `appsettings.json` to configure the database you want to use.
   - Use the `DatabaseProvider` key in `appsettings.json` to switch between MySQL and SQL Server.
   
     Example:
     ```json
     "ConnectionStrings": {
       "SqlServerConnection": "Your SQL Server connection string",
       "MySqlConnection": "Your MySQL connection string"
     },
     "DatabaseProvider": "SqlServer" // Or "MySql"
     ```

5. **Apply Migrations**:
   - SQL Server:
     ```bash
     dotnet ef database update --context ApplicationDbContext -- --provider SqlServer
     ```
   - MySQL:
     ```bash
     dotnet ef database update --context ApplicationDbContext -- --provider MySql
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

Feel free to modify this README based on your project’s evolving features and requirements!
