using EmailSender.Service;
using System.Text.RegularExpressions;
using System.Windows;

namespace UI;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        //EmailService.SendEmail("mistertar@gmail.com", "Test", @"html\news.html");
    }

    private void SendButton_Click(object sender, RoutedEventArgs e)
    {
        string email = EmailNameTextBox.Text.Trim();
        string name = NameNameTextBox.Text.Trim();
        bool isEmailValid = Regex.IsMatch(email, @"^[^\s@]+@[^\s@]+\.[^\s@]+$");
        bool isNameValid = !string.IsNullOrWhiteSpace(name);
        bool isTypeSelected = BirthdayRadioButton.IsChecked == true || PromotionsRadioButton.IsChecked == true || NewsRadioButton.IsChecked == true;

        if (!isEmailValid || !EmailService.IsValidEmail(email))
        {
            MessageBox.Show("Будь ласка, введіть коректний email.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!isNameValid)
        {
            MessageBox.Show("Будь ласка, введіть ваше ім'я.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (!isTypeSelected)
        {
            MessageBox.Show("Будь ласка, оберіть тип листа.", "Помилка", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string emailTemplate = "";
        if (BirthdayRadioButton.IsChecked == true)
        {
            emailTemplate = @"html\happyBirthday.html";
        }
        else if (PromotionsRadioButton.IsChecked == true)
        {
            emailTemplate = @"html\promotion.html";
        }
        else if (NewsRadioButton.IsChecked == true)
        {
            emailTemplate = @"html\news.html";
        }

        EmailService.SendEmail(email, "Персональне повідомлення", emailTemplate, name);

        MessageBox.Show("Лист успішно відправлено!", "Успіх", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}