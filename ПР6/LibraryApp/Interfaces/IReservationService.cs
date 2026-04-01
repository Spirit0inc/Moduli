using LibraryApp.Models;

namespace LibraryApp.Services
{
    public interface IReservationService
    {
        bool ReserveBook(Reservation reservation);
    }
}