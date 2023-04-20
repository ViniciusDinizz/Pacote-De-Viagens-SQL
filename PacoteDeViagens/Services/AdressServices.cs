using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PacoteDeViagens.Models;

namespace PacoteDeViagens.Services
{
    public class AdressServices
    {
        readonly string strConn = @"Server=(localdb)\MSSQLLocalDB;Integrated Security=true;AttachDbFileName=C:\Users\5by5\Desktop\banco\trip.mdf";
        readonly SqlConnection conn;

        public AdressServices()
        {
            conn = new SqlConnection(strConn);
            conn.Open();
        }

        public bool Insert(Adress adress)
        {
            bool status = false;
            try
            {
                string strInsert = "INSERT INTO Adress (Street, Number, Burgh, Cep, Complement, IdCity, DtCadastro) " +
                    "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro)";

                SqlCommand commandinsert = new SqlCommand(strInsert, conn);

                commandinsert.Parameters.Add(new SqlParameter("@Street", adress.Street));
                commandinsert.Parameters.Add(new SqlParameter("@Number", adress.Number));
                commandinsert.Parameters.Add(new SqlParameter("@Burgh", adress.Burgh));
                commandinsert.Parameters.Add(new SqlParameter("@Cep", adress.CEP));
                commandinsert.Parameters.Add(new SqlParameter("@Complement", adress.Complement));
                commandinsert.Parameters.Add(new SqlParameter("@IdCity", InsertCity(adress)));
                commandinsert.Parameters.Add(new SqlParameter("@DtCadastro", adress.DtCadastro));

                commandinsert.ExecuteNonQuery();
                status = true;
            }catch (Exception) 
            {
                status = false;
                throw;
            }
            finally { conn.Close(); }

            return status;
        }

        private int InsertCity (Adress adress)
        {
            string srtInsert = "INSERT  INTO City(Description, DtCadastro) VALUES (@Description, @DtCadastro); select cast(scope_identity() as int)";
            SqlCommand commandinsert = new SqlCommand(srtInsert, conn);

            commandinsert.Parameters.Add(new SqlParameter("@Description", adress.City.Description));
            commandinsert.Parameters.Add(new SqlParameter("@DtCadastro", adress.City.DtCadastro));

            return (int)commandinsert.ExecuteScalar();
        }

        public bool Delete(string adress)
        {
            bool status = false;
            try
            {
                string strDelete = "DELETE FROM Adress Where Id = " + adress;

                SqlCommand commanddelete = new SqlCommand(strDelete, conn);

                commanddelete.ExecuteNonQuery();

                status = true;
            }
            catch (Exception) { status = false; throw; }
            finally { conn.Close(); }

            return status;
        }

        public List<Adress> FindAll() 
        {
            List<Adress> Adress = new();

            StringBuilder sb = new StringBuilder();

            sb.Append("select a.Id, ");
            sb.Append("     a.Street, ");
            sb.Append("     a.Number, ");
            sb.Append("     a.Burgh, ");
            sb.Append("     a.Cep, ");
            sb.Append("     a.Complement, ");
            sb.Append("     c.Description AS City, c.DtCadastro");
            sb.Append(" FROM Adress a, City c ");
            sb.Append(" WHERE a.IdCity = c.Id");

            SqlCommand commandSelect = new(sb.ToString(), conn);
            SqlDataReader dr = commandSelect.ExecuteReader();

            while (dr.Read())
            {
                Adress adress = new Adress();

                adress.Id = (int)dr["Id"];
                adress.Street = (string)dr["Street"];
                adress.Number = (int)dr["Number"];
                adress.Burgh = (string)dr["Burgh"];
                adress.CEP = (string)dr["Cep"];
                adress.Complement = (string)dr["Complement"];
                adress.City = new City() { Description = (string)dr["City"], DtCadastro = (DateTime)dr["DtCadastro"] };

                Adress.Add(adress);
            }
            return Adress;
        }
    }
}
