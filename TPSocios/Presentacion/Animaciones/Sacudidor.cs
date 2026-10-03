namespace TPSocios.Presentacion.Animaciones
{
    internal static class Sacudidor
    {
        private const int AmplitudMaxima = 9;

        private sealed class Sacudida
        {
            internal required Control Control { get; init; }

            internal required Point Origen { get; init; }

            internal required System.Windows.Forms.Timer Reloj { get; init; }
        }

        private static readonly Dictionary<Control, Sacudida> Sacudidas = new();

        internal static void Sacudir(Control control, int duracionMilisegundos = 420)
        {
            Detener(control);

            if (!control.IsHandleCreated || duracionMilisegundos <= 0)
            {
                return;
            }

            Point origen = control.Location;
            System.Windows.Forms.Timer reloj = new() { Interval = 16 };
            int transcurrido = 0;

            reloj.Tick += (sender, e) =>
            {
                transcurrido += reloj.Interval;

                double progreso = (double)transcurrido / duracionMilisegundos;

                if (progreso >= 1d)
                {
                    Detener(control);
                    return;
                }

                double amortiguacion = 1d - progreso;
                double desplazamiento = Math.Sin(transcurrido * 0.35d) * AmplitudMaxima * amortiguacion;

                control.Location = new Point(
                    origen.X + (int)Math.Round(desplazamiento),
                    origen.Y);
            };

            Sacudidas[control] = new Sacudida { Control = control, Origen = origen, Reloj = reloj };
            reloj.Start();
        }

        private static void Detener(Control control)
        {
            if (!Sacudidas.ContainsKey(control))
            {
                return;
            }

            Sacudida sacudida = Sacudidas[control];
            Sacudidas.Remove(control);

            sacudida.Reloj.Stop();
            sacudida.Reloj.Dispose();
            sacudida.Control.Location = sacudida.Origen;
        }

        internal static bool EnCurso(Control control)
        {
            return Sacudidas.ContainsKey(control);
        }
    }
}
