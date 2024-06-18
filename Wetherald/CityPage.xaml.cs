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

namespace Wetherald
{
    /// <summary>
    /// Логика взаимодействия для CityPage.xaml
    /// </summary>
    public partial class CityPage : Page
    {
        // Словарь для сопоставления английских и русских названий городов
        private Dictionary<string, string> cityDictionary = new Dictionary<string, string>
        {
            { "Perm", "Пермь" },
            { "Moscow", "Москва" },
            { "PeterBurg", "Санкт-Петербург" },
            { "Novosibirsk", "Новосибирск" },
            { "Ekaterinburg", "Екатеринбург" },
            { "Kazan", "Казань" },
            { "Nizhny_Novgorod", "Нижний Новгород" },
            { "Samara", "Самара" },
            { "Omsk", "Омск" },
            { "Rostov_on_Don", "Ростов-на-Дону" },
            { "Ufa", "Уфа" },
            { "Krasnoyarsk", "Красноярск" },
            { "Voronezh", "Воронеж" },
            { "Volgograd", "Волгоград" },
            { "Krasnodar", "Краснодар" },
            { "Saratov", "Саратов" },
            { "Tyumen", "Тюмень" },
            { "Irkutsk", "Иркутск" },
            { "Yaroslavl", "Ярославль" },
            { "Vladivostok", "Владивосток" },
            { "Chelyabinsk", "Челябинск" },
            { "Tula", "Тула" },
            { "Kaliningrad", "Калининград" },
            { "Astrakhan", "Астрахань" },
            { "Murmansk", "Мурманск" },
            { "Pskov", "Псков" },
            { "Smolensk", "Смоленск" },
            { "Tver", "Тверь" },
            { "Vladimir", "Владимир" },
            { "Tomsk", "Томск" },
            { "Khabarovsk", "Хабаровск" },
            { "Kostroma", "Кострома" },
            { "Belgorod", "Белгород" },
            { "Penza", "Пенза" },
            { "Ryazan", "Рязань" },
            { "Ulyanovsk", "Ульяновск" },
            { "Arkhangelsk", "Архангельск" },
            { "Chita", "Чита" },
            { "Yakutsk", "Якутск" },
            { "Dubai", "Дубай" },
            { "New_York", "Нью-Йорк" },
            { "Los_Angeles", "Лос-Анджелес" },
            { "London", "Лондон" },
            { "Tokyo", "Токио" },
            { "Berlin", "Берлин" },
            { "Madrid", "Мадрид" },
            { "Rome", "Рим" },
            { "Beijing", "Пекин" },
            { "Sydney", "Сидней" },
            { "Toronto", "Торонто" },
            { "São_Paulo", "Сан-Паулу" },
            { "Mexico_City", "Мехико" },
            { "Seoul", "Сеул" },
            { "Bangkok", "Бангкок" },
            { "Istanbul", "Стамбул" },
            { "Mumbai", "Мумбаи" },
            { "Buenos_Aires", "Буэнос-Айрес" },
            { "Cairo", "Каир" },
            { "Cape_Town", "Кейптаун" },
            { "Jakarta", "Джакарта" },
            { "Lagos", "Лагос" },
            { "Nairobi", "Найроби" },
            { "Hong_Kong", "Гонконг" },
            { "Singapore", "Сингапур" },
            { "Rio_de_Janeiro", "Рио-де-Жанейро" },
            { "Lima", "Лима" },
            { "Shanghai", "Шанхай" },
            { "Manila", "Манила" },
            { "San_Francisco", "Сан-Франциско" },
            { "Chicago", "Чикаго" },
            { "Miami", "Майами" },
            { "Boston", "Бостон" },
            { "Las_Vegas", "Лас-Вегас" },
            { "Paris", "Париж" },
            { "Vienna", "Вена" },
            { "Amsterdam", "Амстердам" },
            { "Zurich", "Цюрих" },
            { "Brussels", "Брюссель" },
            { "Copenhagen", "Копенгаген" },
            { "Stockholm", "Стокгольм" },
            { "Oslo", "Осло" },
            { "Athens", "Афины" },
            { "Helsinki", "Хельсинки" },
            { "Prague", "Прага" },
            { "Budapest", "Будапешт" },
            { "Warsaw", "Варшава" },
            { "Lisbon", "Лиссабон" }
        };

        // Переменная для хранения выбранного города на русском языке
        private string selectedCityInRussian;

        public CityPage()
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
                string helpPage = "okno_mainwindow_vybor_goroda.htm";

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
        private void ReturnToMain(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPageFrame());
        }

        private void NextPageCityButton_Click(object sender, RoutedEventArgs e)
        {
                
                NavigationService.Navigate(new MainMenu_Weather());

        }

        private void CitySelection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (CitySelection.SelectedItem is TextBlock selectedTextBlock)
            {
                string cityNameInEnglish = selectedTextBlock.Name;

                // Проверка наличия выбранного города в словаре
                if (cityDictionary.TryGetValue(cityNameInEnglish, out string cityNameInRussian))
                {
                    selectedCityInRussian = cityNameInRussian;
                    NextPageCityButton.IsEnabled = true;
                    App.RequestLocation = selectedCityInRussian;
                    App.GetInfo();

                }
            }
            
        }
        private void testSpeavka2_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(new ProcessStartInfo { FileName = @"SpravSluzhba.chm", UseShellExecute = true });
        }
        private void LangIco_Button_Click(object sender, RoutedEventArgs e)
        {
            (App.Current as App).PageName = this.Title;
            NavigationService.Navigate(new ChangeTheLangPage());
        }

    }

        
}

