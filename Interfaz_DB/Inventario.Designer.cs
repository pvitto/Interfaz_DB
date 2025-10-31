namespace Interfaz_DB
{
    partial class Inventario
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Inventario));
            this.tabla_Inventario = new System.Windows.Forms.DataGridView();
            this.txtReferencia = new System.Windows.Forms.TextBox();
            this.txtLinea = new System.Windows.Forms.TextBox();
            this.txtTipo = new System.Windows.Forms.TextBox();
            this.boton_Buscar = new System.Windows.Forms.Button();
            this.boton_GuardarCambios = new System.Windows.Forms.Button();
            this.boton_SubirDatos = new System.Windows.Forms.Button();
            this.boton_Cargar = new System.Windows.Forms.Button();
            this.boton_ExportarExcel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tabla_Inventario)).BeginInit();
            this.SuspendLayout();
            // 
            // tabla_Inventario
            // 
            this.tabla_Inventario.BackgroundColor = System.Drawing.SystemColors.ControlDark;
            this.tabla_Inventario.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tabla_Inventario.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tabla_Inventario.Location = new System.Drawing.Point(29, 44);
            this.tabla_Inventario.Name = "tabla_Inventario";
            this.tabla_Inventario.ReadOnly = true;
            this.tabla_Inventario.Size = new System.Drawing.Size(743, 360);
            this.tabla_Inventario.TabIndex = 5;
            this.tabla_Inventario.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.tabla_Inventario_CellContentClick);
            // 
            // txtReferencia
            // 
            this.txtReferencia.Location = new System.Drawing.Point(148, 18);
            this.txtReferencia.Name = "txtReferencia";
            this.txtReferencia.Size = new System.Drawing.Size(100, 20);
            this.txtReferencia.TabIndex = 6;
            // 
            // txtLinea
            // 
            this.txtLinea.Location = new System.Drawing.Point(287, 18);
            this.txtLinea.Name = "txtLinea";
            this.txtLinea.Size = new System.Drawing.Size(100, 20);
            this.txtLinea.TabIndex = 7;
            // 
            // txtTipo
            // 
            this.txtTipo.Location = new System.Drawing.Point(425, 18);
            this.txtTipo.Name = "txtTipo";
            this.txtTipo.Size = new System.Drawing.Size(100, 20);
            this.txtTipo.TabIndex = 8;
            // 
            // boton_Buscar
            // 
            this.boton_Buscar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Buscar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Buscar.Location = new System.Drawing.Point(564, 15);
            this.boton_Buscar.Name = "boton_Buscar";
            this.boton_Buscar.Size = new System.Drawing.Size(75, 23);
            this.boton_Buscar.TabIndex = 9;
            this.boton_Buscar.Text = "Buscar";
            this.boton_Buscar.UseVisualStyleBackColor = false;
            this.boton_Buscar.Click += new System.EventHandler(this.boton_Buscar_Click);
            // 
            // boton_GuardarCambios
            // 
            this.boton_GuardarCambios.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_GuardarCambios.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.boton_GuardarCambios.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_GuardarCambios.Location = new System.Drawing.Point(201, 410);
            this.boton_GuardarCambios.Name = "boton_GuardarCambios";
            this.boton_GuardarCambios.Size = new System.Drawing.Size(74, 23);
            this.boton_GuardarCambios.TabIndex = 10;
            this.boton_GuardarCambios.Text = "Borrar";
            this.boton_GuardarCambios.UseVisualStyleBackColor = false;
            this.boton_GuardarCambios.Click += new System.EventHandler(this.boton_GuardarCambios_Click);
            // 
            // boton_SubirDatos
            // 
            this.boton_SubirDatos.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_SubirDatos.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_SubirDatos.Location = new System.Drawing.Point(309, 410);
            this.boton_SubirDatos.Name = "boton_SubirDatos";
            this.boton_SubirDatos.Size = new System.Drawing.Size(91, 23);
            this.boton_SubirDatos.TabIndex = 11;
            this.boton_SubirDatos.Text = "Subir Cambios";
            this.boton_SubirDatos.UseVisualStyleBackColor = false;
            this.boton_SubirDatos.Click += new System.EventHandler(this.boton_SubirDatos_Click);
            // 
            // boton_Cargar
            // 
            this.boton_Cargar.BackColor = System.Drawing.SystemColors.ControlLight;
            this.boton_Cargar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.boton_Cargar.Location = new System.Drawing.Point(433, 410);
            this.boton_Cargar.Name = "boton_Cargar";
            this.boton_Cargar.Size = new System.Drawing.Size(75, 23);
            this.boton_Cargar.TabIndex = 12;
            this.boton_Cargar.Text = "Cargar";
            this.boton_Cargar.UseVisualStyleBackColor = false;
            this.boton_Cargar.Click += new System.EventHandler(this.boton_Cargar_Click);
            // 
            // boton_ExportarExcel
            // 
            this.boton_ExportarExcel.Location = new System.Drawing.Point(543, 410);
            this.boton_ExportarExcel.Name = "boton_ExportarExcel";
            this.boton_ExportarExcel.Size = new System.Drawing.Size(75, 23);
            this.boton_ExportarExcel.TabIndex = 13;
            this.boton_ExportarExcel.Text = "Exportar";
            this.boton_ExportarExcel.UseVisualStyleBackColor = true;
            this.boton_ExportarExcel.Click += new System.EventHandler(this.boton_ExportarExcel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(666, 410);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "label1";
            // 
            // Inventario
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
            this.Controls.Add(this.boton_Buscar);
            this.Controls.Add(this.txtTipo);
            this.Controls.Add(this.txtLinea);
            this.Controls.Add(this.txtReferencia);
            this.Controls.Add(this.tabla_Inventario);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Inventario";
            this.Text = "Inventario";
            ((System.ComponentModel.ISupportInitialize)(this.tabla_Inventario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView tabla_Inventario;
        private System.Windows.Forms.TextBox txtReferencia;
        private System.Windows.Forms.TextBox txtLinea;
        private System.Windows.Forms.TextBox txtTipo;
        private System.Windows.Forms.Button boton_Buscar;
        private System.Windows.Forms.Button boton_GuardarCambios;
        private System.Windows.Forms.Button boton_SubirDatos;
        private System.Windows.Forms.Button boton_Cargar;
        private System.Windows.Forms.Button boton_ExportarExcel;
        private System.Windows.Forms.Label label1;
    }
}