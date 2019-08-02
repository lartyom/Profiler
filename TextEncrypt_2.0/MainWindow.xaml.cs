using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace TextEncrypt
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            time.Content = DateTime.Now.ToString("dd MMMM yyyy HH:mm");
            encrypt_button.Content = "ЗАШИФРОВАТЬ И\n     ОТПРАВИТЬ";
            decrypt_button.Content = "ДЕШИФРОВАТЬ";
        }

        private void Encrypt(object sender, RoutedEventArgs e)
        {
            Dictionary<char, char> key = new Dictionary<char, char> {
                { 'q', 'p' },
                { 'w', 'y' },
                { 'e', '_' },
                { 'r', 'g' },
                { 't', '@' },
                { 'y', 'q' },
                { 'u', 'Ç' },
                { 'i', '3' },
                { 'o', 'b' },
                { 'p', ')' },
                { 'a', 'r' },
                { 's', ' ' },
                { 'd', '#' },
                { 'f', 'ﬂ' },
                { 'g', ']' },
                { 'h', '‡' },
                { 'j', '0' },
                { 'k', '°' },
                { 'l', 't' },
                { ':', 'h' },
                { '"', ' ' },
                { ';', '|' },
                { '|', 'i' },
                { 'z', 'k' },
                { 'x', '}' },
                { 'c', 'ƒ' },
                { 'v', 'f' },
                { 'b', 'c' },
                { 'n', 'z' },
                { 'm', '8' },
                { ' ', '_' },
                {'!',' ' }
            };
            char[] word = encrypt_txtbox.Text.ToCharArray();
            for (int i = 0; i < encrypt_txtbox.Text.Length; i++)
            {
                if (Char.IsUpper(word[i]))
                {
                    word[i] = Char.ToLower(word[i]);
                }
                try
                {
                    word[i] = key[word[i]];
                }
                catch { }
            }
            encrypt_txtbox.Text = new string('░', word.Length) + '\n' + new string(word) + '\n' + new string('░', word.Length);
            status.Foreground = Brushes.White;
            status.TextAlignment = TextAlignment.Center;
            status.Text = "/!\\ MESSAGE ENCRYPTION COMPLETE /!\\";
        }
  
        private void textChangedEventHandler(object sender, TextChangedEventArgs e)
        {
            status.Foreground = Brushes.Gray;
            status.TextAlignment = TextAlignment.Left;
            status.Text = "300 Макс.символов";

        }
    }
}

      

  
