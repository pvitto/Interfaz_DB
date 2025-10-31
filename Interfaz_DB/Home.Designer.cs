namespace Interfaz_DB
{
    partial class Home
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home));
            this.boton_Inventario = new System.Windows.Forms.Button();
            this.boton_Pedidos = new System.Windows.Forms.Button();
            this.imagen_LogoAgrocosta = new System.Windows.Forms.PictureBox();
            this.boton_Usuarios = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imagen_LogoAgrocosta)).BeginInit();
            this.SuspendLayout();
            // 
            // boton_Inventario
            // 
            this.boton_Inventario.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Inventario.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Inventario.Location = new System.Drawing.Point(222, 274);
            this.boton_Inventario.Name = "boton_Inventario";
            this.boton_Inventario.Size = new System.Drawing.Size(97, 46);
            this.boton_Inventario.TabIndex = 0;
            this.boton_Inventario.Text = "Inventario";
            this.boton_Inventario.UseVisualStyleBackColor = false;
            this.boton_Inventario.Click += new System.EventHandler(this.button1_Click);
            // 
            // boton_Pedidos
            // 
            this.boton_Pedidos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Pedidos.Location = new System.Drawing.Point(495, 274);
            this.boton_Pedidos.Name = "boton_Pedidos";
            this.boton_Pedidos.Size = new System.Drawing.Size(92, 46);
            this.boton_Pedidos.TabIndex = 1;
            this.boton_Pedidos.Text = "Pedidos";
            this.boton_Pedidos.UseVisualStyleBackColor = true;
            this.boton_Pedidos.Click += new System.EventHandler(this.boton_Pedidos_Click);
            // 
            // imagen_LogoAgrocosta
            // 
            this.imagen_LogoAgrocosta.BackColor = System.Drawing.SystemColors.ControlLight;
            this.imagen_LogoAgrocosta.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.imagen_LogoAgrocosta.Location = new System.Drawing.Point(222, 74);
            this.imagen_LogoAgrocosta.Name = "imagen_LogoAgrocosta";
            this.imagen_LogoAgrocosta.Size = new System.Drawing.Size(365, 164);
            this.imagen_LogoAgrocosta.TabIndex = 2;
            this.imagen_LogoAgrocosta.TabStop = false;
            // 
            // boton_Usuarios
            // 
            this.boton_Usuarios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Usuarios.Location = new System.Drawing.Point(362, 274);
            this.boton_Usuarios.Name = "boton_Usuarios";
            this.boton_Usuarios.Size = new System.Drawing.Size(92, 46);
            this.boton_Usuarios.TabIndex = 3;
            this.boton_Usuarios.Text = "Usuarios";
            this.boton_Usuarios.UseVisualStyleBackColor = true;
            this.boton_Usuarios.Click += new System.EventHandler(this.boton_Usuarios_Click);
            // 
            // Home
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.boton_Usuarios);
            this.Controls.Add(this.imagen_LogoAgrocosta);
            this.Controls.Add(this.boton_Pedidos);
            this.Controls.Add(this.boton_Inventario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home";
            this.Text = " ";
            ((System.ComponentModel.ISupportInitialize)(this.imagen_LogoAgrocosta)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button boton_Inventario;
        private System.Windows.Forms.Button boton_Pedidos;
        private System.Windows.Forms.PictureBox imagen_LogoAgrocosta;
        private System.Windows.Forms.Button boton_Usuarios;
    }
}

