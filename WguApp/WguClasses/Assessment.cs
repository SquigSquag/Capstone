using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguApp.WguClasses
{
    public class Assessment
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }
        [Column("CourseId")]
        public int CourseId { get; set; }
        [Column("Name")]
        public string Name { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("TurnedIn")]
        public bool Turnedin { get; set; }
        [Column("Type")]
        public string Type { get; set; }
        [Column("TurnedInDisplay")]
        public string TurnedInDisplay => this.Turnedin ? "Yes" : "No";
    }
}
