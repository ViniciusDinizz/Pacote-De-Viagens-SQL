using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    internal class ClientServices
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public ClientServices()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Client client)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO Client (Name, Telephone, IdEndereco, DtCadastro) VALUES (@Name, @Telephone, @IdEndereco, @DtCadastro)";
                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", client.Name));
                commandInsert.Parameters.Add(new SqlParameter("@Telephone", client.Telefone));
                commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", InsertAdress(client)));
                commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", client.DtCadastro));

                commandInsert.ExecuteNonQuery();

                status = true;
            } catch (Exception)
            {
                status = false;
                throw;
            } finally { conn.Close(); }
            return status;
        }

        private int InsertAdress(Client client)
        {
            string strInsert = "INSERT INTO Adress(Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro) select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", client.Address.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", client.Address.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", client.Address.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", client.Address.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", client.Address.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", client.Address.City));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", client.Address.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }
    }
}
