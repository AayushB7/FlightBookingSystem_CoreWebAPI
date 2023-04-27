using FBS_CoreApi.Data;
using FBS_CoreApi.DTOs;
using FlightBooking.Models;
using Microsoft.EntityFrameworkCore;

namespace FBS_CoreApi.Repos
{
    public interface ISearchRepo
    {
        Task<List<FlightDto>> GetFlightsAsync(string departureAirport, string arrivalAirport, DateTime departureTime);
    }

    public class SearchRepo : ISearchRepo
    {
        private readonly ApplicationDbContext _dbContext;

        public SearchRepo(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<FlightDto>> GetFlightsAsync(string departureAirport, string arrivalAirport, DateTime departureTime)
        {
            var flights = await _dbContext.Flights
                .Where(f => f.DepartureAirport == departureAirport
                            && f.ArrivalAirport == arrivalAirport
                            && f.DepartureTime.Date == departureTime.Date)
                .ToListAsync();

            return flights.Select(flight => new FlightDto
            {
                FlightNumber = flight.FlightNumber,
                DepartureAirport = flight.DepartureAirport,
                ArrivalAirport = flight.ArrivalAirport,
                DepartureTime = flight.DepartureTime,
                ArrivalTime = flight.ArrivalTime,
                Price = flight.Price
            }).ToList();
        }

    }



}
