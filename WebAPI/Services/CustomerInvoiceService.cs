using WebAPI.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Dtos;
using WebAPI.Data;
using System;

namespace WebAPI.Services
{
    public interface ICustomerInvoiceService
    {
        Task<CustomerInvoiceDto> GetCustomerInvoiceByIdAsync(Guid id);
        Task<IEnumerable<CustomerInvoiceDto>> GetAllCustomerInvoicesAsync();
        Task CreateCustomerInvoiceAsync(CustomerInvoiceDto dto);
        Task<bool> UpdateCustomerInvoiceAsync(Guid id, CustomerInvoiceDto dto);
        Task DeleteCustomerInvoiceAsync(Guid id);
    }

    public class CustomerInvoiceService : ICustomerInvoiceService
    {
        private readonly InvoicikaDbContext _context;

        public CustomerInvoiceService(InvoicikaDbContext context)
        {
            _context = context;
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

    }
}
