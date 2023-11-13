using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace automatisatie_csv_s.BO
{
    public class MoodleCourse
    {
        public int Id { get; set; }
        public string ShortName { get; set; }
        public int CategoryId { get; set; }
        public int CategorySortOrder { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
        public string IdNumber { get; set; }
        public string Summary { get; set; }
        public int SummaryFormat { get; set; }
        public string Format { get; set; }
        public int ShowGrades { get; set; }
        public int NewsItems { get; set; }
        public long StartDate { get; set; }
        public long EndDate { get; set; }
        public int NumSections { get; set; }
        public long MaxBytes { get; set; }
        public int ShowReports { get; set; }
        public int Visible { get; set; }
        public int GroupMode { get; set; }
        public int GroupModeForce { get; set; }
        public int DefaultGroupingId { get; set; }
        public long TimeCreated { get; set; }
        public long TimeModified { get; set; }
        public int EnableCompletion { get; set; }
        public int CompletionNotify { get; set; }
        public string Lang { get; set; }
        public string ForceTheme { get; set; }
        public List<CourseFormatOption> CourseFormatOptions { get; set; }
        public bool ShowActivityDates { get; set; }
        public object ShowCompletionConditions { get; set; }
        public List<CustomField> CustomFields { get; set; }
    }

    public class CourseFormatOption
    {
        public string Name { get; set; }

        public string Value { get; set; }
    }

    public class CustomField
    {
        public string Name { get; set; }
        public string ShortName { get; set; }
        public string Type { get; set; }
        public string ValueRaw { get; set; }
        public string Value { get; set; }
    }


}
