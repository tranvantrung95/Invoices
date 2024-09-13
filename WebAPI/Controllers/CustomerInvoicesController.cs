using Microsoft.AspNetCore.Mvc;
using PuppeteerSharp.Media;
using PuppeteerSharp;
using WebAPI.Dtos;
using WebAPI.Services;
using Microsoft.EntityFrameworkCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerInvoicesController : ControllerBase
    {
        private readonly ICustomerInvoiceService _service;

        public CustomerInvoicesController(ICustomerInvoiceService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerInvoiceDto>> GetCustomerInvoice(Guid id)
        {
            try
            {
                var invoice = await _service.GetCustomerInvoiceByIdAsync(id);
                return Ok(invoice);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerInvoiceDto>>> GetAllCustomerInvoices()
        {
            var invoices = await _service.GetAllCustomerInvoicesAsync();
            return Ok(invoices);
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomerInvoice([FromBody] CustomerInvoiceDto dto)
        {
            await _service.CreateCustomerInvoiceAsync(dto);
            return CreatedAtAction(nameof(GetCustomerInvoice), new { id = dto.CustomerInvoiceId }, dto);
        }

        // PUT: api/CustomerInvoice/{id}
        /*
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomerInvoice(Guid id, [FromBody] CustomerInvoiceDto dto)
        {
            try
            {
                await _service.UpdateCustomerInvoiceAsync(id, dto);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        } */

        // PUT: api/CustomerInvoice/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(Guid id, [FromBody] CustomerInvoiceDto updatedInvoice)
        {
            var updated = await _service.UpdateCustomerInvoiceAsync(id, updatedInvoice);
            if (!updated)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomerInvoice(Guid id)
        {
            try
            {
                await _service.DeleteCustomerInvoiceAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("pdf/{id}")]
        public async Task<IActionResult> GenerateInvoicePdf(Guid id)
        {
            try
            {
                // Fetch the invoice details and generate HTML content
                var htmlContent = await _service.GenerateInvoiceHtmlAsync(id);

                // Setup Puppeteer and download the latest Chromium
                var browserFetcher = new BrowserFetcher();
                await browserFetcher.DownloadAsync();

                // Launch headless browser
                using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
                using (var page = await browser.NewPageAsync())
                {
                    // Set content
                    await page.SetContentAsync(htmlContent);

                    // Generate PDF
                    var pdfBytes = await page.PdfDataAsync(new PdfOptions
                    {
                        Format = PaperFormat.A4,
                        PrintBackground = true
                    });

                    // Return PDF as a file
                    return File(pdfBytes, "application/pdf", $"invoice-{id}.pdf");
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                return StatusCode(500, $"Error generating PDF: {ex.Message}");
            }
        }

    }

}
