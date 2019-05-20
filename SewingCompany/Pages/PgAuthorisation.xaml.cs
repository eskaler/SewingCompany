using SewingCompany.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SewingCompany.Pages
{
    /// <summary>
    /// Interaction logic for PgAuthorisation.xaml
    /// </summary>
    public partial class PgAuthorisation : Page
    {
       

        public PgAuthorisation()
        {
            InitializeComponent();
            TbLoginLogin.Focus();

        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var user = Db.Conn.User.Where(u => u.Login == TbLoginLogin.Text && u.Password == PbLoginPassword.Password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль.");
            }
            else
            {
                Transfer.LoggedUser = user;
                switch (user.IdRole)
                {
                    case 1:
                        NavigationService.Navigate(new PgCustomer());
                        break;
                    case 2:
                        NavigationService.Navigate(new PgManager());
                        break;
                    case 3:
                        NavigationService.Navigate(new PgStockman());
                        break;
                    case 4:
                        NavigationService.Navigate(new PgDirector());
                        break;
                }
            }
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            Regex checkSpace = new Regex(@"\s", RegexOptions.None);
            Regex checkPassword = new Regex(@"(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[!@#$%^]).*[A-Za-z\d!@#$%^]{6,}", RegexOptions.None);
            
            if((TbRegLogin.Text != string.Empty) && 
                (PbRegPassword.Password == PbRegPasswordRepeat.Password) && 
                (!checkSpace.IsMatch(PbRegPassword.Password) && 
                checkPassword.IsMatch(PbRegPassword.Password)))
            {
                if (Db.Conn.User.Where(u => u.Login == TbRegLogin.Text).FirstOrDefault() == null)
                {
                    User user = new User();
                    user.Login = TbRegLogin.Text;
                    user.Password = PbRegPassword.Password;
                    user.IdRole = 1;
                    Db.Conn.User.Add(user);
                    Db.Conn.SaveChanges();

                    MessageBox.Show("Регистрация прошла успешно! Теперь вы можете войти в систему используя свой логин и пароль.");
                    TbRegLogin.Text = string.Empty;
                    PbRegPassword.Password = string.Empty;
                    PbRegPasswordRepeat.Password = string.Empty;
                }
                else
                    MessageBox.Show("Данный логин уже занят.");
            }
            else
            {
                MessageBox.Show(@"Ошибка заполнения данных. 
Необходимо задать логин.
Пароль  должен  отвечать  следующим требованиям: 
•  Минимум 6 символов 
•  Минимум 1 прописная буква 
•  Минимум 1 цифра 
•  Минимум один символ из набора: ! @ # $ % ^.");
            }
        }

    }
}
