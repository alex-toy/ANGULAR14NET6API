using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoccerPlayerApi.Entities.Structure;

public class TimeDimension
{
    [Key]
    [Column("_day")]
    public DateTime Day { get; set; }

    [Column("_week")]
    public int Week { get; set; }

    [Column("_month")]
    public int Month { get; set; }

    [Column("_trimester")]
    public int Trimester { get; set; }

    [Column("_semester")]
    public int Semester { get; set; }

    [Column("_year")]
    public int Year { get; set; }

    [MaxLength(10)]
    [Column("_week_label")]
    public string WeekLabel { get; set; }

    [MaxLength(10)]
    [Column("_month_label")]
    public string MonthLabel { get; set; }

    [MaxLength(10)]
    [Column("_trimester_label")]
    public string TrimesterLabel { get; set; } 

    [MaxLength(10)]
    [Column("_semester_label")]
    public string SemesterLabel { get; set; }
}
