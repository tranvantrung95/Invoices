using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Data;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Helpers;

namespace WebAPI.Services
{
    public interface ICustomerInvoiceService
    {
        Task<CustomerInvoiceDto> GetCustomerInvoiceByIdAsync(Guid id);
        Task<IEnumerable<CustomerInvoiceDto>> GetAllCustomerInvoicesAsync();
        Task CreateCustomerInvoiceAsync(CustomerInvoiceDto dto);
        Task<bool> UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto dto);
        Task DeleteCustomerInvoiceAsync(Guid id);
        Task<byte[]> GenerateInvoicePdfAsync(Guid invoiceId);

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


        public async Task<byte[]> GenerateInvoicePdfAsync(Guid invoiceId)
        {
            var invoice = await GetCustomerInvoiceByIdAsync(invoiceId);

            if (invoice == null)
            {
                throw new ArgumentException("Invoice not found", nameof(invoiceId));
            }

            var companyInfo = new
            {
                CompanyName = "Invoicika Inc.",
                Address = "456 Business Road, Metropolis",
                Email = "info@invoicika.com",
                PhoneNumbers = new[] { "555-1010", "555-1020", "555-3030" }
            };

            var customerInfo = new
            {
                Name = invoice.Customer_id,  // Replace with actual values if available
                Address = invoice.Customer_id, 
                Phone = invoice.Customer_id    
            };

            // Calculations for subtotal, VAT, and total
            var subTotal = invoice.SubTotalAmount;
            var vat = subTotal * (invoice.VatAmount/100); 
            var total = invoice.TotalAmount;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4); 
                    page.Margin(1, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(9));

                    page.Header().Row(row =>
                    {
                        row.ConstantItem(20).Image("wwwroot/uploads/invoicika.png").FitArea();
                        row.RelativeItem().Column(column =>
                        {
                            column.Item().Text(companyInfo.CompanyName).Bold().FontSize(18).AlignLeft();
                        });
                    });

                     page.Content().PaddingVertical(20).Column(column =>
                    {
                        
                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Column(columnLeft =>
                            {
                                columnLeft.Item().Text("Company Info").Bold().FontSize(10);
                                columnLeft.Item().Text($"{companyInfo.CompanyName}").Bold().FontSize(12);
                                columnLeft.Item().Text($"Address: {companyInfo.Address}");
                                columnLeft.Item().Text($"Email: {companyInfo.Email}");
                                foreach (var phone in companyInfo.PhoneNumbers)
                                {
                                    columnLeft.Item().Text($"Phone: {phone}");
                                }
                            });

                            row.RelativeItem().Column(columnRight =>
                            {
                                columnRight.Item().Text("Customer Info").Bold().FontSize(10);
                                columnRight.Item().Text($"Name: {customerInfo.Name}").Bold().FontSize(12);
                                columnRight.Item().Text($"Address: {customerInfo.Address}");
                                columnRight.Item().Text($"Phone: {customerInfo.Phone}");
                            });
                        });

                        
                        column.Item().PaddingTop(5).PaddingBottom(15).LineHorizontal(1).LineColor(Colors.Blue.Medium);
                        column.Item().Table(table =>
                        {
                            
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);  
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Product Name");
                                header.Cell().Element(CellStyle).Text("Description");
                                header.Cell().Element(CellStyle).AlignRight().Text("Unit Price");
                                header.Cell().Element(CellStyle).AlignRight().Text("Quantity");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.DefaultTextStyle(x => x.Bold()).PaddingVertical(10).BorderBottom(1).BorderColor(Colors.Black).Background(Colors.Blue.Lighten2);
                                }
                            });

                            foreach (var line in invoice.CustomerInvoiceLines)
                            {
                                table.Cell().Element(CellStyle).Text(line.ItemName);
                                table.Cell().Element(CellStyle).Text(line.ItemDescription);
                                table.Cell().Element(CellStyle).AlignRight().Text($"{line.Price}$");
                                table.Cell().Element(CellStyle).AlignRight().Text(line.Quantity.ToString());
                                table.Cell().Element(CellStyle).AlignRight().Text($"{line.Price * line.Quantity}$");

                                static IContainer CellStyle(IContainer container)
                                {
                                    return container.BorderBottom(1).BorderColor(Colors.Grey.Lighten2).PaddingVertical(3);
                                }
                            }
                        });

                        // Add Subtotal, VAT, and Total section at the bottom
                        column.Item().Table(table =>
                        {
                            // Define two columns: Labels and Values
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(100);  
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            table.Cell().ColumnSpan(4).Element(LabelCellStyle).Text("Subtotal:");
                            table.Cell().Element(ValueCellStyle).AlignRight().Text($"{subTotal}$");

                            table.Cell().ColumnSpan(4).Element(LabelCellStyle).Text("VAT (5%):");
                            table.Cell().Element(ValueCellStyle).AlignRight().Text($"{vat}$");

                            table.Cell().ColumnSpan(4).Element(LabelCellStyle).Text("Total:").FontColor(Colors.Blue.Darken2).Bold().FontSize(10);
                            table.Cell().Element(ValueCellStyle).AlignRight().Text($"{total}$").FontColor(Colors.Blue.Darken2).Bold().FontSize(10);

                            static IContainer LabelCellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.SemiBold()).PaddingVertical(5).AlignRight();
                            }

                            static IContainer ValueCellStyle(IContainer container)
                            {
                                return container.DefaultTextStyle(x => x.Bold()).PaddingVertical(5);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Column(column =>
                    {
                        column.Item().PaddingTop(10).LineHorizontal(1).LineColor(Colors.Grey.Medium);
                        column.Spacing(2);
                        column.Item().Text("This computer-generated document is valid without signature.").AlignCenter();
                    });
                });
            });

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                return await Task.FromResult(stream.ToArray());
            }
        }


    }

}
