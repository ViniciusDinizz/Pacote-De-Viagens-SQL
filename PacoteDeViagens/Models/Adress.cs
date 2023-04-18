using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacoteDeViagens.Models
{
    public class Adress
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int Number { get; set; }
        public string Burgh { get; set; }
        public string CEP { get; set; }
        public string Complement { get; set; }
        public City City { get; set; }
        public DateTime DtCadastro { get; set; }
    }
}
