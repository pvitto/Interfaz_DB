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
    public partial class Home : Form
    { 
        public Home()
        {
            InitializeComponent();

            imagen_LogoAgrocosta.Image = Interfaz_DB.Properties.Resources.logo;
            imagen_LogoAgrocosta.SizeMode = PictureBoxSizeMode.StretchImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Inventario formulario_inventario = new Inventario();
            formulario_inventario.Show(); // Abre el formulario de Inventario

        }

        private void boton_Pedidos_Click(object sender, EventArgs e)
        {
            Pedidos formulario_pedidos = new Pedidos();
            formulario_pedidos.Show();
        }

        private void boton_Usuarios_Click(object sender, EventArgs e)
        {
            Usuarios formulario_usuarios = new Usuarios();
            formulario_usuarios.Show();
        }
    }
}
