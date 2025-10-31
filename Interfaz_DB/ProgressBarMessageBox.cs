using System;
using System.Windows.Forms;

namespace Interfaz_DB
{
    // La clase ProgressBarMessageBox es un formulario personalizado que actúa como una ventana de diálogo (MessageBox)
    // pero con una barra de progreso. Es una clase de utilidad reutilizable para mostrar el progreso de tareas largas.
    public partial class ProgressBarMessageBox : Form
    {
        // Constructor de la clase.
        public ProgressBarMessageBox()
        {
            InitializeComponent(); // Inicializa los componentes diseñados en el editor visual.

            // Establece algunas propiedades visuales básicas del formulario.
            this.Width = 350; // Ancho de la ventana.
            this.Height = 150; // Alto de la ventana.
            this.StartPosition = FormStartPosition.CenterParent; // Hace que la ventana aparezca centrada sobre su ventana padre.
        }

        // --- Propiedades Públicas para Controlar el Formulario ---

        /// <summary>
        /// Obtiene o establece el valor actual de la barra de progreso.
        /// </summary>
        public int ProgressValue
        {
            // 'get' devuelve el valor actual del control ProgressBar.
            get { return progressBar.Value; }
            // 'set' actualiza el valor del control ProgressBar.
            set
            {
                // Se asegura de que el valor esté dentro de los límites (mínimo y máximo) de la barra de progreso antes de asignarlo.
                if (value >= progressBar.Minimum && value <= progressBar.Maximum)
                {
                    progressBar.Value = value;
                }
            }
        }

        /// <summary>
        /// Obtiene o establece el texto que se muestra en la etiqueta de progreso.
        /// </summary>
        public string ProgressText
        {
            // 'get' devuelve el texto actual de la etiqueta.
            get { return labelProgress.Text; }
            // 'set' actualiza el texto de la etiqueta.
            set { labelProgress.Text = value; }
        }

        // --- Método Estático para Facilitar su Uso ---

        /// <summary>
        /// Método estático (de fábrica) que crea, configura y muestra una instancia del formulario.
        /// Esto permite mostrar la barra de progreso con una sola línea de código, de forma similar a como se usa MessageBox.Show().
        /// </summary>
        /// <param name="text">El texto inicial que se mostrará en la etiqueta de progreso.</param>
        /// <param name="caption">El texto que se mostrará en la barra de título de la ventana.</param>
        /// <returns>Devuelve la instancia del formulario creado para que pueda ser manipulado (por ejemplo, para actualizar el progreso o cerrarlo).</returns>
        public static ProgressBarMessageBox ShowProgressBar(string text, string caption)
        {
            // Crea una nueva instancia del formulario.
            var form = new ProgressBarMessageBox
            {
                // Establece el texto y el título usando las propiedades.
                ProgressText = text,
                Text = caption
            };
            // Muestra el formulario. 'Show()' lo abre como una ventana no modal.
            form.Show();
            // Devuelve la referencia al formulario para su control posterior.
            return form;
        }
    }
}
 