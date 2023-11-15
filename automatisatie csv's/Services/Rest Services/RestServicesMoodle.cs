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
        static readonly HttpClient httpClient = new() { BaseAddress= new Uri(StaticVariables.moodleUrl) };
        public static void LoadUsersAndCourses()
        {
            LoadUsers();
            Console.WriteLine("Users loaded from Moodle. " + StaticVariables.UsersCurrentlyInMoodle.Count + " users found");
            LoadCourses();
            Console.WriteLine("Courses loaded from Moodle. " + StaticVariables.CoursesCurrentlyInMoodle.Count + " courses found");
            Console.WriteLine("wait for all transactions to finish..");
            Thread.Sleep(5000); //make sure all users are imported
        }
        private static void LoadCourses()
        {
            var postData = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("wstoken", StaticVariables.token),
                new KeyValuePair<string, string>("wsfunction", "core_course_get_courses"),
                new KeyValuePair<string, string>("moodlewsrestformat", "json")    };

            var content = new FormUrlEncodedContent(postData);

            var response = httpClient.PostAsync("/webservice/rest/server.php", content).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;

                List<MoodleCourse>? courses = JsonConvert.DeserializeObject<List<MoodleCourse>>(result);
                StaticVariables.CoursesCurrentlyInMoodle = courses;
                return;

            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
                return;
            }
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
            HttpResponseMessage response = httpClient.PostAsync("/webservice/rest/server.php", content).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var moodleResponse = JsonConvert.DeserializeObject<MoodleUsersResponse>(result);
                StaticVariables.UsersCurrentlyInMoodle.Clear();
                if (moodleResponse != null) StaticVariables.UsersCurrentlyInMoodle.AddRange(moodleResponse.Users);        
            }
            else
            {
                Console.WriteLine($"HTTP Error: {response.StatusCode}");
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
                if (StaticVariables.CategoriesCurrentlyInMoodle != null)LogService.LogEvent("categories loaded from Moodle. " + StaticVariables.CategoriesCurrentlyInMoodle.Count + " categories found", "MoodleREST");          
            }
            else
            {
                LogService.LogError($"Load Categories http error: {response.StatusCode}");            
            }
        }
        public static async void CreateTeacher(Teacher leraar)
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
                var response = await httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).ConfigureAwait(true);
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var createdUser = JsonConvert.DeserializeObject<List<CreatedUserResponse>>(result);
                        Console.WriteLine($"User created with ID: {createdUser[0].Id}");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Error: {result}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

        }
        public static async void CreateStudent(Student cursist)
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
            try
            {



                var response = await httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).ConfigureAwait(true); ;
                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();
                    try
                    {
                        var createdUser = JsonConvert.DeserializeObject<List<CreatedUserResponse>>(result);
                        Console.WriteLine($"User created with ID: {createdUser[0].Id}");
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Error: {result}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {

                await Console.Out.WriteLineAsync(ex.Message);
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

        public static async Task CreateCourseInMoodle(Course cursus)
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
                    var response = await httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).ConfigureAwait(true);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync().ConfigureAwait(true);

                        try
                        {
                            var createdCourse = JsonConvert.DeserializeObject<List<CourseJSONResult>>(result);
                            // Additional logic for a successful response if needed
                            break; // Exit the loop if successful
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"Error: {result}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                }
                attempts++;
                Console.WriteLine("retrying... " + attempts);
            } while (attempts < maxAttempts);

            Console.WriteLine("Course creation attempts completed.");
        }
        public static async void EnrollUserInMoodle(string roleType, string userName, string courseshortname)
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

                var response = await httpClient.PostAsync(StaticVariables.moodleUrl + "/webservice/rest/server.php", content).ConfigureAwait(false);

                if (response.IsSuccessStatusCode)
                {
                    string result = await response.Content.ReadAsStringAsync();

                    try
                    {


                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Error: {result}");
                    }
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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
}

