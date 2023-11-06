using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRIYANKAPROJECT.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace PRIYANKAPROJECT.Controllers
{
    public class UsersController : Controller
    {
        public string value = "";

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(Users e)
        {
            if (Request.HttpMethod == "POST")
            {
                Users er = new Users();
                using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-DEIVKI9H\\SQLEXPRESS;Integrated Security=true;Initial Catalog=OfficeManagement"))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_UserDetails", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Name", e.Name);
                        cmd.Parameters.AddWithValue("@Email", e.Email);
                        cmd.Parameters.AddWithValue("@Password", e.Password);
                        cmd.Parameters.AddWithValue("@status", "INSERT");
                        con.Open();
                        ViewData["result"] = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            return View();
        }
        
    }
}
