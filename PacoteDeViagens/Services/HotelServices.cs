using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    public class HotelServices
    {
        readonly string strCoom = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public HotelServices()
        {
            conn = new SqlConnection(strCoom);
            conn.Open();
        }

        public bool Insert(Hotel hotel)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO Hotel (Name, DtCadastro, Valor, IdEndereco) " +
                    "VALUES (@Name, @DtCadastro, @Valor, @IdEndereco)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Name", hotel.Name));
                commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", hotel.DtCadastro));
                commandInsert.Parameters.Add(new SqlParameter("@Valor", hotel.Valor));
                commandInsert.Parameters.Add(new SqlParameter("@IdEndereco", InsertEndereco(hotel)));

                commandInsert.ExecuteNonQuery();

                status = true;
                
            } catch (Exception)
            {
                status = false;
                throw;
            } finally { conn.Close(); }
            return status;
        }
        private int InsertEndereco(Hotel hotel)
        {
            string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro); select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand( strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", hotel.Address.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", hotel.Address.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", hotel.Address.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", hotel.Address.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", hotel.Address.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(hotel.Address.City)));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", hotel.Address.DtCadastro));

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
        public bool Delete(string hotel)
        {
            bool status = false;
            try
            {
                string strDelete = "DELETE FROM Hotel WHERE Id = " + hotel;

                SqlCommand commanddelete = new SqlCommand(strDelete, conn);

                commanddelete.ExecuteNonQuery();

                status = true;
            }catch (Exception ex) { status = false; throw; }
            finally { conn.Close(); }
            return status;
        }
        public List<Hotel> FindAll()
        {
            int idEnd = 0;
            List<City> lstCity = GetListCity();
            List<Adress> lstAdress = GetListAdress(lstCity);
            List<Hotel> lstHotel = new();
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT h.Id,");
            sb.Append("       h.Name, ");
            sb.Append("       h.DtCadastro, ");
            sb.Append("       h.Valor, ");
            sb.Append("       h.IdEndereco ");
            sb.Append(" FROM Hotel h ");

            SqlCommand commandSelect = new SqlCommand(sb.ToString(), conn);
            SqlDataReader reader = commandSelect.ExecuteReader();

            while(reader.Read())
            {
                idEnd = (int)reader["IdEndereco"];
                Hotel obHotel = new Hotel();

                obHotel.Id = (int)reader["Id"];
                obHotel.Name = (string)reader["Name"];
                obHotel.DtCadastro = (DateTime)reader["DtCadastro"];
                obHotel.Valor = (decimal)reader["Valor"];
                obHotel.Address = TakeAdress(lstAdress, idEnd);

                lstHotel.Add(obHotel);
            }
            return lstHotel;
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
            while (reader.Read())
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
        private Adress TakeAdress(List<Adress> lstAdress, int idAdress)
        {
            Adress obAdress = new Adress();
            foreach(Adress adress in lstAdress)
            {
                if(adress.Id == idAdress)
                {
                    obAdress = adress;
                }
            }
            return obAdress;
        }
        private City TakeCity (List<City> lstCity, int idCity)
        {
            City obCity = new City();
            foreach(City city in lstCity)
            {
                if(city.Id == idCity)
                {
                    obCity = city;
                }
            }
            return obCity;
        }
    }
}
