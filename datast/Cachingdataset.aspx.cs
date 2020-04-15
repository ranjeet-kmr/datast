using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace datast
{
    public partial class Cachingdataset : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        { 
            if(Cache["Data"] == null)
            { 
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    SqlDataAdapter da = new SqlDataAdapter("select * from sales", con);

                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    Cache["Data"] = ds;
                    gvdetails.DataSource = ds;
                    gvdetails.DataBind();

                    Lblmsg.Text = "Data loadaed from database";
                }
            }
            else
            {
                gvdetails.DataSource = (DataSet)Cache["Data"];
                gvdetails.DataBind();

                Lblmsg.Text = "Data loadaed from Cache ";
            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            if(Cache["Data"] != null)
            {
                Cache.Remove("Data");
                Lblmsg.Text = "Data cleared from Cache ";
            }
            else
            {
                Lblmsg.Text = "There is no Data in Cache ";
            }
        }
    }
}