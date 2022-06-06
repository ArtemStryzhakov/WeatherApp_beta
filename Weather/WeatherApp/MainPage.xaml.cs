using System;
using Xamarin.Forms;

namespace WeatherApp
{
    public partial class MainPage : ContentPage
    {
        RestService _restService;

        public MainPage()
        {
            InitializeComponent();
            _restService = new RestService();
            BackgroundColor = Color.LightBlue;
        }

        async void OnGetWeatherButtonClicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
            {
                WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                weatherData.Main.Temperature = Math.Round(((weatherData.Main.Temperature - 32) * 5 / 9), 2);
                if (weatherData.Weather[0].Visibility == "Clouds")
                {
                    weatherData.Weather[0].Visibility += "☁";
                }
                else if (weatherData.Weather[0].Visibility == "Clear")
                {
                    weatherData.Weather[0].Visibility += "☉";
                }
                else if (weatherData.Weather[0].Visibility == "Mist")
                {
                    weatherData.Weather[0].Visibility += "🌫";
                }


                if (weatherData.Main.Temperature < 5)
                {
                    labelImage.Source = "https://thumbs.dreamstime.com/b/%D0%BF%D0%B8%D0%BA%D1%82%D0%BE%D0%B3%D1%80%D0%B0%D0%BC%D0%BC%D0%B0-%D0%B2%D0%B5%D0%BA%D1%82%D0%BE%D1%80%D0%B0-%D0%BD%D0%B8%D0%B7%D0%BA%D0%BE%D0%B9-%D1%82%D0%B5%D0%BC%D0%BF%D0%B5%D1%80%D0%B0%D1%82%D1%83%D1%80%D1%8B-102823864.jpg";
                }
                else if (weatherData.Main.Temperature >= 5 && weatherData.Main.Temperature <= 14)
                {
                    labelImage.Source = "https://climate.copernicus.eu/sites/default/files/inline-images/surftemp.png";
                }
                else
                {
                    labelImage.Source = "https://cdn-icons-png.flaticon.com/512/1035/1035618.png";
                }
                BindingContext = weatherData;
            }
        }

        string GenerateRequestUri(string endpoint)
        {
            string requestUri = endpoint;
            requestUri += $"?q={_cityEntry.Text}";
            requestUri += "&units=imperial"; // or units=metric
            requestUri += $"&APPID={Constants.OpenWeatherMapAPIKey}";
            return requestUri;
        }
    }
}
