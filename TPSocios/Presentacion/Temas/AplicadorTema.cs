using TPSocios.Presentacion.Animaciones;

namespace TPSocios.Presentacion.Temas
{
    /// <summary>
    /// Aplica un <see cref="Tema"/> sobre el árbol de controles de un formulario.
    /// Recorre la jerarquía y va reconociendo cada tipo de control por
    /// polimorfismo, de modo que agregar un control nuevo al Designer no
    /// obliga a tocar este recorrido.
    /// </summary>
    internal static class AplicadorTema
    {
        /// <summary>
        /// Pinta el formulario completo con la paleta indicada y ajusta la
        /// tipografía de los controles de captura al estilo monoespaciado.
        /// </summary>
        internal static void Aplicar(Form formulario, Tema tema)
        {
            formulario.BackColor = tema.Fondo;
            formulario.ForeColor = tema.Texto;

            Recorrer(formulario, tema);
        }

        /// <summary>
        /// Aplica el tema al control recibido y después a todos sus hijos.
        /// </summary>
        private static void Recorrer(Control control, Tema tema)
        {
            switch (control)
            {
                case ExplosionTexto explosion:
                    explosion.AplicarTema(tema);
                    break;

                case DataGridView grilla:
                    AplicarGrilla(grilla, tema);
                    break;

                case GroupBox grupo:
                    grupo.BackColor = tema.Panel;
                    grupo.ForeColor = tema.Texto;
                    break;

                case TextBoxBase captura:
                    captura.BackColor = tema.Superficie;
                    captura.ForeColor = tema.Texto;
                    captura.Font = tema.FuenteDatos;
                    captura.BorderStyle = BorderStyle.FixedSingle;
                    break;

                case ComboBox combo:
                    combo.BackColor = tema.Superficie;
                    combo.ForeColor = tema.Texto;
                    combo.Font = tema.FuenteDatos;
                    combo.FlatStyle = FlatStyle.Flat;
                    break;

                case DateTimePicker fecha:
                    fecha.BackColor = tema.Superficie;
                    fecha.ForeColor = tema.Texto;
                    fecha.Font = tema.FuenteDatos;
                    fecha.CalendarForeColor = tema.Texto;
                    fecha.CalendarTitleBackColor = tema.CabeceraFondo;
                    fecha.CalendarTitleForeColor = tema.Texto;
                    fecha.CalendarTrailingForeColor = tema.TextoTenue;
                    break;

                case CheckBox check:
                    // No se le cambia la tipografía: el CheckBox ajusta su
                    // ancho al texto, y una fuente más ancha lo corría contra
                    // el botón de cambio de tema.
                    check.ForeColor = tema.Texto;
                    check.BackColor = Color.Transparent;
                    break;

                case Button boton:
                    AplicarBoton(boton, tema);
                    break;

                case Label etiqueta:
                    etiqueta.ForeColor = tema.Texto;
                    break;
            }

            foreach (Control hijo in control.Controls)
            {
                Recorrer(hijo, tema);
            }
        }

        /// <summary>
        /// Los botones quedan planos, con un borde marcado del color de acento,
        /// que es lo que da el aspecto de teclas de una terminal antigua.
        /// </summary>
        private static void AplicarBoton(Button boton, Tema tema)
        {
            boton.BackColor = tema.Panel;
            boton.ForeColor = tema.Texto;
            boton.Font = tema.FuenteNegrita;
            boton.FlatStyle = FlatStyle.Flat;
            boton.UseVisualStyleBackColor = false;
            boton.FlatAppearance.BorderColor = tema.Acento;
            boton.FlatAppearance.BorderSize = 2;
            boton.FlatAppearance.MouseOverBackColor = tema.Acento;
        }

        /// <summary>
        /// La grilla necesita su propio juego de estilos de celda. Se desactivan
        /// los estilos visuales del sistema porque, de lo contrario, el Windows
        /// ignora los colores de la cabecera que acabamos de asignar.
        /// </summary>
        private static void AplicarGrilla(DataGridView grilla, Tema tema)
        {
            grilla.EnableHeadersVisualStyles = false;

            grilla.BackgroundColor = tema.Fondo;
            grilla.BorderStyle = BorderStyle.FixedSingle;
            grilla.GridColor = tema.GrillaLinea;
            grilla.ForeColor = tema.Texto;

            grilla.DefaultCellStyle.BackColor = tema.Fondo;
            grilla.DefaultCellStyle.ForeColor = tema.Texto;
            grilla.DefaultCellStyle.Font = tema.FuenteDatos;
            grilla.DefaultCellStyle.SelectionBackColor = tema.SeleccionFondo;
            grilla.DefaultCellStyle.SelectionForeColor = tema.SeleccionTexto;
            grilla.DefaultCellStyle.Padding = new Padding(2);

            grilla.AlternatingRowsDefaultCellStyle.BackColor = tema.GrillaFilaAlterna;

            grilla.ColumnHeadersDefaultCellStyle.BackColor = tema.CabeceraFondo;
            grilla.ColumnHeadersDefaultCellStyle.ForeColor = tema.Texto;
            grilla.ColumnHeadersDefaultCellStyle.Font = tema.FuenteNegrita;
            grilla.ColumnHeadersDefaultCellStyle.SelectionBackColor = tema.CabeceraFondo;
            grilla.ColumnHeadersDefaultCellStyle.SelectionForeColor = tema.Texto;

            grilla.RowHeadersDefaultCellStyle.BackColor = tema.CabeceraFondo;
            grilla.RowHeadersDefaultCellStyle.ForeColor = tema.Texto;
        }
    }
}
