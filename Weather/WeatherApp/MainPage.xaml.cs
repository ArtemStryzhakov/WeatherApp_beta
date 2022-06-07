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
            try
            {
                if (!string.IsNullOrWhiteSpace(_cityEntry.Text))
                {
                    WeatherData weatherData = await _restService.GetWeatherData(GenerateRequestUri(Constants.OpenWeatherMapEndpoint));
                    weatherData.Main.Temperature = Math.Round(((weatherData.Main.Temperature - 32) * 5 / 9), 2);
                    if (weatherData.Weather[0].Visibility == "Clouds")
                    {
                        weatherData.Weather[0].Visibility += " ☁";
                    }
                    else if (weatherData.Weather[0].Visibility == "Clear")
                    {
                        weatherData.Weather[0].Visibility += " ☉";
                    }
                    else if (weatherData.Weather[0].Visibility == "Mist")
                    {
                        weatherData.Weather[0].Visibility += " 🌫";
                    }


                    if (weatherData.Main.Temperature < 5)
                    {
                        labelTemp.Text = weatherData.Main.Temperature.ToString();
                        labelTemp.Text += " ❄";
                    }
                    else if (weatherData.Main.Temperature >= 5 && weatherData.Main.Temperature <= 14)
                    {
                        labelTemp.Text = weatherData.Main.Temperature.ToString();
                        labelTemp.Text += " 🌌";
                    }
                    else
                    {
                        labelTemp.Text = weatherData.Main.Temperature.ToString();
                        labelTemp.Text += " 🔥";
                    }


                    if (weatherData.Main.Pressure > 5)
                    {
                        labelWind.Text = weatherData.Wind.Speed.ToString();
                        labelWind.Text += " 🌬";
                    }
                    else
                    {
                        labelWind.Text = weatherData.Wind.Speed.ToString();
                        labelWind.Text += " ⛵";
                    }


                    if (weatherData.Main.Humidity < 40)
                    {
                        labelHumidity.Text = weatherData.Main.Humidity.ToString();
                        labelHumidity.Text += "🏜";
                    }
                    else
                    {
                        labelHumidity.Text = weatherData.Main.Humidity.ToString();
                        labelHumidity.Text += "💧";
                    }

                    labelLocation.Text = weatherData.Title.ToString();
                    labelVisibility.Text = weatherData.Weather[0].Visibility.ToString();
                    BindingContext = weatherData;
                }
            }
            catch (Exception)
            {
                labelLocation.Text = "Wrong city!";
                labelTemp.Text = "...";
                labelWind.Text = "...";
                labelHumidity.Text= "...";
                labelVisibility.Text = "...";
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
