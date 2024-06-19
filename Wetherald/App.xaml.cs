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
using System.Diagnostics;

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
        public static double[] ProcessDataTodayTemperture(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<double>> dataSelector)
        {
            double[] sum = new double[8];
            double[] weightSum = new double[8]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources      
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TodayForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i] != -273)
                    {
                        sum[i] += data[i] * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }

        public static double[] ProcessDataTodayHumidity(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<double>> dataSelector)
        {
            double[] sum = new double[8];
            double[] weightSum = new double[8]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TodayForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i] != -1)
                    {
                        sum[i] += data[i] * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }

        public static double[] ProcessDataTomorrowHumidity(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<double>> dataSelector)
        {
            double[] sum = new double[8];
            double[] weightSum = new double[8]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TomorrowForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i] != -1)
                    {
                        sum[i] += data[i] * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }

        public static double[] ProcessDataTomorrowTemperture(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<double>> dataSelector)
        {
            double[] sum = new double[8];
            double[] weightSum = new double[8]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TomorrowForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i] != -273)
                    {
                        sum[i] += data[i] * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }

        public static double[] ProcessDataTodayWind(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<WindData>> dataSelector)
        {
            double[] sum = new double[8];
            double[] weightSum = new double[8]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TodayForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Speed != -1)
                    {
                        sum[i] += data[i].Speed * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }
        public static List<string> ProcessDataTodayWindString(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<WindData>> dataSelector)
        {
            List<string> sum = new List<string>();
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TodayForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Direction != "none")
                    {
                        sum.Add(data[i].Direction);
                        
                    }
                }
            }


            return sum;
        }
        public static List<string> ProcessDataTommorowWindString(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<WindData>> dataSelector)
        {
            List<string> sum = new List<string>();
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TomorrowForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Direction != "none")
                    {
                        sum.Add(data[i].Direction);

                    }
                }
            }


            return sum;
        }

        public static double[] ProcessDataTomorrowWind(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<DayForecast, List<WindData>> dataSelector)
        {
            double[] sum = new double[8];
            double[] weightSum = new double[8]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.TomorrowForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Speed != -1)
                    {
                        sum[i] += data[i].Speed * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }
        public static double[] ProcessDataMonthTemperture(Dictionary<string, double> trust, List<WeatherSource> weatherSources, Func<MonthForecast, Dictionary<int, double>> dataSelector)
        {
            double[] sum = new double[31];
            double[] weightSum = new double[31]; // To keep track of the sum of weights for each position
            foreach (KeyValuePair<string, double> pair in trust)
            {
                var data = weatherSources
                .Where(source => source.Name.Equals(pair.Key))
                .SelectMany(source => dataSelector(source.Forecast.MonthForecast))
                .ToList();

                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Value != -273)
                    {
                        sum[i] += data[i].Value * pair.Value;
                        weightSum[i] += pair.Value;
                    }
                }
            }

            // Normalize the sum by the sum of weights to handle missing values
            for (int i = 0; i < sum.Length; i++)
            {
                if (weightSum[i] != 0)
                {
                    sum[i] /= weightSum[i];
                    sum[i] = Math.Round(sum[i], 2);
                }
            }

            return sum;
        }
        public static double[] TrustDayTempFormula(Dictionary<string, double> trust)
        {
            double[] sum = new double[8];
            foreach (KeyValuePair<string, double> pair in trust)
            {
                int i = 0;
                var result = WeatherSources.Select(x => x)
                                           .Where(source => source.Name.Equals(pair.Key))
                                           .SelectMany(x => x.Forecast.TodayForecast.TemperatureData);
                foreach(var item in result)
                {                    
                    sum[i++] += (double)item * pair.Value;                    
                }
            }
            return sum;
        }

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
                try
                {
                    client.BaseAddress = new Uri("https://pudge.grabitkorovany.org/weatherald/" + RequestLocation);
                    HttpResponseMessage response = client.GetAsync("").Result;
                    response.EnsureSuccessStatusCode();
                    string result = response.Content.ReadAsStringAsync().Result;
                    // Десериализация JSON-строки в список объектов WeatherSource
                    WeatherSources = JsonConvert.DeserializeObject<List<WeatherSource>>(result);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Произошла ошибка при обращении к серверу.\nПроверьте свое соединение с интернетом, а также отключите VPN\nПопытка возобновить подключение...");
                    GetInfo();
                }


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
