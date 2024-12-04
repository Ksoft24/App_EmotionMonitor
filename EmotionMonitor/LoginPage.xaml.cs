using Microsoft.Maui.Controls;
using System;

namespace EmotionMonitor
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        // Evento del botón "INGRESAR"
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Recuperar los valores ingresados
            string username = entryUsername.Text;
            string password = entryPassword.Text;

            // Validación de campos vacíos
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos", "OK");
                return;
            }

            // Simulación de autenticación
            if (username == "admin" && password == "12345")
            {
                
                // Redirigir a la siguiente página
                await Navigation.PushAsync(new EmotionsPage());
            }
            else
            {
                await DisplayAlert("Error", "Credenciales inválidas. Inténtelo de nuevo.", "OK");
            }
        }
    }
}