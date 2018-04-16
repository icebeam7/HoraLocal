using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using HoraLocal.Servicios;

namespace HoraLocal.Paginas
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaginaHoraLocal : ContentPage
	{
		public PaginaHoraLocal ()
		{
			InitializeComponent ();
		}

        void Loading(bool mostrar)
        {
            indicator.IsEnabled = mostrar;
            indicator.IsRunning = mostrar;
        }

        public async void btnObtenerUbicacion_Clicked(object sender, EventArgs e)
        {
            Loading(true);

            var ubicacion = await ServicioGelocalizacion.ObtenerUbicacionActual();

            if (ubicacion != null)
            {
                txtLatitud.Text = ubicacion.Latitude.ToString();
                txtLongitud.Text = ubicacion.Longitude.ToString();
            }
            else
            {
                await DisplayAlert("Advertencia", "La ubicación no se pudo obtener", "OK");
            }

            Loading(false);
        }

        public async void btnObtenerHoraUbicacion_Clicked(object sender, EventArgs e)
        {
            Loading(true);

            double latitud, longitud;

            if (double.TryParse(txtLatitud.Text, out latitud) 
                && double.TryParse(txtLongitud.Text, out longitud))
            {
                var fechaActual = await ServicioTimeZone.ObtenerHoraLocal(latitud, longitud, ServicioTimeZone.token);

                if (fechaActual != null)
                {
                    lblFechaActual.Text = String.Format("{0:dd-MMMM-yyyy hh:mm:ss tt}", fechaActual.Value);
                }
                else
                {
                    await DisplayAlert("Advertencia", "Debes introducir coordenadas geográficas válidas", "OK");
                }
            }
            else
            {
                await DisplayAlert("Advertencia", "Debes introducir coordenadas geográficas válidas", "OK");
            }

            Loading(false);
        }
    }
}