using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using TPSocios.Presentacion.Temas;

namespace TPSocios.Presentacion.Animaciones
{
    [DesignerCategory("Code")]
    internal sealed class TextoEscribiendo : Control
    {
        private const int IntervaloReloj = 15;

        private const int IntervaloBorde = 40;

        private const int GradosPorPulso = 4;

        private const int GrosorBorde = 3;

        private const int DuracionMaximaMs = 4000;

        private readonly System.Windows.Forms.Timer _reloj;

        private readonly System.Windows.Forms.Timer _relojBorde;

        private readonly Stopwatch _relojDeEscritura = new();

        private Font? _fuente;

        private string _mensaje = string.Empty;

        private int _caracteresVisibles;

        private int _tono;

        private Color _colorTextoMensaje = Color.White;

        internal TextoEscribiendo()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);

            this._reloj = new System.Windows.Forms.Timer { Interval = IntervaloReloj };
            this._reloj.Tick += this.AlEscribirCaracter;

            this._relojBorde = new System.Windows.Forms.Timer { Interval = IntervaloBorde };
            this._relojBorde.Tick += this.AlGirarElBorde;
        }

        internal bool BordeActivo => this._relojBorde.Enabled;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal int TonoBorde
        {
            get => this._tono;
            set => this._tono = ((value % 360) + 360) % 360;
        }

        internal string Mensaje => this._mensaje;

        internal bool EscrituraEnCurso => this._reloj.Enabled;

        internal int CaracteresMostrados => this._caracteresVisibles;

        internal int PasosSimulados { get; private set; }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorTexto { get; set; } = Color.White;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorTextoEnPantalla => this._colorTextoMensaje;

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

                this.Invalidate();
            }
        }

        internal void AplicarTema(Tema tema)
        {
            this.BackColor = tema.Fondo;
            this.ColorTexto = tema.Error;
            this.Fuente = tema.FuenteDatos;
        }

        internal void Escribir(string mensaje, Color? color = null)
        {
            this._reloj.Stop();
            this._relojDeEscritura.Stop();

            this._mensaje = mensaje ?? string.Empty;
            this._colorTextoMensaje = color ?? this.ColorTexto;
            this._caracteresVisibles = 0;
            this.PasosSimulados = 0;

            if (this._mensaje.Length == 0)
            {
                this._relojBorde.Stop();
                this.Visible = false;
                this.Invalidate();
                return;
            }

            this.CreateControl();
            this.Visible = true;

            this._relojBorde.Start();
            this._relojDeEscritura.Restart();
            this._reloj.Start();
        }

        internal void Ocultar()
        {
            this._reloj.Stop();
            this._relojBorde.Stop();
            this._relojDeEscritura.Stop();

            this._mensaje = string.Empty;
            this._caracteresVisibles = 0;
            this.Visible = false;
            this.Invalidate();
        }

        private void AlGirarElBorde(object? sender, EventArgs e)
        {
            this.AvanzarBorde();
        }

        internal void AvanzarBorde()
        {
            this._tono = (this._tono + GradosPorPulso) % 360;

            this.Invalidate();
        }

        private void AlEscribirCaracter(object? sender, EventArgs e)
        {
            this.PasosSimulados++;
            this._caracteresVisibles++;

            this.Invalidate();

            bool terminado = this._caracteresVisibles >= this._mensaje.Length
                           || this._relojDeEscritura.ElapsedMilliseconds >= DuracionMaximaMs;

            if (!terminado)
            {
                return;
            }

            this._caracteresVisibles = this._mensaje.Length;
            this.Invalidate();

            this._reloj.Stop();
            this._relojDeEscritura.Stop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            int visibles = Math.Min(this._caracteresVisibles, this._mensaje.Length);

            this.PintarBorde(e);

            if (visibles <= 0)
            {
                return;
            }

            Font fuente = this._fuente ?? this.Font;
            string parcial = this._mensaje[..visibles];

            e.Graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;

            float anchoTexto = e.Graphics.MeasureString(parcial, fuente).Width;

            float escala = anchoTexto > this.AreaUtilAncho ? this.AreaUtilAncho / anchoTexto : 1f;

            if (escala < 1f)
            {
                e.Graphics.ScaleTransform(escala, 1f);
            }

            float alto = fuente.GetHeight(e.Graphics);
            float x = Math.Max(0f, ((this.AreaUtilAncho / escala) - anchoTexto) / 2f);
            float y = (this.Height - alto) / 2f;

            using SolidBrush pincel = new(this._colorTextoMensaje);

            e.Graphics.DrawString(parcial, fuente, pincel, x, y);
        }

        private int AreaUtilAncho => this.Width - (GrosorBorde * 2);

        internal static Color TonoAColor(int tono, int saturacion, int valor)
        {
            float h = ((tono % 360) + 360) % 360 / 60f;
            float s = Math.Clamp(saturacion, 0, 255) / 255f;
            float v = Math.Clamp(valor, 0, 255) / 255f;

            float c = v * s;
            float x = c * (1f - Math.Abs((h % 2f) - 1f));
            float m = v - c;

            float r;
            float g;
            float b;

            if (h < 1f)
            {
                (r, g, b) = (c, x, 0f);
            }
            else if (h < 2f)
            {
                (r, g, b) = (x, c, 0f);
            }
            else if (h < 3f)
            {
                (r, g, b) = (0f, c, x);
            }
            else if (h < 4f)
            {
                (r, g, b) = (0f, x, c);
            }
            else if (h < 5f)
            {
                (r, g, b) = (x, 0f, c);
            }
            else
            {
                (r, g, b) = (c, 0f, x);
            }

            return Color.FromArgb(
                (int)Math.Clamp(Math.Round((r + m) * 255f), 0f, 255f),
                (int)Math.Clamp(Math.Round((g + m) * 255f), 0f, 255f),
                (int)Math.Clamp(Math.Round((b + m) * 255f), 0f, 255f));
        }

        private void PintarBorde(PaintEventArgs e)
        {
            if (this._mensaje.Length == 0)
            {
                return;
            }

            Color tonalidad = TonoAColor(this._tono, 220, 255);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using Pen halo = new(Color.FromArgb(90, tonalidad), GrosorBorde + 4f);
            using Pen nucleo = new(tonalidad, GrosorBorde);

            Rectangle marco = new(
                (GrosorBorde * 2) - 2,
                (GrosorBorde * 2) - 2,
                this.Width - (GrosorBorde * 4) + 2,
                this.Height - (GrosorBorde * 4) + 2);

            e.Graphics.DrawRectangle(halo, marco);
            e.Graphics.DrawRectangle(nucleo, marco);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this._reloj.Tick -= this.AlEscribirCaracter;
                this._reloj.Dispose();
                this._relojBorde.Tick -= this.AlGirarElBorde;
                this._relojBorde.Dispose();
                this._fuente?.Dispose();
                this._fuente = null;
            }

            base.Dispose(disposing);
        }
    }
}