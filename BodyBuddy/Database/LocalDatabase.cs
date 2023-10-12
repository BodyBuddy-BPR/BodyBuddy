using SQLite;
using System.Reflection;

namespace BodyBuddy.Database
{
    public class LocalDatabase
    {

        private SQLiteAsyncConnection _context;

        public LocalDatabase()
        {
        }

        private async Task Init()
        {
            if (_context is not null)
                return;

            // Ensure that the connection is initialized asynchronously
            _context = await GetAsyncConnection();
        }

        public async Task<SQLiteAsyncConnection> GetAsyncConnection()
        {
            if (_context == null)
            {
                // Get the path to the local database file
                string databasePath = SQLiteConstants.DatabasePath;

                // Check if the database file exists in local storage
                if (!File.Exists(databasePath))
                {
                    // If it doesn't exist, copy it from the embedded resource
                    var assembly = Assembly.GetExecutingAssembly();
                    using (var stream = assembly.GetManifestResourceStream("BodyBuddy.Database.BodyBuddyDb.db"))
                    {
                        using (var fileStream = File.Create(databasePath))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                }

                // Initialize the SQLite connection
                _context = new SQLiteAsyncConnection(databasePath, SQLiteConstants.Flags);

            }
            return _context;
        }
    }

}