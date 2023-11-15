using automatisatie_csv_s;
using automatisatie_csv_s.BO;
using automatisatie_csv_s.Services;
using automatisatie_csv_s.Services.IOServices;
using automatisatie_csv_s.Services.Rest_Services;
using automatisatie_csv_s.Services.TestService;
using System.IO.Compression;



internal class Program
{
    private static void Main(string[] args)
    {
        LogService.LogEvent("Application started at " + DateTime.Now.ToShortTimeString + " on " + DateTime.Now.ToShortDateString(), "Application");


        RestServicesMoodle.LoadCategoriesFromMoodle();
        
        string zipfile = Zipservices.SearchForZipFilesOnDesktop();
        
        string[] files = Zipservices.ExtractZipToTemp(zipfile);

        Zipservices.AddNewUsersAndCoursesToMoodle(files);

        RestServicesMoodle.LoadUsersAndCourses();
            
        Zipservices.AddEnrollmentsAndAssignmentsToMoodle(files);

        RestServicesGoogle.CreateGoogleImportList();

        
        LogService.LogEvent("Application finished at " + DateTime.Now.ToShortTimeString + " on " + DateTime.Now.ToShortDateString(), "Application");

    }
}

