using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    internal class TicketController
    {
        public bool Insert(Ticket ticket)
        {
            return new TicketServices().Insert(ticket);
        }
    }
}
