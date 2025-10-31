using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interfaz_DB
{
    // La clase Program es el punto de entrada de la aplicación.
    // Es una clase estática, lo que significa que no se pueden crear instancias de ella.
    internal static class Program
    {
        /// <summary>
        /// El método Main es el punto de entrada principal para cualquier aplicación de C#.
        /// Se ejecuta automáticamente cuando se inicia el programa.
        /// </summary>
        [STAThread] // Este atributo indica que la aplicación utilizará un modelo de subprocesos de un único subproceso (Single-Threaded Apartment), que es necesario para las aplicaciones de Windows Forms.
        static void Main()
        {
            // Application.EnableVisualStyles() habilita los estilos visuales para la aplicación,
            // lo que hace que los controles (botones, ventanas, etc.) adopten la apariencia moderna del sistema operativo.
            Application.EnableVisualStyles();

            // Application.SetCompatibleTextRenderingDefault(false) establece el método de renderizado de texto predeterminado.
            // Se recomienda establecerlo en 'false' para usar el motor de renderizado de texto más nuevo (GDI+).
            Application.SetCompatibleTextRenderingDefault(false);

            // Application.Run() inicia el bucle de mensajes de la aplicación y muestra el formulario especificado.
            // Aquí, 'new Home()' crea una nueva instancia del formulario 'Home' y lo convierte en la ventana principal de la aplicación.
            Application.Run(new Home());
        }
    }
}
