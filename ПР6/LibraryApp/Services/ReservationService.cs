using System.Collections.Generic;
using System.Linq;
using LibraryApp.Models;
using LibraryApp.Validators;

namespace LibraryApp.Services
{
    public class ReservationService : IReservationService
    {
        private readonly List<Reservation> _reservations = new List<Reservation>();
        private readonly ReservationValidator _validator = new ReservationValidator();

        public bool ReserveBook(Reservation reservation)
        {
            var validationResults = _validator.Validate(reservation);
            if (validationResults.Any())
            {
                foreach (var result in validationResults)
                    System.Console.WriteLine(result.ErrorMessage);
                return false;
            }

            // Проверяем, не зарезервирована ли уже эта книга
            if (_reservations.Any(r => r.Book.Title == reservation.Book.Title))
            {
                System.Console.WriteLine("Книга уже зарезервирована.");
                return false;
            }

            _reservations.Add(reservation);
            return true;
        }
    }
}