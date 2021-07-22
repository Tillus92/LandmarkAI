using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
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

namespace LandmarkAI
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Image files(*.png; .*jpg) | *.png;*.jpg;*.jped |All file(*.*)|*.*";
            dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            if (dialog.ShowDialog() == true)
            {
                string fileName = dialog.FileName;
                selectedImage.Source = new BitmapImage(new Uri(fileName));

                MakePredictionAsync(fileName);
            } 

        }

        private async Task MakePredictionAsync(string fileName)
        {
            string uRL = "https://westeurope.api.cognitive.microsoft.com/customvision/v3.0/Prediction/7a2bad97-eb06-4165-97ff-48e3c8930936/classify/iterations/Iteration1/image";
            string predictionKey = "f293dceac9ae46cbbdda2ba9a1586521";
            string contentType = "application/octet-stream";
            var file = File.ReadAllBytes(fileName); 
            
            using( HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Prediction-Key", predictionKey);
                using (var content = new ByteArrayContent(file))
                {
                    content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);
                    var response = await client.PostAsync(uRL, content);
                    var responseString = await response.Content.ReadAsStringAsync();
                }
            }
        }
    }
}
