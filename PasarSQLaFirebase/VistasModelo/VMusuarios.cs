using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Storage;
using PasarSQLaFirebase.Modelo;
using System.Threading.Tasks;
using PasarSQLaFirebase.Servicios;
using Firebase.Database.Query;
using System.IO;
using System.Linq;

namespace PasarSQLaFirebase.VistasModelo
{
    public class VMusuarios
    {
        //lista -> datos a utilizar -> nombre de la lista -> se le asigna el valor de  new etc;
        List<Musuarios> Usuarios = new List<Musuarios>();
        string rutaFoto;
        string Idusuario;
        public async Task <List<Musuarios>> mostrar_usuarios()
        {
            //declaramos el tipo de dato que va a realizar la tarea -> await espera la tarea a realizar
            var data = await Conexionfirebase.firabese
                .Child("Usuarios")
                .OrderByKey()
                .OnceAsync<Musuarios>();
                //child est´preguntando desde Firebase "¿A que tabla deseas acceder?"
                //OrderBy Key Ordena los registros
                foreach(var rdr in data)
            {
                Musuarios parametros = new Musuarios();
                //Key es el ID que crea firebase por defecto
                parametros.Id_usuario = rdr.Key;
                parametros.Usuario = rdr.Object.Usuario;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;
                parametros.Estado = rdr.Object.Estado;
                Usuarios.Add(parametros);
            }
            return Usuarios;
        }
        public async Task <string> insertar_usuarios(Musuarios parametros)
        {
            var datat = await Conexionfirebase.firabese
                .Child("Usuarios")
                .PostAsync(new Musuarios()
                {
                    Usuario = parametros.Usuario,
                    Pass = parametros.Pass,
                    Icono = parametros.Icono,
                    Estado = parametros.Estado
                });
            Idusuario = datat.Key;
            return Idusuario;
        }
        public async Task<string> SubirimagenesStorage(Stream ImagenStream,string Idusuarios)
        {
            var dataAlmacenamiento = await new FirebaseStorage("usuarios-5dfcb.appspot.com")
                .Child("Usuarios")
                .Child(Idusuarios + ".jpg")
                .PutAsync(ImagenStream);
            rutaFoto = dataAlmacenamiento;
            return rutaFoto;
        }
        public async Task EditarFoto(Musuarios parametros)
        {
            var obtenerData = (await Conexionfirebase.firabese
                .Child("Usuarios")
                .OnceAsync<Musuarios>()).Where(a => a.Key == parametros.Id_usuario).FirstOrDefault();
            await Conexionfirebase.firabese
                .Child("Usuarios")
                .Child(obtenerData.Key)
                .PutAsync(new Musuarios()
                {
                    Usuario=parametros.Usuario,
                    Pass=parametros.Pass,
                    Icono= parametros.Icono,
                    Estado=parametros.Estado
                });
        }
        public async Task EliminarUsuarios(Musuarios parametros)
        {
            var data = (await Conexionfirebase.firabese
                .Child("Usuarios")
                .OnceAsync<Musuarios>()).Where(a => a.Key == parametros.Id_usuario).FirstOrDefault();
            await Conexionfirebase.firabese
                .Child("Usuarios").Child(data.Key).DeleteAsync();
        }
        public async Task EliminarImagen(string nombre)
        {
            await new FirebaseStorage("usuarios-5dfcb.appspot.com").Child("Usuarios")
                .Child(nombre)
                .DeleteAsync();
        }
        public async Task <List<Musuarios>>ObtenerDatosUsuarios(Musuarios parametros)
        {
            var data = (await Conexionfirebase.firabese
                .Child("Usuarios")
                .OrderByKey()
                .OnceAsync<Musuarios>()).Where(a => a.Key == parametros.Id_usuario);

            foreach(var rdr in data)
            {
                parametros.Usuario = rdr.Object.Usuario;
                parametros.Pass = rdr.Object.Pass;
                parametros.Icono = rdr.Object.Icono;
                parametros.Estado = rdr.Object.Estado;
                Usuarios.Add(parametros);
            }
            return Usuarios;
        }
    }
}
