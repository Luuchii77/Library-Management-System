using System;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using LibraryManagementSystem.Data;
using LibraryManagementSystem.BLL;
using LibraryManagementSystem.Forms;

namespace LibraryManagementSystem
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Configure DbContext options with the connection string
            var optionsBuilder = new DbContextOptionsBuilder<LibraryDbContext>();
            // Use the same connection string as in DatabaseHelper and appsettings.json
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=LibraryManagementSystem;Trusted_Connection=True;MultipleActiveResultSets=true");

            using (var context = new LibraryDbContext(optionsBuilder.Options))
            {
                // Ensure database and tables are created
                context.Database.EnsureCreated();

                var libraryManager = new LibraryManager(context);

                // Directly launch the main form
                Application.Run(new MainForm(libraryManager));
            }
        }
    }
}
