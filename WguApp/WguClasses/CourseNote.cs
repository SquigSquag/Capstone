using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguApp.WguClasses
{
    public class CourseNote
    {
        [PrimaryKey, AutoIncrement]
        [Column("Id")]
        public int Id { get; set; }
        [Column("CourseId")]
        public int CourseId { get; set; }  // Foreign key
        [Column("Content")]
        public string Content { get; set; }

    }
}
