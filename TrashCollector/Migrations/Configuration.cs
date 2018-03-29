namespace TrashCollector.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.VisualBasic.FileIO;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;
    using TrashCollector.Models;


    internal sealed class Configuration : DbMigrationsConfiguration<TrashCollector.Models.ApplicationDbContext>
    {

        string filePath = @"C:\Users\Zack Fountain\Dropbox\_devCodeCamp\Assignments\week10-2-TrashCollector\Code\Trash-Collector\TrashCollector\Domain\SeedData\states.csv";

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TrashCollector.Models.ApplicationDbContext context)
        {
            //Seed States table with all US States
            bool fileExists = File.Exists(filePath);
            if (fileExists)
            {
                List<State> states = new List<State>();
                using (TextFieldParser parser = new TextFieldParser(filePath))
                {
                    parser.TextFieldType = FieldType.Delimited;
                    parser.SetDelimiters(",");
                    State state;
                    while (!parser.EndOfData)
                    {
                        string[] fields = parser.ReadFields();
                        if (fields.Any(x => x.Length == 0))
                        {
                            Console.WriteLine("We found an empty value in your CSV. Please check your file and try again.\nPress any key to return to main menu.");
                            Console.ReadKey(true);
                        }
                        state = new State();
                        state.Name = fields[0];
                        state.Abbreviation = fields[1];
                        states.Add(state);
                    }
                }
                context.States.AddOrUpdate(c => c.Abbreviation, states.ToArray());

                //Seed users
                //var passwordHash = new PasswordHasher();
                //string password = passwordHash.HashPassword("Password123!");
                //context.Users.AddOrUpdate(u => u.UserName,
                //    new ApplicationUser
                //    {
                //        UserName = "Aaron@zf-trash.com",
                //        PasswordHash = password,
                //        PhoneNumber = "414123456"

                //    });

                //context.Users.AddOrUpdate(u => u.UserName,
                //    new ApplicationUser
                //    {
                //        UserName = "Bob@zf-trash.com",
                //        PasswordHash = password,
                //        PhoneNumber = "414123456"

                //    });

                //if (!context.Users.Any(u => u.UserName == "StarbucksGuy"))
                //{
                //    var store = new UserStore<ApplicationUser>(context);
                //    var manager = new UserManager<ApplicationUser>(store);
                //    var user = new ApplicationUser { UserName = "StarbucksGuy" };

                //    manager.Create(user, "Password123!");
                //    manager.AddToRole(user.Id, "Employee");
                //}
            }




            //Assembly assembly = Assembly.GetExecutingAssembly();
            //string resourceName = "TrashCollector.Domain.SeedData.states.csv";
            //using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            //{
            //    using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            //    {
            //        CsvReader csvReader = new CsvReader(reader);
            //        //csvReader.Configuration.WillThrowOnMissingField = false;
            //        var states = csvReader.GetRecords<State>().ToArray();
            //        context.States.AddOrUpdate(c => c.Abbreviation, states);
            //    }
            //}

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }

    }
}
