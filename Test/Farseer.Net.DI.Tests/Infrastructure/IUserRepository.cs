using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FS.DI.Tests.Infrastructure
{
    public interface IUserRepository : IRepository<UserEntity>
    {
    }
}
