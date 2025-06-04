using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguApp.WguClasses
{
    public class Term
    {
        [AutoIncrement, NotNull, PrimaryKey]
        [Column("TermID")]
        public int TermID { get; set; }
        [Column("TermTitle")]
        public string TermTitle { get; set; }
        [Column("StartDate")]
        public DateTime StartDate { get; set; }
        [Column("EndDate")]
        public DateTime EndDate { get; set; }
        [Column("UserId")]
        public int UserId { get; set; }  // Foreign key

        public static implicit operator Term(List<Term> v)
        {
            throw new NotImplementedException();
        }
    }
}
