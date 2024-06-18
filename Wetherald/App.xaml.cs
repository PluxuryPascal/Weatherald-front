using Newtonsoft.Json;
using OpenTK.Compute.OpenCL;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Xml.Linq;
using System;
using System.Collections.Generic;

namespace Wetherald
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public string PageName { get; set; }
        public static Dictionary<string, double> trust = new Dictionary<string, double>();
        public static string[] trustChoise = new string[3];
        public static string CurrentLanguage { get; set; } = "ru-RU"; // Установить русский язык по умолчанию

        //public static int TrustFormula(int site1, string siteName1, int site2, string siteName2, int site3, string siteName3, int site4, string siteName4)
        //{

        //}

        public static void ChangeLanguage(string culture)
        {
            var dict = new ResourceDictionary();
            switch (culture)
            {
                case "ru-RU":
                    dict.Source = new Uri("Resources/Strings.ru-RU.xaml", UriKind.Relative);
                    break;
                default:
                    dict.Source = new Uri("Resources/Strings.en-US.xaml", UriKind.Relative);
                    break;
                case "zh-CN":
                    dict.Source = new Uri("Resources/Strings.zh-CN.xaml", UriKind.Relative);
                    break;
                case "es-ES":
                    dict.Source = new Uri("Resources/Strings.es-ES.xaml", UriKind.Relative);
                    break;
            }

            // Очистить существующие словари и добавить новый
            Current.Resources.MergedDictionaries.Clear();
            Current.Resources.MergedDictionaries.Add(dict);

            // Обновить текущий язык
            CurrentLanguage = culture;
        }
        public static string RequestLocation { get; set; }

        // Добавьте статическое свойство WeatherSources
        public static List<WeatherSource> WeatherSources { get; set; }

        public static void GetInfo()
        {
            using (var client = new HttpClient(new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate }))
            {
                client.BaseAddress = new Uri("https://pudge.grabitkorovany.org/weatherald/" + RequestLocation);
                HttpResponseMessage response = client.GetAsync("").Result;
                response.EnsureSuccessStatusCode();
                string result = response.Content.ReadAsStringAsync().Result;

                // Десериализация JSON-строки в список объектов WeatherSource
                WeatherSources = JsonConvert.DeserializeObject<List<WeatherSource>>(result);
            }
            //// Пример вывода данных в консоль (можно удалить или закомментировать)
            //foreach (var source in WeatherSources)
            //{
            //    Console.WriteLine($"Name: {source.Name}");
            //    Console.WriteLine($"Today's Temperature Data: {string.Join(", ", source.Forecast.TodayForecast.TemperatureData)}");
            //    Console.WriteLine($"Today's Humidity Data: {string.Join(", ", source.Forecast.TodayForecast.HumidityData)}");
            //    Console.WriteLine($"Today's Wind Data: {string.Join(", ", source.Forecast.TodayForecast.WindData.Select(w => $"{w.Direction} {w.Speed}"))}");
            //    Console.WriteLine($"Прогноз на оставшийся календарный месяц (максимальная температура): {string.Join(", ", source.Forecast.MonthForecast.MaxTemperatureData)}");
            //    Console.WriteLine($"Прогноз на оставшийся календарный месяц (минимальная температура): {string.Join(", ", source.Forecast.MonthForecast.MinTemperatureData)}");
            //    Console.WriteLine();
            //}
        }

        public class WindData
        {
            [JsonProperty("direction")]
            public string Direction { get; set; }

            [JsonProperty("speed")]
            public double Speed { get; set; }
        }

        public class DayForecast
        {
            [JsonProperty("temperatureData")]
            public List<double> TemperatureData { get; set; }

            [JsonProperty("humidityData")]
            public List<double> HumidityData { get; set; }

            [JsonProperty("windData")]
            public List<WindData> WindData { get; set; }

            [JsonProperty("precipitationProbabilityData")]
            public List<double> PrecipitationProbabilityData { get; set; }
        }

        public class MonthForecast
        {
            [JsonProperty("minTemperatureData")]
            public Dictionary<int, double> MinTemperatureData { get; set; }

            [JsonProperty("maxTemperatureData")]
            public Dictionary<int, double> MaxTemperatureData { get; set; }
        }

        public class Forecast
        {
            [JsonProperty("todayForecast")]
            public DayForecast TodayForecast { get; set; }

            [JsonProperty("tomorrowForecast")]
            public DayForecast TomorrowForecast { get; set; }

            [JsonProperty("monthForecast")]
            public MonthForecast MonthForecast { get; set; }
        }

        public class WeatherSource
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("forecast")]
            public Forecast Forecast { get; set; }
        }
    }

}
