namespace Interfaz_DB
{
    partial class Pedidos
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Pedidos));
            this.boton_Buscar = new System.Windows.Forms.Button();
            this.txtReferencia = new System.Windows.Forms.TextBox();
            this.txtIdPedido = new System.Windows.Forms.TextBox();
            this.tabla_Pedidos = new System.Windows.Forms.DataGridView();
            this.dateTimePickerFechaPedido = new System.Windows.Forms.DateTimePicker();
            this.boton_GuardarCambios = new System.Windows.Forms.Button();
            this.boton_SubirDatos = new System.Windows.Forms.Button();
            this.boton_Cargar = new System.Windows.Forms.Button();
            this.boton_ExportarExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tabla_Pedidos)).BeginInit();
            this.SuspendLayout();
            // 
            // boton_Buscar
            // 
            this.boton_Buscar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Buscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Buscar.Location = new System.Drawing.Point(574, 21);
            this.boton_Buscar.Name = "boton_Buscar";
            this.boton_Buscar.Size = new System.Drawing.Size(75, 23);
            this.boton_Buscar.TabIndex = 14;
            this.boton_Buscar.Text = "Buscar";
            this.boton_Buscar.UseVisualStyleBackColor = false;
            this.boton_Buscar.Click += new System.EventHandler(this.boton_Buscar_Click_1);
            // 
            // txtReferencia
            // 
            this.txtReferencia.Location = new System.Drawing.Point(282, 24);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.Size = new System.Drawing.Size(100, 20);
            this.txtReferencia.TabIndex = 12;
            // 
            // txtIdPedido
            // 
            this.txtIdPedido.Location = new System.Drawing.Point(153, 24);
            this.txtIdPedido.Name = "txtIdPedido";
            this.txtIdPedido.Size = new System.Drawing.Size(100, 20);
            this.txtIdPedido.TabIndex = 11;
            // 
            // tabla_Pedidos
            // 
            this.tabla_Pedidos.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.tabla_Pedidos.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabla_Pedidos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabla_Pedidos.Location = new System.Drawing.Point(115, 50);
            this.tabla_Pedidos.Name = "tabla_Pedidos";
            this.tabla_Pedidos.ReadOnly = true;
            this.tabla_Pedidos.Size = new System.Drawing.Size(564, 339);
            this.tabla_Pedidos.TabIndex = 10;
            this.tabla_Pedidos.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabla_Pedidos_CellContentClick);
            // 
            // dateTimePickerFechaPedido
            // 
            this.dateTimePickerFechaPedido.Checked = false;
            this.dateTimePickerFechaPedido.Location = new System.Drawing.Point(411, 24);
            this.dateTimePickerFechaPedido.Name = "dateTimePickerFechaPedido";
            this.dateTimePickerFechaPedido.ShowCheckBox = true;
            this.dateTimePickerFechaPedido.Size = new System.Drawing.Size(127, 20);
            this.dateTimePickerFechaPedido.TabIndex = 15;
            // 
            // boton_GuardarCambios
            // 
            this.boton_GuardarCambios.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_GuardarCambios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_GuardarCambios.Location = new System.Drawing.Point(191, 395);
            this.boton_GuardarCambios.Name = "boton_GuardarCambios";
            this.boton_GuardarCambios.Size = new System.Drawing.Size(78, 23);
            this.boton_GuardarCambios.TabIndex = 16;
            this.boton_GuardarCambios.Text = "Borrar";
            this.boton_GuardarCambios.UseVisualStyleBackColor = false;
            this.boton_GuardarCambios.Click += new System.EventHandler(this.boton_GuardarCambios_Click);
            // 
            // boton_SubirDatos
            // 
            this.boton_SubirDatos.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_SubirDatos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_SubirDatos.Location = new System.Drawing.Point(298, 396);
            this.boton_SubirDatos.Name = "boton_SubirDatos";
            this.boton_SubirDatos.Size = new System.Drawing.Size(91, 23);
            this.boton_SubirDatos.TabIndex = 17;
            this.boton_SubirDatos.Text = "Subir Cambios";
            this.boton_SubirDatos.UseVisualStyleBackColor = false;
            this.boton_SubirDatos.Click += new System.EventHandler(this.boton_SubirDatos_Click);
            // 
            // boton_Cargar
            // 
            this.boton_Cargar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Cargar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Cargar.Location = new System.Drawing.Point(421, 395);
            this.boton_Cargar.Name = "boton_Cargar";
            this.boton_Cargar.Size = new System.Drawing.Size(75, 23);
            this.boton_Cargar.TabIndex = 18;
            this.boton_Cargar.Text = "Cargar";
            this.boton_Cargar.UseVisualStyleBackColor = false;
            this.boton_Cargar.Click += new System.EventHandler(this.boton_Cargar_Click);
            // 
            // boton_ExportarExcel
            // 
            this.boton_ExportarExcel.Location = new System.Drawing.Point(525, 395);
            this.boton_ExportarExcel.Name = "boton_ExportarExcel";
            this.boton_ExportarExcel.Size = new System.Drawing.Size(75, 23);
            this.boton_ExportarExcel.TabIndex = 19;
            this.boton_ExportarExcel.Text = "Exportar";
            this.boton_ExportarExcel.UseVisualStyleBackColor = true;
            this.boton_ExportarExcel.Click += new System.EventHandler(this.boton_ExportarExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(675, 401);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 20;
            this.label1.Text = "label1";
            // 
            // Pedidos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.boton_ExportarExcel);
            this.Controls.Add(this.boton_Cargar);
            this.Controls.Add(this.boton_SubirDatos);
            this.Controls.Add(this.boton_GuardarCambios);
            this.Controls.Add(this.dateTimePickerFechaPedido);
            this.Controls.Add(this.boton_Buscar);
            this.Controls.Add(this.txtReferencia);
            this.Controls.Add(this.txtIdPedido);
            this.Controls.Add(this.tabla_Pedidos);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Pedidos";
            this.Text = "Pedidos";
            this.Load += new System.EventHandler(this.Pedidos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabla_Pedidos)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button boton_Buscar;
        private System.Windows.Forms.TextBox txtReferencia;
        private System.Windows.Forms.TextBox txtIdPedido;
        private System.Windows.Forms.DataGridView tabla_Pedidos;
        private System.Windows.Forms.DateTimePicker dateTimePickerFechaPedido;
        private System.Windows.Forms.Button boton_GuardarCambios;
        private System.Windows.Forms.Button boton_SubirDatos;
        private System.Windows.Forms.Button boton_Cargar;
        private System.Windows.Forms.Button boton_ExportarExcel;
        private System.Windows.Forms.Label label1;
    }
}