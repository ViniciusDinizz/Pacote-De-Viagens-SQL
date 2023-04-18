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
                "VALUES (@Street, @Number, @Burgh, @Cep, @Complement, @IdCity, @DtCadastro) select cast(scope_identity() as int)";

            SqlCommand commandInsert = new SqlCommand( strInsert, conn);

            commandInsert.Parameters.Add(new SqlParameter("@Street", hotel.Address.Street));
            commandInsert.Parameters.Add(new SqlParameter("@Number", hotel.Address.Number));
            commandInsert.Parameters.Add(new SqlParameter("@Burgh", hotel.Address.Burgh));
            commandInsert.Parameters.Add(new SqlParameter("@Cep", hotel.Address.CEP));
            commandInsert.Parameters.Add(new SqlParameter("@Complement", hotel.Address.Complement));
            commandInsert.Parameters.Add(new SqlParameter("@IdCity", hotel.Address.City));
            commandInsert.Parameters.Add(new SqlParameter("@DtCadastro", hotel.Address.DtCadastro));

            return (int)commandInsert.ExecuteScalar();
        }
    }
}
