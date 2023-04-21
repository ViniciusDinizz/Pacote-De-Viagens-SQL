using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacoteDeViagens.Models
{
    public class Packages
    {
        public int Id { get; set; }
        public Hotel Hotel { get; set; }
        public Ticket Ticket { get; set; }
        public DateTime DtCadastro { get; set; }
        public decimal Value { get; set; }
        public Client Client { get; set; }

        public override string ToString()
        {
            return $"Id:{Id}\n{Hotel}\n{Ticket}\n{Client}\nData:{DtCadastro}\nValor{Value}";
        }
    }
}
