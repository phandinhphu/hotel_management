using Hotel_Management.Models;

namespace Hotel_Management.Services.Interfaces
{
    public interface IHotelService
    {
        public Task<Hotel> getInfoHotel();
    }
}
