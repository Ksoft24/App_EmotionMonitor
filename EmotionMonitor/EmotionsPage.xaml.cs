using Microsoft.Maui.Controls;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace EmotionMonitor
{
    public partial class EmotionsPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public EmotionsPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            pickerAmbiente.SelectedIndex = 0; // Seleccionar el primer ambiente por defecto

            // Cargar emociones del primer ambiente al iniciar
            LoadEmotionsForSelectedAmbiente();
        }

        private async void OnAmbienteSelected(object sender, EventArgs e)
        {
            string selectedAmbiente = pickerAmbiente.SelectedItem?.ToString();
            if (!string.IsNullOrEmpty(selectedAmbiente))
            {
                await LoadEmotionsForSelectedAmbiente();
            }
        }

        private async Task LoadEmotionsForSelectedAmbiente()
        {
            try
            {
                string selectedAmbiente = pickerAmbiente.SelectedItem?.ToString();
                if (string.IsNullOrEmpty(selectedAmbiente)) return;

                string apiUrl = $"https://kingbot.pe/api_emotion/emociones.php?ambiente={Uri.EscapeDataString(selectedAmbiente)}";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse>(jsonResponse, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    if (apiResponse?.status == "success" && apiResponse.data != null && apiResponse.data.Any())
                    {
                        // Sumar todas las emociones si hay múltiples registros
                        int totalHappy = apiResponse.data.Sum(d => d.happy);
                        int totalSad = apiResponse.data.Sum(d => d.sad);
                        int totalAngry = apiResponse.data.Sum(d => d.angry);
                        int totalSurprised = apiResponse.data.Sum(d => d.surprised);
                        int totalNeutral = apiResponse.data.Sum(d => d.neutral);
                        int totalDisgust = apiResponse.data.Sum(d => d.disgust);
                        int totalFear = apiResponse.data.Sum(d => d.fear);

                        // Actualizar las etiquetas de cantidad
                        LabelCantidadFeliz.Text = totalHappy.ToString();
                        LabelCantidadTriste.Text = totalSad.ToString();
                        LabelCantidadEnojado.Text = totalAngry.ToString();
                        LabelCantidadSorprendido.Text = totalSurprised.ToString();
                        LabelCantidadTranquilo.Text = totalNeutral.ToString();
                        LabelCantidadDesagrado.Text = totalDisgust.ToString();
                        LabelCantidadMiedo.Text = totalFear.ToString();
                    }
                    else
                    {
                        await DisplayAlert("Error", "No se encontraron datos válidos", "OK");
                        ResetEmotionLabels();
                    }
                }
                else
                {
                    await DisplayAlert("Error", $"No se pudieron cargar las emociones. Código: {response.StatusCode}", "OK");
                    ResetEmotionLabels();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar emociones: {ex.Message}", "OK");
                ResetEmotionLabels();
            }
        }

        private void ResetEmotionLabels()
        {
            // Resetear todas las etiquetas a 0
            LabelCantidadFeliz.Text = "0";
            LabelCantidadEnojado.Text = "0";
            LabelCantidadSorprendido.Text = "0";
            LabelCantidadTriste.Text = "0";
            LabelCantidadDesagrado.Text = "0";
            LabelCantidadMiedo.Text = "0";
            LabelCantidadTranquilo.Text = "0";
        }

        private async void OnHistorialClicked(object sender, EventArgs e)
        {
            // Navegar a la página HistorialPage
            await Navigation.PushAsync(new HistorialPage());
        }

        private async void OnEmotionTapped(object sender, EventArgs e)
        {
            if (sender is Frame frame && frame.Content is Grid grid && grid.Children[1] is Label label)
            {
                string emotion = label.Text;
                await DisplayAlert("Emoción", $"Seleccionaste: {emotion}", "OK");
                // Implementar la lógica para manejar la selección de la emoción
            }
        }
    }
}