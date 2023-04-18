using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    internal class TicketServices
    {
        readonly string strComm = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public TicketServices()
        {
            conn = new SqlConnection(strComm);
            conn.Open();
        }

        public bool Insert (Ticket ticket)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO Ticket ( IdOrigin, IdDestiny, IdClient, Date, Value) " +
                    "VALUES (@IdOrigin, @IdDestiny, @IdClient, @Date, @Value)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@IdOrigin", InsertAdressOrigin(ticket)));
                commandInsert.Parameters.Add(new SqlParameter("@IdDestiny", InsertAdressDestin(ticket)));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(ticket)));
                commandInsert.Parameters.Add(new SqlParameter("@Date", ticket.Data));
                commandInsert.Parameters.Add(new SqlParameter("@Value", ticket.Value));

                commandInsert.ExecuteNonQuery();

                status = true;

            } catch (Exception)
            {
                status = false;
                throw;
            }
            finally { conn.Close();}
            return status;
        }

        public int InsertAdressOrigin(Ticket ticket) 
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro) select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", ticket.Origin.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", ticket.Origin.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", ticket.Origin.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", ticket.Origin.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", ticket.Origin.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", ticket.Origin.City));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", ticket.Origin.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        public int InsertAdressDestin(Ticket ticket)
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro) select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", ticket.Destin.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", ticket.Destin.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", ticket.Destin.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", ticket.Destin.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", ticket.Destin.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", ticket.Destin.City));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", ticket.Destin.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        public int InsertClient(Ticket ticket)
        {
            string strInsert = "INSERT INTO Client(Name, Telephone, IdEndereco, DtCadastro) " +
                "VALUES (@Name, @Telephone, @IdEndereco, @DtCadastro)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Name", ticket.Client.Name));
            commandInsert.Parameters.Add(new SqlParameter("@Telephone", ticket.Client.Telefone));
            commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", ticket.Client.Address));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", ticket.Client.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }
    }
}
