using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ClosedXML.Excel; // Librería para trabajar con archivos de Excel (crear, leer, escribir).
using MySql.Data.MySqlClient; // El conector de MySQL para .NET, para interactuar con bases de datos MySQL.
using ZXing; // Librerías para la generación de códigos de barras (actualmente comentadas).
using ZXing.Common;
using ZXing.Rendering;
using System.Drawing;
using System.Drawing.Imaging;

namespace Interfaz_DB
{
    // La clase Inventario representa la ventana de gestión del inventario.
    public partial class Inventario : Form
    {
        // --- Interoperabilidad con la API de Windows ---
        // Se importa la función 'SendMessage' de la librería 'user32.dll' de Windows.
        // Esto permite enviar mensajes directamente a los controles de la ventana, en este caso, para poner texto de marcador de posición (placeholder).
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);
        private const int EM_SETCUEBANNER = 0x1501; // Constante que representa el mensaje para establecer un 'cue banner' (placeholder).

        // --- Variables de estado del formulario ---
        private string currentSortColumn = "Referencia"; // Almacena la columna por la que se está ordenando la tabla actualmente.
        private string currentSortOrder = "ASC"; // Almacena el orden actual (ascendente o descendente).

        // --- Cadenas de Conexión a las Bases de Datos ---
        // Cadena de conexión para la base de datos de la página web (MySQL).
        private readonly string connectionString_Pagina = "Server=65.109.49.57;Database=agrocosta_db;Uid=agrocosta_pagina;Pwd=Agr0costa*-;Connection Timeout=1000;";
        // Cadena de conexión para la base de datos interna del sistema (SQL Server).
        private readonly string connectionString_Traverse = "SERVER=WIN-HOSNT32J3;DATABASE=AGR;UID=SISTEMAS;PWD=Costa2035***--";

        // --- Origen de Datos para la Tabla ---
        // Un DataView proporciona una vista personalizable (filtrable, ordenable) de un DataTable.
        // Es el intermediario entre los datos crudos y lo que se muestra en el DataGridView.
        private DataView dataView;

        // --- Almacenamiento de Validación ---
        // Una lista para guardar las referencias de los productos a los que les faltan datos obligatorios.
        private List<string> itemsNull;

        // --- Constructor del Formulario ---
        public Inventario()
        {
            InitializeComponent(); // Inicializa los componentes visuales del formulario.

            // Establece textos de marcador de posición (placeholders) en los campos de búsqueda.
            SetCueBanner(txtLinea, "Filtrar por Linea");
            SetCueBanner(txtReferencia, "Filtrar por Referencia");
            SetCueBanner(txtTipo, "Filtrar por Tipo");

            // Configura el modo de selección de la tabla para que se seleccionen filas completas.
            tabla_Inventario.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

            // Inicializa la lista de ítems con valores nulos.
            itemsNull = new List<string>();

            // Llama al método para cargar los datos del inventario al abrir la ventana.
            LoadData();
        }

        // --- Método Principal de Carga de Datos ---
        private void LoadData()
        {
            // Define la consulta SQL para obtener los datos. En este caso, ejecuta un procedimiento almacenado.
            string query = @"
            EXEC [dbo].[PaginaWeb_SoInventarioWeb]
            ";

            try
            {
                // Utiliza 'using' para asegurar que la conexión a la base de datos se cierre correctamente, incluso si hay errores.
                using (SqlConnection connection = new SqlConnection(connectionString_Traverse))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.CommandTimeout = 600; // Aumenta el tiempo de espera del comando a 10 minutos.
                        connection.Open(); // Abre la conexión a la base de datos SQL Server.

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable(); // Crea un DataTable para almacenar los resultados.
                            dataTable.Load(reader); // Carga los datos del SqlDataReader en el DataTable.

                            // --- Procesamiento y Validación de Datos ---
                            // Itera sobre cada fila del DataTable para limpiar y validar los datos.
                            foreach (DataRow row in dataTable.Rows)
                            {
                                // Limpieza de la columna 'descripcion': reemplaza comas para evitar problemas.
                                if (row["descripcion"] != DBNull.Value)
                                {
                                    string descripcion = row["descripcion"].ToString();
                                    if (descripcion.Contains(","))
                                    {
                                        row["descripcion"] = descripcion.Replace(",", "  ");
                                    }
                                }

                                // Validación de campos obligatorios.
                                string referencia = row["Referencia"] == DBNull.Value ? null : row["Referencia"].ToString();
                                object existencias = row["Existencias"];
                                string linea = row["Linea"] == DBNull.Value ? null : row["Linea"].ToString();
                                string tipo = row["Tipo"] == DBNull.Value ? null : row["Tipo"].ToString();

                                // Si algún campo obligatorio está vacío o nulo, se añade la referencia a la lista 'itemsNull'.
                                if (string.IsNullOrEmpty(referencia) || existencias == DBNull.Value || string.IsNullOrEmpty(linea) || string.IsNullOrEmpty(tipo))
                                {
                                    itemsNull.Add(string.IsNullOrEmpty(referencia) ? "Sin Referencia" : referencia);
                                }
                            }

                            // --- Vinculación de Datos a la Interfaz ---
                            // Asigna los datos del DataTable a un DataView para permitir filtrado y ordenamiento.
                            dataView = dataTable.DefaultView;
                            tabla_Inventario.DataSource = dataView; // Establece el DataView como el origen de datos de la tabla.

                            // Actualiza una etiqueta para mostrar el número total de filas cargadas.
                            label1.Text = $"Número de filas: {tabla_Inventario.Rows.Count}";
                        }
                    }
                }

                // Agrega dinámicamente una columna de checkboxes a la tabla si no existe.
                if (!tabla_Inventario.Columns.Contains("checkboxColumn"))
                {
                    DataGridViewCheckBoxColumn checkboxColumn = new DataGridViewCheckBoxColumn
                    {
                        HeaderText = "Seleccionar",
                        Name = "checkboxColumn"
                    };
                    tabla_Inventario.Columns.Insert(0, checkboxColumn); // Inserta la columna al principio.
                }
            }
            catch (Exception ex)
            {
                // Si ocurre cualquier error durante la carga, muestra un mensaje al usuario.
                MessageBox.Show($"Error al cargar los datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // --- Métodos de la Interfaz de Usuario (UI) ---

        // Método auxiliar para establecer el texto de marcador de posición (placeholder) en un TextBox.
        private void SetCueBanner(TextBox textBox, string cueText)
        {
            SendMessage(textBox.Handle, EM_SETCUEBANNER, 0, cueText);
        }

        // Manejador de eventos que se dispara al hacer clic en una celda de la tabla.
        private void tabla_Inventario_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Se asegura de que el clic no fue en el encabezado.
            if (e.RowIndex < 0) return;

            // Si el clic fue en la columna de checkbox, cambia su estado (marcado/desmarcado).
            if (e.ColumnIndex == tabla_Inventario.Columns["checkboxColumn"].Index)
            {
                // Invertir el valor del checkbox.
                DataGridViewCheckBoxCell chkCell = tabla_Inventario.Rows[e.RowIndex].Cells["checkboxColumn"] as DataGridViewCheckBoxCell;
                bool isChecked = chkCell.Value == null ? false : (bool)chkCell.Value;
                chkCell.Value = !isChecked;
            }
            // Si el clic fue en cualquier otra celda, ordena la tabla por esa columna.
            else
            {
                string clickedColumnName = tabla_Inventario.Columns[e.ColumnIndex].Name;
                // Determina si el orden debe ser ascendente o descendente.
                if (currentSortColumn == clickedColumnName)
                {
                    currentSortOrder = (currentSortOrder == "ASC") ? "DESC" : "ASC";
                }
                else
                {
                    currentSortColumn = clickedColumnName;
                    currentSortOrder = "ASC";
                }
                // Aplica el ordenamiento al DataView.
                dataView.Sort = $"{currentSortColumn} {currentSortOrder}";
            }
        }

        // Manejador de evento para el botón de búsqueda/filtrado.
        private void boton_Buscar_Click(object sender, EventArgs e)
        {
            // Construye una cadena de filtro basada en el texto de los campos de búsqueda.
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

            // Aplica el filtro al DataView, lo que actualiza automáticamente la tabla.
            dataView.RowFilter = filter.ToString();

            // Actualiza la etiqueta con el número de filas que coinciden con el filtro.
            label1.Text = $"Número de filas: {dataView.Count}";
        }

        // Manejador de evento para el botón que elimina las filas seleccionadas de la vista.
        private void boton_GuardarCambios_Click(object sender, EventArgs e)
        {
            var rowsToDelete = new List<DataGridViewRow>();

            // Recopila todas las filas que tienen el checkbox marcado.
            foreach (DataGridViewRow row in tabla_Inventario.Rows)
            {
                if (Convert.ToBoolean(row.Cells["checkboxColumn"].Value))
                {
                    rowsToDelete.Add(row);
                }
            }

            if (rowsToDelete.Count > 0)
            {
                // Pide confirmación al usuario antes de eliminar.
                var result = MessageBox.Show("¿Está seguro de que desea eliminar las filas seleccionadas del DataGridView?", "Confirmar eliminación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Elimina las filas seleccionadas de la tabla (solo visualmente, no de la BD).
                    foreach (var row in rowsToDelete)
                    {
                        // Si la fila estaba en la lista de ítems con errores, la quita también de ahí.
                        if (row.Cells["Referencia"].Value != null && itemsNull.Contains(row.Cells["Referencia"].Value.ToString()))
                            itemsNull.Remove(row.Cells["Referencia"].Value.ToString());

                        tabla_Inventario.Rows.Remove(row);
                    }
                    // Actualiza el contador de filas.
                    label1.Text = $"Número de filas: {tabla_Inventario.Rows.Count}";
                }
            }
            else
            {
                MessageBox.Show("No se ha seleccionado ninguna fila para eliminar.", "Ninguna fila seleccionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // --- Método Principal de Sincronización de Datos ---

        // Manejador de evento para el botón de subir datos a la base de datos de la página web.
        private async void boton_SubirDatos_Click(object sender, EventArgs e)
        {
            // Primero, valida si hay ítems con datos faltantes. Si los hay, muestra un error y detiene el proceso.
            if (itemsNull.Count > 0)
            {
                string itemsWithNulls = string.Join(", ", itemsNull);
                MessageBox.Show($"Los siguientes ítems tienen valores nulos en Referencia, Existencias, Linea o Tipo: {itemsWithNulls}. Corrija los datos antes de subirlos.", "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Pide confirmación, advirtiendo que los datos existentes en la web serán eliminados.
            var result = MessageBox.Show("¿Está seguro de que desea subir los datos del DataGridView a la base de datos? Esto eliminará toda la información previa en la base de datos.", "Confirmar subida", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Enabled = false; // Deshabilita el formulario para evitar interacciones durante la subida.
                try
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString_Pagina))
                    {
                        await connection.OpenAsync(); // Abre la conexión a la base de datos MySQL.

                        // Paso 1: Borra todos los datos existentes en la tabla de inventario de la web.
                        string deleteQuery = "DELETE FROM inventario";
                        using (MySqlCommand deleteCommand = new MySqlCommand(deleteQuery, connection))
                        {
                            await deleteCommand.ExecuteNonQueryAsync();
                        }

                        // Prepara las variables para la inserción en lotes y la barra de progreso.
                        int totalRows = tabla_Inventario.Rows.Count;
                        int currentRow = 0;
                        int batchSize = 1000; // Define un tamaño de lote para la inserción.
                        List<string> rowsToInsert = new List<string>();

                        // Muestra la ventana de la barra de progreso.
                        var progressMessageBox = ProgressBarMessageBox.ShowProgressBar("Subiendo datos...", "Progreso");
                        progressMessageBox.Show();

                        // Paso 2: Inserta los nuevos datos. Se ejecuta en un hilo de fondo para no congelar la UI.
                        await Task.Run(async () =>
                        {
                            foreach (DataGridViewRow row in tabla_Inventario.Rows)
                            {
                                if (!row.IsNewRow)
                                {
                                    // Prepara los valores de cada celda, escapando caracteres especiales.
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

                                    // Agrega la fila formateada a la lista para la inserción en lote.
                                    rowsToInsert.Add($"({referencia}, {descripcion}, {alternos}, {existencias}, {bodega} ,{precio}, {linea}, {priceid} ,{tipo},{diassinventas},{abcclass})");

                                    // Cuando la lista alcanza el tamaño del lote, la inserta en la BD.
                                    if (rowsToInsert.Count >= batchSize)
                                    {
                                        await InsertBatch(connection, rowsToInsert);
                                        rowsToInsert.Clear(); // Limpia la lista para el siguiente lote.
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

                            // Inserta cualquier lote restante que no haya alcanzado el tamaño completo.
                            if (rowsToInsert.Count > 0)
                            {
                                await InsertBatch(connection, rowsToInsert);
                            }
                        });

                        progressMessageBox.Close(); // Cierra la ventana de progreso.
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
                    this.Enabled = true; // Vuelve a habilitar el formulario.
                }
            }
        }

        // --- Métodos Auxiliares ---

        // Método auxiliar para insertar un lote de filas en la base de datos.
        private async Task InsertBatch(MySqlConnection connection, List<string> rowsToInsert)
        {
            // Construye una única consulta INSERT con múltiples valores. Es mucho más eficiente que un INSERT por fila.
            string insertQuery = "INSERT INTO inventario (Referencia, descripcion, Alternos, Existencias, Bodega ,Precio, Linea, PriceId ,Tipo, DiasSinVentas, ABCClass) VALUES " + string.Join(", ", rowsToInsert);
            try
            {
                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    await insertCommand.ExecuteNonQueryAsync();
                }
            }
            catch (Exception)
            {
                // Si hay un error, lo relanza para que el bloque 'catch' principal lo maneje.
                throw;
            }
        }

        // Método auxiliar para escapar comillas simples en los valores de texto para evitar errores de SQL Injection.
        private string EscapeSingleQuotes(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "NULL";
            }
            // Reemplaza una comilla simple (') por dos ('') y rodea el valor con comillas simples.
            return $"'{value.Replace("'", "''")}'";
        }

        // Método auxiliar para formatear el campo de existencias, asegurando que sea un número válido.
        private string FormatExistencias(object value)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return "0"; // Si es nulo o vacío, devuelve "0".
            }
            if (int.TryParse(value.ToString(), out int result))
            {
                return result.ToString();
            }
            return "0"; // Si no se puede convertir a entero, devuelve "0".
        }

        // Manejador para el botón de recargar datos.
        private void boton_Cargar_Click(object sender, EventArgs e)
        {
            LoadData();
            MessageBox.Show("Datos han sido correctamente importados.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Manejador para el botón de exportar la tabla a un archivo de Excel.
        private void boton_ExportarExcel_Click(object sender, EventArgs e)
        {
            // Abre un diálogo para que el usuario elija dónde guardar el archivo.
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Files|*.xlsx";
            saveFileDialog.Title = "Guardar como Excel";
            saveFileDialog.FileName = "Inventario.xlsx";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Utiliza la librería ClosedXML para crear el archivo Excel.
                using (var workbook = new XLWorkbook())
                {
                    var worksheet = workbook.Worksheets.Add("Inventario");

                    // Escribe los encabezados de las columnas.
                    for (int i = 1; i < tabla_Inventario.Columns.Count; i++) // Empieza en 1 para omitir la columna de checkbox.
                    {
                        worksheet.Cell(1, i).Value = tabla_Inventario.Columns[i].HeaderText;
                    }

                    // Escribe los datos de cada fila.
                    for (int i = 0; i < tabla_Inventario.Rows.Count; i++)
                    {
                        for (int j = 1; j < tabla_Inventario.Columns.Count; j++)
                        {
                            worksheet.Cell(i + 2, j).Value = tabla_Inventario.Rows[i].Cells[j].Value?.ToString();
                        }
                    }

                    // Guarda el archivo y notifica al usuario.
                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Datos exportados correctamente a Excel", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        /* --- Código Comentado para Generación de Códigos de Barras ---
         * Esta sección parece ser una funcionalidad planeada o en desuso para crear etiquetas
         * con códigos de barras para los productos seleccionados. Utiliza la librería ZXing.
         *
        private void boton_CrearLabel_Click(object sender, EventArgs e)
        {
            //...
        }

        private void GenerateBarcode(string data)
        {
            //...
        }

        private void DisplayBarcode(Bitmap barcodeBitmap)
        {
            //...
        }
        */
    }
}
