using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacoteDeViagens.Models
{
    public class Hotel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Adress Address { get; set; }
        public DateTime DtCadastro { get; set; }
        public decimal Valor { get; set; }

        public override string ToString()
        {
            return $"IdHotel:{Id}\nNome:{Name}\n{Address}\nCadastro:{DtCadastro}\nValor:{Valor}\n";
        }
    }
}
