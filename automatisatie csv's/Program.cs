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
        Console.WriteLine("Load categories from moodle instance");
        StaticVariables.CategoriesCurrentlyInMoodle = RestServicesMoodle.LoadCategoriesFromMoodle();
        Console.WriteLine( "read all zip files on desktop..");
        //select the zip file to use and extract
        string zipfile = Zipservices.SearchForZips();
        string[] files = Zipservices.ExtractZipToTemp(zipfile);
        

        //Load all new students and courses in Moodle
        Zipservices.AddNewUsersAndCoursesToMoodle(files);

        //read all courses and students from moodle to get all ID's
        Console.WriteLine("Loading users and courses from moodle");
        RestServicesMoodle.LoadUsersAndCourses();
        //  PrintData.PrintLists(StaticVariables.Students,StaticVariables.Courses, StaticVariables.Teachers,StaticVariables.TeachingAssignments, StaticVariables.Enrollments);
        Console.WriteLine(  "wait for all transactions to finish..");
        Thread.Sleep(5000); //make sure all users are imported
        //Load all enrollments and teacher assignments into Moodle
        Console.WriteLine("start enrolling students and teachers..");
        Zipservices.AddEnrollmentsAndAssignmentsToMoodle(files);

        RestServicesGoogle.CreateGoogleImportList();

    }
}




//methods
