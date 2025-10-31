using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel;
using MySql.Data.MySqlClient;

namespace Interfaz_DB
{
    // La clase Pedidos representa la ventana de gestión de pedidos (órdenes de compra).
    // Su estructura y funcionamiento son muy similares a la clase Inventario.
    public partial class Pedidos : Form
    {
        // --- Interoperabilidad con la API de Windows para placeholders en TextBoxes ---
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        // --- Variables de estado del formulario ---
        private string currentSortColumn = "IdPedido"; // Columna de ordenamiento por defecto.
        private string currentSortOrder = "ASC"; // Orden por defecto.

        // --- Cadenas de Conexión ---
        private readonly string connectionString_Pagina = "Server=65.109.49.57;Database=agrocosta_db;Uid=agrocosta_pagina;Pwd=Agr0costa*-;";
        private readonly string connectionString_Traverse = "SERVER=WIN-HOSNT32J3;DATABASE=AGR;UID=SISTEMAS;PWD=Costa2035***--";

        // --- Origen de Datos para la Tabla ---
        private DataView dataView;

        // --- Almacenamiento de Validación (actualmente no utilizado, pero presente) ---
        private List<string> itemsNull;

        // --- Constructor ---
        public Pedidos()
        {
            InitializeComponent();

            // Establece placeholders en los campos de búsqueda.
            SetCueBanner(txtIdPedido, "Filtrar por IdPedido");
            SetCueBanner(txtReferencia, "Filtrar por Referencia");

            // Configura el control de selección de fecha (DateTimePicker).
            dateTimePickerFechaPedido.Format = DateTimePickerFormat.Custom;
            dateTimePickerFechaPedido.CustomFormat = "yyyy-MM-dd"; // Formato de fecha estándar.
            dateTimePickerFechaPedido.Value = DateTime.Now; // Valor inicial es la fecha actual.

            tabla_Pedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            itemsNull = new List<string>(); // Inicializa la lista.

            LoadData(); // Carga los datos al iniciar.
        }

        // --- Método Principal de Carga de Datos ---
        private void LoadData()
        {
            // Consulta SQL para obtener los datos de pedidos (órdenes de compra) del sistema interno.
            // Une la tabla de cabecera (tblPoTransHeader) con la de detalle (tblPoTransDetail).
            string query = @"
            SELECT b.TransID AS IdPedido, a.VendorId as Proveedor, a.CurrencyID as Moneda ,b.ItemId AS Referencia, b.QtyOrd AS Cantidad, b.UnitCostFgn as Costo,a.ReqShipDate AS FechaPedido
            FROM tblPoTransHeader a
            INNER JOIN tblPoTransDetail b ON a.TransId = b.TransID
            WHERE a.TransId LIKE 'D%' AND a.TransType = 9
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

                            // --- Procesamiento de Datos ---
                            // La sección de validación de nulos está comentada, por lo que no tiene efecto.
                            /*
                            foreach (DataRow row in dataTable.Rows)
                            {
                                if (...) { itemsNull.Add(...); }
                            }
                            */

                            // Este bucle ajusta los tipos de datos numéricos a enteros (excepto 'Costo').
                            // Esto puede ser para estandarizar la visualización de cantidades, etc.
                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (DataColumn col in dataTable.Columns)
                                {
                                    if (col.DataType == typeof(double) || col.DataType == typeof(float) || col.DataType == typeof(decimal))
                                    {
                                        if (col.ColumnName != "Costo")
                                        {
                                            // Se necesita hacer la columna editable temporalmente para cambiar su valor.
                                            col.ReadOnly = false;
                                            row[col] = Convert.ToInt32(row[col]);
                                            col.ReadOnly = true;
                                        }
                                    }
                                }
                            }

                            // Vincula los datos a la interfaz.
                            dataView = dataTable.DefaultView;
                            tabla_Pedidos.DataSource = dataView;
                            label1.Text = $"Número de filas: {tabla_Pedidos.Rows.Count}";
                        }
                    }
                }

                // Agrega la columna de checkboxes si no existe.
                if (!tabla_Pedidos.Columns.Contains("checkboxColumn"))
                {
                    DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = "Seleccionar",
                        Name = "checkboxColumn"
                    };
                    tabla_Pedidos.Columns.Insert(0, checkboxColumn);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Métodos de la Interfaz de Usuario (UI) ---

        // Método auxiliar para el placeholder.
        private void SetCueBanner(TextBox textBox, string cueText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cueText);
        }

        // Manejador para clics en la tabla (ordenar o marcar checkboxes).
        private void tabla_Pedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            // Lógica para marcar/desmarcar el checkbox.
            if (e.ColumnIndex == tabla_Pedidos.Columns["checkboxColumn"].Index)
            {
                DataGridViewCheckBoxCell chkCell = tabla_Pedidos.Rows[e.RowIndex].Cells["checkboxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = chkCell.Value == null ? false : (bool)chkCell.Value;
                chkCell.Value = !isChecked;
            }
            // Lógica para ordenar por la columna clickeada.
            else
            {
                string clickedColumnName = tabla_Pedidos.Columns[e.ColumnIndex].Name;
                if (currentSortColumn == clickedColumnName)
                {
                    currentSortOrder = (currentSortOrder == "ASC") ? "DESC" : "ASC";
                }
                else
                {
                    currentSortColumn = clickedColumnName;
                    currentSortOrder = "ASC";
                }
                dataView.Sort = $"{currentSortColumn} {currentSortOrder}";
            }
        }

        // Manejador del botón de búsqueda/filtrado.
        private void boton_Buscar_Click_1(object sender, EventArgs e)
        {
            // Construye la cadena de filtro.
            StringBuilder filter = new StringBuilder();
            if (!string.IsNullOrEmpty(txtIdPedido.Text))
            {
                filter.AppendFormat("IdPedido LIKE '%{0}%'", txtIdPedido.Text);
            }
            if (!string.IsNullOrEmpty(txtReferencia.Text))
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.AppendFormat("Referencia LIKE '%{0}%'", txtReferencia.Text);
            }
            // Incluye el filtro por fecha si el checkbox del DateTimePicker está marcado.
            if (dateTimePickerFechaPedido.Checked)
            {
                if (filter.Length > 0) filter.Append(" AND ");
                filter.AppendFormat("FechaPedido = '{0}'", dateTimePickerFechaPedido.Value.Date.ToString("yyyy-MM-dd"));
            }

            // Aplica el filtro.
            dataView.RowFilter = filter.ToString();
            label1.Text = $"Número de filas: {dataView.Count}";
        }

        // Manejador para eliminar (visualmente) las filas seleccionadas.
        private void boton_GuardarCambios_Click(object sender, EventArgs e)
        {
            var rowsToDelete = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in tabla_Pedidos.Rows)
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
                        // Lógica de `itemsNull` (actualmente sin efecto).
                        if (row.Cells["Referencia"].Value != null && itemsNull.Contains(row.Cells["Referencia"].Value.ToString()))
                            itemsNull.Remove(row.Cells["Referencia"].Value.ToString());

                        tabla_Pedidos.Rows.Remove(row);
                    }
                    label1.Text = $"Número de filas: {tabla_Pedidos.Rows.Count}";
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna fila para eliminar.", "Ninguna fila seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- Método Principal de Sincronización de Datos ---

        // Manejador para el botón de subir datos a la web.
        private async void boton_SubirDatos_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Está seguro de que desea subir los datos del DataGridView a la base de datos? Esto eliminará toda la información previa en la base de datos.", "Confirmar subida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Enabled = false; // Deshabilita la ventana.
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString_Pagina))
                    {
                        await connection.OpenAsync();

                        // Paso 1: Borra todos los datos de la tabla de pedidos en la web.
                        string deleteQuery = "DELETE FROM pedidos";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                        {
                            await deleteCommand.ExecuteNonQueryAsync();
                        }

                        // Prepara variables para inserción y barra de progreso.
                        int totalRows = tabla_Pedidos.Rows.Count;
                        int currentRow = 0;
                        int batchSize = 1000; // Tamaño del lote.
                        List<string> rowsToInsert = new List<string>();

                        var progressMessageBox = ProgressBarMessageBox.ShowProgressBar("Subiendo datos...", "Progreso");
                        progressMessageBox.Show();

                        // Paso 2: Inserta los nuevos datos en un hilo de fondo.
                        await Task.Run(async () =>
                        {
                            foreach (DataGridViewRow row in tabla_Pedidos.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    // Recopila y formatea los datos de la fila.
                                    string idPedido = row.Cells["IdPedido"].Value?.ToString();
                                    string proveedor = row.Cells["Proveedor"].Value?.ToString();
                                    string moneda = row.Cells["Moneda"].Value?.ToString();
                                    string referencia = row.Cells["Referencia"].Value?.ToString();
                                    string cantidad = row.Cells["Cantidad"].Value?.ToString();
                                    string costo = row.Cells["Costo"].Value?.ToString();
                                    string fechaPedido = row.Cells["FechaPedido"].Value != null ? ((DateTime)row.Cells["FechaPedido"].Value).ToString("yyyy-MM-dd") : null;

                                    // Validación simple para saltar filas con datos cruciales faltantes.
                                    if (string.IsNullOrEmpty(idPedido) || string.IsNullOrEmpty(referencia) || string.IsNullOrEmpty(cantidad))
                                    {
                                        continue;
                                    }

                                    // Añade la fila formateada al lote.
                                    rowsToInsert.Add($"('{idPedido}','{proveedor}','{moneda}','{referencia}', {cantidad}, {costo}, '{fechaPedido}')");

                                    // Inserta el lote si alcanza el tamaño definido.
                                    if (rowsToInsert.Count >= batchSize)
                                    {
                                        await InsertBatch(connection, rowsToInsert);
                                        rowsToInsert.Clear();
                                    }

                                    // Actualiza la barra de progreso.
                                    currentRow++;
                                    int progressPercentage = (int)((double)currentRow / totalRows * 100);
                                    progressMessageBox.Invoke((Action)(() =>
                                    {
                                        progressMessageBox.ProgressText = $"Progreso: {progressPercentage}% ({currentRow} de {totalRows} líneas preparadas)";
                                        progressMessageBox.ProgressValue = progressPercentage;
                                    }));
                                }
                            }

                            // Inserta el último lote si queda alguno.
                            if (rowsToInsert.Count > 0)
                            {
                                await InsertBatch(connection, rowsToInsert);
                            }
                        });

                        progressMessageBox.Close();
                        MessageBox.Show("Los datos han sido subidos correctamente a la base de datos.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de errores general.
                    MessageBox.Show($"Error general al subir los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Enabled = true; // Rehabilita la ventana.
                }
            }
        }

        // Método auxiliar para la inserción por lotes.
        private async Task InsertBatch(MySqlConnection connection, List<string> rowsToInsert)
        {
            string insertQuery = "INSERT INTO pedidos (IdPedido, Proveedor, Moneda, Referencia, Cantidad, Costo, FechaPedido) VALUES " + string.Join(", ", rowsToInsert);
            try
            {
                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                // Relanza la excepción para que sea capturada por el manejador principal.
                throw;
            }
        }

        // --- Botones Adicionales ---

        // Botón para recargar los datos.
        private void boton_Cargar_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Datos han sido correctamente importados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Botón para exportar a Excel.
        private void boton_ExportarExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Excel Files|*.xlsx",
                Title = "Guardar como Excel",
                FileName = "Pedidos.xlsx"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Pedidos");

                    // Escribe encabezados.
                    for (int i = 1; i < tabla_Pedidos.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i).Value = tabla_Pedidos.Columns[i].HeaderText;
                    }

                    // Escribe datos.
                    for (int i = 0; i < tabla_Pedidos.Rows.Count; i++)
                    {
                        for (int j = 1; j < tabla_Pedidos.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j).Value = tabla_Pedidos.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente a Excel", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        // Este evento se genera al cargar el formulario, pero está vacío.
        private void Pedidos_Load(object sender, EventArgs e)
        {

        }
    }
}
