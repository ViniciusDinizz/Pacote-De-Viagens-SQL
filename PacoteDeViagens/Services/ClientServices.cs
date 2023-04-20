using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    public class ClientServices
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
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(client.Address.City)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", client.Address.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        private int InsertCity(City city)
        {
            string srtInsert = "INSERT  INTO City(Description, DtCadastro) VALUES (@Description, @DtCadastro); select cast(scope_identity() as int)";
            SqlCommand commandinsert = new SqlCommand(srtInsert, conn);

            commandinsert.Parameters.Add(new SqlParameter("@Description",city.Description));
            commandinsert.Parameters.Add(new SqlParameter("@DtCadastro", city.DtCadastro));

            return (int)commandinsert.ExecuteScalar();
        }

        public bool Delete(string client)
        {
            bool status = false;
            try
            {
                string strDelete = "DELETE FROM Client WHERE Id = " + client;

                SqlCommand commanddelete = new SqlCommand(strDelete, conn);

                commanddelete.ExecuteNonQuery();

                status = true;
            }
            catch (Exception ex) { status = false; throw; }
            finally { conn.Close(); }
            return status;
        }

        public List<Client> FindAll()
        {
            List<Client> clients = new List<Client>();

            StringBuilder sb = new StringBuilder();

            sb.Append(" select c.Id, ");
            sb.Append("     c.Name, ");
            sb.Append("     c.Telephone, ");
            sb.Append("     c.DtCadastro, ");
            sb.Append("     c.IdEndereco, ");
            sb.Append("     a.Street, ");
            sb.Append("     a.Number, ");
            sb.Append("     a.Burgh, ");
            sb.Append("     a.Cep, ");
            sb.Append("     a.Complement, ");
            sb.Append("     a.IdCity, ");
            sb.Append("     ci.Description ");
            sb.Append("   FROM Client c, Adress a, City ci ");
            sb.Append(" WHERE c.IdEndereco = a.Id AND a.IdCity = ci.Id");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while(dr.Read())
            {
                Client client = new Client();

                client.Id = (int)dr["Id"];
                client.Name = (string)dr["Name"];
                client.Telefone = (string)dr["Telephone"];
                client.DtCadastro = (DateTime)dr["DtCadastro"];
                client.Address = new Adress()
                {
                    Id = (int)dr["IdEndereco"],
                    Street = (string)dr["Street"],
                    Number = (int)dr["Number"],
                    Burgh = (string)dr["Burgh"],
                    CEP = (string)dr["Cep"],
                    Complement = (string)dr["Complement"]
                };

                clients.Add(client);
            }
            return clients;
        }
    }
}
