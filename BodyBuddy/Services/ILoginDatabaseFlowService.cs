using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BodyBuddy.Services
{
    public interface ILoginDatabaseFlowService
    {
        /// <summary>
        /// Starting flow to replace SQLite DB with Online DB Data
        /// </summary>
        /// <returns></returns>
        Task StartLoginDatabaseFlow();
    }
}
