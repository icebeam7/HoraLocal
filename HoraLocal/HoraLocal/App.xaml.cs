using Xamarin.Forms;

namespace HoraLocal
{
	public partial class App : Application
	{
		public App ()
		{
			InitializeComponent();

			MainPage = new Paginas.PaginaHoraLocal();
		}
	}
}
