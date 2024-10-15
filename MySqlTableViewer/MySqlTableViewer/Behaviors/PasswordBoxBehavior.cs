using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MySqlTableViewer.Behaviors
{
    /// <summary>
    /// A behavior that binds the Password property of a PasswordBox to a ViewModel.
    /// </summary>
    public class PasswordBoxBehavior : Behavior<PasswordBox>
    {

        /// <summary>
        /// Dependency property for binding the Password of the PasswordBox.
        /// </summary>
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(PasswordBoxBehavior), new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        /// <summary>
        /// Gets or sets the Password value of the PasswordBox. This is a bindable property.
        /// </summary>
        public string Password
        {
            get => (string)GetValue(PasswordProperty);
            set => SetValue(PasswordProperty, value);
        }

        /// <summary>
        /// Attaches the behavior to the PasswordBox and subscribes to the PasswordChanged event.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PasswordChanged += OnPasswordChanged;
        }

        /// <summary>
        /// Detaches the behavior from the PasswordBox and unsubscribes from the PasswordChanged event.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.PasswordChanged -= OnPasswordChanged;
        }

        /// <summary>
        /// Event handler for when the PasswordBox's password is changed. It updates the Password property.
        /// </summary>
        /// <param name="sender">The PasswordBox that triggered the event.</param>
        /// <param name="e">The event data.</param>
        private void OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox != null)
            {
                Password = passwordBox.Password;
            }
        }
    }
}
