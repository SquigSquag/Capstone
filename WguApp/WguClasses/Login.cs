using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WguApp.WguClasses
{
    
    public class Login
    {
        [PrimaryKey, AutoIncrement]
        [Column("UserId")]
        public int UserId { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Password")]
        public string Password { get; set; }
        [Column("Role")]
        public string Role { get; set; }

        public virtual Task<List<Term>> GetVisibleTerms(SqlData.SqlDatahelper db)
        {
            return db.PullTerms(this);
        }
    }
}
