using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для StartPageFrame.xaml
    /// </summary>
    public partial class StartPageFrame : Page
    {
        public StartPageFrame()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Подписываемся на событие KeyDown для окна
            KeyDown += MainWindow_KeyDown;
        }
        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            // Проверяем, была ли нажата кнопка F1
            if (e.Key == Key.F1)
            {
                // Вызываем метод для открытия справки
                ShowHelp();
            }
        }

        private void ShowHelp()
        {
            try
            {

                // Путь к файлу справки CHM
                string pathToHelpFile = @"SpravSluzhba.chm";
                // Название страницы в справке CHM
                string helpPage = "okno_mainwindow_nachalo.htm";

                // Строка с командой для открытия файла справки
                string command = $"hh.exe {pathToHelpFile}::{helpPage}";

                // Создаем новый процесс для выполнения команды
                ProcessStartInfo psi = new ProcessStartInfo("cmd.exe", "/c " + command)
                {
                    // Скрываем окно командной строки
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process.Start(psi);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка открытия справки: {ex.Message}");
            }
        }
        private async void MainButton_Click(object sender, RoutedEventArgs e)
        {
            MainButton.IsEnabled = false;
            testSpeavka.IsEnabled = false;
            LangIco_Button.IsEnabled = false;

            TranslateTransform trans = new TranslateTransform();
            MainIcon.RenderTransform = trans;

            DoubleAnimation animX = new DoubleAnimation(0, -290, TimeSpan.FromSeconds(0.5));

            CubicEase easeX = new CubicEase { EasingMode = EasingMode.EaseOut };
            animX.EasingFunction = easeX;
            trans.BeginAnimation(TranslateTransform.XProperty, animX);

            DoubleAnimation animY = new DoubleAnimation(0, 78.5, TimeSpan.FromSeconds(0.5));
            CubicEase easeY = new CubicEase { EasingMode = EasingMode.EaseOut };
            animY.EasingFunction = easeY;
            trans.BeginAnimation(TranslateTransform.YProperty, animY);

            await Task.Delay(550);
            NavigationService.Navigate(new TrustForm());
            //NavigationService.Navigate(new MainMenu_Weather());

        }
        private async Task Delay(object sender, RoutedEventArgs e)
        {
            
            Animation_Complete(sender, new EventArgs());
        }
        private void Animation_Complete(object sender, EventArgs e)
        {
        }

        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });
        }

        private void LangIco_Button_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).PageName = "StartPageFrame";
            NavigationService.Navigate(new ChangeTheLangPage());
        }


    }
}
