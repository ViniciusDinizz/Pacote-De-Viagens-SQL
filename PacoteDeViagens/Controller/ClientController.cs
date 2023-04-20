using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

namespace PacoteDeViagens.Controller
{
    internal class ClientController
    {
        public bool Insert(Client client)
        {
            return new ClientServices().Insert(client);
        }

        public bool Delete(string client)
        {
            return new ClientServices().Delete(client);
        }

        public List<Client> FindAll() 
        {
            return new ClientServices().FindAll();
        }
    }
}
