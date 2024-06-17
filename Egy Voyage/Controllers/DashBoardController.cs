using EgyVoyageApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Egy_Voyage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashBoardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashBoardController(AppDbContext context)
        {
            _context=context;
        }
        [HttpGet("chart")]
        public async Task<IActionResult> Get()
        {
            var monthNames = new[] { "JAN", "FEB", "MAR", "APR", "MAY", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            var reservation = await _context.receipts
            .GroupBy(r => r.reservation_date.Month).OrderBy(g => g.Key).Select(g => new
            {
                MonthName = monthNames[g.Key - 1],
                Reservations = g.ToList()
            })
            .ToListAsync();
            return Ok(reservation);
        }
        [HttpGet("Piechart")]
        public async Task<IActionResult> pie()
        {

            var counts = await _context.receipts
             .GroupBy(r => r.Hotel.Name)
             .Select(g => new
             {
                 HotelName = g.Key,
                 ReservationCount = g.Count()
             })
             .ToListAsync();

            return Ok(counts);
        }
    }
}
