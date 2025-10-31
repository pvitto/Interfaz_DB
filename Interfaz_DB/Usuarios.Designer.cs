namespace Interfaz_DB
{
    partial class Usuarios
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.boton_ExportarExcel = new System.Windows.Forms.Button();
            this.boton_Cargar = new System.Windows.Forms.Button();
            this.boton_SubirDatos = new System.Windows.Forms.Button();
            this.tabla_Usuarios = new System.Windows.Forms.DataGridView();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtUsuario = new System.Windows.Forms.TextBox();
            this.boton_Buscar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.tabla_Usuarios)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(666, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "label1";
            // 
            // boton_ExportarExcel
            // 
            this.boton_ExportarExcel.Location = new System.Drawing.Point(474, 411);
            this.boton_ExportarExcel.Name = "boton_ExportarExcel";
            this.boton_ExportarExcel.Size = new System.Drawing.Size(75, 23);
            this.boton_ExportarExcel.TabIndex = 19;
            this.boton_ExportarExcel.Text = "Exportar";
            this.boton_ExportarExcel.UseVisualStyleBackColor = true;
            this.boton_ExportarExcel.Click += new System.EventHandler(this.boton_ExportarExcel_Click);
            // 
            // boton_Cargar
            // 
            this.boton_Cargar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Cargar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Cargar.Location = new System.Drawing.Point(364, 411);
            this.boton_Cargar.Name = "boton_Cargar";
            this.boton_Cargar.Size = new System.Drawing.Size(75, 23);
            this.boton_Cargar.TabIndex = 18;
            this.boton_Cargar.Text = "Cargar";
            this.boton_Cargar.UseVisualStyleBackColor = false;
            this.boton_Cargar.Click += new System.EventHandler(this.boton_Cargar_Click);
            // 
            // boton_SubirDatos
            // 
            this.boton_SubirDatos.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_SubirDatos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_SubirDatos.Location = new System.Drawing.Point(240, 411);
            this.boton_SubirDatos.Name = "boton_SubirDatos";
            this.boton_SubirDatos.Size = new System.Drawing.Size(91, 23);
            this.boton_SubirDatos.TabIndex = 17;
            this.boton_SubirDatos.Text = "Subir Cambios";
            this.boton_SubirDatos.UseVisualStyleBackColor = false;
            this.boton_SubirDatos.Click += new System.EventHandler(this.boton_SubirDatos_Click);
            // 
            // tabla_Usuarios
            // 
            this.tabla_Usuarios.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.tabla_Usuarios.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabla_Usuarios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabla_Usuarios.Location = new System.Drawing.Point(29, 44);
            this.tabla_Usuarios.Name = "tabla_Usuarios";
            this.tabla_Usuarios.ReadOnly = true;
            this.tabla_Usuarios.Size = new System.Drawing.Size(743, 360);
            this.tabla_Usuarios.TabIndex = 15;
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(362, 18);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(100, 20);
            this.txtNombre.TabIndex = 22;
            // 
            // txtUsuario
            // 
            this.txtUsuario.Location = new System.Drawing.Point(233, 18);
            this.txtUsuario.Name = "txtUsuario";
            this.txtUsuario.Size = new System.Drawing.Size(100, 20);
            this.txtUsuario.TabIndex = 21;
            // 
            // boton_Buscar
            // 
            this.boton_Buscar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Buscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Buscar.Location = new System.Drawing.Point(492, 15);
            this.boton_Buscar.Name = "boton_Buscar";
            this.boton_Buscar.Size = new System.Drawing.Size(75, 23);
            this.boton_Buscar.TabIndex = 23;
            this.boton_Buscar.Text = "Buscar";
            this.boton_Buscar.UseVisualStyleBackColor = false;
            this.boton_Buscar.Click += new System.EventHandler(this.boton_Buscar_Click);
            // 
            // Usuarios
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.boton_Buscar);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.txtUsuario);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.boton_ExportarExcel);
            this.Controls.Add(this.boton_Cargar);
            this.Controls.Add(this.boton_SubirDatos);
            this.Controls.Add(this.tabla_Usuarios);
            this.Name = "Usuarios";
            this.Text = "Usuario";
            ((System.ComponentModel.ISupportInitialize)(this.tabla_Usuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button boton_ExportarExcel;
        private System.Windows.Forms.Button boton_Cargar;
        private System.Windows.Forms.Button boton_SubirDatos;
        private System.Windows.Forms.DataGridView tabla_Usuarios;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtUsuario;
        private System.Windows.Forms.Button boton_Buscar;
    }
}