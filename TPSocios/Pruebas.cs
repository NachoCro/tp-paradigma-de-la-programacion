using System.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TPSocios.Configuracion;
using TPSocios.Datos;
using TPSocios.Entidades;
using TPSocios.Forms;
using TPSocios.Repositorios;

namespace TPSocios
{
    /// <summary>
    /// Verificaciones de la TP con datos ficticios, que no acceden a la base
    /// de datos. Se ejecutan con: TPSocios.exe --pruebas
    /// Si alguna falla, el proceso termina con un código de error distinto de cero.
    /// </summary>
    internal static class Pruebas
    {
        private static int _fallos;

        public static int Ejecutar()
        {
            _fallos = 0;

            ProbarInyeccionDeDependencias();
            ProbarAccesoADatos();
            ProbarTipoSocio();
            ProbarSocio();
            ProbarReglas();
            ProbarFormulario();
            ProbarValidacionFormulario();

            Console.WriteLine(_fallos == 0
                ? "\nTODAS LAS PRUEBAS OK"
                : $"\n{_fallos} PRUEBA(S) FALLARON");

            return _fallos;
        }

        private static void Verificar(string nombre, bool condicion)
        {
            if (!condicion)
            {
                _fallos++;
            }

            Console.WriteLine($"  {(condicion ? "OK   " : "FALLA")}  {nombre}");
        }

        private static void Seccion(string titulo)
        {
            Console.WriteLine($"\n-- {titulo} --");
        }

        /// <summary>
        /// Verifica que el contenedor resuelva las dependencias declaradas
        /// y que las dos implementaciones del repositorio respeten el contrato.
        /// </summary>
        private static void ProbarInyeccionDeDependencias()
        {
            Seccion("Inyección de dependencias");

            ServiceCollection services = new ServiceCollection();
            services.AddScoped<ISocioRepository, SocioRepositorySQL>();
            services.AddTransient<frmTPSocios>();

            using ServiceProvider provider = services.BuildServiceProvider();

            Verificar("El contenedor resuelve el repositorio SQL",
                provider.GetRequiredService<ISocioRepository>() is SocioRepositorySQL);

            Verificar("El contenedor resuelve el formulario",
                provider.GetRequiredService<frmTPSocios>() is frmTPSocios);

            using frmTPSocios formulario = (frmTPSocios)provider.GetRequiredService<frmTPSocios>();

            Verificar("Cada pedido del formulario obtiene su propia instancia",
                !ReferenceEquals(provider.GetRequiredService<frmTPSocios>(), formulario));

            Verificar("La implementación CSV se instancia sin errores de compilación",
                LanzaNotImplemented(() => new SocioRepositoryCSV().ObtenerTodosAsync()));
        }

        /// <summary>
        /// Verifica que los nombres del enumerado coincidan con los valores
        /// admitidos por la restricción de la base de datos.
        /// </summary>
        private static void ProbarTipoSocio()
        {
            Seccion("Enumerado TipoSocio");

            Verificar("Menor    -> \"Menor\"", TipoSocio.Menor.ToString() == "Menor");
            Verificar("Mayor    -> \"Mayor\"", TipoSocio.Mayor.ToString() == "Mayor");
            Verificar("Jubilado -> \"Jubilado\"", TipoSocio.Jubilado.ToString() == "Jubilado");
            Verificar("Familiar -> \"Familiar\"", TipoSocio.Familiar.ToString() == "Familiar");
        }

        /// <summary>
        /// Verifica los dos constructores de la clase Socio, el encapsulamiento
        /// de sus propiedades y el cálculo de la edad.
        /// </summary>
        private static void ProbarSocio()
        {
            Seccion("Clase Socio");

            Socio vacio = new Socio();
            Verificar("Constructor sin parámetros: IdSocio en cero", vacio.IdSocio == 0);
            Verificar("Constructor sin parámetros: CuotaMensual en cero", vacio.CuotaMensual == 0m);
            Verificar("Constructor sin parámetros: Disponible en falso", !vacio.Disponible);
            Verificar("Constructor sin parámetros: textos vacíos",
                vacio.LegajoSocio.Length == 0 && vacio.Apellido.Length == 0 &&
                vacio.Nombre.Length == 0 && vacio.Email.Length == 0);

            DateTime nacimiento = new DateTime(1990, 3, 15);
            Socio socio = new Socio(7, "M-0042", "Gómez", "Ana", "ana@email.com",
                                    nacimiento, 9500.50m, TipoSocio.Mayor, true);

            Verificar("Constructor parametrizado: IdSocio", socio.IdSocio == 7);
            Verificar("Constructor parametrizado: LegajoSocio", socio.LegajoSocio == "M-0042");
            Verificar("Constructor parametrizado: Apellido", socio.Apellido == "Gómez");
            Verificar("Constructor parametrizado: Nombre", socio.Nombre == "Ana");
            Verificar("Constructor parametrizado: Email", socio.Email == "ana@email.com");
            Verificar("Constructor parametrizado: FechaNacimiento", socio.FechaNacimiento == nacimiento);
            Verificar("Constructor parametrizado: CuotaMensual", socio.CuotaMensual == 9500.50m);
            Verificar("Constructor parametrizado: TipoSocio", socio.TipoSocio == TipoSocio.Mayor);
            Verificar("Constructor parametrizado: Disponible", socio.Disponible);

            socio.Apellido = "Pérez";
            Verificar("Propiedad encapsulada: escritura y lectura", socio.Apellido == "Pérez");

            Verificar("Edad: ayer del cumpleaños",
                new Socio(0, string.Empty, string.Empty, string.Empty, string.Empty,
                          DateTime.Today.AddYears(-30).AddDays(-1), 0m, TipoSocio.Mayor, false).Edad == 30);

            Verificar("Edad: mañana del cumpleaños",
                new Socio(0, string.Empty, string.Empty, string.Empty, string.Empty,
                          DateTime.Today.AddYears(-30).AddDays(1), 0m, TipoSocio.Mayor, false).Edad == 29);
        }

        /// <summary>
        /// Verifica los rangos de edad por tipo de socio y el formato del email.
        /// </summary>
        private static void ProbarReglas()
        {
            Seccion("Reglas de negocio");

            Verificar("Menor acepta 10 años", Cumple(10, TipoSocio.Menor));
            Verificar("Menor rechaza 18 años", !Cumple(18, TipoSocio.Menor));
            Verificar("Mayor acepta 18 años", Cumple(18, TipoSocio.Mayor));
            Verificar("Mayor acepta 59 años", Cumple(59, TipoSocio.Mayor));
            Verificar("Mayor rechaza 60 años", !Cumple(60, TipoSocio.Mayor));
            Verificar("Jubilado acepta 60 años", Cumple(60, TipoSocio.Jubilado));
            Verificar("Jubilado rechaza 59 años", !Cumple(59, TipoSocio.Jubilado));
            Verificar("Familiar acepta 5 años", Cumple(5, TipoSocio.Familiar));
            Verificar("Familiar acepta 80 años", Cumple(80, TipoSocio.Familiar));

            Verificar("Email válido: ana.martinez@email.com",
                frmTPSocios.EmailValido("ana.martinez@email.com"));
            Verificar("Email inválido: sin arroba", !frmTPSocios.EmailValido("anaemail.com"));
            Verificar("Email inválido: dos arrobas", !frmTPSocios.EmailValido("ana@@email.com"));
            Verificar("Email inválido: sin dominio", !frmTPSocios.EmailValido("ana@"));
            Verificar("Email inválido: con espacio", !frmTPSocios.EmailValido("ana@email .com"));

            bool Cumple(int edad, TipoSocio tipo)
            {
                return frmTPSocios.CumpleEdadSegunTipo(edad, tipo);
            }
        }

        /// <summary>
        /// Verifica que el formulario se presente como pide el enunciado:
        /// título, centrado, no maximizable y con los controles requeridos.
        /// </summary>
        private static void ProbarFormulario()
        {
            Seccion("Formulario frmTPSocios");

            using frmTPSocios f = new frmTPSocios(new SocioRepositorySQL());

            Verificar("Título: \"Club Social Deportivo\"", f.Text == "Club Social Deportivo");
            Verificar("Se presenta centrado en pantalla",
                f.StartPosition == FormStartPosition.CenterScreen);
            Verificar("No se puede maximizar", !f.MaximizeBox);
            Verificar("Enter invoca al botón Registrar",
                f.AcceptButton is Button registrar && registrar.Name == "btnRegistrar");
            Verificar("Escape invoca al botón Cancelar",
                f.CancelButton is Button cancelar && cancelar.Name == "btnCancelar");
            Verificar("El formulario recibe el teclado (tecla Supr)", f.KeyPreview);

            Control? Buscar(string nombre)
            {
                return f.Controls.Find(nombre, true).FirstOrDefault();
            }

            Verificar("Existe btnRegistrar", Buscar("btnRegistrar") is Button);
            Verificar("Existe btnCancelar", Buscar("btnCancelar") is Button);
            Verificar("Existe mtxtLegajoSocio (MaskedTextBox)", Buscar("mtxtLegajoSocio") is MaskedTextBox);
            Verificar("Existe dtpFechaNacimiento (DateTimePicker)", Buscar("dtpFechaNacimiento") is DateTimePicker);
            Verificar("Existe cmbTipoSocio (ComboBox)", Buscar("cmbTipoSocio") is ComboBox);
            Verificar("Existe chkDisponible (CheckBox)", Buscar("chkDisponible") is CheckBox);
            Verificar("Existe dgvSocios (DataGridView)", Buscar("dgvSocios") is DataGridView);

            Verificar("Máscara del legajo: un carácter, guion y cuatro dígitos",
                Buscar("mtxtLegajoSocio") is MaskedTextBox { Mask: "?\\-####" });

            Verificar("MaxLength de Apellido: 50", Buscar("txtApellido") is TextBox { MaxLength: 50 });
            Verificar("MaxLength de Nombre: 50", Buscar("txtNombre") is TextBox { MaxLength: 50 });
            Verificar("MaxLength de Email: 100", Buscar("txtEmail") is TextBox { MaxLength: 100 });

            DataGridView grilla = (DataGridView)Buscar("dgvSocios")!;
            Verificar("Grilla de solo lectura", grilla.ReadOnly);
            Verificar("Grilla sin agregar filas", !grilla.AllowUserToAddRows);
            Verificar("Grilla sin eliminar filas", !grilla.AllowUserToDeleteRows);
            Verificar("Grilla de selección de una sola fila", !grilla.MultiSelect);

            // Se invoca por fuera lo mismo que hace el evento Load,
            // para no tener que conectar la base de datos.
            f.ConfigurarFormulario();

            DataGridViewColumn id = grilla.Columns["IdSocio"]!;
            Verificar("Columna IdSocio oculta", !id.Visible);
            Verificar("Título espaciado: \"Legajo Socio\"",
                grilla.Columns["LegajoSocio"]!.HeaderText == "Legajo Socio");
            Verificar("Título espaciado: \"Fecha Nacimiento\"",
                grilla.Columns["FechaNacimiento"]!.HeaderText == "Fecha Nacimiento");
            Verificar("Título espaciado: \"Cuota Mensual\"",
                grilla.Columns["CuotaMensual"]!.HeaderText == "Cuota Mensual");
            Verificar("Título espaciado: \"Tipo Socio\"",
                grilla.Columns["TipoSocio"]!.HeaderText == "Tipo Socio");
            Verificar("Fecha Nacimiento con formato dd/MM/yyyy",
                (string?)grilla.Columns["FechaNacimiento"]!.DefaultCellStyle.Format == "dd/MM/yyyy");
            Verificar("Cuota Mensual con formato N2",
                (string?)grilla.Columns["CuotaMensual"]!.DefaultCellStyle.Format == "N2");

            ComboBox cmb = (ComboBox)Buscar("cmbTipoSocio")!;
            Verificar("ComboBox de solo selección",
                cmb.DropDownStyle == ComboBoxStyle.DropDownList);
            Verificar("ComboBox con las cuatro opciones", cmb.Items.Count == 4);
            Verificar("Primera opción del ComboBox seleccionada", cmb.SelectedIndex == 0);
            Verificar("Primera opción: \"Menor (menos de 18 años)\"",
                cmb.Items[0] is ItemTipoSocio
                {
                    Tipo: TipoSocio.Menor,
                    Descripcion: "Menor (menos de 18 años)"
                });

            Verificar("La fecha de nacimiento no admite un valor posterior a hoy",
                ((DateTimePicker)Buscar("dtpFechaNacimiento")!).MaxDate < DateTime.Today.AddDays(1));

            Verificar("El botón de registro comienza en modo alta",
                ((Button)Buscar("btnRegistrar")!).Text == "Registrar");
        }

        /// <summary>
        /// Verifica la validación de formulario con un alta correcta.
        /// Solo se prueba el camino válido: el inválido presenta un MessageBox.
        /// </summary>
        private static void ProbarValidacionFormulario()
        {
            Seccion("Validación de formulario");

            using frmTPSocios f = new frmTPSocios(new SocioRepositoryCSV());
            f.ConfigurarFormulario();

            f.Controls.Find("mtxtLegajoSocio", true)[0].Text = "A-0007";
            f.Controls.Find("txtApellido", true)[0].Text = "Gómez";
            f.Controls.Find("txtNombre", true)[0].Text = "Ana";
            f.Controls.Find("txtEmail", true)[0].Text = "ana.gomez@email.com";
            // El texto se arma con el formato de la configuración regional,
            // para que la conversión a número no dependa del equipo.
            f.Controls.Find("txtCuotaMensual", true)[0].Text = 9500.50m.ToString("N2");
            ((DateTimePicker)f.Controls.Find("dtpFechaNacimiento", true)[0]).Value =
                DateTime.Today.AddYears(-30);
            ((ComboBox)f.Controls.Find("cmbTipoSocio", true)[0]).SelectedValue = TipoSocio.Mayor;
            ((CheckBox)f.Controls.Find("chkDisponible", true)[0]).Checked = true;

            bool valido = f.ValidacionFormulario(out Socio socio);

            Verificar("Acepta un formulario correctamente completo", valido);
            Verificar("IdSocio en cero durante un alta", socio.IdSocio == 0);
            Verificar("Legajo sin espacios sobrantes", socio.LegajoSocio == "A-0007");
            Verificar("Apellido con acentos", socio.Apellido == "Gómez");
            Verificar("Cuota convertida a número", socio.CuotaMensual == 9500.50m);
            Verificar("Tipo de socio leído del ComboBox", socio.TipoSocio == TipoSocio.Mayor);
            Verificar("Disponible leído del CheckBox", socio.Disponible);
        }

        /// <summary>
        /// Verifica la configuración de la conexión y la clase Database.
        /// Estas comprobaciones no abren ninguna conexión: solo validan que la
        /// cadena de conexión esté bien formada y que la clase rechace las
        /// entradas inválidas antes de intentar conectarse al servidor.
        /// </summary>
        private static void ProbarAccesoADatos()
        {
            Seccion("Configuración y acceso a datos");

            string conexion = CadenaConexion.Valor;

            Verificar("La cadena de conexión no está vacía",
                !string.IsNullOrWhiteSpace(conexion));

            Verificar("La cadena de conexión apunta a la base TPSocios",
                conexion.Contains("Database=TPSocios", StringComparison.OrdinalIgnoreCase));

            Verificar("La cadena de conexión indica el servidor localhost,1433",
                conexion.Contains("localhost,1433", StringComparison.OrdinalIgnoreCase));

            Verificar("La cadena de conexión ignora el certificado del servidor",
                conexion.Contains("TrustServerCertificate=True", StringComparison.OrdinalIgnoreCase));

            Verificar("La cadena de conexión no está vacía en App.config",
                ConfigurationAppConfig().Length > 0);

            Verificar("Database rechaza una cadena de conexión vacía",
                LanzaArgumento(() => new Database(string.Empty)));

            Verificar("Database acepta una cadena de conexión válida",
                new Database(conexion) is not null);

            // La consulta vacía se valida antes de abrir la conexión, así que
            // esta prueba funciona aunque no haya ningún SQL Server en la máquina.
            Verificar("Database rechaza una consulta vacía",
                LanzaArgumento(async () => await new Database(conexion).ConsultarAsync(" ")));

            Verificar("Database rechaza una consulta nula",
                LanzaArgumento(async () => await new Database(conexion).ConsultarAsync(null!)));

            Verificar("El repositorio SQL se construye con el ayudante de datos",
                new SocioRepositorySQL(new Database(conexion)) is SocioRepositorySQL);

            Verificar("El repositorio SQL se construye con la cadena de conexión",
                new SocioRepositorySQL(conexion) is SocioRepositorySQL);
        }

        /// <summary>
        /// Lee directamente el App.config, sin pasar por la clase
        /// CadenaConexion, para comprobar que el archivo se distribuyó
        /// correctamente junto al ejecutable.
        /// </summary>
        private static string ConfigurationAppConfig()
        {
            try
            {
                return ConfigurationManager.ConnectionStrings[CadenaConexion.Nombre]?.ConnectionString
                       ?? string.Empty;
            }
            catch (ConfigurationErrorsException)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Indica si la operación lanza ArgumentException, que es el error que
        /// produce la validación de los datos de entrada.
        /// </summary>
        private static bool LanzaArgumento(Func<Task> accion)
        {
            try
            {
                accion().GetAwaiter().GetResult();
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        private static bool LanzaArgumento(Action accion)
        {
            try
            {
                accion();
                return false;
            }
            catch (ArgumentException)
            {
                return true;
            }
        }

        /// <summary>
        /// Indica si la operación lanza NotImplementedException, es decir, si el
        /// repositorio CSV fue construido sin desarrollar, como pide el enunciado.
        /// </summary>
        private static bool LanzaNotImplemented(Action accion)
        {
            try
            {
                accion();
                return false;
            }
            catch (NotImplementedException)
            {
                return true;
            }
            catch (AggregateException excepcion) when (excepcion.InnerException is NotImplementedException)
            {
                return true;
            }
        }
    }
}
