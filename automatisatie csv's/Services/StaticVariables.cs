using automatisatie_csv_s.BO;
using automatisatie_csv_s.Services.Rest_Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.Services
{
    public class StaticVariables
    {
        //lists loaded from csv's
        public static List<Student> Students = new List<Student>();
        public static  List<Course> Courses = new List<Course>();
        public static  List<Teacher> Teachers = new List<Teacher>();
        public static  List<TeachingAssignment> TeachingAssignments = new List<TeachingAssignment>();
        public static  List<Enrollment> Enrollments = new List<Enrollment>();

        //Moodle Lists
        public static List<MoodleCourse> CoursesCurrentlyInMoodle = new List<MoodleCourse>();
        public static List<MoodleUser> UsersCurrentlyInMoodle = new List<MoodleUser>();
        public static List<MoodleCategory> CategoriesCurrentlyInMoodle = new List<MoodleCategory>();

        
        //Moodle credentials
        public static string moodleUrl = "https://moodle.cvoantwerpen.org";  // Replace with your Moodle URL
        public static string token = "";  // Replace with your own token


        private string GetToken()
        {
            return token;   
        }
    }
}
