namespace TPSocios.Presentacion.Animaciones
{
    /// <summary>
    /// Sacude un control durante un instante, como el aviso de un aparato
    /// que rechaza una tecla. La amplitud decrece con el tiempo, de modo que
    /// el movimiento se apaga en lugar de cortarse de golpe.
    /// <para>
    /// No bloquea al llamador: el control vuelve solo a su posición original
    /// cuando termina el tiempo, y el resto del formulario sigue respondiendo.
    /// </para>
    /// </summary>
    internal static class Sacudidor
    {
        /// <summary>Desplazamiento máximo del primer movimiento, en píxeles.</summary>
        private const int AmplitudMaxima = 9;

        /// <summary>Control en movimiento, con su posición original y su reloj.</summary>
        private sealed class Sacudida
        {
            internal required Control Control { get; init; }

            internal required Point Origen { get; init; }

            internal required System.Windows.Forms.Timer Reloj { get; init; }
        }

        private static readonly Dictionary<Control, Sacudida> Sacudidas = new();

        /// <summary>
        /// Comienza a sacudir el control. Si ya estaba sacudiéndose, se
        /// reinicia desde la posición original.
        /// </summary>
        /// <param name="control">Control que se va a mover.</param>
        /// <param name="duracionMilisegundos">Duración total del movimiento.</param>
        internal static void Sacudir(Control control, int duracionMilisegundos = 420)
        {
            Detener(control);

            // Sin ventana no hay dónde dibujar el movimiento.
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

                // La senoide da el vaivén; la amortiguación hace que se apague.
                double amortiguacion = 1d - progreso;
                double desplazamiento = Math.Sin(transcurrido * 0.35d) * AmplitudMaxima * amortiguacion;

                control.Location = new Point(
                    origen.X + (int)Math.Round(desplazamiento),
                    origen.Y);
            };

            Sacudidas[control] = new Sacudida { Control = control, Origen = origen, Reloj = reloj };
            reloj.Start();
        }

        /// <summary>
        /// Frena la sacudida del control, si la hubiera, y lo devuelve a su
        /// posición original.
        /// </summary>
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

        /// <summary>Indica si el control está siendo sacudido.</summary>
        internal static bool EnCurso(Control control)
        {
            return Sacudidas.ContainsKey(control);
        }
    }
}
