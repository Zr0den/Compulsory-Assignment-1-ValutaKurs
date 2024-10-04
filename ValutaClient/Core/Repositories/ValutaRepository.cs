using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ValutaClient.Core.Entities;

namespace ValutaClient.Core.Repositories
{
    public class ValutaRepository : EntityFrameworkRepository<Valuta>
    {
        public ValutaRepository(DB.Database dbcontext) : base(dbcontext) { }
    }
}
