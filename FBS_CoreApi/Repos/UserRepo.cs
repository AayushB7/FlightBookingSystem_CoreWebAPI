using FBS_CoreApi.Data;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Security.Claims;

namespace FBS_CoreApi.Repositories
{
    public interface IUserRepo
    {
        UserWithBookingsDto GetUserWithBookings(string userId);
    }

    public class UserRepo : IUserRepo
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserRepo(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public UserWithBookingsDto GetUserWithBookings(string userId)
        {
            var authenticatedUserId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            if (authenticatedUserId != userId)
            {
                throw new UnauthorizedAccessException("You do not have permission to access this resource.");
            }

            var user = _context.Users.FirstOrDefault(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var userWithBookingsDto = new UserWithBookingsDto
            {
           //     UserId = user.Id,
            //    UserName = user.UserName,
                Email = user.Email,
                Bookings = _context.Bookings
                    .Where(b => b.UserId == userId)
                    .Select(b => new BookingDTO
                    {
                        PassengerName = b.PassengerName,
                        Email = b.Email,
                        PhoneNumber = b.PhoneNumber,
                        Age = b.Age,
                        Gender = b.Gender,
                        CabinClass = b.CabinClass,
                        NoOfTicket = b.NoOfTicket
                    })
                    .ToList()
            };

            return userWithBookingsDto;
        }
    }
}