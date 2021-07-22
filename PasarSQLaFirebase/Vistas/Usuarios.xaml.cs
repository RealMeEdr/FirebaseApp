using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasarSQLaFirebase.VistasModelo;
using PasarSQLaFirebase.Modelo;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.Media.Abstractions;
using Plugin.Media;
using System.Diagnostics;

namespace PasarSQLaFirebase.Vistas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Usuarios : ContentPage
    {
        public Usuarios()
        {
            InitializeComponent();
            MostrarUsuarios();
        }
        MediaFile file;
        string rutafoto;
        string idusuario;
        string estado;
        string estadoImagen;
        private async void btnGuardar_Clicked(object sender, EventArgs e)
        {
           await InsertarUsuarios();
           await SubirImagenesStore();
           await EditarFoto();        
        }
        private async Task EditarFoto()
        {
            VMusuarios funcion = new VMusuarios();
            Musuarios parametros = new Musuarios();
            parametros.Icono = rutafoto;
            parametros.Id_usuario = idusuario;
            parametros.Usuario = txtUsuario.Text;
            parametros.Pass = txtContraseña.Text;
            parametros.Estado = "Activo";
            await funcion.EditarFoto(parametros);
            await DisplayAlert("Listo", "Usuario agregado", "Cerrar");
            await MostrarUsuarios();
            txtContraseña.Text = "";
            txtUsuario.Text = "";
            
        }
        private async Task InsertarUsuarios()
        {
            VMusuarios funcion = new VMusuarios();
            Musuarios parametros = new Musuarios();

            //se agregan los elementos.
            parametros.Usuario = txtUsuario.Text;
            parametros.Pass = txtContraseña.Text;
            parametros.Icono = "-";
            parametros.Estado = "Activo";
            idusuario = await funcion.insertar_usuarios(parametros);
        }
        private async Task MostrarUsuarios()
        {
            VMusuarios funcion = new VMusuarios();
            var dt = await funcion.mostrar_usuarios();
            ListaUsuarios.ItemsSource = dt;
        }
        private async void btnAgregarImagen_Clicked(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            try
            {
                file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                {
                    PhotoSize= PhotoSize.Small

                });
                if (file == null)
                {
                    estadoImagen = "Nulo";
                    return;
                }
                else
                {
                ImagenCelular.Source = ImageSource.FromStream(() =>
                 {
                     var rutaImagen = file.GetStream();
                     return rutaImagen;
                 });
                    estadoImagen = "Lleno";
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
        private async Task SubirImagenesStore()
        {
            VMusuarios funcion = new VMusuarios();
            rutafoto = await funcion.SubirimagenesStorage(file.GetStream(), idusuario);
        }
        private async void btnIcono_Clicked(object sender, EventArgs e)
        {
            idusuario = (sender as ImageButton).CommandParameter.ToString();
            await ObtenerDatosUsuario();
        }
        private async Task ObtenerDatosUsuario()
        {
            VMusuarios funcion = new VMusuarios();
            Musuarios parametros = new Musuarios();
            parametros.Id_usuario = idusuario;
            var dt = await funcion.ObtenerDatosUsuarios(parametros);
            foreach (var fila in dt)
            {
                txtUsuario.Text = fila.Usuario;
                txtContraseña.Text = fila.Pass;
                ImagenCelular.Source = fila.Icono;
                estado = fila.Estado;
                rutafoto = fila.Icono;
            }
        }
        private async Task EliminarUsuario()
        {
            VMusuarios funcion = new VMusuarios();
            Musuarios parametros = new Musuarios();
            parametros.Id_usuario = idusuario;
            await funcion.EliminarUsuarios(parametros);
        }
        private async void btnEliminar_Clicked(object sender, EventArgs e)
        {
            await EliminarUsuario();
            await EliminarImagenxUsuario();
            await MostrarUsuarios();
            await DisplayAlert("Eliminado", "Registro eliminado con éxito", "Cerrar");
            txtContraseña.Text = "";
            txtUsuario.Text = "";
            ImagenCelular.Source = null;
            
        }
        private async Task EliminarImagenxUsuario()
        {
            VMusuarios funcion = new VMusuarios();
            await funcion.EliminarImagen(idusuario + ".jpg");
        }
        private async void btnEditar_Clicked(object sender, EventArgs e)
        {
            if (estadoImagen == "Lleno")
            {
                await EliminarImagenxUsuario();
                await SubirImagenesStore();
            }
            await EditarFoto();
        }
    }
}