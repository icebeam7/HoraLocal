// Ejemplo adaptado de la documentación: https://jamesmontemagno.github.io/GeolocatorPlugin/CurrentLocation.html
using System;
using System.Threading.Tasks;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace HoraLocal.Servicios
{
    public static class ServicioGelocalizacion
    {
        public static async Task<Position> ObtenerUbicacionActual()
        {
            Position ubicacion = null;

            try
            {
                var gps = CrossGeolocator.Current;
                gps.DesiredAccuracy = 100;

                if (!gps.IsGeolocationAvailable || !gps.IsGeolocationEnabled)
                    return null;

                ubicacion = await gps.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);
            }
            catch (Exception ex) { }

            if (ubicacion == null)
                return null;

            return ubicacion;
        }
    }
}
