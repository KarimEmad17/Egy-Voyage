using EgyVoyageApi.Data;
using EgyVoyageApi.DTOs;
using EgyVoyageApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EgyVoyageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {
        private readonly AppDbContext _context;

        public FeedBackController(AppDbContext context)
        {
            _context=context;
        }
        [HttpPost]
        public async Task<IActionResult> GiveFeedBack(feedbackDTO reveiw,int hotel_id)
        {
            var feed = new feedback_Hotel
            {
                HotelId = hotel_id,
                description=reveiw.description,
                rating=reveiw.rating
            };
            await _context.AddAsync(feed);
            await _context.SaveChangesAsync();

            int count = await _context.feedbacks.Where(x=>x.HotelId==hotel_id).CountAsync();
            if (count >100)
            {
                var records = await _context.feedbacks.Where(x => x.HotelId == hotel_id).ToListAsync();
                _context.RemoveRange(records);
                await _context.SaveChangesAsync();
            }
            else if (count > 25)
            {
                var rating = await _context.feedbacks.Where(x=>x.HotelId==hotel_id).Select(x => x.rating).AverageAsync();
               var hotel= await _context.hotels.FindAsync(hotel_id);
                hotel.rating = rating;
                await _context.SaveChangesAsync();

            }
            

            return Ok("thanks");
        }

       
    }
}
