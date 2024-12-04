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

        // Evento del bot�n "INGRESAR"
        private async void OnLoginClicked(object sender, EventArgs e)
        {
            // Recuperar los valores ingresados
            string username = entryUsername.Text;
            string password = entryPassword.Text;

            // Validaci�n de campos vac�os
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                await DisplayAlert("Error", "Por favor complete todos los campos", "OK");
                return;
            }

            // Simulaci�n de autenticaci�n
            if (username == "admin" && password == "12345")
            {
                
                // Redirigir a la siguiente p�gina
                await Navigation.PushAsync(new EmotionsPage());
            }
            else
            {
                await DisplayAlert("Error", "Credenciales inv�lidas. Int�ntelo de nuevo.", "OK");
            }
        }
    }
}