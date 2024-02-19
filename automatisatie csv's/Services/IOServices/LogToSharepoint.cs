using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services.IOServices
{
    public class LogToSharepoint
    {

        private static ClientContext context;
        private static LogToSharepoint _instance;

        public static LogToSharepoint CreateObject()
        {
            if (_instance == null)
            {
                _instance = new LogToSharepoint();
            }
            return _instance;
        }
         private  LogToSharepoint()
        {
            // Replace these with your actual SharePoint site and credentials
            string siteUrl = "https://yoursite.sharepoint.com/";
            string userName = "yourusername";
            string password = "yourpassword";

            // Securely store the password
            var securePassword = new System.Security.SecureString();
            foreach (char c in password)
            {
                securePassword.AppendChar(c);
            }



            // Connect to the SharePoint site
            ClientContext context = new ClientContext(siteUrl);
           // context.Credentials = new SharePointOnlineCredentials(userName, securePassword);

        }
        public void CreateNewUserLog(string userName, string role)
        {

        }



    }
}
