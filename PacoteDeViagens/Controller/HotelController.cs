using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    public class HotelController
    {
        public bool InsertHotel(Hotel hotel)
        {
            return new HotelServices().Insert(hotel);
        }

       public bool Delete(string hotel)
        {
            return new HotelServices().Delete(hotel);
        }

        public List<Hotel> FindAll()
        {
            return new HotelServices().FindAll();
        }
    }
}
