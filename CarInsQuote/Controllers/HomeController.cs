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
                                    DateTime dateOfBirth, int carYear, string carMake, string carModel,
                                    string dUI, int speedingTicket, string coverageType, double quote)
        {
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(emailAddress) ||
                carYear <= 1885 || string.IsNullOrEmpty(carModel) || speedingTicket < 0)
                {
                return View("~/Views/Shared/error.cshtml");
                }
            else
            {

                //==========================================================================================//
                //QUESTIONS FOR INSTRUCTORS// 
                //1) Would it be better to encapsulate all of the quote-variable-calculating code 
                //   into a separate method someplace else and then called in the home controller? 
                //
                //2) Can the code function as-written without making them into separate-but-linked 
                //   methods like the HTML/Javascript Online Pizza exercise (see github 'pizza_functions' link in e-mail for example)?
                //   The code as it is now does not have anything calling it, simply lines of calculations within 
                //   the else-statement. Does it need () methods? 
                // 
                //3) Similar to #2, I need this if/else statement to function as follows -> 
                // - Customer enters information 
                // - If there are any issues, go to the Error Screen 
                // - If all is well, calculate quote variable AND post all user input + quote calculation to DB. 
                //
                // Will this code I've written do this, or is it on the way to doing this? If not, what am I missing, 
                // or how can I structure it to do so? What do I need to write/add to link it to the add/POST statement 
                // following the quote-value calculation? 

                {

                    //function ageCheck()
                    var date1 = DateTime.Now.Year;
                    var DOB = dateOfBirth.Year;
                    var baseCost = 50;
                    var ageFactor = 0;
                    

                    var dateCheck = date1 - DOB;
                    if (dateCheck < 18)
                    {
                        ageFactor = 100;
                    }
                    else if (dateCheck > 18 && dateCheck < 25)
                    {
                        ageFactor = 25; 
                    }
                    else if (dateCheck > 100)
                    {
                        ageFactor = 100; 
                    }
   

                    //function carYearCheck()
                    var carYearFactor = 0; 
                    if (carYear < 2000 || carYear > 2015)
                    {
                        carYearFactor = 25;
                    }
                    else
                    {
                        carYearFactor = 0; 
                    }


                    //function carMakeCheck()
                    var isPorsche = carMake.ToLower();
                    var carMakeFactor = 0;
                    if (isPorsche == "porsche")
                    {
                        carMakeFactor = 25; 
                    }
                    else
                    {
                        carMakeFactor = 0;
                    }


                    //function carModelCheck()
                    var isCarerra = carModel.ToLower();
                    var carModelFactor = 0;
                    if (carModel == "carrera 911" || carModel == "carrera911" || carModel == "911 carrera" || carModel == "911carrera")
                    {
                        carModelFactor = 25; 
                    }
                    else
                    {
                        carModelFactor = 0; 
                    }

                   
                    //function speedingTicketCheck() 
                    var speedingTicketFactor = speedingTicket * 10;

                    //TOTAL BEFORE PERCENTAGES 
                    double runningTotal = baseCost + ageFactor + carYearFactor + carMakeFactor + carModelFactor + speedingTicketFactor;

                    //function dUICheck(runningTotal)
                    double dUIFactor = 0; 
                    if (dUI == "Yes")
                    {
                        dUIFactor = runningTotal * 0.25; 
                    }
                    else
                    {
                        dUIFactor = 0; 
                    }

                    double runningTotal2 = runningTotal + dUIFactor;

                    //function coverageCheck(runningTotal2)
                    double coverageFactor = 0; 
                    if (coverageType == "Full")
                    {
                        coverageFactor = runningTotal2 * 0.50;
                    }

                    quote = runningTotal2 + coverageFactor;

                   
                }
//==========================================================================================//
//CODE ABOVE REQUIRES LINK TO INSERT COMMAND BELOW 

                string queryString = @"INSERT INTO Insurees (FirstName, LastName, EmailAddress, DateOfBirth,
                                                             CarYear, CarMake, CarModel, DUI, SpeedingTicket, 
                                                             CoverageType, Quote) VALUES 
                                                            (@FirstName, @LastName, @EmailAddress, @DateOfBirth,
                                                             @CarYear, @CarMake, @CarModel, @DUI, @SpeedingTicket, 
                                                             @CoverageType, @Quote)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    command.Parameters.Add("@FirstName", SqlDbType.VarChar);
                    command.Parameters.Add("@LastName", SqlDbType.VarChar);
                    command.Parameters.Add("@EmailAddress", SqlDbType.VarChar);
                    command.Parameters.Add("@DateOfBirth", SqlDbType.DateTime);
                    command.Parameters.Add("@CarYear", SqlDbType.Int);
                    command.Parameters.Add("@CarMake", SqlDbType.VarChar);
                    command.Parameters.Add("@CarModel", SqlDbType.VarChar);
                    command.Parameters.Add("@DUI", SqlDbType.VarChar);
                    command.Parameters.Add("@SpeedingTicket", SqlDbType.Int);
                    command.Parameters.Add("@CoverageType", SqlDbType.VarChar);
                    command.Parameters.Add("@Quote", SqlDbType.Decimal);

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
                    command.Parameters["@Quote"].Value = quote; 

                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                }
                return View("QuotationView"); 
            }
        }
        public ActionResult Admin()
        {
            string queryString = @"SELECT Id, FirstName, LastName, EmailAddress, DateOfBirth,
                                   CarYear, CarMake, CarModel, DUI, SpeedingTicket, CoverageType, Quote FROM Insurees";
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
                    customer.DateOfBirth = Convert.ToDateTime(reader["DateOfBirth"]);
                    customer.CarYear = Convert.ToInt32(reader["CarYear"]);
                    customer.CarMake = reader["CarMake"].ToString();
                    customer.CarModel = reader["CarModel"].ToString();
                    customer.DUI = reader["DUI"].ToString();
                    customer.SpeedingTicket = Convert.ToInt32(reader["SpeedingTicket"]);
                    customer.CoverageType = reader["CoverageType"].ToString();
                    customer.Quote = Convert.ToDouble(reader["Quote"]);
                    quotelist.Add(customer);
                }
            }
                return View(quotelist);
        }
       
    }
}