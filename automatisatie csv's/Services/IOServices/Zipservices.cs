using automatisatie_csv_s.BO;
using automatisatie_csv_s.Services.Rest_Services;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services.IOServices
{
    public class Zipservices
    {
        public static string[] ExtractZipToTemp(string zipFilePath)
        {
           CleanUpTempDir(zipFilePath);
            // Generate temp directory path
            string tempDir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(zipFilePath));

            // Create temp directory if it doesn't exist
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }
          

            // Extract ZIP file to temp directory
            ZipFile.ExtractToDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/" + zipFilePath, tempDir);

            // Get list of all files in temp directory
            return Directory.GetFiles(tempDir, "*", SearchOption.AllDirectories);
        }
        private static void CleanUpTempDir(string zipFilePath)
        {
            try
            {
                string tempDir = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(zipFilePath));

                // Get all files in the directory
                string[] files = Directory.GetFiles(tempDir);

                // Delete each file
                foreach (string file in files)
                {
                    File.Delete(file);
                
                }

                Console.WriteLine("All files deleted successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"No files to delete");
            }
        }
        public static string SearchForZipFilesOnDesktop()
        {
            // Get the user's desktop directory
            string desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            // Get all .zip files from the desktop
            string[] zipFiles = Directory.GetFiles(desktopDir, "*.zip");

            // Check if there are any .zip files on the desktop
            if (zipFiles.Length == 0)
            {
                Console.WriteLine("No .zip files found on the desktop.");
                return "";
            }

            // Display .zip files and ask the user to choose one
            Console.WriteLine("List of .zip files on your desktop:");
            for (int i = 0; i < zipFiles.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileName(zipFiles[i])}");
            }

            int selectedIndex = 0;
            while (true)
            {
                Console.WriteLine("Enter the number corresponding to the file you want to choose:");
                string userInput = Console.ReadLine();

                if (int.TryParse(userInput, out selectedIndex) && selectedIndex >= 1 && selectedIndex <= zipFiles.Length)
                {
                    break; // Exit loop if valid input
                }
                else
                {
                    Console.WriteLine("Invalid input. Please try again.");
                }
            }

            // Show the user's choice
            string selectedFile = zipFiles[selectedIndex - 1];
            Console.WriteLine($"You selected: {Path.GetFileName(selectedFile)}");
            return Path.GetFileName(selectedFile);
        }

       
        public static void AddNewUsersAndCoursesToMoodle(string[] files)
        {
            foreach (string file in files)
            {
                if (file.Contains("moodle") && file.Contains("cursist"))
                {
                    StaticVariables.Students = ReadCSVFiles.ReadCSVmoodleCursist(file);
                    foreach (var cursist in StaticVariables.Students)
                    {
                        RestServicesMoodle.CreateStudent(cursist);
                        
                    }

                }

                if (file.Contains("moodle") && file.Contains("personeel"))
                {
                    StaticVariables.Teachers = ReadCSVFiles.ReadCSVmoodleLeraar(file);
                    foreach (var leraar in StaticVariables.Teachers)
                    {
                        RestServicesMoodle.CreateTeacher(leraar);
                        
                    }
                }


                if (file.Contains("moodle") && file.Contains("cursus") && !file.Contains("opdracht") && !file.Contains("inschrijving") && !file.Contains("uitschrijving"))
                {
                    StaticVariables.Courses = ReadCSVFiles.ReadCSVmoodleCursus(file);
                    foreach (var cursus in StaticVariables.Courses)
                    {
                        RestServicesMoodle.CreateCourseInMoodle(cursus);                       
                    }
                }
            }
        }
        public static void AddEnrollmentsAndAssignmentsToMoodle(string[] files)
        {
            foreach (string file in files)
            {
                if (file.Contains("moodle") && file.Contains("inschrijving") && !file.Contains("opleiding"))
                {
                    StaticVariables.Enrollments = ReadCSVFiles.ReadCSVmoodleInschrijvingen(file);
                    foreach (var inschrijving in StaticVariables.Enrollments)
                    {
                        RestServicesMoodle.EnrollUserInMoodle("5", inschrijving.Username, inschrijving.CourseShortName);
                       // Thread.Sleep(10);
                    }
                }

                if (file.Contains("moodle") && file.Contains("cursusopdracht"))
                {
                    StaticVariables.TeachingAssignments = ReadCSVFiles.ReadCSVmoodleCursusopdracht(file);
                    
                    foreach (var opdracht in StaticVariables.TeachingAssignments)
                    {
                        RestServicesMoodle.EnrollUserInMoodle("3", opdracht.Username, opdracht.CourseShortName);
                    }
                }
               

            }
        }
        public static void UnEnrollusers(string[] files)
        {
            foreach (string file in files)
            {
                if (file.Contains("moodle") && file.Contains("uitschrijving"))
                {
                    StaticVariables.Uitschrijvingen = ReadCSVFiles.ReadCSVmoodleInschrijvingen(file);

                    foreach (var opdracht in StaticVariables.Uitschrijvingen)
                    {
                        RestServicesMoodle.UnEnrollUserInMoodle("3", opdracht.Username, opdracht.CourseShortName);
                    }
                }
            }
        }

        }
}
