using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MH.DataAccess
{
    public class MHDbContext: DbContext
    {
        public MHDbContext(DbContextOptions<MHDbContext> options) : base(options)
        {

        }
    }
}
