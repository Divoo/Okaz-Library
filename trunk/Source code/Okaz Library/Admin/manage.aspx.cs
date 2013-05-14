﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Okaz_Library.Admin
{
    public partial class Manage : System.Web.UI.Page
    {
        AccessManager ServerManager;
        int RequestID , BookID, UserID;
        string status , name;
        protected void Page_Load(object sender, EventArgs e)
        {
             try
             {
                 Label1.Text = "اهلا "+Session["Name"].ToString();
                 ServerManager = (AccessManager)Session["AccessManager"];
             }

             catch (NullReferenceException)
             {
                 Response.Redirect("Login.aspx");
             }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                RequestID = int.Parse(TextBox1.Text);
                status = ServerManager.getUniqueData("select [status] from request where R_ID=" + RequestID.ToString() + "");
                BookID = int.Parse(ServerManager.getUniqueData("select book_ID from request where R_ID=" + RequestID.ToString() + ""));
                UserID = int.Parse(ServerManager.getUniqueData("select U_ID from request where R_ID=" + RequestID.ToString() + ""));
                name = ServerManager.getUniqueData("select Name from request where R_ID=" + RequestID.ToString() + "");

                if (status == "borrow")
                {
                    ServerManager.Query("UPDATE book SET Available = 0 WHERE book_ID = " + BookID.ToString() + "");
                    ServerManager.Query("UPDATE request SET [status] = 'count' WHERE R_ID = " + RequestID.ToString()+ "");
                    ServerManager.Query("UPDATE request SET Rdate = GETDATE() WHERE R_ID = " + RequestID.ToString() + "");
                }
                else if (status == "loan")
                {
                    ServerManager.Query("UPDATE book SET Available = 1 WHERE book_ID = " + BookID + "");
                    ServerManager.Query("DELETE FROM request WHERE R_ID = " + RequestID + "");
                }

                ServerManager.Query("insert into User_book values(" + BookID.ToString() + "," + UserID.ToString() + ",'" + status + "',GETDATE(),'" + name + "')");
                

                Label2.Text = "تمت العملية بنجاح";

            }

            catch (Exception)
            {
                Label2.Text = "الرجاء التحقق من رقم الطلب";
            }

            TextBox1.Text = "";
        }

        void resetall()
        {
            TextBox1.Text = "";
            Label2.Text = "";
        }


    }
}