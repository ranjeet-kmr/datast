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
    public partial class sqlcommandbuilder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGetStudent_Click(object sender, EventArgs e)
        {
            string connectionString =
            ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);



            string selectQuery = "Select * from tblStudents where ID = " +
                txtStudentID.Text;
            SqlDataAdapter dataAdapter = new SqlDataAdapter(selectQuery, connection);

            DataSet dataSet = new DataSet();
            dataAdapter.Fill(dataSet, "Students");

            // Store DataSet and the select query in ViewState, so they can be used
            // later to generate the T-SQL commands using SqlCommandBuilder class
            ViewState["DATASET"] = dataSet;
            ViewState["SELECT_QUERY"] = selectQuery;

            if (dataSet.Tables["Students"].Rows.Count > 0)
            {
                DataRow dataRow = dataSet.Tables["Students"].Rows[0];
                txtStudentName.Text = dataRow["Name"].ToString();
                txtTotalMarks.Text = dataRow["TotalMarks"].ToString();
                ddlGender.SelectedValue = dataRow["Gender"].ToString();
                lblStatus.Text = "";
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "No record with ID = " + txtStudentID.Text;
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            string connectionString =
            ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            SqlConnection con = new SqlConnection(connectionString);

            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            // Retrieve the Select query from ViewState and use it to build
            // SqlCommand command object, which will then be set as the 
            // SelectCommand of the SqlDataAdapter object
            dataAdapter.SelectCommand =
                new SqlCommand((string)ViewState["SELECT_QUERY"], con);

            // Associate SqlDataAdapter object with SqlCommandBuilder. At this point
            // SqlCommandBuilder should generate T-SQL statements automatically
            SqlCommandBuilder builder = new SqlCommandBuilder(dataAdapter);

            DataSet ds = (DataSet)ViewState["DATASET"];
            DataRow dr = ds.Tables["Students"].Rows[0];
            dr["Name"] = txtStudentName.Text;
            dr["Gender"] = ddlGender.SelectedValue;
            dr["TotalMarks"] = txtTotalMarks.Text;
            dr["Id"] = txtStudentID.Text;

            int rowsUpdated = dataAdapter.Update(ds, "Students");
            if (rowsUpdated == 0)
            {
                lblStatus.ForeColor = System.Drawing.Color.Red;
                lblStatus.Text = "No rows updated";
            }
            else
            {
                lblStatus.ForeColor = System.Drawing.Color.Green;
                lblStatus.Text = rowsUpdated.ToString() + " row(s) updated";
            }
        }
    }
    
}