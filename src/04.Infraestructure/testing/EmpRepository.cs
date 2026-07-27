using MemoryOnline.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace testing
{
    public class EmpRepository : Repository<Usuario>, IEmpRepository
    {
        public EmpRepository(MyDbContext empDBContext)
            : base(empDBContext)
        {
        }
        //TODO: Write here custom methods that are required for specific requirements.
    }
}
