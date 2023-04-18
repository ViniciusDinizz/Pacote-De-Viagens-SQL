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
            string srtInsert = "Insert into City (Description) VALUES (@Description) select cast(scope_identity() as int)";
            SqlCommand commandinsert = new SqlCommand(srtInsert, conn);
            commandinsert.Parameters.Add(new SqlParameter("@Description", adress.City.Description));

            return (int)commandinsert.ExecuteScalar();
        }

    
    }
}
