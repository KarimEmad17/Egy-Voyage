using EgyVoyageApi.Data;
using EgyVoyageApi.DTOs;
using EgyVoyageApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EgyVoyageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomController(AppDbContext context)
        {
            _context=context;
        }
        //----------------Begin---------------viewroom--------------Begin------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllRoom()
        {
            var rooms = await _context.rooms.Include(x => x.Hotel).Select(x => new
            {
                Id=x.Id,
                x.capacity,
                x.breakfast,
                x.freeWifi,
                x.smoking,
                Name=x.RoomNumber,
                category=x.category,
                price=x.price,
                hotelname=x.Hotel.Name,
               

            }).ToListAsync();
            return Ok(rooms);
        }
        //----------------End---------------viewroom--------------End-----------------------------

        //----------------Begin-------------CreateRoom------------Begin---------------------------
        [HttpPost]
        public async Task<IActionResult> creatRoom([FromForm]RoomDto room)
        {
            var newroom = new room 
            {
                
                RoomNumber = room.Name,
                category = room.category,
                breakfast = room.breakfast,
                freeWifi = room.freeWifi,
                smoking = room.smoking,
                capacity=room.capacity,
                price = room.price,
                HotelId = room.HotelId,
                image=room.image
            };
            await _context.rooms.AddAsync(newroom);
            await _context.SaveChangesAsync();
           
            return Ok(newroom);
        }
        //----------------End-------------CreateRoom------------End---------------------------

    }
}
