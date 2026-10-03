using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using TPSocios.Configuracion;
using TPSocios.Datos;
using TPSocios.Entidades;
using TPSocios.Forms;
using TPSocios.Presentacion.Animaciones;
using TPSocios.Presentacion.Temas;
using TPSocios.Repositorios;

namespace TPSocios
{
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
            ProbarBotonUnicoSegunElModo();
            ProbarAperturaEnModoAlta();
            ProbarTemas();
            ProbarAnimacionDeError();
            ProbarLayoutSinSolapamientos();

            Console.WriteLine(_fallos == 0
                ? "\nTODAS LAS PRUEBAS OK"
                : $"\n{_fallos} PRUEBA(S) FALLARON");

            return _fallos;
        }

        private static void Verificar(string nombre, bool condicion, string detalle = "")
        {
            if (!condicion)
            {
                _fallos++;
            }

            string texto = condicion
                ? nombre
                : (string.IsNullOrEmpty(detalle) ? nombre : $"{nombre} -> {detalle}");

            Console.WriteLine($"  {(condicion ? "OK   " : "FALLA")}  {texto}");
        }

        private static void Seccion(string titulo)
        {
            Console.WriteLine($"\n-- {titulo} --");
        }

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

        private static void ProbarTipoSocio()
        {
            Seccion("Enumerado TipoSocio");

            Verificar("Menor    -> \"Menor\"", TipoSocio.Menor.ToString() == "Menor");
            Verificar("Mayor    -> \"Mayor\"", TipoSocio.Mayor.ToString() == "Mayor");
            Verificar("Jubilado -> \"Jubilado\"", TipoSocio.Jubilado.ToString() == "Jubilado");
            Verificar("Familiar -> \"Familiar\"", TipoSocio.Familiar.ToString() == "Familiar");
        }

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

            List<Button> botones = BotonesDe(f.Controls).ToList();

            Verificar("El formulario tiene solo dos botones (consigna 2.2)",
                botones.Count == 2,
                string.Join(" | ", botones.Select(b => b.Name)));

            Verificar("Los dos botones son btnRegistrar y btnCancelar",
                botones.Any(b => b.Name == "btnRegistrar")
                && botones.Any(b => b.Name == "btnCancelar"));
            Verificar("Existe mtxtLegajoSocio (MaskedTextBox)", Buscar("mtxtLegajoSocio") is MaskedTextBox);
            Verificar("Existe dtpFechaNacimiento (DateTimePicker)", Buscar("dtpFechaNacimiento") is DateTimePicker);
            Verificar("Existe cmbTipoSocio (ComboBox)", Buscar("cmbTipoSocio") is ComboBox);
            Verificar("Existe chkDisponible (CheckBox)", Buscar("chkDisponible") is CheckBox);
            Verificar("Existe dgvSocios (DataGridView)", Buscar("dgvSocios") is DataGridView);

            Verificar("La interfaz muestra a los dos integrantes",
                Buscar("lblIntegrantes") is Label
                {
                    Text: "Integrantes: Fabricio Gullino - Ignacio Crocetti"
                });

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
            Verificar("Primera opción: \"Menor (< 18 años)\"",
                cmb.Items[0] is ItemTipoSocio
                {
                    Tipo: TipoSocio.Menor,
                    Descripcion: "Menor (< 18 años)"
                });

            Verificar("Segunda opción: \"Mayor (>= 18 y < 60)\"",
                cmb.Items[1] is ItemTipoSocio
                {
                    Tipo: TipoSocio.Mayor,
                    Descripcion: "Mayor (>= 18 y < 60)"
                });

            Verificar("Tercera opción: \"Jubilado (>= 60)\"",
                cmb.Items[2] is ItemTipoSocio
                {
                    Tipo: TipoSocio.Jubilado,
                    Descripcion: "Jubilado (>= 60)"
                });

            Verificar("Cuarta opción: \"Familiar (sin restricción)\"",
                cmb.Items[3] is ItemTipoSocio
                {
                    Tipo: TipoSocio.Familiar,
                    Descripcion: "Familiar (sin restricción)"
                });

            Verificar("La fecha de nacimiento no admite un valor posterior a hoy",
                ((DateTimePicker)Buscar("dtpFechaNacimiento")!).MaxDate < DateTime.Today.AddDays(1));

            Verificar("El botón de registro comienza en modo alta",
                ((Button)Buscar("btnRegistrar")!).Text == "Registrar");
        }

        private static void ProbarValidacionFormulario()
        {
            Seccion("Validación de formulario");

            using frmTPSocios f = new frmTPSocios(new SocioRepositoryCSV());
            f.ConfigurarFormulario();

            f.Controls.Find("mtxtLegajoSocio", true)[0].Text = "A-0007";
            f.Controls.Find("txtApellido", true)[0].Text = "Gómez";
            f.Controls.Find("txtNombre", true)[0].Text = "Ana";
            f.Controls.Find("txtEmail", true)[0].Text = "ana.gomez@email.com";
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

        private static void ProbarLayoutSinSolapamientos()
        {
            Seccion("Layout sin solapamientos");

            using frmTPSocios f = new frmTPSocios(new SocioRepositoryCSV());
            f.ConfigurarFormulario();
            f.CreateControl();

            List<string> choques = RevisarSolapamientos(f.Controls);

            Verificar("Ningún control se pisa con otro de la misma fila",
                choques.Count == 0, string.Join(" | ", choques));

            Verificar("La ayuda queda a la izquierda de los botones",
                f.Controls.Find("lblAyuda", true)[0].Right
                    < f.Controls.Find("btnRegistrar", true)[0].Left);

            Verificar("El botón Registrar queda a la izquierda de Cancelar",
                f.Controls.Find("btnRegistrar", true)[0].Right
                    < f.Controls.Find("btnCancelar", true)[0].Left);

            Verificar("La animación de error no tapa la grilla",
                f.Controls.Find("explosionErrores", true)[0].Top
                    > f.Controls.Find("dgvSocios", true)[0].Bottom);

            Verificar("Los integrantes quedan debajo de la ayuda y de los botones",
                f.Controls.Find("lblIntegrantes", true)[0].Top
                    > f.Controls.Find("lblAyuda", true)[0].Bottom
                && f.Controls.Find("lblIntegrantes", true)[0].Top
                    > f.Controls.Find("btnRegistrar", true)[0].Bottom);
        }

        private static IEnumerable<Button> BotonesDe(Control.ControlCollection controles)
        {
            foreach (Control control in controles)
            {
                if (control is Button boton)
                {
                    yield return boton;
                }

                foreach (Button anidado in BotonesDe(control.Controls))
                {
                    yield return anidado;
                }
            }
        }

        private static List<string> RevisarSolapamientos(Control.ControlCollection controles)
        {
            List<string> choques = [];

            List<(string Nombre, Rectangle R)> hermanos = [];

            foreach (Control control in controles)
            {
                hermanos.Add(($"{control.Name} ({control.GetType().Name})", control.Bounds));
            }

            for (int i = 0; i < hermanos.Count; i++)
            {
                for (int j = i + 1; j < hermanos.Count; j++)
                {
                    if (hermanos[i].R.Width <= 0 || hermanos[i].R.Height <= 0)
                    {
                        continue;
                    }

                    Rectangle interseccion =
                        Rectangle.Intersect(hermanos[i].R, hermanos[j].R);

                    if (interseccion.Width > 0 && interseccion.Height > 0)
                    {
                        choques.Add(
                            $"'{hermanos[i].Nombre}' con '{hermanos[j].Nombre}' " +
                            $"({interseccion.Width}x{interseccion.Height}px)");
                    }
                }
            }

            foreach (Control control in controles)
            {
                if (control is DataGridView)
                {
                    continue;
                }

                choques.AddRange(RevisarSolapamientos(control.Controls));
            }

            return choques;
        }

        private static void ProbarBotonUnicoSegunElModo()
        {
            Seccion("Botón único de alta y modificación");

            Socio socio = new(1, "A-0001", "Gómez", "Ana", "ana@email.com",
                DateTime.Today.AddYears(-30), 9000m, TipoSocio.Mayor, true);

            using frmTPSocios f = new frmTPSocios(new RepositorioDePrueba(socio));
            f.ConfigurarFormulario();

            Button registrar = (Button)f.Controls.Find("btnRegistrar", true)[0];
            TextBox apellido = (TextBox)f.Controls.Find("txtApellido", true)[0];

            Verificar("En modo alta el botón dice Registrar",
                registrar.Text == "Registrar", $"texto='{registrar.Text}'");

            Verificar("En modo alta, Enter confirma el alta",
                ReferenceEquals(f.AcceptButton, registrar));

            f.CargarSocioEnEdicion(socio);

            Verificar("Al elegir una fila se cargan sus datos",
                apellido.Text == "Gómez", $"apellido='{apellido.Text}'");

            Verificar("En modo modificación el mismo botón dice Actualizar",
                registrar.Text == "Actualizar", $"texto='{registrar.Text}'");

            Verificar("En modo modificación, Enter confirma la actualización",
                ReferenceEquals(f.AcceptButton, registrar));

            f.PrepararAlta();

            Verificar("Volver al modo alta restaura el texto Registrar y vacía el formulario",
                registrar.Text == "Registrar" && apellido.Text.Length == 0,
                $"texto='{registrar.Text}', apellido='{apellido.Text}'");
        }

        private static void ProbarAperturaEnModoAlta()
        {
            Seccion("Apertura en modo alta");

            RepositorioDePrueba repositorio = new(
                new Socio(1, "A-0001", "Gómez", "Ana", "ana@email.com",
                    DateTime.Today.AddYears(-30), 9000m, TipoSocio.Mayor, true),
                new Socio(2, "B-0002", "Sosa", "Beto", "beto@email.com",
                    DateTime.Today.AddYears(-40), 8000m, TipoSocio.Familiar, false));

            using frmTPSocios f = new frmTPSocios(repositorio);
            f.Show();

            for (int i = 0; i < 5; i++)
            {
                Application.DoEvents();
            }

            DataGridView grilla = (DataGridView)f.Controls.Find("dgvSocios", true)[0];
            TextBox apellido = (TextBox)f.Controls.Find("txtApellido", true)[0];
            Button boton = (Button)f.Controls.Find("btnRegistrar", true)[0];

            Verificar("La grilla muestra todas las filas", grilla.Rows.Count == 2);

            Verificar("Terminada la carga, las selecciones ya son del usuario",
                !f.IgnoraSeleccionInicial);

            Verificar("La grilla queda sin fila elegida al abrir",
                grilla.CurrentRow is null);

            Verificar("La selección automática de la grilla no carga un socio",
                apellido.Text.Length == 0
                && boton.Text == "Registrar",
                $"apellido='{apellido.Text}', botón='{boton.Text}'");

            Verificar("Al abrir el botón de guardado queda habilitado y en modo alta",
                boton.Enabled && boton.Text == "Registrar");

            Verificar("La ventana queda con los cinco campos vacíos",
                ((TextBox)f.Controls.Find("txtNombre", true)[0]).Text.Length == 0
                && ((TextBox)f.Controls.Find("txtEmail", true)[0]).Text.Length == 0
                && ((TextBox)f.Controls.Find("txtCuotaMensual", true)[0]).Text.Length == 0);

            f.Hide();
        }

        private static void ProbarTemas()
        {
            Seccion("Temas retro");

            using frmTPSocios f = new frmTPSocios(new SocioRepositoryCSV());
            f.ConfigurarFormulario();

            DataGridView grilla = (DataGridView)f.Controls.Find("dgvSocios", true)[0];
            TextBox apellido = (TextBox)f.Controls.Find("txtApellido", true)[0];

            Verificar("El formulario arranca en el tema oscuro",
                f.TipoTemaActual == TipoTema.Oscuro);

            Verificar("El fondo oscuro se aplica al formulario",
                f.BackColor == Temas.Oscuro.Fondo);

            Verificar("El fondo oscuro se aplica a la grilla",
                grilla.DefaultCellStyle.BackColor == Temas.Oscuro.Fondo);

            Verificar("El color de captura se aplica a los campos",
                apellido.BackColor == Temas.Oscuro.Superficie);

            Verificar("El texto de los campos toma el verde fósforo",
                apellido.ForeColor == Temas.Oscuro.Texto);

            Verificar("La cabecera de la grilla toma el color propio del tema",
                grilla.ColumnHeadersDefaultCellStyle.BackColor == Temas.Oscuro.CabeceraFondo);

            f.CambiarTema();

            Verificar("CambiarTema pasa al tema claro",
                f.TipoTemaActual == TipoTema.Claro);

            Verificar("El fondo claro se aplica al formulario",
                f.BackColor == Temas.Claro.Fondo);

            Verificar("El fondo claro se aplica a la grilla",
                grilla.DefaultCellStyle.BackColor == Temas.Claro.Fondo);

            Verificar("El color de captura cambia con el tema",
                apellido.BackColor == Temas.Claro.Superficie);

            Verificar("Cambiar de nuevo vuelve al tema oscuro",
                Temas.Alternar(f.TipoTemaActual) == TipoTema.Oscuro);

            ExplosionTexto letras = (ExplosionTexto)f.Controls.Find("explosionErrores", true)[0];

            Verificar("La animación usa una copia propia de la fuente del tema",
                letras.Fuente is not null
                && !ReferenceEquals(letras.Fuente, Temas.Oscuro.FuenteTitulo)
                && !ReferenceEquals(letras.Fuente, Temas.Claro.FuenteTitulo));

            using frmTPSocios otra = new frmTPSocios(new SocioRepositoryCSV());
            otra.ConfigurarFormulario();
            otra.CambiarTema();

            Verificar("Una segunda ventana puede alternar el tema sin fallar",
                otra.TipoTemaActual == TipoTema.Claro);

            otra.CambiarTema();

            Verificar("Y volver al tema oscuro",
                otra.TipoTemaActual == TipoTema.Oscuro);
        }

        private static void ProbarAnimacionDeError()
        {
            Seccion("Animación de error");

            using frmTPSocios f = new frmTPSocios(new SocioRepositoryCSV());
            f.ConfigurarFormulario();

            f.CreateControl();

            MaskedTextBox legajo = (MaskedTextBox)f.Controls.Find("mtxtLegajoSocio", true)[0];
            Label ayuda = (Label)f.Controls.Find("lblAyuda", true)[0];

            Point origen = legajo.Location;

            legajo.Text = "A-00";

            bool valido = f.ValidacionFormulario(out _);

            Verificar("Un legajo incompleto se rechaza", !valido);
            Verificar("La animación dice CAMPOS INCORRECTOS",
                f.MensajeErrorAnimado == "CAMPOS INCORRECTOS");
            Verificar("La explosión de letras arranca",
                f.AnimacionErrorEnCurso);
            Verificar("La ayuda se esconde mientras dura el error",
                !ayuda.Visible);

            Stopwatch reloj = Stopwatch.StartNew();

            DateTime limite = DateTime.UtcNow.AddSeconds(10);

            while (f.AnimacionErrorEnCurso && DateTime.UtcNow < limite)
            {
                Application.DoEvents();
                Thread.Sleep(5);
            }

            reloj.Stop();

            Console.WriteLine($"        (duración medida: {reloj.ElapsedMilliseconds}ms, {f.PasosExplosion} pasos)");

            Verificar("La explosión de letras se detiene sola",
                !f.AnimacionErrorEnCurso,
                $"siguió corriendo {reloj.ElapsedMilliseconds}ms");

            Verificar("La explosión dura lo suficiente para verse",
                reloj.ElapsedMilliseconds >= 700,
                $"duró {reloj.ElapsedMilliseconds}ms");

            Verificar("La explosión dura menos de 2 segundos",
                reloj.ElapsedMilliseconds < 2000,
                $"tardó {reloj.ElapsedMilliseconds}ms");

            Verificar("La explosión simula muchos pasos, no uno solo",
                f.PasosExplosion > 20,
                $"solo {f.PasosExplosion} pasos");

            Verificar("El campo sacudido vuelve a su posición original",
                legajo.Location == origen);
        }

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

            Verificar("Database rechaza una cadena de conexión vacía",
                LanzaArgumento(() => new Database(string.Empty)));

            Verificar("Database acepta una cadena de conexión válida",
                new Database(conexion) is not null);

            Verificar("Database rechaza una consulta vacía",
                LanzaArgumento(async () => await new Database(conexion).ConsultarAsync(" ")));

            Verificar("Database rechaza una consulta nula",
                LanzaArgumento(async () => await new Database(conexion).ConsultarAsync(null!)));

            Verificar("El repositorio SQL se construye con el ayudante de datos",
                new SocioRepositorySQL(new Database(conexion)) is SocioRepositorySQL);

            Verificar("El repositorio SQL se construye con la cadena de conexión",
                new SocioRepositorySQL(conexion) is SocioRepositorySQL);
        }

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

        private sealed class RepositorioDePrueba : ISocioRepository
        {
            private readonly List<Socio> _socios;

            internal RepositorioDePrueba(params Socio[] socios)
            {
                this._socios = [.. socios];
            }

            public Task<List<Socio>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
            {
                return Task.FromResult(new List<Socio>(this._socios));
            }

            public Task<int> InsertarAsync(Socio socio, CancellationToken cancellationToken = default)
            {
                throw new NotSupportedException();
            }

            public Task ActualizarAsync(Socio socio, CancellationToken cancellationToken = default)
            {
                throw new NotSupportedException();
            }

            public Task EliminarAsync(int idSocio, CancellationToken cancellationToken = default)
            {
                throw new NotSupportedException();
            }

            public Task<bool> ExisteLegajoAsync(string legajoSocio, int idSocioExcluir = 0,
                                                 CancellationToken cancellationToken = default)
            {
                return Task.FromResult(false);
            }
        }
    }
}
