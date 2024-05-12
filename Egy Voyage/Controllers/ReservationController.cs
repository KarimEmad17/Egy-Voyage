using System.Net.Mail;
using System.Net;
using EgyVoyageApi.Data;
using EgyVoyageApi.DTOs;
using EgyVoyageApi.Entities;
using FluentEmail.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Egy_Voyage.DTOs;

namespace EgyVoyageApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly AppDbContext _Context;

        public ReservationController(AppDbContext context)
        {
            _Context=context;
        }



        //--------------Begin--------------GetAllResvation--------------Begin--------------------------------
        [HttpGet]
        public async Task<IActionResult> GetAllResvation()
        {
            var resvations = await _Context.reservations.Include(x => x.user).Include(x => x.room)
                .Select(x => new
                {
                   user = x.user.FName+" "+x.user.LName,
                   x.roomId ,
                    roomnumber=x.room.RoomNumber,
                    start=x.Start,
                    end=x.End,
                }).ToListAsync();
            return StatusCode(statusCode: 200, resvations);
        }
        //--------------End--------------GetAllResvation--------------End--------------------------------


        //--------------Begin--------------Reserve--------------Begin--------------------------------
        [HttpPost]
        public async Task<IActionResult> Reserve(string Email,int roomid, reservationDTO Reserve)
        {
            var check =await _Context.users.Where(x=>x.Email==Email).AnyAsync();
            if (check)
            {
                int user_id = await _Context.users
                                   .Where(x => x.Email == Email)
                                   .Select(x => x.Id)
                                   .FirstOrDefaultAsync();
                var newreserve = new Reservation
                {
                    UserId= user_id,
                    roomId=roomid,
                    Start=Reserve.Start,
                    End=Reserve.End,
                };
                await _Context.reservations.AddAsync(newreserve);
                await _Context.SaveChangesAsync();
                return StatusCode(statusCode: 200,newreserve);
            }
            else
            {
                return StatusCode(StatusCodes.Status400BadRequest, "No User Found");

            }

            //--------------End--------------Reserve--------------End--------------------------------

            
            
        }
        [HttpPost("search")]
        public async Task<IActionResult> search(searchDTO search)
        {
            Console.WriteLine(search);
            var hotel = await _Context.hotels
     .Include(x => x.rooms)
     .Where(x => x.location == search.distination ||
                 (x.location == search.distination &&
                 (x.rooms == null || x.rooms.All(room => room.Reservations.Any(reservation => reservation.Start > search.end || reservation.End < search.start)))))
     .Select(x => new
     {
         id = x.Id,
         name = x.Name,
         rating = x.rating,
         cordinate = x.cordinate,
         location = x.location,
         Description = x.Description,
         minprice = x.rooms != null ? (int?)x.rooms.Select(room => room.price).Min() : null, // Use nullable int to allow null value
         image = x.images.Select(img => img.image),
         rooms = x.rooms != null ? x.rooms.Where(room => room.Reservations.Any(reservation => reservation.Start > search.end || reservation.End < search.start))
                                     .Select(room => new
                                     {
                                         room.Id,
                                         room.RoomNumber,
                                         room.price,
                                         room.category,
                                         room.image
                                     }) : null // If rooms is null, assign null to rooms
     })
     .ToListAsync();

            return Ok(hotel);


           
           

        }

        //get user's data for the reservation form
        [HttpGet("getuser")]
        public async Task<IActionResult> GetUser(string email)
        {
            var user = await _Context.users.Where(x => x.Email == email).ToListAsync();
            return Ok(user);
        }
        //reserv
        [HttpPost("reserv")]
        public async Task<IActionResult> reserv(ReserveDto reserv)
        {
            int userId = _Context.users.Where(x => x.Email == reserv.email).Select(x => x.Id).FirstOrDefault();
            Reservation newreserv = new Reservation
            {
                UserId = userId,
                roomId = reserv.roomId,
                
                Start = reserv.Start,
                End = reserv.End,

            };
            var random = new Random();
            string pincode = random.Next(100000, 999999).ToString();
            long num_process = (long)random.Next(1000000000, int.MaxValue) * 10 + random.Next(10);
            Console.WriteLine(num_process);
            string pass = "nzrpygitegotrnng";
            var client = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("egyvoyage@gmail.com", pass),
                EnableSsl = true
            };


            var message = new MailMessage("egyvoyage@gmail.com", reserv.email, "Welcome, Your reservation has been completed", pincode);
            try
            {
                await client.SendMailAsync(message);
                await _Context.AddAsync(newreserv);
                await _Context.SaveChangesAsync();
                return Ok(num_process);
            }
            catch (Exception ex)
            {
                return BadRequest($"Smtp send failed with the exception: {ex.Message}.");
            }

        }









    }
        }
    

