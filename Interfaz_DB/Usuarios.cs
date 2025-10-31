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
    // La clase Usuarios representa la ventana de gestión de usuarios.
    // Sigue un patrón similar a las otras, pero con una estrategia de actualización de base de datos diferente y más drástica.
    public partial class Usuarios : Form
    {
        // --- Interoperabilidad y Cadenas de Conexión (igual que en las otras clases) ---
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        // Variables de estado y cadenas de conexión
        private string currentSortColumn = "Idusuario";
        private string currentSortOrder = "ASC";
        private readonly string connectionString_Pagina = "Server=65.109.49.57;Database=agrocosta_db;Uid=agrocosta_pagina;Pwd=Agr0costa*-;";
        private readonly string connectionString_Traverse = "SERVER=WIN-HOSNT32J3;DATABASE=AGR;UID=SISTEMAS;PWD=Costa2035***--";
        private DataView dataView;

        // --- Constructor ---
        public Usuarios()
        {
            InitializeComponent();
            SetCueBanner(txtNombre, "Filtrar por Nombre");
            SetCueBanner(txtUsuario, "Filtrar por Usuario");
            tabla_Usuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LoadData();
        }

        // Método auxiliar para placeholders.
        private void SetCueBanner(TextBox textBox, string cueText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cueText);
        }

        // --- Método de Carga y Procesamiento de Datos ---
        private void LoadData()
        {
            // Ejecuta un procedimiento almacenado para obtener los datos de los usuarios.
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

                            // --- Transformación de Datos ---
                            // Itera sobre las columnas para renombrar algunas de ellas.
                            // Esto puede ser para hacer coincidir los nombres con la tabla de destino en MySQL.
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                if (column.ColumnName != "Pass" && column.ColumnName.Contains("P"))
                                {
                                    column.ColumnName = column.ColumnName.Replace("P", "d");
                                }
                            }

                            // Itera sobre todas las celdas para limpiar los datos.
                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (DataColumn col in dataTable.Columns)
                                {
                                    col.ReadOnly = false; // Permite la modificación temporal.
                                    // Cambia los valores NULL a 0.
                                    if (row.IsNull(col))
                                    {
                                        row[col] = 0;
                                    }
                                    // Convierte valores decimales/flotantes a enteros.
                                    else if (col.DataType == typeof(double) || col.DataType == typeof(float) || col.DataType == typeof(decimal))
                                    {
                                        row[col] = Convert.ToInt32(row[col]);
                                    }
                                    col.ReadOnly = true; // Vuelve a hacer la columna de solo lectura.
                                }
                            }

                            // Vincula los datos procesados a la interfaz.
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

        // --- Métodos de la Interfaz de Usuario ---

        // Botón para recargar los datos.
        private void boton_Cargar_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Datos han sido correctamente importados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Botón para exportar los datos a Excel.
        private void boton_ExportarExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Guardar como Excel",
                FileName = "Usuarios.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Usuarios");
                    // Escribe encabezados.
                    for (int i = 0; i < tabla_Usuarios.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i + 1).Value = tabla_Usuarios.Columns[i].HeaderText;
                    }
                    // Escribe datos.
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

        // Botón para filtrar la vista de datos.
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

        // --- Método Principal de Sincronización (DROP and CREATE) ---
        private async void boton_SubirDatos_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de que desea subir los datos del DataGridView a la base de datos? Esto eliminará toda la información previa en la base de datos.", "Confirmar subida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Enabled = false; // Deshabilita la UI.

                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString_Pagina))
                    {
                        await connection.OpenAsync();

                        // --- PASO 1: Eliminar la tabla de usuarios existente ---
                        // Esta es una operación destructiva que borra la tabla y todos sus datos.
                        string dropTableQuery = "DROP TABLE IF EXISTS usuarios";
                        using (MySqlCommand dropTableCommand = new MySqlCommand(dropTableQuery, connection))
                        {
                            await dropTableCommand.ExecuteNonQueryAsync();
                        }

                        // --- PASO 2: Crear la tabla de nuevo dinámicamente ---
                        // La estructura de la nueva tabla se basa en las columnas del DataGridView.
                        StringBuilder createTableQuery = new StringBuilder("CREATE TABLE usuarios (");
                        foreach (DataGridViewColumn column in tabla_Usuarios.Columns)
                        {
                            string columnName = column.Name;
                            // Determina el tipo de dato en MySQL basándose en el nombre de la columna.
                            string columnType = columnName.StartsWith("d") ? "INT" : "VARCHAR(255)";
                            // Define si la columna puede ser nula.
                            string nullConstraint = columnName.Equals("Nombre") ? "NULL" : "NOT NULL";
                            createTableQuery.AppendFormat("{0} {1} {2},", columnName, columnType, nullConstraint);
                        }
                        createTableQuery.Length--; // Elimina la última coma.
                        createTableQuery.Append(")");

                        using (MySqlCommand createTableCommand = new MySqlCommand(createTableQuery.ToString(), connection))
                        {
                            await createTableCommand.ExecuteNonQueryAsync();
                        }

                        // --- PASO 3: Insertar todos los datos en una sola consulta ---
                        int totalRows = tabla_Usuarios.Rows.Count;
                        int currentRow = 0;
                        var progressMessageBox = ProgressBarMessageBox.ShowProgressBar("Subiendo datos...", "Progreso");
                        progressMessageBox.Show();

                        // Construye la parte inicial de la consulta INSERT.
                        StringBuilder insertQuery = new StringBuilder("INSERT INTO usuarios (");
                        foreach (DataGridViewColumn column in tabla_Usuarios.Columns)
                        {
                            insertQuery.AppendFormat("{0},", column.Name);
                        }
                        insertQuery.Length--; // Elimina la coma final.
                        insertQuery.Append(") VALUES ");

                        // Procesa las filas en un hilo de fondo.
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
                                        // Obtiene y formatea el valor de la celda.
                                        object value = columnName.StartsWith("d") ? Convert.ToInt32(row.Cells[columnName].Value) : (row.Cells[columnName].Value ?? DBNull.Value);
                                        insertQuery.AppendFormat("'{0}',", value.ToString().Replace("'", "''")); // Escapa comillas simples.
                                    }
                                    insertQuery.Length--; // Elimina la última coma.
                                    insertQuery.Append("),");

                                    // Actualiza el progreso.
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

                        insertQuery.Length--; // Elimina la última coma.

                        // Ejecuta la consulta masiva de INSERT.
                        using (MySqlCommand insertCommand = new MySqlCommand(insertQuery.ToString(), connection))
                        {
                            await insertCommand.ExecuteNonQueryAsync();
                        }

                        progressMessageBox.Close();
                        MessageBox.Show("Datos subidos correctamente a la base de datos", "Subida completada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores.
                    MessageBox.Show($"Error general al subir los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Enabled = true; // Rehabilita la UI.
                }
            }
        }
    }
}