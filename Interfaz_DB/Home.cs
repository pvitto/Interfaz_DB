using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Interfaz_DB
{
    // La clase Home representa la ventana principal de la aplicación, que actúa como un menú de navegación.
    // 'partial class' indica que la definición de esta clase puede estar dividida en varios archivos.
    // En este caso, la otra parte está en Home.Designer.cs, que contiene el código generado por el diseñador de Windows Forms.
    public partial class Home : Form
    {
        // Este es el constructor de la clase Home. Se ejecuta cuando se crea una nueva instancia de la ventana.
        public Home()
        {
            // InitializeComponent() es un método generado automáticamente por el diseñador de Visual Studio.
            // Carga todos los controles visuales (botones, etiquetas, etc.) que se definieron en el diseñador.
            InitializeComponent();

            // Carga la imagen del logo desde los recursos del proyecto.
            // 'Interfaz_DB.Properties.Resources.logo' accede al recurso de imagen llamado 'logo'.
            imagen_LogoAgrocosta.Image = Interfaz_DB.Properties.Resources.logo;

            // Ajusta el tamaño de la imagen para que se estire y llene completamente el control PictureBox.
            imagen_LogoAgrocosta.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        // Este método es un "manejador de eventos". Se ejecuta cuando el usuario hace clic en el botón 'button1' (probablemente el botón de Inventario).
        // 'object sender' es el control que desencadenó el evento (el botón).
        // 'EventArgs e' contiene los datos del evento.
        private void button1_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia del formulario de Inventario.
            Inventario formulario_inventario = new Inventario();

            // Muestra el formulario de Inventario en una nueva ventana no modal.
            // 'Show()' permite al usuario interactuar con la ventana principal mientras la de inventario está abierta.
            formulario_inventario.Show();
        }

        // Este manejador de eventos se ejecuta cuando el usuario hace clic en el botón 'boton_Pedidos'.
        private void boton_Pedidos_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia del formulario de Pedidos.
            Pedidos formulario_pedidos = new Pedidos();

            // Muestra el formulario de Pedidos.
            formulario_pedidos.Show();
        }

        // Este manejador de eventos se ejecuta cuando el usuario hace clic en el botón 'boton_Usuarios'.
        private void boton_Usuarios_Click(object sender, EventArgs e)
        {
            // Crea una nueva instancia del formulario de Usuarios.
            Usuarios formulario_usuarios = new Usuarios();

            // Muestra el formulario de Usuarios.
            formulario_usuarios.Show();
        }
    }
}
