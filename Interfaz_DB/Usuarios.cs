using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Interfaz_DB
{
    public partial class Usuarios : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private string currentSortColumn = "Idusuario";
        private string currentSortOrder = "ASC";

        private readonly string connectionString_Pagina = "Server=65.109.49.57;Database=agrocosta_db;Uid=agrocosta_pagina;Pwd=Agr0costa*-;";
        private readonly string connectionString_Traverse = "SERVER=WIN-HOSNT32J3;DATABASE=AGR;UID=SISTEMAS;PWD=Costa2035***--";

        private DataView dataView;

        public Usuarios()
        {
            InitializeComponent();
            SetCueBanner(txtNombre, "Filtrar por Nombre");
            SetCueBanner(txtUsuario, "Filtrar por Usuario");

            tabla_Usuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LoadData();
        }

        private void SetCueBanner(TextBox textBox, string cueText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cueText);
        }

        private void LoadData()
        {
            string query = @"
                EXEC [dbo].[PaginaWeb_SoUsuariosWeb]
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString_Traverse))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Cambiar "p" por "d" en los nombres de columnas excepto "Pass"
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                if (column.ColumnName != "Pass" && column.ColumnName.Contains("P"))
                                {
                                    column.ColumnName = column.ColumnName.Replace("P", "d");
                                }
                            }

                            // Cambiar valores NULL a 0 en todas las celdas
                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (DataColumn col in dataTable.Columns)
                                {
                                    col.ReadOnly = false;
                                    if (row.IsNull(col))
                                    {
                                        row[col] = 0;
                                    }
                                    else if (col.DataType == typeof(double) || col.DataType == typeof(float) || col.DataType == typeof(decimal))
                                    {
                                        row[col] = Convert.ToInt32(row[col]);
                                    }
                                    col.ReadOnly = true;
                                }
                            }

                            // Asignar los datos al DataView para filtrado y ordenamiento local
                            dataView = dataTable.DefaultView;
                            tabla_Usuarios.DataSource = dataView;

                            label1.Text = $"Número de filas: {tabla_Usuarios.Rows.Count}";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void boton_Cargar_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Datos han sido correctamente importados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void boton_ExportarExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Guardar como Excel";
            saveFileDialog.FileName = "Usuarios.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Usuarios");

                    // Agregar encabezados de columna
                    for (int i = 0; i < tabla_Usuarios.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = tabla_Usuarios.Columns[i].HeaderText;
                    }

                    // Agregar filas
                    for (int i = 0; i < tabla_Usuarios.Rows.Count; i++)
                    {
                        for (int j = 0; j < tabla_Usuarios.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j + 1).Value = tabla_Usuarios.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente a Excel", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void boton_Buscar_Click(object sender, EventArgs e)
        {
            StringBuilder filter = new StringBuilder();
            if (!string.IsNullOrEmpty(txtUsuario.Text))
            {
                filter.AppendFormat("Idusuario LIKE '%{0}%'", txtUsuario.Text);
            }
            if (!string.IsNullOrEmpty(txtNombre.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.AppendFormat("Nombre LIKE '%{0}%'", txtNombre.Text);
            }

            dataView.RowFilter = filter.ToString();
            label1.Text = $"Número de filas: {dataView.Count}";
        }

        private async void boton_SubirDatos_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de que desea subir los datos del DataGridView a la base de datos? Esto eliminará toda la información previa en la base de datos.", "Confirmar subida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Enabled = false;

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString_Pagina))
                    {
                        await connection.OpenAsync();

                        // Eliminar la tabla existente
                        string dropTableQuery = "DROP TABLE IF EXISTS usuarios";
                        using (MySqlCommand dropTableCommand = new MySqlCommand(dropTableQuery, connection))
                        {
                            await dropTableCommand.ExecuteNonQueryAsync();
                        }

                        // Crear la tabla de nuevo basada en las columnas del DataGridView
                        StringBuilder createTableQuery = new StringBuilder("CREATE TABLE usuarios (");
                        foreach (DataGridViewColumn column in tabla_Usuarios.Columns)
                        {
                            string columnName = column.Name;
                            string columnType = columnName.StartsWith("d") ? "INT" : "VARCHAR(255)";
                            string nullConstraint = columnName.Equals("Nombre") ? "NULL" : "NOT NULL";
                            createTableQuery.AppendFormat("{0} {1} {2},", columnName, columnType, nullConstraint);
                        }
                        createTableQuery.Length--; // Eliminar la última coma
                        createTableQuery.Append(")");

                        using (MySqlCommand createTableCommand = new MySqlCommand(createTableQuery.ToString(), connection))
                        {
                            await createTableCommand.ExecuteNonQueryAsync();
                        }

                        // Calcular el número total de filas para el progreso
                        int totalRows = tabla_Usuarios.Rows.Count;
                        int currentRow = 0;

                        // Mostrar el MessageBox personalizado con barra de progreso y texto dinámico
                        var progressMessageBox = ProgressBarMessageBox.ShowProgressBar("Subiendo datos...", "Progreso");
                        progressMessageBox.Show();

                        // Construir el comando INSERT con todas las filas
                        StringBuilder insertQuery = new StringBuilder("INSERT INTO usuarios (");
                        foreach (DataGridViewColumn column in tabla_Usuarios.Columns)
                        {
                            insertQuery.AppendFormat("{0},", column.Name);
                        }
                        insertQuery.Length--; // Eliminar la última coma
                        insertQuery.Append(") VALUES ");

                        await Task.Run(() =>
                        {
                            foreach (DataGridViewRow row in tabla_Usuarios.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    insertQuery.Append("(");
                                    foreach (DataGridViewColumn column in tabla_Usuarios.Columns)
                                    {
                                        string columnName = column.Name;
                                        object value = columnName.StartsWith("d") ? Convert.ToInt32(row.Cells[columnName].Value) : row.Cells[columnName].Value ?? DBNull.Value;
                                        insertQuery.AppendFormat("'{0}',", value);
                                    }
                                    insertQuery.Length--; // Eliminar la última coma
                                    insertQuery.Append("),");

                                    currentRow++;
                                    int progressPercentage = (int)((double)currentRow / totalRows * 100);
                                    progressMessageBox.Invoke((Action)(() =>
                                    {
                                        progressMessageBox.ProgressText = $"Progreso: {progressPercentage}% ({currentRow} de {totalRows} líneas preparadas)";
                                        progressMessageBox.ProgressValue = progressPercentage;
                                    }));
                                }
                            }
                        });

                        insertQuery.Length--; // Eliminar la última coma

                        // Ejecutar el comando INSERT
                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery.ToString(), connection))
                        {
                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        progressMessageBox.Close();
                        MessageBox.Show("Datos subidos correctamente a la base de datos", "Subida completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show($"Error específico de MySQL al subir los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error general al subir los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Enabled = true;
                }
            }
        }



    }
}