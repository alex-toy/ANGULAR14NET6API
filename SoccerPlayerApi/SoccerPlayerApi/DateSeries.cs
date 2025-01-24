using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace YourNamespace
{
    public class DateSeries
    {
        [Key]  // Specifies that this property is the primary key
        public DateTime Day { get; set; }  // _day column

        public int Week { get; set; }  // _week column

        public int Month { get; set; }  // _month column

        public int Trimester { get; set; }  // _trimester column

        public int Semester { get; set; }  // _semester column

        public int Year { get; set; }  // _year column

        [MaxLength(10)]
        public string WeekLabel { get; set; }  // _week_label column

        [MaxLength(10)]
        public string MonthLabel { get; set; }  // _month_label column

        [MaxLength(10)]
        public string TrimesterLabel { get; set; }  // _trimester_label column

        [MaxLength(10)]
        public string SemesterLabel { get; set; }  // _semester_label column
    }
}
