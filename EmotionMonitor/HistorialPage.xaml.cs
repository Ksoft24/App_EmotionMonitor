using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmotionMonitor
{
    public partial class HistorialPage : ContentPage
    {
        public HistorialPage()
        {
            InitializeComponent();
            // Vincular eventos
            datePicker.DateSelected += (s, e) => LoadData();
            pickerEmocion.SelectedIndexChanged += (s, e) => LoadData();
            pickerAmbientes.SelectedIndexChanged += (s, e) => LoadData();
        }

        private async void LoadData()
        {
            try
            {
                // Obtener los valores seleccionados
                var fechaSeleccionada = datePicker.Date.ToString("yyyy-MM-dd");  // Asegúrate de que el formato sea yyyy-MM-dd
                var emocionSeleccionada = pickerEmocion.SelectedItem?.ToString();
                var ambienteSeleccionado = pickerAmbientes.SelectedItem?.ToString();

                // Validar que todos los campos estén seleccionados
                if (string.IsNullOrEmpty(emocionSeleccionada) || string.IsNullOrEmpty(ambienteSeleccionado))
                {
                    resultadosStack.Children.Clear();
                    resultadosStack.Children.Add(new Label
                    {
                        Text = "Por favor selecciona todos los filtros.",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.Gray
                    });
                    return;
                }

                // Convertir la emoción seleccionada a inglés usando el mapeo
                string emocionEnIngles = MapEmocionToEnglish(emocionSeleccionada);

                // Construir la URL de la API con el valor de emoción en inglés
                string apiUrl = $"https://kingbot.pe/api_emotion/historial.php?fecha={fechaSeleccionada}&emocion={emocionEnIngles}&ambiente={ambienteSeleccionado}";

                // Realizar la solicitud
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync(apiUrl);

                // Parsear la respuesta (supongo que es JSON)
                var datos = System.Text.Json.JsonSerializer.Deserialize<List<HistorialDato>>(response);

                // Limpiar resultados previos
                resultadosStack.Children.Clear();

                // Filtrar los datos de acuerdo a la emoción seleccionada y ambiente
                var datosFiltrados = datos.Where(item =>
                    item.fecha == fechaSeleccionada &&
                    item.ambiente == ambienteSeleccionado).ToList();

                // Mostrar los nuevos resultados
                if (datosFiltrados.Any())
                {
                    foreach (var item in datosFiltrados)
                    {
                        var frame = new Frame
                        {
                            BorderColor = Colors.Gray,
                            CornerRadius = 5,
                            Padding = 10,
                            Content = new Label { Text = item.imagen_base64, VerticalOptions = LayoutOptions.Center }
                        };

                        var tapGesture = new TapGestureRecognizer();
                        tapGesture.Tapped += (s, e) => OnDataTapped(item.imagen_base64);
                        frame.GestureRecognizers.Add(tapGesture);

                        resultadosStack.Children.Add(frame);
                    }
                }
                else
                {
                    resultadosStack.Children.Add(new Label
                    {
                        Text = "No se encontraron resultados para los filtros seleccionados.",
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center,
                        TextColor = Colors.Gray
                    });
                }
            }
            catch (Exception ex)
            {
                resultadosStack.Children.Clear();
                resultadosStack.Children.Add(new Label
                {
                    Text = "No se encontraron datos o ocurrió un error.",
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    TextColor = Colors.Red
                });
            }
        }



        private async void OnDataTapped(string imagenUrl)
        {
            try
            {
                // Cargar la imagen desde la URL
                imagenEmocion.Source = ImageSource.FromUri(new Uri(imagenUrl));
            }
            catch
            {
                await DisplayAlert("Error", "No se pudo cargar la imagen.", "OK");
            }
        }


        private string MapEmocionToEnglish(string emocionEspañol)
        {
            // Diccionario de mapeo entre emociones en español e inglés
            var emocionMap = new Dictionary<string, string>
        {
            { "Happy", "happy" },
            { "Sad", "sad" },
            { "Angry", "angry" },
            { "Surprised", "surprised" },
            { "Disgusted", "disgusted" },
            { "Fear", "fear" },
            { "Quiet", "quiet" }
        };

            // Retornar la emoción en inglés o "unknown" si no se encuentra
            return emocionMap.ContainsKey(emocionEspañol) ? emocionMap[emocionEspañol] : "unknown";
        }
    }

}