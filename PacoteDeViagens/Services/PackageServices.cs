using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    public class PackageServices
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public PackageServices()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert (Packages package)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO Packages(DtCadastro, Value, IdClient, IdTicket)" +
                    "VALUES (@DtCadastro, @Value, @IdClient, @IdTicket)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", package.DtCadastro));
                commandInsert.Parameters.Add(new SqlParameter("@Value", package.Value));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(package)));
                commandInsert.Parameters.Add(new SqlParameter("@IdTIcket", InsertTicket(package)));

                commandInsert.ExecuteNonQuery();

                status = true;
            } catch (Exception)
            {
                status = false;
                throw;
            }
            finally { conn.Close(); }
            return status;
        }

        public int InsertClient(Packages package)
        {
            string strInsert = "INSERT INTO Client (Name, Telephone, IdEndereco, DtCadastro)" +
                "VALUES (@Name, @Telephone, @IdEndereco, @DtCadastro) select cast (sope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand( strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Name", package.Client.Name));
            commandInsert.Parameters.Add(new SqlParameter("@Telephone", package.Client.Telefone));
            commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", package.Client.Address));
            commandInsert.Parameters.Add(new SqlParameter("@Name", package.Client.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }
        public int InsertTicket(Packages package)
        {
            string strInsert = "INSERT INTO Ticket(IdOrigin, IdDestiny, IdClient, Date, Value) " +
                "VALUES(@IdOrigin, @IdDestiny, @IdClient, @Date, @Value) select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@IdOrigin", package.Ticket.Origin));
            commandInsert.Parameters.Add(new SqlParameter("@IdDetiny",package.Ticket.Destiny));
            commandInsert.Parameters.Add(new SqlParameter("@IdClient",package.Ticket.Client));
            commandInsert.Parameters.Add(new SqlParameter("@Date", package.Ticket.Data));
            commandInsert.Parameters.Add(new SqlParameter("@Value", package.Ticket.Value));

            return (int)commandInsert.ExecuteScalar();
        }
    }
}
