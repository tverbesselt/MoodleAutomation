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
        bool DoUnenRollments = false;
        bool DoEnrollments = true;
        bool Loopcontinously = false;
        int timeoutBetweenLoopsInMinutes = 60;

        StartProgramFlow(DoUnenRollments, DoEnrollments, timeoutBetweenLoopsInMinutes, Loopcontinously);
    }

    private static void StartProgramFlow(bool doUnenRollments, bool doEnrollments, int timeoutBetweenLoopsInMinutes, bool loopcontinously)
    {
        LogService.LogEvent("Application started at " + DateTime.Now.ToShortTimeString() + " on " + DateTime.Now.ToShortDateString(), "Application");

        RestServicesMoodle.LoadCourses();


        RestServicesMoodle.LoadCategoriesFromMoodle();

        string zipfile = Zipservices.SearchForZipFilesOnDesktop();

        string[] files = Zipservices.ExtractZipToTemp(zipfile);

        if (doEnrollments)
        {
            Zipservices.AddNewUsersAndCoursesToMoodle(files);
        }
        

        RestServicesMoodle.LoadUsersAndCourses();


        Zipservices.AddEnrollmentsAndAssignmentsToMoodle(files);

        RestServicesGoogle.CreateGoogleImportList();

        //Zipservices.UnEnrollusers(files);

        LogService.LogEvent("Application finished at " + DateTime.Now.ToShortTimeString() + " on " + DateTime.Now.ToShortDateString(), "Application");
        Console.WriteLine("tis weer allemaal gelukt. Goe bezig Chris!");
    }
}

