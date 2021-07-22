using System;
using System.Collections.Generic;
using System.Text;
using Firebase.Database;

namespace PasarSQLaFirebase.Servicios
{
    public class Conexionfirebase
    {
        public static FirebaseClient firabese = new FirebaseClient("https://usuarios-5dfcb-default-rtdb.firebaseio.com/");
    }
}
