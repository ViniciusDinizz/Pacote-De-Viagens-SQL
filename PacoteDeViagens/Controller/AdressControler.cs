using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    internal class AdressControler
    {
        public bool Insert(Adress adress)
        {
            return new AdressServices().Insert(adress);
        }

        public List<Adress> FindAll()
        {
            return new AdressServices().FindAll();
        }
    }
}
