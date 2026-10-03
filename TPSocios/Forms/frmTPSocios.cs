using System.Diagnostics;
using TPSocios.Entidades;
using TPSocios.Presentacion.Animaciones;
using TPSocios.Presentacion.Temas;
using TPSocios.Repositorios;

namespace TPSocios.Forms
{
    public partial class frmTPSocios : Form
    {
        private const string TituloVentana = "Club Social Deportivo";
        private const string TextoBotonRegistrar = "Registrar";
        private const string TextoBotonActualizar = "Actualizar";
        private const string ExitoAlta = "El socio fue registrado correctamente.";
        private const string ExitoBaja = "El socio fue eliminado correctamente.";
        private const string ExitoActualizacion = "Los datos del socio fueron actualizados correctamente.";
        private const int CargaMinimaMs = 700;

        private static readonly Color ColorExito = Color.FromArgb(57, 255, 20);

        private readonly ISocioRepository _socioRepository;
        private List<Socio> _sociosCargados;
        private Socio? _socioEnEdicion;

        private bool _ignorandoSeleccionInicial = true;

        private bool _operacionEnCurso;

        private bool _cargaEnCurso;

        private TipoTema _tipoTema = TipoTema.Oscuro;

        internal TipoTema TipoTemaActual => this._tipoTema;

        internal string MensajeErrorAnimado => this.mensajeErrores.Mensaje;

        internal bool AnimacionErrorEnCurso => this.mensajeErrores.EscrituraEnCurso;

        internal int PasosEscritura => this.mensajeErrores.PasosSimulados;

        internal bool CargaEnCurso => this._cargaEnCurso;

        internal bool MensajeEnPantalla => this.mensajeErrores.BordeActivo;

        internal bool BotonRegistrarHabilitado => this.btnRegistrar.Enabled;

        public frmTPSocios(ISocioRepository socioRepository)
        {
            InitializeComponent();

            this._socioRepository = socioRepository;
            this._sociosCargados = new List<Socio>();
            this._socioEnEdicion = null;
            this._ignorandoSeleccionInicial = true;
        }

        private async void frmTPSocios_Load(object sender, EventArgs e)
        {
            this.CenterToScreen();

            this.ConfigurarFormulario();

            await this.CargarGrillaAsync();
        }

        internal void ConfigurarFormulario()
        {
            this.AplicarTemaActual();

            this.CargarTiposSocio();
            this.ConfigurarGrilla();

            this.dtpFechaNacimiento.MaxDate = DateTime.Today;

            this.PrepararAlta();
        }

        internal void AplicarTemaActual()
        {
            Tema tema = Temas.Obtener(this._tipoTema);

            AplicadorTema.Aplicar(this, tema);

            this.lblAyuda.ForeColor = tema.TextoTenue;
            this.lblIntegrantes.ForeColor = tema.TextoTenue;
        }

        internal void CambiarTema()
        {
            this._tipoTema = Temas.Alternar(this._tipoTema);
            this.AplicarTemaActual();
        }

        private void CargarTiposSocio()
        {
            this.cmbTipoSocio.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoSocio.DisplayMember = nameof(ItemTipoSocio.Descripcion);
            this.cmbTipoSocio.ValueMember = nameof(ItemTipoSocio.Tipo);
            this.cmbTipoSocio.DataSource = new List<ItemTipoSocio>
            {
                new(TipoSocio.Menor, "Menor (< 18 años)"),
                new(TipoSocio.Mayor, "Mayor (>= 18 y < 60)"),
                new(TipoSocio.Jubilado, "Jubilado (>= 60)"),
                new(TipoSocio.Familiar, "Familiar (sin restricción)")
            };

            this.cmbTipoSocio.SelectedIndex = 0;
        }

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

        private async Task CargarGrillaAsync()
        {
            List<Socio> socios;

            try
            {
                socios = await this._socioRepository.ObtenerTodosAsync();
            }
            catch (Exception excepcion)
            {
                socios = new List<Socio>();
                this.MostrarErrorBase(excepcion, "No se pudo obtener el listado de socios.");
            }

            this.CargarGrilla(socios);
        }

        internal void CargarGrilla(IList<Socio> socios)
        {
            this._sociosCargados = new List<Socio>(socios);

            this._ignorandoSeleccionInicial = true;

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

            this.PrepararAlta();
            this.ProgramarHabilitarSeleccion();
        }

        private void dgvSocios_SelectionChanged(object? sender, EventArgs e)
        {
            if (this._ignorandoSeleccionInicial)
            {
                return;
            }

            Socio? socio = this.ObtenerSocioSeleccionado();

            if (socio is null)
            {
                return;
            }

            this.CargarSocioEnEdicion(socio);
        }

        internal void CargarSocioEnEdicion(Socio socio)
        {
            this._socioEnEdicion = socio;

            this.mtxtLegajoSocio.Text = socio.LegajoSocio;
            this.txtApellido.Text = socio.Apellido;
            this.txtNombre.Text = socio.Nombre;
            this.txtEmail.Text = socio.Email;
            this.dtpFechaNacimiento.Value = socio.FechaNacimiento;
            this.txtCuotaMensual.Text = socio.CuotaMensual.ToString("N2");
            this.cmbTipoSocio.SelectedValue = socio.TipoSocio;
            this.chkDisponible.Checked = socio.Disponible;

            this.BotonesSegunElModo();
            this.mtxtLegajoSocio.Focus();
            this.mtxtLegajoSocio.SelectAll();
        }

        private void BotonesSegunElModo()
        {
            bool alta = this._socioEnEdicion is null;

            this.btnRegistrar.Text = alta ? TextoBotonRegistrar : TextoBotonActualizar;
            this.btnRegistrar.Enabled = !this._operacionEnCurso;
        }

        private void ProgramarHabilitarSeleccion()
        {
            if (!this.IsHandleCreated)
            {
                this.AlMostrarLaVentana();
                return;
            }

            this.BeginInvoke(new Action(this.AlMostrarLaVentana));
        }

        internal bool IgnoraSeleccionInicial => this._ignorandoSeleccionInicial;

        internal int SociosCargados => this._sociosCargados.Count;

        internal void AlMostrarLaVentana()
        {
            this.dgvSocios.SelectionChanged -= this.dgvSocios_SelectionChanged;
            this.dgvSocios.CurrentCell = null;
            this.dgvSocios.ClearSelection();
            this.dgvSocios.SelectionChanged += this.dgvSocios_SelectionChanged;

            this._ignorandoSeleccionInicial = false;

            this.PrepararAlta(limpiarMensaje: false);
        }

        private async void frmTPSocios_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.D)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;

                this.CambiarTema();
                return;
            }

            if (e.KeyCode != Keys.Delete || this.TieneFocoEnCaptura())
            {
                return;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;

            await this.EliminarSocioSeleccionadoAsync();
        }

        private bool TieneFocoEnCaptura()
        {
            return this.ActiveControl is TextBoxBase or ComboBox or DateTimePicker;
        }

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

            this._operacionEnCurso = true;
            this.BotonesSegunElModo();

            try
            {
                await this.ConCargaAsync(() => this._socioRepository.EliminarAsync(socio.IdSocio));

                this.PrepararAlta();
                await this.CargarGrillaAsync();

                this.mensajeErrores.Escribir(ExitoBaja, ColorExito);
            }
            catch (Exception excepcion)
            {
                this.MostrarErrorBase(excepcion, "No se pudo eliminar el socio.");
            }
            finally
            {
                this._operacionEnCurso = false;
                this.BotonesSegunElModo();
            }
        }

        private async Task ConCargaAsync(Func<Task> operacion)
        {
            this._cargaEnCurso = true;
            this.pbOperacion.Valor = 0;
            this.pbOperacion.Visible = true;

            using System.Windows.Forms.Timer reloj = new() { Interval = 25 };

            Stopwatch cronometro = Stopwatch.StartNew();

            reloj.Tick += (_, _) => this.pbOperacion.Valor =
                (int)Math.Clamp(cronometro.ElapsedMilliseconds * 100 / CargaMinimaMs, 0, 100);

            reloj.Start();

            try
            {
                await operacion();

                int restante = CargaMinimaMs - (int)cronometro.ElapsedMilliseconds;

                if (restante > 0)
                {
                    await Task.Delay(restante);
                }

                this.pbOperacion.Valor = 100;
            }
            finally
            {
                reloj.Stop();
                cronometro.Stop();

                await Task.Delay(140);

                this._cargaEnCurso = false;
                this.pbOperacion.Visible = false;
            }
        }

        private async void btnRegistrar_Click(object sender, EventArgs e)
        {
            await this.GuardarAsync();
        }

        internal async Task GuardarAsync()
        {
            bool esModificacion = this._socioEnEdicion is not null;

            if (!this.ValidacionFormulario(out Socio socio))
            {
                return;
            }

            if (!await this.ValidacionReglas(socio))
            {
                return;
            }

            this._operacionEnCurso = true;
            this.BotonesSegunElModo();

            try
            {
                string exito;

                if (esModificacion)
                {
                    await this.ConCargaAsync(() => this._socioRepository.ActualizarAsync(socio));

                    exito = ExitoActualizacion;
                }
                else
                {
                    await this.ConCargaAsync(() => this._socioRepository.InsertarAsync(socio));

                    exito = ExitoAlta;
                }

                this.PrepararAlta();
                await this.CargarGrillaAsync();

                this.mensajeErrores.Escribir(exito, ColorExito);
            }
            catch (Exception excepcion)
            {
                this.MostrarErrorBase(excepcion,
                    esModificacion ? "No se pudo actualizar el socio." : "No se pudo registrar el socio.");
            }
            finally
            {
                this._operacionEnCurso = false;
                this.BotonesSegunElModo();
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            this.PrepararAlta();
        }

        internal void PrepararAlta(bool limpiarMensaje = true)
        {
            this._socioEnEdicion = null;

            if (limpiarMensaje)
            {
                this.mensajeErrores.Ocultar();
            }

            this.lblAyuda.Visible = true;

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

            this.BotonesSegunElModo();
            this.mtxtLegajoSocio.Focus();
        }

        private Socio? ObtenerSocioSeleccionado()
        {
            int indice = this.dgvSocios.CurrentRow?.Index ?? -1;

            if (indice < 0 || indice >= this._sociosCargados.Count)
            {
                return null;
            }

            return this._sociosCargados[indice];
        }

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

        internal static bool EmailValido(string email)
        {
            int arroba = email.IndexOf('@');

            return arroba > 0
                   && arroba == email.LastIndexOf('@')
                   && arroba < email.Length - 1
                   && !email.Contains(' ')
                   && email.IndexOf('.', arroba) > arroba;
        }

        private bool MarcarError(Control control, string mensaje)
        {
            Sacudidor.Sacudir(control);

            this.mensajeErrores.Escribir(mensaje);

            control.Focus();

            return false;
        }

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
