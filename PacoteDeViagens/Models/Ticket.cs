using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacoteDeViagens.Models
{
    public class Ticket
    {
        public int Id { get; set; }
        public Adress Origin { get; set; }
        public Adress Destin { get; set; }
        public Client Client { get; set; }
        public DateTime Data { get; set; }
        public decimal Value { get; set; }

        public override string ToString()
        {
            return $"{Client}\nId:{Id}\nOrigin:{Origin}\nDestin:{Destin}\nData:{Data}\nValor:{Value}\n\n";
        }
    }
}
