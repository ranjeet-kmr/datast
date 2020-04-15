using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;


namespace datast
{
    public partial class sqlbulkcpy : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            string cs = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using(var con = new SqlConnection(cs))
            {
                DataSet ds = new DataSet();
                ds.ReadXml(Server.MapPath("~/Data.xml"));

                DataTable dtDep = ds.Tables["Department"];
                DataTable dtEmp = ds.Tables["Employee"];

                con.Open();
                using(SqlBulkCopy sb = new SqlBulkCopy(con))
                {
                    sb.DestinationTableName = "Departments";
                    sb.ColumnMappings.Add("ID", "ID");
                    sb.ColumnMappings.Add("Name", "Name");
                    sb.ColumnMappings.Add("Location", "Location");
                    sb.WriteToServer(dtDep);

                }

                using (SqlBulkCopy sb = new SqlBulkCopy(con))
                {
                    sb.DestinationTableName = "Employeess";
                    sb.ColumnMappings.Add("ID", "ID");
                    sb.ColumnMappings.Add("Name", "Name");
                    sb.ColumnMappings.Add("Gender", "Gender");
                    sb.ColumnMappings.Add("DepartmentId", "DepartmentId");
                    sb.WriteToServer(dtEmp);

                }

            }
        }
    }
}