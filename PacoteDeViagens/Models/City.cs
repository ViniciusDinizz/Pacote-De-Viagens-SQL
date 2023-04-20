using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacoteDeViagens.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime DtCadastro { get; set; }

        public override string ToString()
        {
            return $"{Description}";
        }
    }
}
