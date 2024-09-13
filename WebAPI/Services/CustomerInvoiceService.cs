using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Data;
using System;
using PuppeteerSharp.Media;
using PuppeteerSharp;

namespace WebAPI.Services
{
    public interface ICustomerInvoiceService
    {
        Task<CustomerInvoiceDto> GetCustomerInvoiceByIdAsync(Guid id);
        Task<IEnumerable<CustomerInvoiceDto>> GetAllCustomerInvoicesAsync();
        Task CreateCustomerInvoiceAsync(CustomerInvoiceDto dto);
        Task<bool> UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto dto);
        Task DeleteCustomerInvoiceAsync(Guid id);
        Task<string> GenerateInvoiceHtmlAsync(Guid invoiceId);
     
    }

    public class CustomerInvoiceService : ICustomerInvoiceService
    {
        private readonly InvoicikaDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CustomerInvoiceService(InvoicikaDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<CustomerInvoiceDto> GetCustomerInvoiceByIdAsync(Guid id)
        {
            var invoice = await _context.CustomerInvoices
                .Include(c => c.Customer)
                .Include(c => c.User)
                .Include(c => c.VAT)
                .Include(c => c.CustomerInvoiceLines)
                .ThenInclude(l => l.Item)
                .FirstOrDefaultAsync(c => c.CustomerInvoiceId == id);

            // Map to DTO (consider using AutoMapper)
            return new CustomerInvoiceDto
            {
                CustomerInvoiceId = invoice.CustomerInvoiceId,
                Customer_id = invoice.Customer_id,
                User_id = invoice.User_id,
                InvoiceDate = invoice.InvoiceDate,
                CreationDate = invoice.CreationDate,
                UpdateDate = invoice.UpdateDate,
                SubTotalAmount = invoice.SubTotalAmount,
                VatAmount = invoice.VatAmount,
                TotalAmount = invoice.TotalAmount,
                Vat_id = invoice.Vat_id,
                CustomerInvoiceLines = invoice.CustomerInvoiceLines.Select(l => new CustomerInvoiceLineDto
                {
                    InvoiceLineId = l.InvoiceLineId,
                    CustomerInvoice_id = l.CustomerInvoice_id,
                    Item_id = l.Item_id,
                    ItemName = l.Item.Name,
                    ItemDescription = l.Item.Description,
                    Quantity = l.Quantity,
                    Price = l.Price
                }).ToList()
            };
        }

        public async Task<IEnumerable<CustomerInvoiceDto>> GetAllCustomerInvoicesAsync()
        {
            var invoices = await _context.CustomerInvoices
                .Include(c => c.Customer)
                .Include(c => c.User)
                .Include(c => c.VAT)
                .Include(c => c.CustomerInvoiceLines)
                .ThenInclude(l => l.Item)
                .ToListAsync();

            // Map to DTOs (consider using AutoMapper)
            return invoices.Select(invoice => new CustomerInvoiceDto
            {
                CustomerInvoiceId = invoice.CustomerInvoiceId,
                Customer_id = invoice.Customer_id,
                User_id = invoice.User_id,
                InvoiceDate = invoice.InvoiceDate,
                CreationDate = invoice.CreationDate,
                UpdateDate = invoice.UpdateDate,
                SubTotalAmount = invoice.SubTotalAmount,
                VatAmount = invoice.VatAmount,
                TotalAmount = invoice.TotalAmount,
                Vat_id = invoice.Vat_id,
                CustomerInvoiceLines = invoice.CustomerInvoiceLines.Select(l => new CustomerInvoiceLineDto
                {
                    InvoiceLineId = l.InvoiceLineId,
                    CustomerInvoice_id = l.CustomerInvoice_id,
                    Item_id = l.Item_id,
                    ItemName = l.Item.Name,
                    ItemDescription = l.Item.Description,
                    Quantity = l.Quantity,
                    Price = l.Price
                }).ToList()
            });
        }

        public async Task CreateCustomerInvoiceAsync(CustomerInvoiceDto dto)
        {
            var invoice = new CustomerInvoice
            {
                CustomerInvoiceId = dto.CustomerInvoiceId,
                Customer_id = dto.Customer_id,
                User_id = dto.User_id,
                Vat_id = dto.Vat_id,
                InvoiceDate = dto.InvoiceDate,
                CreationDate = dto.CreationDate,
                UpdateDate = dto.UpdateDate,
                SubTotalAmount = dto.SubTotalAmount,
                VatAmount = dto.VatAmount,
                TotalAmount = dto.TotalAmount,
                CustomerInvoiceLines = dto.CustomerInvoiceLines.Select(l => new CustomerInvoiceLine
                {
                    InvoiceLineId = l.InvoiceLineId,
                    CustomerInvoice_id = l.CustomerInvoice_id,
                    Item_id = l.Item_id,
                    Quantity = l.Quantity,
                    Price = l.Price
                }).ToList()
            };

            _context.CustomerInvoices.Add(invoice);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // Handle exception or log detailed error information here
                throw;
            }
        }
        /*
        public async Task UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto dto)
        {
            var invoice = await _context.CustomerInvoices
                .Include(c => c.CustomerInvoiceLines)
                .FirstOrDefaultAsync(c => c.CustomerInvoiceId == id);

            if (invoice == null) throw new KeyNotFoundException("Invoice not found");

            invoice.Customer_id = dto.Customer_id;
            invoice.User_id = dto.User_id;
            invoice.InvoiceDate = dto.InvoiceDate;
            invoice.CreationDate = dto.CreationDate;
            invoice.UpdateDate = dto.UpdateDate;
            invoice.SubTotalAmount = dto.SubTotalAmount;
            invoice.VatAmount = dto.VatAmount;
            invoice.TotalAmount = dto.TotalAmount;
            invoice.Vat_id = dto.Vat_id;

            // Update InvoiceLines if necessary
            // This is simplified, you might need to handle existing lines and additions

            _context.CustomerInvoices.Update(invoice);
            await _context.SaveChangesAsync();
        }
        */
        public async Task<bool> UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto updatedInvoice)
        {
            var existingInvoice = await _context.CustomerInvoices
                .Include(i => i.CustomerInvoiceLines)
                .FirstOrDefaultAsync(i => i.CustomerInvoiceId == id);

            if (existingInvoice == null) return false;

            existingInvoice.Customer_id = updatedInvoice.Customer_id;
            existingInvoice.User_id = updatedInvoice.User_id;
            existingInvoice.InvoiceDate = updatedInvoice.InvoiceDate;
            existingInvoice.UpdateDate = DateTime.UtcNow; //
            existingInvoice.SubTotalAmount = updatedInvoice.SubTotalAmount;
            existingInvoice.VatAmount = updatedInvoice.VatAmount;
            existingInvoice.TotalAmount = updatedInvoice.TotalAmount;
            existingInvoice.Vat_id = updatedInvoice.Vat_id;

            // Update invoice lines
            existingInvoice.CustomerInvoiceLines.Clear(); //
            foreach (var line in updatedInvoice.CustomerInvoiceLines)
            {
                existingInvoice.CustomerInvoiceLines.Add(new CustomerInvoiceLine
                {
                    InvoiceLineId = line.InvoiceLineId,
                    CustomerInvoice_id = line.CustomerInvoice_id,
                    Item_id = line.Item_id,
                    Quantity = line.Quantity,
                    Price = line.Price
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
        public async Task DeleteCustomerInvoiceAsync(Guid id)
        {
            var invoice = await _context.CustomerInvoices.FindAsync(id);

            if (invoice == null) throw new KeyNotFoundException("Invoice not found");

            _context.CustomerInvoices.Remove(invoice);
            await _context.SaveChangesAsync();
        }


        public async Task<string> GenerateInvoiceHtmlAsync(Guid invoiceId)
        {
            var invoiceDto = await GetCustomerInvoiceByIdAsync(invoiceId);

            if (invoiceDto == null)
            {
                throw new Exception("Invoice not found.");
            }

            var customer = await _context.Customers.FindAsync(invoiceDto.Customer_id);
            var logoPath = Path.Combine(_environment.WebRootPath, "uploads", "b108a579-ae05-432f-9dc4-fca8f87c8009_gh.png");
            var logoBase64 = Convert.ToBase64String(await File.ReadAllBytesAsync(logoPath));

            var htmlContent = $@"
                <!DOCTYPE html>
                <html lang='en'>
                <head>
                  <meta charset='UTF-8'>
                  <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                  <title>Invoice</title>
                  <link href='https://fonts.googleapis.com/css2?family=Nunito:wght@400;700&display=swap' rel='stylesheet'>
                  <style>
                    body {{
                      font-family: 'Nunito', sans-serif;
                      font-size: 14px;
                      font-weight:400;
                      margin: 0;
                      padding: 0;
                    }}

                    .invoice-container {{
                      width: 100%;
                      max-width: 720px;
                      margin: 0 auto;
                      padding: 20px;
                    }}

                    .header {{
                      display: flex;
                      justify-content: space-between;
                      align-items: flex-start;
                      margin-bottom: 20px;
                    }}

                    .invoice-title {{
                      font-size: 32px;
                      font-weight: bold;
                      text-align: left;
                      margin-bottom: 5px;
                      color: #1890ff;
                    }}

                    .date-info {{
                      text-align: right;
                    }}

                    .company-info {{
                      text-align: left;
                    }}

                    .company-info img {{
                      width: 180px;
                      height: 80px;
                      margin-bottom: 10px;
                    }}

                    .company-info p {{
                      margin: 5px 0;
                    }}

                    .customer-info p {{
                      margin: 5px 0;
                    }}

                    .customer-info {{
                      text-align: right;
                    }}

                    .invoice-table {{
                      width: 100%;
                      border-collapse: collapse;
                      margin-top: 20px;
                    }}

                    .invoice-table th {{
                      background-color: #1890ff;
                      text-align: right;
                      padding: 10px;
                      font-weight: bold;
                    }}

                    .invoice-table th:first-child {{
                      text-align: left;
                    }}

                    .invoice-table td {{
                      padding: 10px;
                      text-align: right;
                    }}

                    .invoice-table td:first-child {{
                      text-align: left;
                    }}

                    .invoice-table tr:nth-child(even) {{
                      background-color: #f9f9f9;
                    }}

                    .invoice-table tr:nth-child(odd) {{
                      background-color: #ffffff;
                    }}
                    .product-description {{ display: block;
                      font-size: 12px;
                      color: #666;
                    }}
                    .summary-row td {{
                      font-weight: bold;
                      padding: 10px 10px;
                    }}

                    .total-row td {{
                      font-weight: bold;
                      background-color: #1890ff;
                      padding: 10px 10px;
                    }}

                    .summary td {{
                      text-align: right;
                    }}

                    .summary-row td:first-child,
                    .total-row td:first-child {{
                      text-align: right;
                      padding-right: 50px;
                    }}
                  </style>
                </head>

                <body>

                  <div class='invoice-container'>
                    <!-- Header Section -->
                    <div class='header'>
                      <div>
                        <p class='invoice-title'>INVOICE</p>
                        <div class='company-info'>
                          <img src='data:image/png;base64,{logoBase64}' alt='Company Logo'>
                          <p><strong>CodeBANGLA Ltd.</strong></p>
                          <p>1234 Market St., San Francisco, CA 94103</p>
                          <p>Phone: (123) 456-7890</p>
                          <p>Email: romi@codebangla.com</p>
                        </div>
                      </div>
                      <div class='date-info'>
                        <p><strong>Invoice No: #{invoiceDto.CustomerInvoiceId}</strong></p>
                        <p><strong>Date: {invoiceDto.InvoiceDate:yyyy-MM-dd}</strong></p>
                        <div class='customer-info'>
                          <p><strong>{customer.Name}</strong></p>
                          <p>{customer.Address}</p>
                          <p>{customer.PhoneNumber}</p>
                          <p>Email: {customer.Email}</p>
                        </div>
                      </div>
                    </div>

                    <!-- Invoice Table -->
                    <table class='invoice-table'>
                      <thead>
                        <tr>
                          <th>Item</th>
                          <th>Quantity</th>
                          <th>Price</th>
                          <th>Total</th>
                        </tr>
                      </thead>
                      <tbody>
                        {string.Join("", invoiceDto.CustomerInvoiceLines.Select(line => $@"
                        <tr>
                          <td>{line.ItemName} <span class=""product-description"">{line.ItemDescription}</span></td>
                          <td>{line.Quantity}</td>
                          <td>{line.Price:C}</td>
                          <td>{(line.Price * line.Quantity):C}</td>
                        </tr>"))}
                      </tbody>
                      <!-- Subtotal, VAT, Total -->
                      <tfoot style='border-top: 1px solid #1890ff;'>
                        <tr class='summary-row'>
                          <td colspan='3' style='padding-right: 10px;'>Subtotal</td>
                          <td>{invoiceDto.SubTotalAmount:C}</td>
                        </tr>
                        <tr class='summary-row'>
                          <td colspan='3' style='padding-right: 10px;'>VAT</td>
                          <td>{invoiceDto.VatAmount:C}</td>
                        </tr>
                        <tr class='total-row'>
                          <td colspan='3' style='padding-right: 10px;'>Total</td>
                          <td>{invoiceDto.TotalAmount:C}</td>
                        </tr>
                      </tfoot>
                    </table>
                  </div>

                </body>
                </html>";
            return htmlContent;
        }

    }

}
