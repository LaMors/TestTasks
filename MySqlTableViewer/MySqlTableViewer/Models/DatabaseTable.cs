using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySqlTableViewer.Models
{
    /// <summary>
    /// Represents a database table with information about its database and table name.
    /// </summary>
    public class DatabaseTable
    {
        /// <summary>
        /// Gets or sets the name of the database that contains the table.
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Gets or sets the name of the table within the database.
        /// </summary>
        public string? TableName { get; set; }
    }
}
