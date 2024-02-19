using automatisatie_csv_s.BO;
using automatisatie_csv_s.Services.IOServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Google.Apis.Requests.BatchRequest;

namespace automatisatie_csv_s.Services.Rest_Services
{
    public partial class RestServicesMoodle
    {
        static readonly HttpClient httpClient = new() { BaseAddress = new Uri(StaticVariables.moodleUrl) };
        public static void LoadUsersAndCourses()
        {
            LogService.LogEvent("start Loading users", "application flow");
            LoadUsers();
            LogService.LogEvent("Users loaded from Moodle. " + StaticVariables.UsersCurrentlyInMoodle.Count + " users found", "application flow");
            Thread.Sleep(4000);
            LoadCourses();
            LogService.LogEvent("Courses loaded from Moodle. " + StaticVariables.CoursesCurrentlyInMoodle.Count + " courses found", "application flow");

        }
        public static bool LoadCourses()
        {
            var postData = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "core_course_get_courses"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json")    };

            var content = new FormUrlEncodedContent(postData);
            try
            {
                var response = httpClient.PostAsync("/webservice/rest/server.php", content).Result;

                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    if (result.Contains("exception")){
                        LogService.LogError($"Load courseserror. courses not loaded!!!!!");

                    }
                    else
                    {
List<MoodleCourse>? courses = JsonConvert.DeserializeObject<List<MoodleCourse>>(result);
                    StaticVariables.CoursesCurrentlyInMoodle = courses;
                    return true;
                    }
                    
                }
                else
                {
                    LogService.LogError($"Load courses http error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError($"Load courses exception: {ex.Message}");

            }
            return false;
        }
        private static void LoadUsers()
        {
            var postData = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                     new KeyValuePair<string, string>("wsfunction", "core_user_get_users"),
                     new KeyValuePair<string, string>("moodlewsrestformat", "json"),
                     new KeyValuePair<string, string>("criteria[0][key]", "email"),
                     new KeyValuePair<string, string>("criteria[0][value]", "%")
                 };
            var content = new FormUrlEncodedContent(postData);
            try
            {
                HttpResponseMessage response = httpClient.PostAsync("/webservice/rest/server.php", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                    var moodleResponse = JsonConvert.DeserializeObject<MoodleUsersResponse>(result);
                    StaticVariables.UsersCurrentlyInMoodle.Clear();
                    StaticVariables.UsersCurrentlyInMoodle.AddRange(moodleResponse.Users);
                }
                else
                {
                    LogService.LogError($"Load Users http error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {

                LogService.LogError($"Load Users http error: {ex.Message}");
            }
        }
        public static void LoadCategoriesFromMoodle()
        {

            var postData = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "core_course_get_categories"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json")    };
            var content = new FormUrlEncodedContent(postData);

            var response = httpClient.PostAsync("/webservice/rest/server.php", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                List<MoodleCategory>? categories = JsonConvert.DeserializeObject<List<MoodleCategory>>(result);
                StaticVariables.CategoriesCurrentlyInMoodle = categories;
                if (StaticVariables.CategoriesCurrentlyInMoodle != null) LogService.LogEvent("categories loaded from Moodle. " + StaticVariables.CategoriesCurrentlyInMoodle.Count + " categories found", "MoodleREST");
            }
            else
            {
                LogService.LogError($"Load Categories http error: {response.StatusCode}");
            }
        }
        public static void CreateTeacher(Teacher leraar)
        {
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "core_user_create_users"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json"),
                new KeyValuePair<string, string>("users[0][username]", leraar.Username + "@cvoantwerpen.org"),
                new KeyValuePair<string, string>("users[0][firstname]", leraar.Firstname),
                new KeyValuePair<string, string>("users[0][lastname]", leraar.Lastname),
                new KeyValuePair<string, string>("users[0][email]", leraar.Email),
                new KeyValuePair<string, string>("users[0][password]", "Moodle+23")
            };

            var content = new FormUrlEncodedContent(postData);
            try
            {
                var response = httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    string result = response.Content.ReadAsStringAsync().Result;
                }
                else
                {
                    LogService.LogError($"Creating teacher failed. Response statuscode: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                LogService.LogError($"Creating teacher error: {ex.Message}");
            }

        }
        public static void CreateStudent(Student cursist)
        {
            var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "core_user_create_users"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json"),
                new KeyValuePair<string, string>("users[0][username]", cursist.Username + "@cvoantwerpen.org"),
                new KeyValuePair<string, string>("users[0][firstname]", cursist.Firstname),
                new KeyValuePair<string, string>("users[0][lastname]", cursist.Lastname),
                new KeyValuePair<string, string>("users[0][email]", cursist.Email),
                new KeyValuePair<string, string>("users[0][password]", "Moodle+23")
            };
            var content = new FormUrlEncodedContent(postData);
            var attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    var response = httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        if (result.Contains("Ongeldige parameterwaarde gevonden"))
                        {
                            LogService.LogEvent("Student allready exists: " + cursist.Email, "moodleRest");
                        }
                        else
                        {
                            LogService.LogEvent("Student created: " + cursist.Email, "moodleRest");
                        }

                        break;
                    }
                    else
                    {
                        LogService.LogError($"Creating student failed. Response statuscode: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    LogService.LogError($"Creating student error: {ex.Message}");
                }

                attempts++;
                LogService.LogError($"error creating student {cursist.Firstname} {cursist.Lastname} ...({attempts})");
            }


        }

        public static int GetCourseIdByShortName(string shortName)
        {
            var id = (from MoodleCourse course in StaticVariables.CoursesCurrentlyInMoodle
                      where course.ShortName.Equals(shortName)
                      select course.Id).FirstOrDefault();
            return id;
        }
        public static int? GetUserIdByUsername(string Username)
        {
            return (from MoodleUser user in StaticVariables.UsersCurrentlyInMoodle
                    where user.Username == Username
                    select user.Id).FirstOrDefault();
        }

        public static void CreateCourseInMoodle(Course cursus)
        {
            int maxAttempts = 10;
            int attempts = 0;
            do
            {
                var postData = new List<KeyValuePair<string, string>>
                {
                     new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "core_course_create_courses"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json"),
                new KeyValuePair<string, string>("courses[0][fullname]", cursus.FullName),
                new KeyValuePair<string, string>("courses[0][shortname]", cursus.ShortName),
                new KeyValuePair<string, string>("courses[0][categoryid]", cursus.CategoryID),
                new KeyValuePair<string, string>("courses[0][startdate]",ConvertToTimeStamp( cursus.StartDate)),
                new KeyValuePair<string, string>("courses[0][enddate]",ConvertToTimeStamp( cursus.EndDate))
                };
                var content = new FormUrlEncodedContent(postData);

                try
                {
                    var response = httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;

                        try
                        {
                            if (result.Contains("shortnametaken"))
                            {
                                LogService.LogError($"course {cursus.FullName} allready exists");
                            }
                            else
                            {
                                var createdCourse = JsonConvert.DeserializeObject<List<CourseJSONResult>>(result);
                                LogService.LogEvent("course created: " + cursus.FullName, "moodleRest");
                            }

                            break; // Exit the loop if successful
                        }
                        catch (Exception ex)
                        {
                            exceptionObject? erroroccured = JsonConvert.DeserializeObject<exceptionObject>(result);

                            LogService.LogError(erroroccured.message);


                        }
                    }
                    else
                    {
                        LogService.LogError($"Error creating course: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {

                    LogService.LogError($"Exception creating course: {ex.Message}");
                }
                attempts++;

                LogService.LogError($"retrying creating course {cursus.FullName} ({attempts})");
            } while (attempts < maxAttempts);


        }
        public static void EnrollUserInMoodle(string roleType, string userName, string courseshortname)
        {
            int poging = 0;
            string rol = "leraar";
            if (roleType == "5") rol = "cursist";
            bool success = false;
            while (success != true && poging < 10)
            {
                var userID = GetUserIdByUsername(userName + "@cvoantwerpen.org").ToString();
                var courseID = GetCourseIdByShortName(courseshortname).ToString();
                var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "enrol_manual_enrol_users"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json"),
                new KeyValuePair<string, string>("enrolments[0][roleid]", roleType),
                new KeyValuePair<string, string>("enrolments[0][userid]", userID),
                new KeyValuePair<string, string>("enrolments[0][courseid]",courseID )
            };
                var content = new FormUrlEncodedContent(postData);
                try
                {
                    var response = httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        success = true;
                        if (result == "null")
                        {
                            LogService.LogError($"Error enrolling {rol}: user {userName}  allready enrolled in  course:{courseshortname} ");
                        }
                        else
                        {
                            if (result.Contains("invalid_parameter"))
                            {
                                if (userID == "0")
                                {
                                    LogService.LogError($"Error enrolling user: user {userName}  not found  (course:{courseshortname}) ");
                                }
                                if (courseID == "0")
                                {
                                    LogService.LogError($"Error enrolling user: course {courseshortname}  not found  (user:{userName})");
                                }
                            }
                            else
                            {
                                LogService.LogEvent($"{rol} {userName} ingeschreven in cursus {courseshortname}", "inschrijving cursus");
                            }
                        }


                    }
                    else
                    {
                        LogService.LogError($"Error enrolling user {userName}: {response.StatusCode} (poging {poging})");
                        Thread.Sleep(100);

                    }
                }
                catch (Exception ex)
                {
                    LogService.LogError($"Exception enrolling user {userName} {ex.Message}  retrying  ({poging})");
                    // LogService.LogError($"Exception enrolling user {userName} {ex.Message} {ex.InnerException}  (poging {poging})");
                    Thread.Sleep(10000);
                }
                poging++;
            }

        }
        public static void UnEnrollUserInMoodle(string roleType, string userName, string courseshortname)
        {
            int poging = 0;
            string rol = "leraar";
            if (roleType == "5") rol = "cursist";
            bool success = false;
            while (success != true && poging < 10)
            {
                var userID = GetUserIdByUsername(userName + "@cvoantwerpen.org").ToString();
                var courseID = GetCourseIdByShortName(courseshortname).ToString();
                var postData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "enrol_manual_unenrol_users"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json"),
                new KeyValuePair<string, string>("enrolments[0][roleid]", roleType),
                new KeyValuePair<string, string>("enrolments[0][userid]", userID),
                new KeyValuePair<string, string>("enrolments[0][courseid]",courseID )
            };
                var content = new FormUrlEncodedContent(postData);
                try
                {
                    var response = httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        string result = response.Content.ReadAsStringAsync().Result;
                        success = true;
                        if (result == "null")
                        {
                            LogService.LogEvent($"gebruiker {userName} uitgeschreven in cursus {courseshortname}", "uitschrijving cursus");
                        }
                        else
                        {
                            if (result.Contains("invalid_parameter"))
                            {
                                if (userID == "0")
                                {
                                    LogService.LogError($"Error unenrolling user: user {userName}  not found  (course:{courseshortname}) ");
                                }
                                if (courseID == "0")
                                {
                                    LogService.LogError($"Error unenrolling user: course {courseshortname}  not found  (user:{userName})");
                                }
                            }
                            else
                            {
                                LogService.LogEvent($"{rol} {userName} uitgeschreven in cursus {courseshortname}", "uitschrijving cursus");
                            }
                        }


                    }
                    else
                    {
                        LogService.LogError($"Error unenrolling user {userName}: {response.StatusCode} (poging {poging})");
                        Thread.Sleep(100);

                    }
                }
                catch (Exception ex)
                {
                    LogService.LogError($"Exception unenrolling user {userName} {ex.Message}  retrying  ({poging})");
                    // LogService.LogError($"Exception enrolling user {userName} {ex.Message} {ex.InnerException}  (poging {poging})");
                    Thread.Sleep(10000);
                }
                poging++;
            }

        }
        private static string ConvertToTimeStamp(string date)
        {

            DateTime dateTime = DateTime.ParseExact(date, "dd.MM.yyyy", null);
            // Convert DateTime to a Unix timestamp (seconds since Unix epoch)
            long unixTimestamp = (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;

            return unixTimestamp.ToString();
        }
    }

    public class exceptionObject
    {
        public string exception { get; set; }
        public string errorcode { get; set; }
        public string message { get; set; }
    }

}

