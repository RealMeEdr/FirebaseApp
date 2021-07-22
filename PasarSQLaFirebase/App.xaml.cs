using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PasarSQLaFirebase
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new Vistas.Usuarios();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
