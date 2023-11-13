using automatisatie_csv_s.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services.IOServices
{
    public class ReadCSVFiles
    {
        public static List<Teacher> ReadCSVmoodleLeraar(string path)
        {
            List<Teacher> leraren = new List<Teacher>();
            using (StreamReader sr = new StreamReader(path))
            {
                // Skip the header line
                string headerLine = sr.ReadLine();

                // Read each line
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length >= 6)  // Ensure we have the correct number of data fields
                    {
                        Teacher leraar = new Teacher
                        {
                            Lastname = parts[0],
                            Firstname = parts[1],
                            Username = parts[2],
                            Password = parts[3],
                            OrgUnitPath = parts[5],
                           Email = parts[2] + "@cvoantwerpen.org"
                            
                            
                        };
                        leraren.Add(leraar);
                    }
                }
            }
            return leraren;
        }
        
        public static List<Student> ReadCSVmoodleCursist(string path)
        {
            List<Student> students = new List<Student>();
            using (StreamReader sr = new StreamReader(path))
            {
                // Skip the header line
                string headerLine = sr.ReadLine();

                // Read each line
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length >= 9)  // Ensure we have the correct number of data fields
                    {
                        Student student = new Student
                        {
                            Lastname = parts[0],
                            Firstname = parts[1],
                            Username = parts[2],
                            Password = parts[3],
                            Email = parts[4],
                            Phone1 = parts[5],
                            Aim = parts[6],
                            Course1 = parts[7],
                            Type1 = parts[8]
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }

        public static List<Course> ReadCSVmoodleCursus(string path)
        {
            List<Course> cursussen = new List<Course>();
            using (StreamReader sr = new StreamReader(path))
            {
                // Skip the header line
                string headerLine = sr.ReadLine();

                // Read each line
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length >= 9)  // Ensure we have the correct number of data fields
                    {
                        Course cursus = new Course
                        {
                            IdNumber = parts[0],
                            ShortName = parts[1],
                            FullName = parts[2],
                            Summary = parts[3],
                            CategoryPath = parts[4],
                            StartDate = parts[5],
                            EndDate = parts[6],
                            TemplateCourse = parts[7],
                            EnableCompletion = parts[8]
                        };
                        cursussen.Add(cursus);
                    }
                }
            }
            return cursussen;
        }

        public static List<Enrollment> ReadCSVmoodleInschrijvingen(string path)
        {
            List<Enrollment> inschrijvingen = new List<Enrollment>();
            using (StreamReader sr = new StreamReader(path))
            {
                // Skip the header line
                string headerLine = sr.ReadLine();

                // Read each line
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length >= 3)  // Ensure we have the correct number of data fields
                    {
                        Enrollment inschrijving = new Enrollment
                        {
                            Username = parts[0],
                            CourseShortName = parts[1],
                            Type1 = parts[2]                 
                        };
                        inschrijvingen.Add(inschrijving);
                    }
                }
            }
            return inschrijvingen;
        }

        public static List<TeachingAssignment> ReadCSVmoodleCursusopdracht(string path)
        {
            List<TeachingAssignment> cursusopdrachten = new List<TeachingAssignment>();
            using (StreamReader sr = new StreamReader(path))
            {
                // Skip the header line
                string headerLine = sr.ReadLine();

                // Read each line
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if (string.IsNullOrEmpty(line)) continue;

                    string[] parts = line.Split(';');
                    if (parts.Length >= 3)  // Ensure we have the correct number of data fields
                    {
                        TeachingAssignment opdracht = new TeachingAssignment
                        {
                            Username = parts[0],
                            CourseShortName = parts[1],
                            Type1 = parts[2]
                        };
                        cursusopdrachten.Add(opdracht);
                    }
                }
            }
            return cursusopdrachten;
        }


    }
}
