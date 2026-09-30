using TPSocios.Entidades;
using TPSocios.Repositorios;

namespace TPSocios.Forms
{
    /// <summary>
    /// Formulario único del trabajo práctico.
    /// Concentra las cuatro operaciones del CRUD sobre la tabla Socios:
    /// alta (btnRegistrar sin socio seleccionado), modificación (btnRegistrar
    /// con una fila de la grilla seleccionada) y baja (tecla Supr sobre la fila
    /// seleccionada). También realiza todas las validaciones, tal como pide
    /// el enunciado, para que los repositorios no tengan que repetirlas.
    /// </summary>
    public partial class frmTPSocios : Form
    {
        private const string TituloVentana = "Club Social Deportivo";
        private const string TextoBotonAlta = "Registrar";
        private const string TextoBotonModificacion = "Actualizar";

        private readonly ISocioRepository _socioRepository;
        private List<Socio> _sociosCargados;
        private Socio? _socioEnEdicion;

        /// <summary>
        /// El repositorio se recibe por inyección de dependencias desde Program,
        /// de modo que el formulario nunca crea la implementación que utiliza.
        /// </summary>
        public frmTPSocios(ISocioRepository socioRepository)
        {
            InitializeComponent();

            this._socioRepository = socioRepository;
            this._sociosCargados = new List<Socio>();
            this._socioEnEdicion = null;
        }

        /// <summary>
        /// Al cargar el formulario en memoria: centrarlo, configurar los
        /// controles y cargar el listado de socios.
        /// </summary>
        private async void frmTPSocios_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            this.ConfigurarFormulario();

            await this.CargarGrillaAsync();
        }

        /// <summary>
        /// Deja el formulario en condiciones de recibir un alta:
        /// combo con la primera opción seleccionada y grilla sin selección.
        /// </summary>
        internal void ConfigurarFormulario()
        {
            this.CargarTiposSocio();
            this.ConfigurarGrilla();

            // La fecha de nacimiento no puede ser posterior a hoy.
            this.dtpFechaNacimiento.MaxDate = DateTime.Today;

            this.PrepararAlta();
        }

        /// <summary>
        /// Carga el ComboBox con las opciones de tipo de socio admitidas.
        /// Se presenta la descripción y se conserva el valor enumerado
        /// que es el que finalmente se persiste.
        /// </summary>
        private void CargarTiposSocio()
        {
            this.cmbTipoSocio.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoSocio.DisplayMember = nameof(ItemTipoSocio.Descripcion);
            this.cmbTipoSocio.ValueMember = nameof(ItemTipoSocio.Tipo);
            this.cmbTipoSocio.DataSource = new List<ItemTipoSocio>
            {
                new(TipoSocio.Menor, "Menor (menos de 18 años)"),
                new(TipoSocio.Mayor, "Mayor (de 18 a menos de 60 años)"),
                new(TipoSocio.Jubilado, "Jubilado (60 años o más)"),
                new(TipoSocio.Familiar, "Familiar (sin restricción)")
            };

            this.cmbTipoSocio.SelectedIndex = 0;
        }

        /// <summary>
        /// Define las columnas de la grilla, que es de solo lectura:
        /// no se pueden agregar, editar ni eliminar filas.
        /// Las descripciones compactadas de la base se presentan con títulos
        /// espaciados y los valores se formatean según su tipo de dato.
        /// </summary>
        private void ConfigurarGrilla()
        {
            this.dgvSocios.AutoGenerateColumns = false;
            this.dgvSocios.Columns.Clear();

            this.dgvSocios.Columns.Add(Columna("IdSocio", "Id Socio", 70, false));
            this.dgvSocios.Columns.Add(Columna("LegajoSocio", "Legajo Socio", 95));
            this.dgvSocios.Columns.Add(Columna("Apellido", "Apellido", 105));
            this.dgvSocios.Columns.Add(Columna("Nombre", "Nombre", 105));
            this.dgvSocios.Columns.Add(Columna("Email", "Email", 175));
            this.dgvSocios.Columns.Add(Columna("FechaNacimiento", "Fecha Nacimiento", 120, true, "dd/MM/yyyy"));
            this.dgvSocios.Columns.Add(Columna("CuotaMensual", "Cuota Mensual", 110, true, "N2"));
            this.dgvSocios.Columns.Add(Columna("TipoSocio", "Tipo Socio", 100));
            this.dgvSocios.Columns.Add(Columna("Disponible", "Disponible", 80));
        }

        /// <summary>
        /// Crea una columna de solo lectura con el formato solicitado.
        /// </summary>
        private static DataGridViewTextBoxColumn Columna(string propiedad, string titulo,
                                                        int ancho, bool visible = true,
                                                        string? formato = null)
        {
            DataGridViewTextBoxColumn columna = new DataGridViewTextBoxColumn
            {
                Name = propiedad,
                HeaderText = titulo,
                Width = ancho,
                Visible = visible,
                ReadOnly = true,
                SortMode = DataGridViewColumnSortMode.NotSortable
            };

            if (formato is not null)
            {
                columna.DefaultCellStyle.Format = formato;
            }

            return columna;
        }

        /// <summary>
        /// Obtiene todos los socios y los presenta en la grilla.
        /// </summary>
        private async Task CargarGrillaAsync()
        {
            try
            {
                this._sociosCargados = await this._socioRepository.ObtenerTodosAsync();
            }
            catch (Exception excepcion)
            {
                this._sociosCargados = new List<Socio>();
                this.MostrarErrorBase(excepcion, "No se pudo obtener el listado de socios.");
            }

            this.dgvSocios.SuspendLayout();

            if (this.dgvSocios.CurrentCell is not null)
            {
                this.dgvSocios.CurrentCell = null;
            }

            this.dgvSocios.Rows.Clear();

            foreach (Socio socio in this._sociosCargados)
            {
                this.dgvSocios.Rows.Add(
                    socio.IdSocio,
                    socio.LegajoSocio,
                    socio.Apellido,
                    socio.Nombre,
                    socio.Email,
                    socio.FechaNacimiento,
                    socio.CuotaMensual,
                    socio.TipoSocio.ToString(),
                    socio.Disponible ? "Sí" : "No");
            }

            this.dgvSocios.ResumeLayout();

            // La grilla se presenta sin ninguna fila elegida: el formulario
            // queda en modo alta y no se modifica ningún socio por accidente.
            if (this.dgvSocios.Rows.Count > 0)
            {
                this.dgvSocios.CurrentCell = null;
                this.dgvSocios.ClearSelection();
            }
        }

        /// <summary>
        /// Al seleccionar una fila de la grilla se cargan sus datos en los
        /// controles para poder modificarlos, y el botón pasa a "Actualizar".
        /// </summary>
        private void dgvSocios_SelectionChanged(object sender, EventArgs e)
        {
            Socio? socio = this.ObtenerSocioSeleccionado();

            if (socio is null)
            {
                return;
            }

            this._socioEnEdicion = socio;

            this.mtxtLegajoSocio.Text = socio.LegajoSocio;
            this.txtApellido.Text = socio.Apellido;
            this.txtNombre.Text = socio.Nombre;
            this.txtEmail.Text = socio.Email;
            this.dtpFechaNacimiento.Value = socio.FechaNacimiento;
            this.txtCuotaMensual.Text = socio.CuotaMensual.ToString("N2");
            this.cmbTipoSocio.SelectedValue = socio.TipoSocio;
            this.chkDisponible.Checked = socio.Disponible;

            this.btnRegistrar.Text = TextoBotonModificacion;
            this.mtxtLegajoSocio.Focus();
            this.mtxtLegajoSocio.SelectAll();
        }

        /// <summary>
        /// Con la tecla Supr y una fila seleccionada se realiza la baja,
        /// previa confirmación del usuario.
        /// Si el foco está en un control de captura de datos, la tecla Supr
        /// sigue borrando el texto y no elimina ningún socio.
        /// </summary>
        private async void frmTPSocios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Delete || this.TieneFocoEnCaptura())
            {
                return;
            }

            // Se intercepta la tecla para que no llegue al control que tiene el foco.
            e.Handled = true;
            e.SuppressKeyPress = true;

            await this.EliminarSocioSeleccionadoAsync();
        }

        /// <summary>
        /// Indica si el foco está en un control donde la tecla Supr debe
        /// cumplir su función habitual: borrar el texto tipeado.
        /// </summary>
        private bool TieneFocoEnCaptura()
        {
            return this.ActiveControl is TextBoxBase or ComboBox or DateTimePicker;
        }

        /// <summary>
        /// Confirma y ejecuta la baja del socio seleccionado en la grilla.
        /// </summary>
        private async Task EliminarSocioSeleccionadoAsync()
        {
            Socio? socio = this.ObtenerSocioSeleccionado();

            if (socio is null)
            {
                MessageBox.Show("Debe seleccionar un socio de la grilla para eliminarlo.",
                                TituloVentana, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            DialogResult confirmacion = MessageBox.Show(
                $"¿Desea eliminar al socio {socio.Apellido}, {socio.Nombre} " +
                $"(legajo {socio.LegajoSocio})?",
                "Confirmar baja", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmacion != DialogResult.Yes)
            {
                return;
            }

            this.btnRegistrar.Enabled = false;

            try
            {
                await this._socioRepository.EliminarAsync(socio.IdSocio);

                this.PrepararAlta();
                await this.CargarGrillaAsync();
            }
            catch (Exception excepcion)
            {
                this.MostrarErrorBase(excepcion, "No se pudo eliminar el socio.");
            }
            finally
            {
                this.btnRegistrar.Enabled = true;
            }
        }

        /// <summary>
        /// Registra un socio nuevo o, si hay uno seleccionado en la grilla,
        /// modifica los datos del socio existente.
        /// </summary>
        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            if (!this.ValidacionFormulario(out Socio socio))
            {
                return;
            }

            if (!await this.ValidacionReglas(socio))
            {
                return;
            }

            bool esModificacion = socio.IdSocio > 0;

            this.btnRegistrar.Enabled = false;

            try
            {
                if (esModificacion)
                {
                    await this._socioRepository.ActualizarAsync(socio);

                    MessageBox.Show("Los datos del socio fueron actualizados correctamente.",
                                    TituloVentana, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await this._socioRepository.InsertarAsync(socio);

                    MessageBox.Show("El socio fue registrado correctamente.",
                                    TituloVentana, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                this.PrepararAlta();
                await this.CargarGrillaAsync();
            }
            catch (Exception excepcion)
            {
                this.MostrarErrorBase(excepcion,
                    esModificacion ? "No se pudo actualizar el socio." : "No se pudo registrar el socio.");
            }
            finally
            {
                this.btnRegistrar.Enabled = true;
            }
        }

        /// <summary>
        /// Cancela la operación en curso: limpia los controles, deselecciona la
        /// grilla y vuelve al modo de alta.
        /// </summary>
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.PrepararAlta();
        }

        /// <summary>
        /// Deja el formulario en modo de alta, con los controles vacíos.
        /// </summary>
        private void PrepararAlta()
        {
            this._socioEnEdicion = null;

            this.mtxtLegajoSocio.Clear();
            this.txtApellido.Clear();
            this.txtNombre.Clear();
            this.txtEmail.Clear();
            this.dtpFechaNacimiento.Value = DateTime.Today;
            this.txtCuotaMensual.Clear();
            this.cmbTipoSocio.SelectedIndex = 0;
            this.chkDisponible.Checked = false;

            this.dgvSocios.ClearSelection();

            if (this.dgvSocios.CurrentCell is not null)
            {
                this.dgvSocios.CurrentCell = null;
            }

            this.btnRegistrar.Text = TextoBotonAlta;
            this.mtxtLegajoSocio.Focus();
        }

        /// <summary>
        /// Devuelve el socio que se encuentra seleccionado en la grilla,
        /// o null si todavía no hay ninguna fila seleccionada.
        /// </summary>
        private Socio? ObtenerSocioSeleccionado()
        {
            int indice = this.dgvSocios.CurrentRow?.Index ?? -1;

            if (indice < 0 || indice >= this._sociosCargados.Count)
            {
                return null;
            }

            return this._sociosCargados[indice];
        }

        /// <summary>
        /// Validación de formulario: controla los datos ingresados por el usuario
        /// y arma el objeto Socio. No consulta la base de datos.
        /// </summary>
        internal bool ValidacionFormulario(out Socio socio)
        {
            socio = new Socio();

            string legajo = (this.mtxtLegajoSocio.Text ?? string.Empty).Trim();
            string apellido = (this.txtApellido.Text ?? string.Empty).Trim();
            string nombre = (this.txtNombre.Text ?? string.Empty).Trim();
            string email = (this.txtEmail.Text ?? string.Empty).Trim();
            string cuotaTexto = (this.txtCuotaMensual.Text ?? string.Empty).Trim();

            if (!this.mtxtLegajoSocio.MaskCompleted || legajo.Length != 6 || legajo[1] != '-')
            {
                return this.MarcarError(this.mtxtLegajoSocio,
                    "El legajo debe tener el formato L-0000: un carácter, un guion y cuatro dígitos.");
            }

            if (apellido.Length == 0)
            {
                return this.MarcarError(this.txtApellido, "El apellido no puede estar vacío.");
            }

            if (apellido.Length > 50)
            {
                return this.MarcarError(this.txtApellido, "El apellido no puede superar los 50 caracteres.");
            }

            if (nombre.Length == 0)
            {
                return this.MarcarError(this.txtNombre, "El nombre no puede estar vacío.");
            }

            if (nombre.Length > 50)
            {
                return this.MarcarError(this.txtNombre, "El nombre no puede superar los 50 caracteres.");
            }

            if (email.Length == 0)
            {
                return this.MarcarError(this.txtEmail, "El email no puede estar vacío.");
            }

            if (email.Length > 100)
            {
                return this.MarcarError(this.txtEmail, "El email no puede superar los 100 caracteres.");
            }

            if (!EmailValido(email))
            {
                return this.MarcarError(this.txtEmail, "El formato del email no es correcto.");
            }

            if (!decimal.TryParse(cuotaTexto, out decimal cuota))
            {
                return this.MarcarError(this.txtCuotaMensual,
                    "La cuota mensual debe ser un valor numérico.");
            }

            if (cuota <= 0m)
            {
                return this.MarcarError(this.txtCuotaMensual, "La cuota mensual debe ser mayor a cero.");
            }

            if (this.cmbTipoSocio.SelectedValue is not TipoSocio tipoSocio)
            {
                return this.MarcarError(this.cmbTipoSocio, "Debe seleccionar un tipo de socio.");
            }

            // El IdSocio se conserva solo si el usuario está modificando un
            // socio ya existente; en un alta vale cero.
            socio = new Socio(
                idSocio: this._socioEnEdicion?.IdSocio ?? 0,
                legajoSocio: legajo,
                apellido: apellido,
                nombre: nombre,
                email: email,
                fechaNacimiento: this.dtpFechaNacimiento.Value.Date,
                cuotaMensual: cuota,
                tipoSocio: tipoSocio,
                disponible: this.chkDisponible.Checked);

            return true;
        }

        /// <summary>
        /// Validación de reglas de negocio: la edad debe respetar el rango del
        /// tipo de socio seleccionado y el legajo debe ser único.
        /// La consulta de unicidad sí accede a la base de datos.
        /// </summary>
        internal async Task<bool> ValidacionReglas(Socio socio)
        {
            if (socio.FechaNacimiento > DateTime.Today)
            {
                return this.MarcarError(this.dtpFechaNacimiento,
                    "La fecha de nacimiento no puede ser posterior a hoy.");
            }

            if (!CumpleEdadSegunTipo(socio.Edad, socio.TipoSocio))
            {
                return this.MarcarError(this.dtpFechaNacimiento,
                    $"La edad obtenida ({socio.Edad} años) no corresponde al rango del tipo de socio seleccionado.");
            }

            try
            {
                if (await this._socioRepository.ExisteLegajoAsync(socio.LegajoSocio, socio.IdSocio))
                {
                    return this.MarcarError(this.mtxtLegajoSocio,
                        $"El legajo {socio.LegajoSocio} ya se encuentra registrado por otro socio.");
                }
            }
            catch (Exception excepcion)
            {
                this.MostrarErrorBase(excepcion, "No se pudo verificar la unicidad del legajo.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Indica si la edad obtenida cumple el rango del tipo de socio.
        /// Menor: menos de 18 años. Mayor: de 18 a menos de 60.
        /// Jubilado: 60 años o más. Familiar: sin restricción.
        /// </summary>
        internal static bool CumpleEdadSegunTipo(int edad, TipoSocio tipoSocio)
        {
            return tipoSocio switch
            {
                TipoSocio.Menor => edad < 18,
                TipoSocio.Mayor => edad >= 18 && edad < 60,
                TipoSocio.Jubilado => edad >= 60,
                TipoSocio.Familiar => true,
                _ => false
            };
        }

        /// <summary>
        /// Valida el formato del email: un único arroba con texto a ambos lados,
        /// un punto en el dominio y sin espacios.
        /// </summary>
        internal static bool EmailValido(string email)
        {
            int arroba = email.IndexOf('@');

            return arroba > 0
                   && arroba == email.LastIndexOf('@')
                   && arroba < email.Length - 1
                   && !email.Contains(' ')
                   && email.IndexOf('.', arroba) > arroba;
        }

        /// <summary>
        /// Presenta el error, lleva el foco al control causante y devuelve false
        /// para interrumpir el proceso de alta o modificación.
        /// </summary>
        private bool MarcarError(Control control, string mensaje)
        {
            MessageBox.Show(mensaje, "Datos inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            control.Focus();
            return false;
        }

        /// <summary>
        /// Presenta el detalle de un error ocurrido al acceder a la base de datos.
        /// </summary>
        private void MostrarErrorBase(Exception excepcion, string mensaje)
        {
            MessageBox.Show(
                $"{mensaje}\n\nDetalle: {excepcion.Message}",
                "Error de base de datos",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}
