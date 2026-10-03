using TPSocios.Presentacion.Animaciones;

namespace TPSocios.Presentacion.Temas
{
    internal static class AplicadorTema
    {
        internal static void Aplicar(Form formulario, Tema tema)
        {
            formulario.BackColor = tema.Fondo;
            formulario.ForeColor = tema.Texto;

            Recorrer(formulario, tema);
        }

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
