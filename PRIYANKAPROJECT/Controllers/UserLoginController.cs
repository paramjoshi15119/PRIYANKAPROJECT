using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PRIYANKAPROJECT.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Security;

namespace PRIYANKAPROJECT.Controllers
{
    public class UserLoginController : Controller
    {
        
        // GET: /UserLogin/

        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(Users e)
        {
            String SqlCon = ConfigurationManager.ConnectionStrings["ConnDB"].ConnectionString;
            SqlConnection con = new SqlConnection(SqlCon);
            string SqlQuery = "select Email,Password from Users where Email=@Email and Password=@Password";
            string status;
            con.Open();
            SqlCommand cmd = new SqlCommand(SqlQuery,con);;
            cmd.Parameters.AddWithValue("@Email", e.Email);
            cmd.Parameters.AddWithValue("@Password", e.Password);
            SqlDataReader sdr = cmd.ExecuteReader();
            if (sdr.Read())
            {
                Session["Email"] = e.Email.ToString();
                return RedirectToAction("Welcome");
            }
            else
            {
                ViewData["Message"] = "User Login Details Failed!!";
            }
            if (e.Email.ToString() != null)
            {
                Session["Email"] = e.Email.ToString();
                status = "1";
            }
            else
            {
                status = "3";
            }
            con.Close();
            return View();
            //return new JsonResult { Data = new { status = status } };
        }
       
        [HttpGet]
        public ActionResult Welcome()
        {
            Users user = new Users();
            DataSet ds = new DataSet();

            using (SqlConnection con = new SqlConnection("Data Source=LAPTOP-DEIVKI9H\\SQLEXPRESS;Integrated Security=true;Initial Catalog=OfficeManagement"))
            {
                using (SqlCommand cmd = new SqlCommand("select * from sp_GetUserDetails", con))
                {
                    cmd.Parameters.AddWithValue("@status", "Get");
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Email", SqlDbType.VarChar, 30).Value = Session["Email"].ToString();
                    con.Open();
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);
                    List<Users> userlist = new List<Users>();
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        Users uobj = new Users();
                        uobj.ID = Convert.ToInt32(ds.Tables[0].Rows[i]["ID"].ToString());
                        uobj.Name = ds.Tables[0].Rows[i]["Name"].ToString();
                        uobj.Email = ds.Tables[0].Rows[i]["Email"].ToString();
                        uobj.Password = ds.Tables[0].Rows[i]["Password"].ToString();
                        userlist.Add(uobj);

                    }
                    user.Userinfo = userlist;
                }
                con.Close();

            }
            return View(user);
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon();
            return RedirectToAction("Index", "UserLogin");
        }
    }
}
