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
    }
}
