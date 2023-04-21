using PacoteDeViagens.Controller;
using PacoteDeViagens.Models;
using PacoteDeViagens.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("MVC - Agencia de turismo");

        /*City city = new()
        {
            Id = 1,
            Description = "Araraquara",
            DtCadastro = DateTime.Now,
        };
        new CityController().Insert(city);*/

        /*Adress adress = new Adress()
        {
            Street = "Rua Hermanoteu",
            Number = 2,
            Burgh = "Santa Cruz",
            CEP = "15990-365",
            City = new City() { Description = "Araraquara", DtCadastro= DateTime.Now },
            Complement = "Residencia",
            DtCadastro = DateTime.Now,
        };
        new AdressControler().Insert(adress);*/

        /*Ticket ticket = new Ticket()
        {
            Origin = new Adress() { Street = "Rua Napoleão", Number = 64, Burgh = "Bairro Sapucaí", CEP = "15990000", Complement = "Hotel", City =  new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now  },
            Destin = new Adress() { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
            Client = new Client() { Name = "Vinicius", Telefone = "169963265", Address = new Adress() { Street = "Rua Napoleão", Number = 64, Burgh = "Bairro Sapucaí", CEP = "15990000", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
            Data = DateTime.Now,
            Value = 2520,
        };
        new TicketController().Insert(ticket);*/

        /*Client client = new Client()
        {
            Name = "Vinicius", 
            Telefone = "165569823",
            Address = new Adress() { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
            DtCadastro = DateTime.Now,
        };
        new ClientServices().Insert(client);*/

        /*Hotel hotel = new Hotel()
        {
            Name = "Pouso Novo",
            Address = new Adress() { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
            DtCadastro = DateTime.Now,
            Valor = 300,
        };
        new HotelServices().Insert(hotel);*/

        /* Packages packages = new Packages()
         {
             Hotel = new Hotel()
             {
                 Name = "Pouso Novo",
                 Address = new Adress()
                 { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
                 DtCadastro = DateTime.Now,
                 Valor = 300
             },
             Ticket = new Ticket()
             {
                 Origin = new Adress() { Street = "Rua Hermanoteu", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Rio de Janeiro", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
                 Destin = new Adress() { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
                 Client = new Client() { Name = "Renan", Telefone = "992242656", Address = new Adress() { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
                 Data = DateTime.Now,
                 Value = 400
             },
             DtCadastro = DateTime.Now,
             Value = 1200,
             Client = new Client() { Name = "Renan", Telefone = "992242656", Address = new Adress() { Street = "Rua Moscou", Number = 66, Burgh = "Bairro Saguão", CEP = "15990556", Complement = "Hotel", City = new City() { Description = "Araraquara", DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now }, DtCadastro = DateTime.Now },
         };
         new PackageController().Insert(packages);*/

        //new TicketController().Delete("1");
        //new PackageController().Delete("3");
        //new HotelController().Delete("1");
        //new ClientController().Delete(1);
        //new AdressControler().Delete(1);
        //new CityController().Delete(2);

        new PackageController().FindAll().ForEach(Console.WriteLine);
    }
}