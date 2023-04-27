using System.Net;
using System.Net.Mail;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;
using FBS_CoreApi.DTOs;
using FBS_CoreApi.Models;
using FBS_CoreApi.Data;
using static FBS_CoreApi.Repositories.BookingRepository;


namespace FBS_CoreApi.Repositories
{
    public interface IBookingRepo
    {
        Task<BookFlightResult> BookFlight(int flightId, BookingDTO booking, string userId);
        Task<CancelBookingResult> CancelBooking(int bookingId, string userId);
    }

    public class BookingRepository : IBookingRepo
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<BookFlightResult> BookFlight(int flightId, BookingDTO booking, string userId)
        {
            var flight = await _context.Flights
                .FirstOrDefaultAsync(f => f.Id == flightId);

            if (flight == null)
            {
                return new BookFlightResult(false, "Invalid flight ID");
            }

            if (booking.NoOfTicket < 1)
            {
                return new BookFlightResult(false, "Number of tickets must be at least 1");
            }

            // Calculate total price based on the number of tickets and flight price
            decimal totalPrice = flight.Price * booking.NoOfTicket;


            // Create a new Booking entity
            var newBooking = new Booking
            {
                PassengerName = booking.PassengerName,
                Email = booking.Email,
                PhoneNumber = booking.PhoneNumber,
                Age = booking.Age,
                Gender = booking.Gender,
                CabinClass = booking.CabinClass,
                NoOfTicket = booking.NoOfTicket,
                TotalPrice = totalPrice,
                FlightId = flight.Id,
                UserId = userId
            };

            // Add the new booking to the database
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();

            // Send confirmation email to user
            await SendConfirmationEmailAsync(flight, booking, totalPrice, "");

            return new BookFlightResult(true, "");
        }

        public async Task<CancelBookingResult> CancelBooking(int bookingId, string userId)
        {
            var booking = await _context.Bookings.FindAsync(bookingId);

            if (booking == null)
            {
                return new CancelBookingResult(false, "Invalid booking ID");
            }

            if (booking.UserId != userId)
            {
                return new CancelBookingResult(false, "You are not authorized to cancel this booking");
            }

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            // Send cancellation confirmation email to user

            return new CancelBookingResult(true, "");
        }

        private async Task SendConfirmationEmailAsync(Flight flight, BookingDTO booking, decimal totalPrice, string fromEmail)
        {
            var message = new MailMessage();
            message.To.Add(new MailAddress(booking.Email));
            message.Subject = "Flight Booking Confirmation";
            message.Body = $"Dear {booking.PassengerName},\n\nThank you for booking your flight with us. Your booking details are as follows:\n\nFlight: {flight.FlightNumber}\nDate: {flight.DepartureTime}\nPassenger Name: {booking.PassengerName}\nNumber of Tickets: {booking.NoOfTicket}\nTotal Price: {totalPrice}\n\nPlease do not hesitate to contact us if you have any questions or concerns.\n\nSincerely,\nThe Flight Booking Team";

            message.From = new MailAddress(fromEmail);

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "",
                    Password = ""
                };
                smtp.UseDefaultCredentials = false;

                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                await smtp.SendMailAsync(message);
            }
        }

       

        public class BookFlightResult
        {
            public bool IsSuccessful { get; }
            public string ErrorMessage { get; }

            public BookFlightResult(bool isSuccessful, string errorMessage)
            {
                IsSuccessful = isSuccessful;
                ErrorMessage = errorMessage;
            }
        }

        public class CancelBookingResult
        {
            public bool IsSuccessful { get; }
            public string ErrorMessage { get; }

            public CancelBookingResult(bool isSuccessful, string errorMessage)
            {
                IsSuccessful = isSuccessful;
                ErrorMessage = errorMessage;
            }
        }
    }
}
