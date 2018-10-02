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
                string connectionString = @"Data Source=YUI-LT14\TECHACSQLEXPRESS;Initial Catalog=CarInsurance;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

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
                return View("QuotationView");
            }
            //USE PIZZA CALCULATION ON GITHUB, PUT IN MODELS FOLDER! 
            //Create calculate()/CalculateQuote() method, link to Index.cshtml 
            



                //Revise SQL Table to include Grand Total field, for Admin usage 
        }
        
    }
}