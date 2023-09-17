using BodyBuddy.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
                System.Diagnostics.Debug.WriteLine($"Database path: {databasePath}");

                // Check if the database file exists in local storage
                if (!File.Exists(databasePath))
                {
                    // If it doesn't exist, copy it from the embedded resource
                    var assembly = Assembly.GetExecutingAssembly();
                    using (var stream = assembly.GetManifestResourceStream("BodyBuddy.Database.BodyBoddyDb.db"))
                    {
                        using (var fileStream = File.Create(databasePath))
                        {
                            await stream.CopyToAsync(fileStream);
                        }
                    }
                    // Add a debug statement to confirm the file copy
                    System.Diagnostics.Debug.WriteLine("Database file copied successfully.");
                }

                // Initialize the SQLite connection
                _context = new SQLiteAsyncConnection(databasePath, SQLiteConstants.Flags);

                //// Add a debug statement to confirm the connection creation
                //System.Diagnostics.Debug.WriteLine("SQLite connection created successfully.");

                //// Add debug statements to confirm actions
                //System.Diagnostics.Debug.WriteLine("Database file copied or found.");
                //System.Diagnostics.Debug.WriteLine("SQLite connection created successfully.");
            }
            return _context;
        }
    }

}