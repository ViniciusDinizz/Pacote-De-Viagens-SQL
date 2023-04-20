using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    public class CityController
    {
        public bool Insert(City city)
        {
            return new CityServices().Insert(city);
        }

        public bool Delete(string city)
        {
            return new CityServices().Delete(city);
        }

        public List<City> FindAll()
        {
            return new CityServices().FindAll();
        }
    }
}
