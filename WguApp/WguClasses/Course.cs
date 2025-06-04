using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguApp.WguClasses
{
    public class Course
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [Column("TermID")]
        public int TermID { get; set; }  // Foreign key
        [Column("Title")]
        public string Title { get; set; }
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("Status")]
        public string Status { get; set; }
        [Column("InstructorName")]
        public string InstructorName { get; set; }
        [Column("InstructorPhone")]
        public string InstructorPhone { get; set; }
        [Column("InstructorEmail")]
        public string InstructorEmail { get; set; }
        [Column ("Notes")]
        public string Notes {get; set;}
    }
}
