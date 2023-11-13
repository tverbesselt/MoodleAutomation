using System.Security.Cryptography;

namespace automatisatie_csv_s.Services.Rest_Services
{
    public class MoodleCategory
    {
       // private List<MoodleCategory> _categories = new List<MoodleCategory>();

        public int ID { get; set; }
        public string name { get; set; }
        public int parent { get; set; }
  
        public string getFullPath()
        {
            if (parent == 0) return name;
            else return getFullPath(parent) +" / "+ name;      
        }
        public string getFullPath(int iD)
        {
            MoodleCategory? category =  (from cat in StaticVariables.CategoriesCurrentlyInMoodle
                    where cat.ID == iD
                    select cat).FirstOrDefault();
            if (category.parent == 0) return category.name;
            else
            {
                return getFullPath(category.parent) + " / " + category.name;
            }
        }
    }
}