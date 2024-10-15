using MySql.Data.MySqlClient;
using MySqlTableViewer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlTableViewer.ViewModels
{
    /// <summary>
    /// ViewModel for managing the main view of the application that displays database tables.
    /// </summary>
    public class MainViewModel : BindableBase
    {
        /// <summary>
        /// Collection of database tables to be displayed in the UI.
        /// </summary>
        private ObservableCollection<DatabaseTable> tables = new();

        /// <summary>
        /// Gets or sets the collection of tables in the connected database.
        /// </summary>
        public ObservableCollection<DatabaseTable> Tables
        {
            get => tables;
            set => SetProperty(ref tables, value);
        }

        /// <summary>
        /// The MySqlConnection instance used to interact with the database.
        /// </summary>
        private MySqlConnection connection;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class with the specified MySQL connection
        /// and loads the list of tables from the database.
        /// </summary>
        /// <param name="connection">The MySQL connection to be used for fetching table information.</param>
        public MainViewModel(MySqlConnection connection)
        {
            this.connection = connection;
            LoadTables();
        }

        /// <summary>
        /// Loads the list of tables from the connected MySQL database and populates the Tables collection.
        /// </summary>
        private void LoadTables()
        {
            string query = "SELECT TABLE_SCHEMA, TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

            MySqlCommand cmd = new MySqlCommand(query, connection);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Tables.Add(new DatabaseTable
                    {
                        DatabaseName = reader.GetString("TABLE_SCHEMA"),
                        TableName = reader.GetString("TABLE_NAME")
                    });
                }
            }
        }
    }
}
