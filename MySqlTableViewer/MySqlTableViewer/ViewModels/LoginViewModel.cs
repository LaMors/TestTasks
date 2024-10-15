using MySql.Data.MySqlClient;
using MySqlTableViewer.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MySqlTableViewer.ViewModels
{
    /// <summary>
    /// ViewModel for handling user login to a MySQL server.
    /// </summary>
    public class LoginViewModel : BindableBase
    {
        /// <summary>
        /// Stores the MySQL server address.
        /// </summary>
        private string _server;
        /// <summary>
        /// Gets or sets the MySQL server address.
        /// </summary>
        public string Server
        {
            get => _server;
            set => SetProperty(ref _server, value);
        }

        /// <summary>
        /// Stores the username for MySQL server authentication.
        /// </summary>
        private string username;
        /// <summary>
        /// Gets or sets the username for MySQL server authentication.
        /// </summary>
        public string Username
        {
            get => username;
            set => SetProperty(ref username, value);
        }

        /// <summary>
        /// Stores the password for MySQL server authentication.
        /// </summary>
        private string password;
        /// <summary>
        /// Gets or sets the password for MySQL server authentication.
        /// </summary>
        public string Password
        {
            get => password;
            set => SetProperty(ref password, value);
        }

        /// <summary>
        /// Command for executing the login operation.
        /// </summary>
        public DelegateCommand LoginCommand { get; }

        /// <summary>
        /// Initializes a new instance of the LoginViewModel class and sets up the login command.
        /// </summary>
        public LoginViewModel()
        {
            LoginCommand = new DelegateCommand(ExecuteLogin, CanLogin);
            this.PropertyChanged += (o, a) => { LoginCommand.RaiseCanExecuteChanged(); };
        }

        /// <summary>
        /// Determines whether the login can be executed by checking if all required fields (Server, Username, and Password) are filled.
        /// </summary>
        /// <returns>
        /// Returns true if none of the required fields are empty or contain only white-space characters; otherwise, returns false.
        /// </returns>
        private bool CanLogin()
        {
            var isAnyEmpty = string.IsNullOrWhiteSpace(Server) || 
                             string.IsNullOrWhiteSpace(Username) || 
                             string.IsNullOrWhiteSpace(Password);

            return !isAnyEmpty;
        }

        /// <summary>
        /// Executes the login process by connecting to the MySQL server and opening the main view.
        /// </summary>
        private void ExecuteLogin()
        {
            string connectionString = $"Server={Server};User ID={Username};Password={Password};";
            try
            {
                var connection = new MySqlConnection(connectionString);
                connection.Open();

                MainView mainView = new MainView(connection);
                mainView.Show();

                Application.Current.Windows[0].Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Connection error: {ex.Message}");
            }
        }
    }
}
