using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WguApp.WguClasses
{
    class Admin : Login
    {
        public override Task<List<Term>> GetVisibleTerms(SqlData.SqlDatahelper db)
        {
            // Admins can see all terms, so we don't filter by user ID
            return db.GetAllTerms();
        }
    }
}
