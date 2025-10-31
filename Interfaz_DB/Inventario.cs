using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using MySql.Data.MySqlClient;
using ZXing;
using ZXing.Common;
using ZXing.Rendering;
using System.Drawing;
using System.Drawing.Imaging;

namespace Interfaz_DB
{
    public partial class Inventario : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private string currentSortColumn = "Referencia";
        private string currentSortOrder = "ASC";

        private readonly string connectionString_Pagina = "Server=65.109.49.57;Database=agrocosta_db;Uid=agrocosta_pagina;Pwd=Agr0costa*-;Connection Timeout=1000;";
        private readonly string connectionString_Traverse = "SERVER=WIN-HOSNT32J3;DATABASE=AGR;UID=SISTEMAS;PWD=Costa2035***--";

        private DataView dataView;

        private List<string> itemsNull;

        public Inventario()
        {
            InitializeComponent();
            SetCueBanner(txtLinea, "Filtrar por Linea");
            SetCueBanner(txtReferencia, "Filtrar por Referencia");
            SetCueBanner(txtTipo, "Filtrar por Tipo");

            tabla_Inventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            itemsNull = new List<string>();

            LoadData();
        }

        private void LoadData()
        {
            string query = @"
            EXEC [dbo].[PaginaWeb_SoInventarioWeb]
            ";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString_Traverse))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 600;
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);

                            // Revisar y modificar la columna "descripcion"
                            foreach (DataRow row in dataTable.Rows)
                            {
                                string descripcion = row["descripcion"].ToString();
                                // Manejo de la columna "descripcion"
                                if (row["descripcion"] != DBNull.Value)
                                {

                                    if (descripcion.Contains(","))
                                    {
                                        string newString = descripcion.Replace(",", "  ");
                                        if (newString.Length > dataTable.Columns["descripcion"].MaxLength && dataTable.Columns["descripcion"].MaxLength > 0)
                                        {
                                            dataTable.Columns["descripcion"].MaxLength = newString.Length;
                                            row["descripcion"] = newString;
                                        }
                                        else
                                        {
                                            row["descripcion"] = newString;
                                        }
                                    }
                                }

                                // Verificar si alguno de los campos obligatorios es nulo
                                string referencia = row["Referencia"] == DBNull.Value ? null : row["Referencia"].ToString();
                                object existencias = row["Existencias"];
                                string linea = row["Linea"] == DBNull.Value ? null : row["Linea"].ToString();
                                string tipo = row["Tipo"] == DBNull.Value ? null : row["Tipo"].ToString();

                                if (referencia == null || referencia == "" || existencias == DBNull.Value || linea == null || linea == ""|| tipo == null || tipo == "")
                                {
                                    if (referencia != null || referencia != "")
                                    {
                                        itemsNull.Add(referencia);
                                    }
                                    else
                                        itemsNull.Add("Sin Referencia");
                                }
                            }

                            // Asignar los datos al DataGridView a través de un DataView
                            dataView = dataTable.DefaultView;
                            tabla_Inventario.DataSource = dataView;

                            // Actualizar el Label con el número de filas cargadas
                            label1.Text = $"Número de filas: {tabla_Inventario.Rows.Count}";
                        }
                    }
                }

                // Agregar columna de CheckBox después de cargar los datos
                if (!tabla_Inventario.Columns.Contains("checkboxColumn"))
                {
                    DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = "Seleccionar",
                        Name = "checkboxColumn"
                    };
                    tabla_Inventario.Columns.Insert(0, checkboxColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void SetCueBanner(TextBox textBox, string cueText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cueText);
        }

        private void tabla_Inventario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != tabla_Inventario.Columns["checkboxColumn"].Index)
            {
                string sortOrder = "ASC";
                if (currentSortColumn == tabla_Inventario.Columns[e.ColumnIndex].Name)
                {
                    sortOrder = currentSortOrder == "ASC" ? "DESC" : "ASC";
                }

                currentSortColumn = tabla_Inventario.Columns[e.ColumnIndex].Name;
                currentSortOrder = sortOrder;

                dataView.Sort = $"{currentSortColumn} {currentSortOrder}";
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == tabla_Inventario.Columns["checkboxColumn"].Index)
            {
                DataGridViewCheckBoxCell chkCell = tabla_Inventario.Rows[e.RowIndex].Cells["checkboxColumn"] as DataGridViewCheckBoxCell;

                if (chkCell.Value != null && chkCell.Value != DBNull.Value)
                {
                    bool isChecked = Convert.ToBoolean(chkCell.Value);
                    chkCell.Value = !isChecked;
                }
                else
                {
                    chkCell.Value = true;
                }
            }
        }

        private void boton_Buscar_Click(object sender, EventArgs e)
        {
            StringBuilder filter = new StringBuilder();
            if (!string.IsNullOrEmpty(txtLinea.Text))
            {
                filter.AppendFormat("Linea = '{0}'", txtLinea.Text);
            }
            if (!string.IsNullOrEmpty(txtReferencia.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.AppendFormat("Referencia LIKE '%{0}%'", txtReferencia.Text);
            }
            if (!string.IsNullOrEmpty(txtTipo.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.AppendFormat("Tipo LIKE '%{0}%'", txtTipo.Text);
            }

            dataView.RowFilter = filter.ToString();

            label1.Text = $"Número de filas: {dataView.Count}";
        }

        private void boton_GuardarCambios_Click(object sender, EventArgs e)
        {
            var rowsToDelete = new List<DataGridViewRow>();

            foreach (DataGridViewRow row in tabla_Inventario.Rows)
            {
                if (Convert.ToBoolean(row.Cells["checkboxColumn"].Value))
                {
                    rowsToDelete.Add(row);
                }
            }

            if (rowsToDelete.Count > 0)
            {
                var result = MessageBox.Show("¿Está seguro de que desea eliminar las filas seleccionadas del DataGridView?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    foreach (var row in rowsToDelete)
                    {
                        if (itemsNull.Contains(row.Cells["Referencia"].Value.ToString()))
                            itemsNull.Remove(row.Cells["Referencia"].Value.ToString());

                        tabla_Inventario.Rows.Remove(row);
                    }

                    label1.Text = $"Número de filas: {tabla_Inventario.Rows.Count}";
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna fila para eliminar.", "Ninguna fila seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async void boton_SubirDatos_Click(object sender, EventArgs e)
        {
            if (itemsNull.Count > 0)
            {
                string itemsWithNulls = string.Join(", ", itemsNull);
                MessageBox.Show($"Los siguientes ítems tienen valores nulos en Referencia, Existencias, Linea o Tipo: {itemsWithNulls}. Corrija los datos antes de subirlos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var result = MessageBox.Show("¿Está seguro de que desea subir los datos del DataGridView a la base de datos? Esto eliminará toda la información previa en la base de datos.", "Confirmar subida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Enabled = false;
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString_Pagina))
                    {
                        await connection.OpenAsync();

                        // Eliminar todos los datos existentes en la tabla
                        string deleteQuery = "DELETE FROM inventario";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                        {
                            await deleteCommand.ExecuteNonQueryAsync();
                        }

                        // Calcular el número total de filas para el progreso
                        int totalRows = tabla_Inventario.Rows.Count;
                        int currentRow = 0;
                        int batchSize = tabla_Inventario.Rows.Count;
                        List<string> rowsToInsert = new List<string>();

                        // Mostrar el MessageBox personalizado con barra de progreso y texto dinámico
                        var progressMessageBox = ProgressBarMessageBox.ShowProgressBar("Subiendo datos...", "Progreso");
                        progressMessageBox.Show();

                        // Insertar los datos del DataGridView en la base de datos en un hilo de fondo
                        await Task.Run(async () =>
                        {
                            foreach (DataGridViewRow row in tabla_Inventario.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    string referencia = EscapeSingleQuotes(row.Cells["Referencia"].Value?.ToString());
                                    string descripcion = EscapeSingleQuotes(row.Cells["descripcion"].Value?.ToString());
                                    string alternos = EscapeSingleQuotes(row.Cells["Alternos"].Value?.ToString());
                                    string existencias = FormatExistencias(row.Cells["Existencias"].Value);
                                    string bodega = EscapeSingleQuotes(row.Cells["Bodega"].Value?.ToString());
                                    string precio = EscapeSingleQuotes(row.Cells["Precio"].Value?.ToString());
                                    string linea = EscapeSingleQuotes(row.Cells["Linea"].Value?.ToString());
                                    string priceid = EscapeSingleQuotes(row.Cells["PriceId"].Value?.ToString());
                                    string tipo = EscapeSingleQuotes(row.Cells["Tipo"].Value?.ToString());
                                    string diassinventas = EscapeSingleQuotes(row.Cells["DiasSinVentas"].Value?.ToString());
                                    string abcclass = EscapeSingleQuotes(row.Cells["ABCClass"].Value?.ToString());

                                    rowsToInsert.Add($"({referencia}, {descripcion}, {alternos}, {existencias}, {bodega} ,{precio}, {linea}, {priceid} ,{tipo},{diassinventas},{abcclass})");

                                    // Insertar en grupos de 1,000
                                    if (rowsToInsert.Count == batchSize)
                                    {
                                        await InsertBatch(connection, rowsToInsert);
                                        rowsToInsert.Clear();
                                    }

                                    // Actualizar el progreso
                                    currentRow++;
                                    int progressPercentage = (int)((double)currentRow / totalRows * 100);
                                    progressMessageBox.Invoke((Action)(() =>
                                    {
                                        progressMessageBox.ProgressText = $"Progreso: {progressPercentage}% ({currentRow} de {totalRows} líneas preparadas)";
                                        progressMessageBox.ProgressValue = progressPercentage;
                                    }));
                                }
                            }

                            // Insertar cualquier grupo restante
                            if (rowsToInsert.Count > 0)
                            {
                                await InsertBatch(connection, rowsToInsert);
                            }
                        });

                        progressMessageBox.Close();
                        MessageBox.Show("Los datos han sido subidos correctamente a la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
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


        private async Task InsertBatch(MySqlConnection connection, List<string> rowsToInsert)
        {
            string insertQuery = "INSERT INTO inventario (Referencia, descripcion, Alternos, Existencias, Bodega ,Precio, Linea, PriceId ,Tipo, DiasSinVentas, ABCClass) VALUES " + string.Join(", ", rowsToInsert);
            try
            {
                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (MySqlException ex)
            {
                // Registrar la longitud de la consulta cuando se produce un error
                MessageBox.Show($"Error al insertar un lote de datos: {ex.Message}\nConsulta Length: {insertQuery.Length}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error general al insertar un lote de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private string EscapeSingleQuotes(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "NULL";
            }

            // Escapar comillas simples y devolver el valor entre comillas simples
            return $"'{value.Replace("'", "''")}'";
        }

        private string FormatExistencias(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return "0"; // Valor predeterminado
            }

            if (int.TryParse(value.ToString(), out int result))
            {
                return result.ToString();
            }

            return "0"; // Valor predeterminado
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
            saveFileDialog.FileName = "Inventario.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Inventario");

                    // Agregar encabezados de columna
                    for (int i = 1; i < tabla_Inventario.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i).Value = tabla_Inventario.Columns[i].HeaderText;
                    }

                    // Agregar filas
                    for (int i = 0; i < tabla_Inventario.Rows.Count; i++)
                    {
                        for (int j = 1; j < tabla_Inventario.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j).Value = tabla_Inventario.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente a Excel", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /*
        private void boton_CrearLabel_Click(object sender, EventArgs e)
        {
            var rowsToLabel = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in tabla_Inventario.Rows)
            {
                if (Convert.ToBoolean(row.Cells["checkboxColumn"].Value))
                {
                    rowsToLabel.Add(row);
                }
            }

            if (rowsToLabel.Count > 0)
            {
                foreach (var row in rowsToLabel)
                {
                    string rowData = $"{row.Cells["Referencia"].Value},{row.Cells["descripcion"].Value},{row.Cells["Alternos"].Value},{row.Cells["Existencias"].Value},{row.Cells["Precio"].Value},{row.Cells["Linea"].Value},{row.Cells["Tipo"].Value}";
                    GenerateBarcode(rowData);
                }

                MessageBox.Show("Códigos de barras generados para las filas seleccionadas.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No se han seleccionado filas.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        
        private void GenerateBarcode(string data)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.CODE_128,
                Options = new EncodingOptions
                {
                    Height = 150,
                    Width = 300,
                    Margin = 1
                },
                Renderer = new BitmapRenderer()
            };

            var barcodeBitmap = writer.Write(data);

            // Save the barcode as an image
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{Guid.NewGuid()}.png");
            barcodeBitmap.Save(filePath, ImageFormat.Png);

            // Optionally, you can display the barcode image
            DisplayBarcode(barcodeBitmap);
        }

        private void DisplayBarcode(Bitmap barcodeBitmap)
        {
            Form barcodeForm = new Form
            {
                Text = "Código de Barras",
                Size = new Size(320, 220)
            };

            PictureBox pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                Image = barcodeBitmap,
                SizeMode = PictureBoxSizeMode.CenterImage
            };

            barcodeForm.Controls.Add(pictureBox);
            barcodeForm.ShowDialog();
        }*/
    }
}
