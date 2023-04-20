using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacoteDeViagens.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Telefone { get; set; }
        public Adress Address { get; set; }
        public DateTime DtCadastro { get; set; }

        public override string ToString()
        {
            return $"Nome:{Name}\nTelefone:{Telefone}\nDataCadastro:{DtCadastro}\n{Address} ";
        }
    }
}
