using System;
using System.Windows.Forms;

namespace Interfaz_DB
{
    public partial class ProgressBarMessageBox : Form
    {
        public ProgressBarMessageBox()
        {
            InitializeComponent();
            this.Width = 350; 
            this.Height = 150; 
            this.StartPosition = FormStartPosition.CenterParent; 
        }

        // Propiedades para configurar el progreso y el texto
        public int ProgressValue
        {
            get { return progressBar.Value; }
            set
            {
                if (value >= progressBar.Minimum && value <= progressBar.Maximum)
                {
                    progressBar.Value = value;
                }
            }
        }

        public string ProgressText
        {
            get { return labelProgress.Text; }
            set { labelProgress.Text = value; }
        }

        // Método estático para mostrar el MessageBox con progreso
        public static ProgressBarMessageBox ShowProgressBar(string text, string caption)
        {
            var form = new ProgressBarMessageBox
            {
                ProgressText = text,
                Text = caption
            };
            form.Show();
            return form;
        }
    }
}
 