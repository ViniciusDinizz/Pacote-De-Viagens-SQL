using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    public class CityServices
    {
        readonly string strComm = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public CityServices()
        {
            conn = new SqlConnection(strComm);
            conn.Open();
        }

        public bool Insert(City city)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO City(Description, DtCadastro) " +
                    "VALUES (@Description, @DtCadastro)";

                SqlCommand commandInsert = new SqlCommand(strInsert, conn);

                commandInsert.Parameters.Add(new SqlParameter("@Description", city.Description));
                commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", city.DtCadastro));

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

        public bool Delete(string city)
        {
            bool status = false;
            try
            {
                string strDelete = "DELETE FROM City WHERE Id =" + city;
                Console.WriteLine(strDelete);
                SqlCommand commandDelete = new SqlCommand(strDelete, conn);

                commandDelete.ExecuteNonQuery ();
                status = true;
            }catch (Exception)
            {
                status = false; 
                throw;
            }finally { conn.Close(); }
            return status;
        }

        public List<City> FindAll()
        {
            List<City> Cities = new List<City>();

            StringBuilder sb = new StringBuilder();

            sb.Append(" SELECT c.Id, ");
            sb.Append("     c.Description, ");
            sb.Append("     c.DtCadastro ");
            sb.Append("  FROM City c");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while(dr.Read())
            {
                City city = new City();

                city.Id = (int)dr["Id"];
                city.Description = (string)dr["Description"];
                city.DtCadastro = (DateTime)dr["DtCadastro"];

                Cities.Add(city);
            }
            return Cities;
        }
    }
}
