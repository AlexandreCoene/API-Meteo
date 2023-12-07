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

using System.IO; // Ajouter le namespace System.IO pour utiliser les fichiers
using Newtonsoft.Json; // Ajouter le namespace Newtonsoft.Json.Linq pour utiliser les fichiers JSON
using Alexandre_Coene_App_Meteo.Service; // Ajouter le namespace Alexandre_Coene_Meteo_LastV.Service pour utiliser la classe Ville
using System.Net.Http; // Ajouter le namespace System.Net.Http pour utiliser HttpClient

namespace Alexandre_Coene_App_Meteo // Définissez l'espace de noms Alexandre_Coene_App_Meteo
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApiMeteo apiMeteo; // Créez un objet ApiMeteo
        Ville newVille; // Créez un objet Ville
        List<string> ls_ville = new List<string>(); // Créez une liste de chaînes pour stocker les villes
        public MainWindow() // Définissez le constructeur MainWindow
        {
            InitializeComponent(); // Initialisez les composants
            newVille = new Ville(); // Créez une nouvelle ville
            CMB_Cities.ItemsSource = newVille.ReadCitiesFromFile(); // Ajouter la liste des villes à la ComboBox
        _: GetWeather("Annecy"); // Appel de la méthode GetWeather avec la ville d'Annecy
            
        }

        public async Task<string> GetWeather(string ville) // Définissez la méthode GetWeather comme asynchrone
        {
            try
            {
                HttpClient client = new HttpClient(); // Créez un nouveau client HTTP
                HttpResponseMessage response = await client.GetAsync("https://www.prevision-meteo.ch/services/json/" + ville); // Appel de l'API avec la ville

                if (response.IsSuccessStatusCode) // Vérifiez si la réponse est un succès
                {
                    var content = await response.Content.ReadAsStringAsync(); // Lire le contenu de la réponse
                    ApiMeteo.Root root = JsonConvert.DeserializeObject<ApiMeteo.Root>(content); // Désérialiser le contenu de la réponse en objet Root
                    ApiMeteo.HourlyData hourlyData = JsonConvert.DeserializeObject<ApiMeteo.HourlyData>(content); // Désérialiser le contenu de la réponse en objet HourlyData
                    Condition condition = JsonConvert.DeserializeObject<Condition>(content); // Désérialiser le contenu de la réponse en objet Condition
                    ApiMeteo.FcstDay0 fcstDay0 = JsonConvert.DeserializeObject<ApiMeteo.FcstDay0>(content); // Désérialiser le contenu de la réponse en objet FcstDay0
                    ApiMeteo.FcstDay0 forecastDay0 = root.fcst_day_0; // Définissez la prévision du jour 0 comme fcst_day_0
                    ApiMeteo.FcstDay1 forecastDay1 = root.fcst_day_1; // Définissez la prévision du jour 1 comme fcst_day_1
                    ApiMeteo.FcstDay2 forecastDay2 = root.fcst_day_2; // Définissez la prévision du jour 2 comme fcst_day_2
                    ApiMeteo.FcstDay3 forecastDay3 = root.fcst_day_3; // Définissez la prévision du jour 3 comme fcst_day_3
                    ApiMeteo.FcstDay4 forecastDay4 = root.fcst_day_4; // Définissez la prévision du jour 4 comme fcst_day_4

                    if (content.Contains("error")) // Vérifiez si le contenu contient le mot erreur
                    {
                        MessageBox.Show("Ville non trouvée"); // Afficher une erreur
                        return "ok"; // Retourne
                    }

                    Day0_Image.Source = new BitmapImage(new Uri(forecastDay0.icon)); // Afficher l'image du jour 0
                    Day1_Image.Source = new BitmapImage(new Uri(forecastDay1.icon)); // Afficher l'image du jour 1
                    Day2_Image.Source = new BitmapImage(new Uri(forecastDay2.icon)); // Afficher l'image du jour 2
                    Day3_Image.Source = new BitmapImage(new Uri(forecastDay3.icon)); // Afficher l'image du jour 3
                    Day4_Image.Source = new BitmapImage(new Uri(forecastDay4.icon)); // Afficher l'image du jour 4

                    TB_Humidité.Text = "Humidité : " + root.current_condition.humidity.ToString() + "g/m3"; // Afficher l'humidité
                    TB_Vent.Text = "Vent : " + root.current_condition.wnd_spd.ToString() + "km/h"; // Afficher le vent
                    TB_Temperature_min.Text = "Température min : " + root.fcst_day_0.tmin.ToString() + "°C"; // Afficher la température minimale
                    TB_Temperature_max.Text = "Température max : " + root.fcst_day_0.tmax.ToString() + "°C"; // Afficher la température maximale
                    TB_Condition.Text = "Condition : " + root.current_condition.condition; // Afficher la condition
                    TB_Température.Text = "Température : " + root.current_condition.tmp.ToString() + "°C"; // Afficher la température
                    TB_Pression.Text = "Pression : " + root.current_condition.pressure.ToString() + "hPa"; // Afficher la pression
                    TB_Latitude.Text = "Latitude : " + root.city_info.latitude.ToString(); // Afficher la latitude
                    TB_Longitude.Text = "Longitude : " + root.city_info.longitude.ToString(); // Afficher la longitude
                    TB_Elevation.Text = "Élévation : " + root.city_info.elevation.ToString() + "m"; // Afficher l'élévation

                    TB_Sunrise.Text = "Lever du soleil : " + root.city_info.sunrise.ToString(); // Afficher le lever du soleil
                    TB_Sunset.Text = "Coucher du soleil : " + root.city_info.sunset.ToString(); // Afficher le coucher du soleil

                    TB_min_j1.Text = "Température min : " + root.fcst_day_1.tmin.ToString() + "°C"; // Afficher la température minimale du jour 1
                    TB_max_j1.Text = "Température max : " + root.fcst_day_1.tmax.ToString() + "°C"; // Afficher la température maximale du jour 1
                    TB_Condition_J1.Text = "Condition : " + root.fcst_day_1.condition; // Afficher la condition du jour 1

                    TB_min_j2.Text = "Température min : " + root.fcst_day_2.tmin.ToString() + "°C"; // Afficher la température minimale du jour 2
                    TB_max_j2.Text = "Température max : " + root.fcst_day_2.tmax.ToString() + "°C"; // Afficher la température maximale du jour 2
                    TB_Condition_J2.Text = "Condition : " + root.fcst_day_2.condition; // Afficher la condition du jour 2

                    TB_min_j3.Text = "Température min : " + root.fcst_day_3.tmin.ToString() + "°C"; // Afficher la température minimale du jour 3
                    TB_max_j3.Text = "Température max : " + root.fcst_day_3.tmax.ToString() + "°C"; // Afficher la température maximale du jour 3
                    TB_Condition_J3.Text = "Condition : " + root.fcst_day_3.condition; // Afficher la condition du jour 3

                    TB_min_j4.Text = "Température min : " + root.fcst_day_4.tmin.ToString() + "°C"; // Afficher la température minimale du jour 4
                    TB_max_j4.Text = "Température max : " + root.fcst_day_4.tmax.ToString() + "°C"; // Afficher la température maximale du jour 4
                    TB_Condition_J4.Text = "Condition : " + root.fcst_day_4.condition; // Afficher la condition du jour 4

                    TB_Jour0.Text = "Aujourd'hui : " + root.fcst_day_0.day_long; // Afficher le jour 0
                    TB_Jour1.Text = root.fcst_day_1.day_long; // Afficher le jour 1
                    TB_Jour2.Text = root.fcst_day_2.day_long; // Afficher le jour 2
                    TB_Jour3.Text = root.fcst_day_3.day_long; // Afficher le jour 3
                    TB_Jour4.Text = root.fcst_day_4.day_long; // Afficher le jour 4
                }
                return "OK"; // Retourne OK
            }

            catch (Exception ex) // Attraper une exception
            {
                Console.WriteLine("Exception during GetWeather: " + ex.Message); // Afficher une exception
                return "KO"; // Retourne KO
            }
        }

        private void CMB_Cities_SelectionChanged(object sender, SelectionChangedEventArgs e) // Définissez la méthode CMB_Cities_SelectionChanged
        {
            if (CMB_Cities.SelectedItem != null) // Vérifiez si l'élément sélectionné n'est pas nul
            {
                string ville = CMB_Cities.SelectedItem.ToString(); // Définissez la ville comme l'élément sélectionné
            _: GetWeather(ville); // Appelez la méthode GetWeather avec la ville sélectionnée
                TB_NewCity.Text = ville; // Définissez le texte de TB_NewCity comme la ville sélectionnée
            }
        }

        private object lockObject = new object(); // Définissez un objet de verrouillage

        private void BTN_ADD_Click(object sender, RoutedEventArgs e) // Définissez la méthode BTN_ADD_Click
        {
            string newCity = TB_NewCity.Text.Trim(); // Définissez la nouvelle ville comme le texte de TB_NewCity

            newVille.AddVille(newCity); // Appelez la méthode AddVille avec la nouvelle ville
            CMB_Cities.ItemsSource = null; // Définissez la source de la ComboBox comme nulle
            CMB_Cities.ItemsSource = newVille.GetVilles(); // Définissez la source de la ComboBox comme la liste des villes
            TB_NewCity.Text = string.Empty; // Définissez le texte de TB_NewCity comme vide          
        }

        private void Button_Dell_Click(object sender, RoutedEventArgs e) // Définissez la méthode Button_Dell_Click
        {
            string newCity = TB_NewCity.Text.Trim(); // Définissez la nouvelle ville comme le texte de TB_NewCity

            newVille.RemoveVille(newCity); // Appelez la méthode RemoveVille avec la nouvelle ville
            CMB_Cities.ItemsSource = null; // Définissez la source de la ComboBox comme nulle
            CMB_Cities.ItemsSource = newVille.GetVilles(); // Définissez la source de la ComboBox comme la liste des villes
            TB_NewCity.Text = string.Empty; // Définissez le texte de TB_NewCity comme vide          
        }

        private void Window_Closed(object sender, EventArgs e) // Définissez la méthode Window_Closed
        {
            newVille.WriteCitiesToFile(); // Appelez la méthode WriteCitiesToFile
        }
    }
}
