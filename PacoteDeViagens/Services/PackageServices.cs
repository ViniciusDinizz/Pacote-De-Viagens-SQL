using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
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
                string strInsert = "INSERT INTO Packages(IdHotel, IdTicket, DtCadastro, Value, IdClient)" +
                    "VALUES (@IdHotel, @IdTicket, @DtCadastro, @Value, @IdClient)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@IdHotel", InsertHotel(package.Hotel)));
                commandInsert.Parameters.Add(new SqlParameter("@IdTicket", InsertTicket(package)));
                commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", package.DtCadastro));
                commandInsert.Parameters.Add(new SqlParameter("@Value", package.Value));
                commandInsert.Parameters.Add(new SqlParameter("@IdClient", InsertClient(package)));

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
                "VALUES (@Name, @Telephone, @IdEndereco, @DtCadastro); select cast (scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand( strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Name", package.Client.Name));
            commandInsert.Parameters.Add(new SqlParameter("@Telephone", package.Client.Telefone));
            commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", InsertAdress(package.Client.Address)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", package.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }
        public int InsertTicket(Packages package)
        {
            string strInsert = "INSERT INTO Ticket(IdOrigin, IdDestin, IdClient, Date, Value) " +
                "VALUES(@IdOrigin, @IdDestin, @IdClient, @Date, @Value) select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@IdOrigin", InsertAdress(package.Ticket.Origin)));
            commandInsert.Parameters.Add(new SqlParameter("@IdDestin",InsertAdress(package.Ticket.Destin)));
            commandInsert.Parameters.Add(new SqlParameter("@IdClient",InsertClient(package)));
            commandInsert.Parameters.Add(new SqlParameter("@Date", package.Ticket.Data));
            commandInsert.Parameters.Add(new SqlParameter("@Value", package.Ticket.Value));

            return (int)commandInsert.ExecuteScalar();
        }
        public int InsertAdress(Adress adress)
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                    "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro); select cast(scope_identity() as int)";

            SqlCommand commandinsert = new SqlCommand(strInsert, conn);

            commandinsert.Parameters.Add(new SqlParameter("@Street", adress.Street));
            commandinsert.Parameters.Add(new SqlParameter("@Number", adress.Number));
            commandinsert.Parameters.Add(new SqlParameter("@Burgh", adress.Burgh));
            commandinsert.Parameters.Add(new SqlParameter("@Cep", adress.CEP));
            commandinsert.Parameters.Add(new SqlParameter("@Complement", adress.Complement));
            commandinsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(adress)));
            commandinsert.Parameters.Add(new SqlParameter("@DtCadastro", adress.DtCadastro));

            return (int)commandinsert.ExecuteScalar();
        }
        private int InsertCity(Adress adress)
        {
            string srtInsert = "INSERT  INTO City(Description, DtCadastro) VALUES (@Description, @DtCadastro); select cast(scope_identity() as int)";
            SqlCommand commandinsert = new SqlCommand(srtInsert, conn);

            commandinsert.Parameters.Add(new SqlParameter("@Description", adress.City.Description));
            commandinsert.Parameters.Add(new SqlParameter("@DtCadastro", adress.City.DtCadastro));

            return (int)commandinsert.ExecuteScalar();
        }
        public int InsertHotel(Hotel hotel)
        {
            string strInsert = "INSERT INTO Hotel(Name, DtCadastro, Valor, IdEndereco)" +
                "VALUES(@Name, @DtCadastro, @Valor, @IdEndereco); select cast(scope_identity() as int)";
            
            SqlCommand commandInsert = new SqlCommand(strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Name", hotel.Name));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", hotel.DtCadastro));
            commandInsert.Parameters.Add(new SqlParameter("@Valor", hotel.Valor));
            commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", InsertAdress(hotel.Address)));

            return (int)commandInsert.ExecuteScalar();
        }
        public bool Delete(string package)
        {
            bool status = false;
            try
            {
                string strDelete =  "Delete Packages FROM Packages Where Id = "+package ;

                SqlCommand commandDelete = new SqlCommand(strDelete, conn);

                commandDelete.ExecuteNonQuery();

                status = true;
            }
            catch (Exception) { status = false; throw; }
            finally { conn.Close(); }
            return status;
        }
        public List<Packages> FindAll()
        {
            List<Client> lstClient = GetListClient();
            List<Ticket> lstTicket = GetListTicket();
            List<Hotel>  lstHotel = GetListHotel();
            int idClient = 0, idTicket = 0, idHotel = 0; 
            List<Packages> lstPackages = new List<Packages>();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT p.Id, ");
            sb.Append("       p.DtCadastro, ");
            sb.Append("       p.Value, ");
            sb.Append("       p.IdClient, ");
            sb.Append("       p.IdTicket, ");
            sb.Append("       p.IdHotel ");
            sb.Append(" FROM Packages p ");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while(reader.Read())
            {
                idClient = Convert.ToInt32(reader["IdClient"]);
                idTicket = Convert.ToInt32(reader["IdTicket"]);
                idHotel = Convert.ToInt32(reader["IdHotel"]);
                Packages obPackage = new();

                obPackage.Id = (int)reader["Id"];
                obPackage.DtCadastro = (DateTime)reader["DtCadastro"];
                obPackage.Value = (decimal)reader["Value"];
                obPackage.Client = TakeClient(lstClient, idClient);
                obPackage.Ticket = TakeTicket(lstTicket, idTicket);
                obPackage.Hotel = TakeHotel(lstHotel, idHotel);

                lstPackages.Add(obPackage);
            }
            conn.Close();
            reader.Close();
            return lstPackages;
        }
        private List<Hotel> GetListHotel()
        {
            int idEnd = 0;
            List<Adress> lstAdress = GetListAdress();
            List<Hotel> lstHotel = new List<Hotel>();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT h.Id, ");
            sb.Append("       h.Name, ");
            sb.Append("       h.DtCadastro, ");
            sb.Append("       h.Valor, ");
            sb.Append("       h.IdEndereco");
            sb.Append(" FROM Hotel h");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while(reader.Read())
            {
                idEnd = Convert.ToInt32(reader["IdEndereco"]);
                Hotel obHotel = new Hotel();

                obHotel.Id = (int)reader["Id"];
                obHotel.Name = (string)reader["Name"];
                obHotel.DtCadastro = (DateTime)reader["DtCadastro"];
                obHotel.Valor = (decimal)reader["Valor"];
                obHotel.Address = TakeAdress(lstAdress, idEnd);

                lstHotel.Add(obHotel);
            }
            reader.Close();
            return lstHotel;
        }
        private List<Ticket> GetListTicket()
        {
            int IdOrigin = 0;
            int IdDestin = 0;
            int IdClient = 0;
            List<Adress> lstAdress = GetListAdress();
            List<Client> lstClient = GetListClient();
            List<Ticket> lstTicket = new();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT t.Id, ");
            sb.Append("       t.IdOrigin, ");
            sb.Append("       t.IdDestin, ");
            sb.Append("       t.IdClient, ");
            sb.Append("       t.Date, ");
            sb.Append("       t.Value ");
            sb.Append(" FROM Ticket t");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while(reader.Read())
            {
                Ticket obTicket = new Ticket();
                IdOrigin = (int)reader["IdOrigin"];
                IdDestin = (int)reader["IdDestin"];
                IdClient = (int)reader["IdClient"];

                obTicket.Id = (int)reader["Id"];
                obTicket.Origin = TakeAdress(lstAdress, IdOrigin);
                obTicket.Destin = TakeAdress(lstAdress, IdDestin);
                obTicket.Client = TakeClient(lstClient, IdClient);
                obTicket.Data = (DateTime)reader["Date"];
                obTicket.Value = (decimal)reader["Value"];

                lstTicket.Add(obTicket);
            }
            reader.Close();
            return lstTicket;
        }
        private List<Client> GetListClient()
        {
            int IdEnd = 0;
            List<Adress> lstAdress = GetListAdress();
            List<Client> lstClient = new List<Client>();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT c.Id, ");
            sb.Append("       c.Name, ");
            sb.Append("       c.Telephone, ");
            sb.Append("       c.IdEndereco, ");
            sb.Append("       c.DtCadastro ");
            sb.Append(" FROM Client c");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while(reader.Read())
            {
                Client obClient = new();
                IdEnd = (int)reader["IdEndereco"];

                obClient.Id = (int)reader["Id"];
                obClient.Name = (string)reader["Name"];
                obClient.Telefone = (string)reader["Telephone"];
                obClient.DtCadastro = (DateTime)reader["DtCadastro"];
                obClient.Address = TakeAdress(lstAdress, IdEnd);

                lstClient.Add(obClient);
            }
            reader.Close();
            return lstClient;
        }
        private List<Adress> GetListAdress()
        {
            int idCity = 0;
            List<City> lstCity = GetListCity();
            List<Adress> lstAdress = new List<Adress>();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT a.Id, ");
            sb.Append("       a.Street, ");
            sb.Append("       a.Number, ");
            sb.Append("       a.Burgh, ");
            sb.Append("       a.Cep, ");
            sb.Append("       a.Complement, ");
            sb.Append("       a.IdCity, ");
            sb.Append("       a.DtCadastro ");
            sb.Append(" FROM Adress a");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while (reader.Read())
            {
                Adress obAdress = new Adress();
                idCity = Convert.ToInt32(reader["IdCity"]);

                obAdress.Id = (int)reader["Id"];
                obAdress.Street = (string)reader["Street"];
                obAdress.Number = (int)reader["Number"];
                obAdress.Burgh = (string)reader["Burgh"];
                obAdress.CEP = (string)reader["Cep"];
                obAdress.Complement = (string)reader["Complement"];
                obAdress.City = TakeCity(lstCity, idCity);
                obAdress.DtCadastro = (DateTime)reader["DtCadastro"];

                lstAdress.Add(obAdress);
            }
            reader.Close();
            return lstAdress;
        }
        private List<City> GetListCity()
        {
            List<City> lstCity = new List<City>();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT c.Id, ");
            sb.Append("       c.Description, ");
            sb.Append("       c.DtCadastro ");
            sb.Append("  FROM City c");


            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while(reader.Read())
            {
                City obCity = new City();

                obCity.Id = (int)reader["Id"];
                obCity.Description = (string)reader["Description"];
                obCity.DtCadastro = (DateTime)reader["DtCadastro"];

                lstCity.Add(obCity);
            }
            reader.Close();
            return lstCity;
        }
        private Hotel TakeHotel(List<Hotel> lstHotel, int idHotel)
        {
            Hotel obHotel = new();
            foreach( Hotel hotel in lstHotel )
            {
                if(hotel.Id == idHotel)
                {
                    obHotel = hotel; break;
                }
            }
            return obHotel;
        }
        private Ticket TakeTicket (List<Ticket> lstTicket, int idTicket)
        {
            Ticket obTicket = new();
            foreach(Ticket ticket in lstTicket)
            {
                if(ticket.Id == idTicket)
                {
                    obTicket = ticket;break;
                }
            }
            return obTicket;
        }
        private Client TakeClient(List<Client> lstClient, int idClient)
        {
            Client obClient = new();
            foreach(Client cli in lstClient)
            {
                if(cli.Id == idClient)
                {
                    obClient = cli;break;
                }
            }
            return obClient;
        }
        private Adress TakeAdress(List<Adress> lstAdress, int idAdress)
        {
            Adress obAdress = new();
            foreach(Adress ad in lstAdress) 
            {
                if(ad.Id == idAdress)
                {
                    obAdress = ad; break;
                }
            }
            return obAdress;
        }
        private City TakeCity (List<City> lstCity, int idCity)
        {
            City obCity = new City();
            foreach(City c in lstCity)
            {
                if(c.Id == idCity)
                {
                    obCity = c; break;
                }
            }
            return obCity;
        }
    }
}
