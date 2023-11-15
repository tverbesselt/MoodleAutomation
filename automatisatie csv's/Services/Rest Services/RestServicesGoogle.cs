using Google.Apis.Admin.Directory.directory_v1;
using Google.Apis.Admin.Directory.directory_v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services.IOServices
{
    public class RestServicesGoogle
    {

        public static async Task CreateUser()
        {
            GoogleCredential credential = GoogleCredential.FromFile("fit-boulevard-387811-e480dd080da1.json")
    .CreateScoped(DirectoryService.Scope.AdminDirectoryUser);



            // Create Directory API service.
            var service = new DirectoryService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = "google classroomtest"
            });

            // Create Google user
            User newUser = new()
            {
                PrimaryEmail = "newuser@cvoantwerpen.org",  // Replace with the new user's email address
                Name = new UserName
                {
                    GivenName = "New",
                    FamilyName = "User"
                },
                Password = "your-password-here",  // Replace with the new user's password
                OrgUnitPath = "/personeel"  // Root Org Unit
            };

            UsersResource.InsertRequest request = service.Users.Insert(newUser);

            try
            {
                User createdUser =  request.ExecuteAsync().Result;
                Console.WriteLine("User created: " + createdUser.PrimaryEmail);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: " + e.Message);
            }
        }

        public static void CreateGoogleImportList()
        {
            // Define the file path on the user's desktop
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, "user_data.csv");

            // Create and write to the CSV file
            using (StreamWriter writer = new(filePath, true)) // Pass true to append to existing file
            {
                // If the file is empty, write the header row
                if (writer.BaseStream.Length == 0)
                {
                    writer.WriteLine("First Name [Required],Last Name [Required],Email Address [Required],Password [Required],Org Unit Path [Required]");
                }

                // Write data rows
                foreach (var user in StaticVariables.Students)
                {
                    writer.WriteLine($"{user.Firstname},{user.Lastname},{user.Email},{user.Password},/Cursisten");
                }
                foreach (var user in StaticVariables.Teachers)
                {
                    writer.WriteLine($"{user.Firstname},{user.Lastname},{user.Email},{user.Password},/Personeel");
                }
            }

            Console.WriteLine($"Data appended to the CSV file at: {filePath}");
        }
    }



}


