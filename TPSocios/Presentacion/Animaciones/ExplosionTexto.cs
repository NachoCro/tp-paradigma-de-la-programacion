using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using TPSocios.Presentacion.Temas;

namespace TPSocios.Presentacion.Animaciones
{
    internal sealed class ExplosionTexto : Control
    {
        private const int IntervaloReloj = 16;

        private const float ConstanteResorte = 0.18f;

        private const float ConstanteAmortiguamiento = 0.86f;

        private const float ImpulsoMaximo = 11f;

        private const float EscalaInicial = 1.7f;

        private const float ToleranciaPosicion = 0.4f;

        private const float ToleranciaVelocidad = 0.2f;

        private const int DuracionMinimaMs = 750;

        private const int DuracionMaximaMs = 2000;

        private static readonly StringFormat Formato = StringFormat.GenericTypographic;

        private sealed class Letra
        {
            internal required char Caracter { get; init; }

            internal required PointF Objetivo { get; init; }

            internal PointF Posicion { get; set; }

            internal PointF Velocidad { get; set; }

            internal float Rotacion { get; set; }

            internal float VelocidadRotacion { get; set; }

            internal float Escala { get; set; } = 1f;
        }

        private readonly List<Letra> _letras = new();
        private readonly System.Windows.Forms.Timer _reloj;
        private readonly Random _azar = new();

        private TaskCompletionSource? _finalizada;
        private Font? _fuente;

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

        internal string Mensaje { get; private set; } = string.Empty;

        internal bool AnimacionEnCurso => this._reloj.Enabled;

        internal int PasosSimulados { get; private set; }

        private readonly Stopwatch _duracion = new();

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorTexto { get; set; } = Color.White;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorDestello { get; set; } = Color.Gold;

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

        internal void AplicarTema(Tema tema)
        {
            this.BackColor = tema.Fondo;
            this.ColorTexto = tema.Error;
            this.ColorDestello = tema.Acento;
            this.Fuente = tema.FuenteTitulo;
        }

        internal Task ExplotarAsync(string mensaje)
        {
            this.FijarEnSuLugar();

            this.Mensaje = mensaje;

            this.CreateControl();

            this.CalcularObjetivos();

            foreach (Letra letra in this._letras)
            {
                letra.Posicion = letra.Objetivo;
                letra.Escala = EscalaInicial;
                letra.Rotacion = 0f;

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

        internal void Ocultar()
        {
            this.FijarEnSuLugar();
            this.Mensaje = string.Empty;
            this._letras.Clear();
            this.Visible = false;
            this.Invalidate();
        }

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

            long transcurrido = this._duracion.ElapsedMilliseconds;

            bool tocaTerminar = (todasEnSuLugar && transcurrido >= DuracionMinimaMs)
                             || transcurrido >= DuracionMaximaMs;

            if (!tocaTerminar)
            {
                return;
            }

            this._reloj.Stop();
            this._duracion.Stop();

            this.FijarEnSuLugar();
        }

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

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.CalcularObjetivos();
        }

        private float Azar(float minimo, float maximo)
        {
            return minimo + ((float)this._azar.NextDouble() * (maximo - minimo));
        }

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
