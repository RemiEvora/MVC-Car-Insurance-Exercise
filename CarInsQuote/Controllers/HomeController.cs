using CarInsQuote.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarInsQuote.Controllers
{
    public class HomeController : Controller
    {
        private readonly string connectionString = @"Data Source=YUI-LT14\TECHACSQLEXPRESS;Initial Catalog=CarInsurance;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CalculateQuote(string firstName, string lastName, string emailAddress,
                                    int dateOfBirth, int carYear, string carMake, string carModel,
                                    string dUI, int speedingTicket, string coverageType)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) ||
                dateOfBirth <= 1900 || carYear <= 1885 || string.IsNullOrEmpty(carModel) || speedingTicket < 0)
                {
                return View("~/Views/Shared/error.cshtml");
                }
            else
            {
                string queryString = @"INSERT INTO Insurees (FirstName, LastName, EmailAddress, DateOfBirth,
                                                             CarYear, CarMake, CarModel, DUI, SpeedingTicket, 
                                                             CoverageType) VALUES 
                                                            (@FirstName, @LastName, @EmailAddress, @DateOfBirth,
                                                             @CarYear, @CarMake, @CarModel, @DUI, @SpeedingTicket, 
                                                             @CoverageType)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    command.Parameters.Add("@DateOfBirth", SqlDbType.Int);
                    command.Parameters.Add("@CarYear", SqlDbType.Int);
                    command.Parameters.Add("@CarMake", SqlDbType.VarChar);
                    command.Parameters.Add("@CarModel", SqlDbType.VarChar);
                    command.Parameters.Add("@DUI", SqlDbType.VarChar);
                    command.Parameters.Add("@SpeedingTicket", SqlDbType.Int);
                    command.Parameters.Add("@CoverageType", SqlDbType.VarChar);

                    command.Parameters["@FirstName"].Value = firstName;
                    command.Parameters["@LastName"].Value = lastName;
                    command.Parameters["@EmailAddress"].Value = emailAddress;
                    command.Parameters["@DateOfBirth"].Value = dateOfBirth;
                    command.Parameters["@CarYear"].Value = carYear;
                    command.Parameters["@CarMake"].Value = carMake;
                    command.Parameters["@CarModel"].Value = carModel;
                    command.Parameters["@DUI"].Value = dUI;
                    command.Parameters["@SpeedingTicket"].Value = speedingTicket;
                    command.Parameters["@CoverageType"].Value = coverageType;

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return View("QuotationView"); //Not working at the moment due to not having any Quote value entered.
            }
        }
        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, DateOfBirth,
                                   CarYear, CarMake, CarModel, DUI, SpeedingTicket, CoverageType FROM Insurees";
            List<Insuree> quotelist = new List<Insuree>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryString, connection);

                connection.Open();

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var customer = new Insuree();
                    customer.Id = Convert.ToInt32(reader["Id"]);
                    customer.FirstName = reader["FirstName"].ToString();
                    customer.LastName = reader["LastName"].ToString();
                    customer.EmailAddress = reader["EmailAddress"].ToString();
                    customer.DateOfBirth = Convert.ToInt32(reader["DateOfBirth"]);
                    customer.CarYear = Convert.ToInt32(reader["CarYear"]);
                    customer.CarMake = reader["CarMake"].ToString();
                    customer.CarModel = reader["CarModel"].ToString();
                    customer.DUI = reader["DUI"].ToString();
                    customer.SpeedingTicket = Convert.ToInt32(reader["SpeedingTicket"]);
                    customer.CoverageType = reader["CoverageType"].ToString();
                    customer.Quote = Convert.ToInt32(reader["Quote"]);
                    quotelist.Add(customer);
                }
            }
                return View(quotelist);
        }
        //USE PIZZA CALCULATION ON GITHUB, PUT IN MODELS FOLDER! 
        //Create calculate()/CalculateQuote() method, link to Index.cshtml 
    }
}