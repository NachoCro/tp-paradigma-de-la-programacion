using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using TPSocios.Presentacion.Temas;

namespace TPSocios.Presentacion.Animaciones
{
    /// <summary>
    /// Control que dibuja un texto lettre por lettre y lo hace "explotar":
    /// al iniciarse, cada carácter recibe un impulso que lo lanza en una
    /// dirección aleatoria y luego un resorte con amortiguación lo devuelve
    /// a su lugar. El resultado es un estallido de letras que rebotan hasta
    /// formar de nuevo la palabra.
    /// <para>
    /// El dibujo es propio porque una etiqueta normal no permite ubicar cada
    /// carácter por separado, que es justo lo que necesita la animación.
    /// </para>
    /// </summary>
    internal sealed class ExplosionTexto : Control
    {
        /// <summary>Frecuencia con la que se recalcula la física de las letras.</summary>
        private const int IntervaloReloj = 16;

        /// <summary>Rigidez del resorte que devuelve cada letra a su posición.</summary>
        private const float ConstanteResorte = 0.18f;

        /// <summary>Frenado aplicado en cada paso: frena sin detener de golpe.</summary>
        private const float ConstanteAmortiguamiento = 0.86f;

        /// <summary>Velocidad inicial máxima del estallido, en píxeles por paso.</summary>
        private const float ImpulsoMaximo = 11f;

        /// <summary>Escala con la que arranca cada letra y a la que vuelve.</summary>
        private const float EscalaInicial = 1.7f;

        /// <summary>Desplazamiento por debajo del cual una letra ya está en su sitio.</summary>
        private const float ToleranciaPosicion = 0.4f;

        /// <summary>Velocidad por debajo de la cual una letra ya está quieta.</summary>
        private const float ToleranciaVelocidad = 0.2f;

        /// <summary>
        /// Tiempo mínimo de la explosión. Sin este piso, unas letras que ya
        /// entran cerca de su lugar se ordenarían en un solo paso y el efecto
        /// pasaría inadvertido, y el MessageBox del error lo taparía de
        /// inmediato.
        /// </summary>
        private const int DuracionMinimaMs = 750;

        /// <summary>
        /// Tiempo máximo. Es una red de contención: si por lo que sea las
        /// letras no llegan a acomodarse, la animación termina igual y el
        /// detalle del error no queda esperando para siempre.
        /// </summary>
        private const int DuracionMaximaMs = 2000;

        /// <summary>
        /// Formato de medición y dibujo compartido, para que el ancho medido
        /// coincida exactamente con el ancho pintado.
        /// </summary>
        private static readonly StringFormat Formato = StringFormat.GenericTypographic;

        /// <summary>Cada letra de la palabra, con su posición y su velocidad.</summary>
        private sealed class Letra
        {
            internal required char Caracter { get; init; }

            /// <summary>Lugar que la letra debe ocupar cuando todo se acomoda.</summary>
            internal required PointF Objetivo { get; init; }

            /// <summary>Lugar en el que la letra está dibujada este instante.</summary>
            internal PointF Posicion { get; set; }

            /// <summary>Desplazamiento por paso.</summary>
            internal PointF Velocidad { get; set; }

            /// <summary>Giro actual, en grados.</summary>
            internal float Rotacion { get; set; }

            /// <summary>Giro por paso.</summary>
            internal float VelocidadRotacion { get; set; }

            /// <summary>Factor de tamaño: arranca mayor y se normaliza.</summary>
            internal float Escala { get; set; } = 1f;
        }

        private readonly List<Letra> _letras = new();
        private readonly System.Windows.Forms.Timer _reloj;
        private readonly Random _azar = new();

        private TaskCompletionSource? _finalizada;
        private Font? _fuente;

        /// <summary>
        /// Crea el control con doble búfer, para que las letras se muevan
        /// sin parpadeos. Arranca oculto hasta la primera explosión.
        /// </summary>
        internal ExplosionTexto()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);

            this._reloj = new System.Windows.Forms.Timer { Interval = IntervaloReloj };
            this._reloj.Tick += this.AlActualizarReloj;

            this.TabStop = false;
            this.Visible = false;
        }

        /// <summary>Texto que se dibuja letra por letra.</summary>
        internal string Mensaje { get; private set; } = string.Empty;

        /// <summary>
        /// Indica si la explosión se está animando. Si quedara encendida para
        /// siempre, la espera del formulario nunca terminaría y el detalle del
        /// error no llegaría a mostrarse nunca.
        /// </summary>
        internal bool AnimacionEnCurso => this._reloj.Enabled;

        /// <summary>Cantidad de pasos simulados en la última explosión.</summary>
        internal int PasosSimulados { get; private set; }

        /// <summary>Mide cuánto lleva corriendo la explosión actual.</summary>
        private readonly Stopwatch _duracion = new();

        /// <summary>Color con el que se pinta el texto una vez ordenado.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorTexto { get; set; } = Color.White;

        /// <summary>Color del fogonazo inicial de cada letra.</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorDestello { get; set; } = Color.Gold;

        /// <summary>
        /// Acepta null para usar la fuente del propio control.
        /// El control se queda con una copia propia: los temas comparten una
        /// sola instancia de cada fuente durante toda la aplicación, así que
        /// liberarla al cambiar de tema dejaría inservible la fuente del tema
        /// anterior para el resto de la ventana.
        /// </summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Font? Fuente
        {
            get => this._fuente;
            set
            {
                if (ReferenceEquals(this._fuente, value))
                {
                    return;
                }

                this._fuente?.Dispose();
                this._fuente = value is null
                    ? null
                    : new Font(value.FontFamily, value.SizeInPoints, value.Style, value.Unit);

                this.CalcularObjetivos();
            }
        }

        /// <summary>
        /// Adopta los colores y la fuente del tema. Como el ancho del texto
        /// depende de la tipografía, se recalculan las posiciones objetivo.
        /// </summary>
        internal void AplicarTema(Tema tema)
        {
            this.BackColor = tema.Fondo;
            this.ColorTexto = tema.Error;
            this.ColorDestello = tema.Acento;
            this.Fuente = tema.FuenteTitulo;
        }

        /// <summary>
        /// Arranca la explosión y devuelve una tarea que termina cuando las
        /// letras ya volvieron a su lugar. Si había una explosión en curso se la
        /// da por terminada antes de empezar la nueva, para que ninguna espera
        /// quede colgada.
        /// </summary>
        internal Task ExplotarAsync(string mensaje)
        {
            this.FijarEnSuLugar();

            this.Mensaje = mensaje;

            // Sin ventana no hay forma de medir el texto, y la animación se
            // quedaría sin letras que mover. Se fuerza la creación del handle
            // antes de calcular los destinos.
            this.CreateControl();

            this.CalcularObjetivos();

            foreach (Letra letra in this._letras)
            {
                letra.Posicion = letra.Objetivo;
                letra.Escala = EscalaInicial;
                letra.Rotacion = 0f;

                // El impulso tiene dos componentes: una radial, que empuja la
                // letra hacia afuera desde el centro de la palabra, y otra
                // aleatoria, para que ninguna salga por el mismo lado.
                float desvioX = letra.Objetivo.X - (this.Width / 2f);
                float desvioY = letra.Objetivo.Y - (this.Height / 2f);
                float longitud = MathF.Max(1f, MathF.Sqrt((desvioX * desvioX) + (desvioY * desvioY)));

                letra.Velocidad = new PointF(
                    ((desvioX / longitud) * ImpulsoMaximo) + this.Azar(-ImpulsoMaximo / 2f, ImpulsoMaximo / 2f),
                    ((desvioY / longitud) * ImpulsoMaximo) + this.Azar(-ImpulsoMaximo / 2f, ImpulsoMaximo / 2f));

                letra.VelocidadRotacion = this.Azar(-0.9f, 0.9f);
            }

            this._finalizada = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

            this.PasosSimulados = 0;
            this.Visible = true;
            this._duracion.Restart();
            this._reloj.Start();

            return this._finalizada.Task;
        }

        /// <summary>
        /// Oculta el mensaje. Se usa cuando el formulario vuelve a un estado
        /// sin errores, para que el texto no quede pegado en la pantalla.
        /// </summary>
        internal void Ocultar()
        {
            this.FijarEnSuLugar();
            this.Mensaje = string.Empty;
            this._letras.Clear();
            this.Visible = false;
            this.Invalidate();
        }

        /// <summary>
        /// Frena la animación en curso y deja las letras ya ordenadas,
        /// sin ocultarlas: el texto se queda en pantalla para que se pueda leer.
        /// </summary>
        private void FijarEnSuLugar()
        {
            this._reloj.Stop();

            foreach (Letra letra in this._letras)
            {
                letra.Posicion = letra.Objetivo;
                letra.Velocidad = PointF.Empty;
                letra.Rotacion = 0f;
                letra.VelocidadRotacion = 0f;
                letra.Escala = 1f;
            }

            TaskCompletionSource? anterior = this._finalizada;
            this._finalizada = null;
            anterior?.TrySetResult();

            this.Invalidate();
        }

        /// <summary>
        /// Recalcula el lugar que debe ocupar cada carácter, centrado
        /// horizontal y verticalmente dentro del control.
        /// </summary>
        private void CalcularObjetivos()
        {
            this._letras.Clear();

            if (string.IsNullOrEmpty(this.Mensaje))
            {
                return;
            }

            Font fuente = this._fuente ?? this.Font;

            if (!this.IsHandleCreated)
            {
                // Todavía no hay ventana: la medición se reintenta al pintar.
                return;
            }

            using Graphics graphics = this.CreateGraphics();

            float alto = fuente.GetHeight(graphics);
            float anchoTotal = graphics.MeasureString(this.Mensaje, fuente, int.MaxValue, Formato).Width;

            float inicioX = (this.Width - anchoTotal) / 2f;
            float centroY = (this.Height - alto) / 2f;

            foreach (char caracter in this.Mensaje)
            {
                float ancho = graphics.MeasureString(caracter.ToString(), fuente, int.MaxValue, Formato).Width;

                this._letras.Add(new Letra
                {
                    Caracter = caracter,
                    Objetivo = new PointF(inicioX, centroY)
                });

                inicioX += ancho;
            }
        }

        /// <summary>
        /// Un paso de la animación: aplica el resorte a cada letra y vuelve a
        /// dibujar. Cuando todas llegaron a su lugar, la animación termina.
        /// </summary>
        private void AlActualizarReloj(object? sender, EventArgs e)
        {
            this.PasosSimulados++;

            bool todasEnSuLugar = true;

            foreach (Letra letra in this._letras)
            {
                letra.Velocidad = new PointF(
                    letra.Velocidad.X + ((letra.Objetivo.X - letra.Posicion.X) * ConstanteResorte),
                    letra.Velocidad.Y + ((letra.Objetivo.Y - letra.Posicion.Y) * ConstanteResorte));

                letra.Velocidad = new PointF(
                    letra.Velocidad.X * ConstanteAmortiguamiento,
                    letra.Velocidad.Y * ConstanteAmortiguamiento);

                letra.Posicion = new PointF(
                    letra.Posicion.X + letra.Velocidad.X,
                    letra.Posicion.Y + letra.Velocidad.Y);

                letra.Rotacion += letra.VelocidadRotacion;
                letra.VelocidadRotacion *= ConstanteAmortiguamiento;
                letra.Escala += (1f - letra.Escala) * ConstanteResorte;

                bool quieta = MathF.Abs(letra.Velocidad.X) < ToleranciaVelocidad
                              && MathF.Abs(letra.Velocidad.Y) < ToleranciaVelocidad
                              && MathF.Abs(letra.VelocidadRotacion) < ToleranciaVelocidad;

                bool enPosicion = MathF.Abs(letra.Objetivo.X - letra.Posicion.X) < ToleranciaPosicion
                                  && MathF.Abs(letra.Objetivo.Y - letra.Posicion.Y) < ToleranciaPosicion;

                if (quieta && enPosicion)
                {
                    letra.Posicion = letra.Objetivo;
                    letra.Velocidad = PointF.Empty;
                    letra.Rotacion = 0f;
                    letra.VelocidadRotacion = 0f;
                    letra.Escala = 1f;
                }
                else
                {
                    todasEnSuLugar = false;
                }
            }

            this.Invalidate();

            // Piso de duración: aunque las letras ya estén en su lugar, el
            // efecto tiene que verse. Techo: si algo se traba, termina igual
            // para que el detalle del error no quede esperando.
            long transcurrido = this._duracion.ElapsedMilliseconds;

            bool tocaTerminar = (todasEnSuLugar && transcurrido >= DuracionMinimaMs)
                             || transcurrido >= DuracionMaximaMs;

            if (!tocaTerminar)
            {
                return;
            }

            this._reloj.Stop();
            this._duracion.Stop();

            // En los dos finales las letras quedan ordenadas: si se cortó por
            // el techo, se acomodan de golpe para no dejar el texto a medio
            // camino.
            this.FijarEnSuLugar();
        }

        /// <summary>
        /// Dibuja cada letra en su posición, con su giro y su escala.
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (this._letras.Count == 0)
            {
                return;
            }

            Font fuente = this._fuente ?? this.Font;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;

            float alto = fuente.GetHeight(e.Graphics);

            foreach (Letra letra in this._letras)
            {
                // Cada letra sale con el color de acento, que se va apagando
                // hacia el color de error a medida que se ordena.
                float avance = Math.Clamp(1f - ((letra.Escala - 1f) / (EscalaInicial - 1f)), 0f, 1f);

                using SolidBrush pincel = new(Color.FromArgb(
                    (int)(this.ColorTexto.R + ((this.ColorDestello.R - this.ColorTexto.R) * avance)),
                    (int)(this.ColorTexto.G + ((this.ColorDestello.G - this.ColorTexto.G) * avance)),
                    (int)(this.ColorTexto.B + ((this.ColorDestello.B - this.ColorTexto.B) * avance))));

                GraphicsState estado = e.Graphics.Save();

                e.Graphics.TranslateTransform(letra.Posicion.X, letra.Posicion.Y + (alto / 2f));
                e.Graphics.RotateTransform(letra.Rotacion);
                e.Graphics.ScaleTransform(letra.Escala, letra.Escala);

                e.Graphics.DrawString(letra.Caracter.ToString(), fuente, pincel, 0f, -(alto / 2f), Formato);

                e.Graphics.Restore(estado);
            }
        }

        /// <summary>
        /// Refleja un cambio de tamaño recalculando las posiciones objetivo.
        /// </summary>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.CalcularObjetivos();
        }

        /// <summary>Devuelve un número real dentro del rango indicado.</summary>
        private float Azar(float minimo, float maximo)
        {
            return minimo + ((float)this._azar.NextDouble() * (maximo - minimo));
        }

        /// <summary>Libera el reloj y la fuente propia.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._reloj.Tick -= this.AlActualizarReloj;
                this._reloj.Dispose();
                this._fuente?.Dispose();
                this._fuente = null;
            }

            base.Dispose(disposing);
        }
    }
}
