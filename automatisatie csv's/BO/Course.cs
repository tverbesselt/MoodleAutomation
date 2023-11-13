using automatisatie_csv_s.Services;
using automatisatie_csv_s.Services.Rest_Services;

public class Course
{
    public string IdNumber { get; set; }
    public string ShortName { get; set; }
    public string FullName { get; set; }
    public string Summary { get; set; }
    public string CategoryPath { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public string TemplateCourse { get; set; }
    public string EnableCompletion { get; set; }

   
    public string CategoryID
    {
        get {
            var result = (from MoodleCategory cat in StaticVariables.CategoriesCurrentlyInMoodle
                        where cat.getFullPath().Equals(CategoryPath)
                        select cat.ID).First();
            return ""+result; }
       
    }

}
