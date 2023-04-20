using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    public class TicketServices
    {
        readonly string strComm = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public TicketServices()
        {
            conn = new SqlConnection(strComm);
            conn.Open();
        }

        public bool Insert(Ticket ticket)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO Ticket ( IdOrigin, IdDestin, IdClient, Date, Value) " +
                    "VALUES (@IdOrigin, @IdDestiny, @IdClient, @Date, @Value)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@IdOrigin", InsertAdressOrigin(ticket)));
                commandInsert.Parameters.Add(new SqlParameter("@IdDestiny", InsertAdressDestin(ticket)));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(ticket)));
                commandInsert.Parameters.Add(new SqlParameter("@Date", ticket.Data));
                commandInsert.Parameters.Add(new SqlParameter("@Value", ticket.Value));

                commandInsert.ExecuteNonQuery();

                status = true;

            }
            catch (Exception)
            {
                status = false;
                throw;
            }
            finally { conn.Close(); }
            return status;
        }

        public int InsertAdressOrigin(Ticket ticket)
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro); select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", ticket.Origin.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", ticket.Origin.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", ticket.Origin.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", ticket.Origin.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", ticket.Origin.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(ticket.Origin.City)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", ticket.Origin.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        public int InsertAdressDestin(Ticket ticket)
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro); select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", ticket.Destin.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", ticket.Destin.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", ticket.Destin.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", ticket.Destin.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", ticket.Destin.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(ticket.Destin.City)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", ticket.Destin.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        public int InsertClient(Ticket ticket)
        {
            string strInsert = "INSERT INTO Client(Name, Telephone, IdEndereco, DtCadastro) " +
                "VALUES (@Name, @Telephone, @IdEndereco, @DtCadastro); select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Name", ticket.Client.Name));
            commandInsert.Parameters.Add(new SqlParameter("@Telephone", ticket.Client.Telefone));
            commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", InsertAdress(ticket.Client.Address)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", ticket.Client.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        private int InsertCity(City city)
        {
            string srtInsert = "INSERT  INTO City(Description, DtCadastro) VALUES (@Description, @DtCadastro); select cast(scope_identity() as int)";
            SqlCommand commandinsert = new SqlCommand(srtInsert, conn);

            commandinsert.Parameters.Add(new SqlParameter("@Description", city.Description));
            commandinsert.Parameters.Add(new SqlParameter("@DtCadastro", city.DtCadastro));

            return (int)commandinsert.ExecuteScalar();
        }

        public int InsertAdress(Adress adress)
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro); select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", adress.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", adress.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", adress.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", adress.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", adress.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(adress.City)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", adress.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }

        public bool Delete(string ticket)
        {
            bool status = false;
            try
            {
                string strDelete = "DELETE Ticket FROM Ticket WHERE Id = " + ticket;

                SqlCommand commandInsert = new SqlCommand(strDelete, conn);

                commandInsert.ExecuteNonQuery();

                status = true;
            }
            catch (Exception) { status = false; throw; }
            finally { conn.Close(); }
            return status;
        }

        public List<Ticket> FindAll()
        {
            List<City> lstCity = GetListCity();
            List<Adress> lstAdress = GetListAdress(lstCity);
            List<Client> lstClient = GetListClient(lstAdress);
            List<Ticket> tickets = new List<Ticket>();

            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT  t.IdOrigin,  ");
            sb.Append("        t.IdDestin,  ");
            sb.Append("        t.IdClient,  ");
            sb.Append("        t.Date,  ");
            sb.Append("        t.Value  ");
            sb.Append(" FROM Ticket t");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            sb.Clear();
            SqlDataReader dr = commandSelect.ExecuteReader();

            while (dr.Read())
            {
                int IdOrigin = (int)dr["IdOrigin"];
                int IdDestin = (int)dr["IdDestin"];
                int IdClient = (int)dr["IdClient"];

                Ticket ticket = new();

                ticket.Origin = TakeAddress(lstAdress, IdOrigin);
                ticket.Destin = TakeAddress(lstAdress, IdDestin);
                ticket.Client = TakeClient(lstClient, IdClient);
                ticket.Data = (DateTime)dr["Date"];
                ticket.Value = (decimal)dr["Value"];

                tickets.Add(ticket);
            }
            return tickets;
        }

        private List<Client> GetListClient(List<Adress> lstAdress)
        {
            int IdEnd = 0;
            List<Client> lstClient = new();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT c.Id, ");
            sb.Append("       c.Name, ");
            sb.Append("       c.Telephone, ");
            sb.Append("       c.IdEndereco, ");
            sb.Append("       c.DtCadastro ");
            sb.Append("  FROM Client c ");
            
            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while (reader.Read())
            {
                IdEnd = (int)reader["IdEndereco"];
                Client obClient = new Client();

                obClient.Id = (int)reader["Id"];
                obClient.Name = (string)reader["Name"];
                obClient.Telefone = (string)reader["Telephone"];
                obClient.Address = TakeAddress(lstAdress, IdEnd);
                obClient.DtCadastro = (DateTime)reader["DtCadastro"];

                lstClient.Add(obClient);
            }
            reader.Close();
            return lstClient;
        }

        private List<Adress> GetListAdress(List<City> lstCity)
        {
            int iddCity = 0;
            List<Adress> lstAdress = new();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT a.Id, ");
            sb.Append("       a.Street, ");
            sb.Append("       a.Number, ");
            sb.Append("       a.Burgh, ");
            sb.Append("       a.Cep, ");
            sb.Append("       a.Complement, ");
            sb.Append("       a.IdCity, ");
            sb.Append("       a.DtCadastro ");
            sb.Append("   FROM Adress a ");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();
            while (reader.Read())
            {
                Adress obAdress = new();
                iddCity = (int)reader["IdCity"];

                obAdress.Id = (int)reader["Id"];
                obAdress.Street = (string)reader["Street"];
                obAdress.Number = (int)reader["Number"];
                obAdress.Burgh = (string)reader["Burgh"];
                obAdress.CEP = (string)reader["Cep"];
                obAdress.Complement = (string)reader["Complement"];
                obAdress.City = TakeCity(lstCity, iddCity);
                obAdress.DtCadastro = (DateTime)reader["DtCadastro"];

                lstAdress.Add(obAdress);
            }
            reader.Close();
            return lstAdress;
        }

        private List<City> GetListCity()
        {
            List<City> list = new List<City>();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT c.Id, ");
            sb.Append("       c.Description, ");
            sb.Append("       c.DtCadastro ");
            sb.Append("  FROM City c    ");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();
            while(reader.Read())
            {
                City obCity = new City();

                obCity.Id = (int)reader["Id"];
                obCity.Description = (string)reader["Description"];
                obCity.DtCadastro = (DateTime)reader["DtCadastro"];

                list.Add(obCity);
            }
            reader.Close();
            return list;
        }
        private Adress TakeAddress(List<Adress> lstAdress, int idAdress)
        {
            Adress obAdress = new();
            foreach(Adress adress in lstAdress)
            {
                if(adress.Id == idAdress)
                {
                    obAdress = adress;
                }
            }
            return obAdress;
        }

        private City TakeCity(List<City> lstCity, int idCity)
        {
            City obCity = new();
            foreach(City city in lstCity)
            {
                if(city.Id == idCity)
                {
                    obCity = city;
                }
            }
            return obCity;
        }

        public Client TakeClient(List<Client> lstClient, int idClient)
        {
            Client obClient = new();
            foreach(Client client in lstClient)
            {
                if (client.Id == idClient)
                {
                    obClient = client;
                }
            }
            return obClient;
        }
    }
}
