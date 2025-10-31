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
    public partial class Pedidos : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501;

        private string currentSortColumn = "IdPedido";
        private string currentSortOrder = "ASC";

        private readonly string connectionString_Pagina = "Server=65.109.49.57;Database=agrocosta_db;Uid=agrocosta_pagina;Pwd=Agr0costa*-;";
        private readonly string connectionString_Traverse = "SERVER=WIN-HOSNT32J3;DATABASE=AGR;UID=SISTEMAS;PWD=Costa2035***--";

        private DataView dataView;

        private List<string> itemsNull;

        public Pedidos()
        {
            InitializeComponent();
            SetCueBanner(txtIdPedido, "Filtrar por IdPedido");
            SetCueBanner(txtReferencia, "Filtrar por Referencia");

            // Configurar el DateTimePicker
            dateTimePickerFechaPedido.Format = DateTimePickerFormat.Custom;
            dateTimePickerFechaPedido.CustomFormat = "yyyy-MM-dd";
            dateTimePickerFechaPedido.Value = DateTime.Now; // Puedes establecer un valor predeterminado si lo deseas

            tabla_Pedidos.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            itemsNull = new List<string>();

            LoadData();
        }

        private void LoadData()
        {
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


                            foreach (DataRow row in dataTable.Rows)
                            {
                                /*
                                if (row["Referencia"] == null || row["Referencia"] == DBNull.Value ||
                                        row["Cantidad"] == null || row["Cantidad"] == DBNull.Value ||
                                        row["IdPedido"] == null || row["IdPedido"] == DBNull.Value)
                                {
                                    string IdPedido = row["IdPedido"].ToString();
                                    itemsNull.Add(IdPedido);
                                }
                                */
                            }

                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (DataColumn col in dataTable.Columns)
                                {
                                    
                                    if (col.DataType == typeof(double) || col.DataType == typeof(float) || col.DataType == typeof(decimal))
                                    {
                                        if (col.ColumnName != "Costo")
                                        {

                                            col.ReadOnly = false;
                                            row[col] = Convert.ToInt32(row[col]);
                                            col.ReadOnly = true;


                                        }
                                       
                                    }
                                    
                                }
                            }

                            // Asignar los datos al DataView para filtrado y ordenamiento local
                            dataView = dataTable.DefaultView;
                            tabla_Pedidos.DataSource = dataView;

                            label1.Text = $"Número de filas: {tabla_Pedidos.Rows.Count}";
                        }
                    }
                }

                // Agregar columna de CheckBox después de cargar los datos
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

        private void SetCueBanner(TextBox textBox, string cueText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cueText);
        }

        private void tabla_Pedidos_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex != tabla_Pedidos.Columns["checkboxColumn"].Index)
            {
                string sortOrder = "ASC";
                if (currentSortColumn == tabla_Pedidos.Columns[e.ColumnIndex].Name)
                {
                    sortOrder = currentSortOrder == "ASC" ? "DESC" : "ASC";
                }

                currentSortColumn = tabla_Pedidos.Columns[e.ColumnIndex].Name;
                currentSortOrder = sortOrder;

                dataView.Sort = $"{currentSortColumn} {currentSortOrder}";
            }
            else if (e.RowIndex >= 0 && e.ColumnIndex == tabla_Pedidos.Columns["checkboxColumn"].Index)
            {
                DataGridViewCheckBoxCell chkCell = tabla_Pedidos.Rows[e.RowIndex].Cells["checkboxColumn"] as DataGridViewCheckBoxCell;

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

        private void boton_Buscar_Click_1(object sender, EventArgs e)
        {
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
            if (dateTimePickerFechaPedido.Checked)
            {
                filter.AppendFormat("FechaPedido = '{0}'", dateTimePickerFechaPedido.Value.Date.ToString("yyyy-MM-dd"));
            }

            dataView.RowFilter = filter.ToString();

            label1.Text = $"Número de filas: {dataView.Count}";
        }

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
                        if (itemsNull.Contains(row.Cells["Referencia"].Value.ToString()))
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

                        // Eliminar todos los datos existentes en la tabla
                        string deleteQuery = "DELETE FROM pedidos";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                        {
                            await deleteCommand.ExecuteNonQueryAsync();
                        }

                        // Calcular el número total de filas para el progreso
                        int totalRows = tabla_Pedidos.Rows.Count;
                        int currentRow = 0;
                        int batchSize = tabla_Pedidos.Rows.Count;
                        List<string> rowsToInsert = new List<string>();

                        // Mostrar el MessageBox personalizado con barra de progreso y texto dinámico
                        var progressMessageBox = ProgressBarMessageBox.ShowProgressBar("Subiendo datos...", "Progreso");
                        progressMessageBox.Show();

                        await Task.Run(async () =>
                        {
                            foreach (DataGridViewRow row in tabla_Pedidos.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    string idPedido = row.Cells["IdPedido"].Value?.ToString();
                                    string proveedor = row.Cells["Proveedor"].Value?.ToString();
                                    string moneda = row.Cells["Moneda"].Value?.ToString();
                                    string referencia = row.Cells["Referencia"].Value?.ToString();
                                    string cantidad = row.Cells["Cantidad"].Value?.ToString();
                                    string Costo = row.Cells["Costo"].Value?.ToString();
                                    string fechaPedido = row.Cells["FechaPedido"].Value != null ? ((DateTime)row.Cells["FechaPedido"].Value).ToString("yyyy-MM-dd") : null;

                                    // Verificar que los campos obligatorios no sean nulos
                                    if (string.IsNullOrEmpty(idPedido) || string.IsNullOrEmpty(referencia) || string.IsNullOrEmpty(cantidad))
                                    {
                                        continue; // O manejar el error de otra forma
                                    }

                                    rowsToInsert.Add($"('{idPedido}','{proveedor}','{moneda}','{referencia}', {cantidad}, {Costo}, '{fechaPedido}')");

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
            string insertQuery = "INSERT INTO pedidos (IdPedido, Proveedor, Moneda, Referencia, Cantidad, Costo, FechaPedido) VALUES " + string.Join(", ", rowsToInsert);
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
            saveFileDialog.FileName = "Pedidos.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Pedidos");

                    // Agregar encabezados de columna
                    for (int i = 1; i < tabla_Pedidos.Columns.Count; i++)
                    {
                        worksheet.Cell(1, i).Value = tabla_Pedidos.Columns[i].HeaderText;
                    }

                    // Agregar filas
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

        private void Pedidos_Load(object sender, EventArgs e)
        {

        }
    }
}
