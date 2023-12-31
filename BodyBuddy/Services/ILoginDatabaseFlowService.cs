﻿namespace BodyBuddy.Services
{
    public interface ILoginDatabaseFlowService
    {
        /// <summary>
        /// Login Data Flow
        /// Backup local db to remote db if param is true
        /// Purges local db and replaces with data from remote db
        /// </summary>
        /// <param name="backUpCurrentData">if true, then it backup local db to remote db, and possible overwrite remote data</param>
        /// <returns></returns>
        Task StartLoginDatabaseFlow(bool backUpCurrentData);
    }
}
