using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    public class TicketController
    {
        public bool Insert(Ticket ticket)
        {
            return new TicketServices().Insert(ticket);
        }

        public bool Delete(string ticket)
        {
            return new TicketServices().Delete(ticket);
        }

        public List<Ticket> FindAll() 
        {
            return new TicketServices().FindAll();
        }

    }
}
