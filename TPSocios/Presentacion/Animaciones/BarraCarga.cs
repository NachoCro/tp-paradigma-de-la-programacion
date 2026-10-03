using System.ComponentModel;
using TPSocios.Presentacion.Temas;

namespace TPSocios.Presentacion.Animaciones
{
    [DesignerCategory("Code")]
    internal sealed class BarraCarga : Control
    {
        private const int Segmentos = 26;

        private const int Separacion = 2;

        private const int Relieve = 1;

        private int _valor;

        internal BarraCarga()
        {
            this.SetStyle(
                ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw,
                true);
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal int Valor
        {
            get => this._valor;
            set
            {
                int acotado = Math.Clamp(value, 0, 100);

                if (acotado == this._valor)
                {
                    return;
                }

                this._valor = acotado;
                this.Invalidate();
            }
        }

        internal int SegmentosPintados => this._valor * Segmentos / 100;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorPista { get; set; } = Color.FromArgb(30, 30, 30);

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorSegmento { get; set; } = Color.Lime;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        internal Color ColorSegmentoLleno { get; set; } = Color.White;

        internal void AplicarTema(Tema tema)
        {
            this.BackColor = tema.Fondo;
            this.ColorPista = tema.Superficie;
            this.ColorSegmento = tema.Acento;
            this.ColorSegmentoLleno = tema.Texto;
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.Clear(this.BackColor);

            int pintados = this.SegmentosPintados;

            float anchoTotal = this.Width - (Separacion * (Segmentos - 1));
            float anchoSegmento = anchoTotal / Segmentos;

            for (int i = 0; i < Segmentos; i++)
            {
                float x = i * (anchoSegmento + Separacion);

                Rectangle segmento = new(
                    (int)Math.Round(x),
                    Relieve,
                    (int)Math.Round(anchoSegmento),
                    this.Height - (Relieve * 2));

                if (segmento.Width <= 0)
                {
                    continue;
                }

                using SolidBrush fondo = new(i < pintados ? this.ColorSegmento : this.ColorPista);

                e.Graphics.FillRectangle(fondo, segmento);

                if (i < pintados)
                {
                    using Pen borde = new(this.ColorSegmentoLleno, 1f);

                    e.Graphics.DrawRectangle(borde, segmento);
                }
            }

            using Pen marco = new(this.ColorSegmento, 1f);

            e.Graphics.DrawRectangle(marco, 0, 0, this.Width - 1, this.Height - 1);
        }
    }
}