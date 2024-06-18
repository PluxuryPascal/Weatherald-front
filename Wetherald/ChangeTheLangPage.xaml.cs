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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для ChangeTheLangPage.xaml
    /// </summary>
    public partial class ChangeTheLangPage : Page
    {
        public ChangeTheLangPage()
        {
            InitializeComponent();
            App.ChangeLanguage(App.CurrentLanguage); // Установить начальный язык на текущий

            // Установить выбранный элемент ComboBox на основе текущего языка
            SetLanguageSelector(App.CurrentLanguage);
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
        private void Spravka_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });
        }
        private void LanguageSelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LanguageSelection.SelectedItem is ComboBoxItem selectedItem)
            {
                string culture = selectedItem.Tag.ToString();
                SetLanguage(culture);
            }
        }
        public void SetLanguage(string culture)
        {
            if (LanguageSelection.SelectedItem is ComboBoxItem selectedItem)
            {
                string culture1 = selectedItem.Tag.ToString();
                App.ChangeLanguage(culture1);
            }
        }
        private void ReturnMain_Click(object sender, RoutedEventArgs e)
        {

            string pageName = (App.Current as App).PageName;
            string fullTypeName = $"Wetherald.{pageName}";

            Type pageType = Type.GetType(fullTypeName);
            if (pageType != null)
            {
                object pageInstance = Activator.CreateInstance(pageType);
                NavigationService.Navigate(pageInstance);
            }
            else
            {
                MessageBox.Show($"Страница '{pageName}' не найдена.");
            }
        }
        private void SetLanguageSelector(string culture)
        {
            foreach (ComboBoxItem item in LanguageSelection.Items)
            {
                if (item.Tag.ToString() == culture)
                {
                    LanguageSelection.SelectedItem = item;
                    break;
                }
            }
        }

    }
}
