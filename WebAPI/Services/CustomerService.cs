using WebAPI.Models;
using WebAPI.Data;
using Microsoft.EntityFrameworkCore;
using WebAPI.Helpers;

namespace WebAPI.Services
{
    public interface ICustomerService
    {
        Task<PaginationResult<Customer>> GetCustomersPagedAndSortedAsync(int pageNumber, int pageSize);
        Task<Customer> GetCustomerByIdAsync(Guid id);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);
    }
    public class CustomerService : ICustomerService
    {
        private readonly InvoicikaDbContext _context;

        public CustomerService(InvoicikaDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationResult<Customer>> GetCustomersPagedAndSortedAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _context.Customers.CountAsync();
            var customers = await _context.Customers
                .OrderByDescending(i => i.CreationDate) // Sorting by creation date
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResult<Customer>(customers, totalCount, pageNumber, pageSize);
        }

        public async Task<Customer> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers
                .FirstOrDefaultAsync(i => i.CustomerId == id);
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var customer = await GetCustomerByIdAsync(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }
    }
 }


