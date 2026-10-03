namespace TPSocios.Presentacion.Temas
{
    internal static class Temas
    {
        private const string FamiliaRetro = "Consolas";

        private static readonly Color Claro_Papel = Color.FromArgb(239, 224, 189);

        private static readonly Color Claro_Panel = Color.FromArgb(227, 207, 160);

        private static readonly Color Claro_Superficie = Color.FromArgb(248, 240, 219);

        private static readonly Color Claro_Texto = Color.FromArgb(63, 45, 12);

        private static readonly Color Claro_TextoTenue = Color.FromArgb(122, 98, 52);

        private static readonly Color Claro_Acento = Color.FromArgb(168, 88, 12);

        private static readonly Color Claro_Error = Color.FromArgb(158, 27, 27);

        private static readonly Color Claro_Borde = Color.FromArgb(185, 154, 92);

        private static readonly Color Claro_Seleccion = Color.FromArgb(192, 138, 62);

        private static readonly Color Claro_Cabecera = Color.FromArgb(205, 178, 122);

        private static readonly Color Claro_Linea = Color.FromArgb(192, 162, 101);

        private static readonly Color Claro_FilaAlterna = Color.FromArgb(232, 215, 174);

        private static readonly Color Oscuro_Fondo = Color.FromArgb(5, 10, 5);

        private static readonly Color Oscuro_Panel = Color.FromArgb(11, 26, 13);

        private static readonly Color Oscuro_Superficie = Color.FromArgb(14, 36, 18);

        private static readonly Color Oscuro_Texto = Color.FromArgb(59, 227, 110);

        private static readonly Color Oscuro_TextoTenue = Color.FromArgb(31, 122, 61);

        private static readonly Color Oscuro_Acento = Color.FromArgb(255, 176, 0);

        private static readonly Color Oscuro_Error = Color.FromArgb(255, 59, 78);

        private static readonly Color Oscuro_Borde = Color.FromArgb(31, 92, 51);

        private static readonly Color Oscuro_Seleccion = Color.FromArgb(42, 122, 63);

        private static readonly Color Oscuro_Cabecera = Color.FromArgb(20, 66, 31);

        private static readonly Color Oscuro_Linea = Color.FromArgb(18, 61, 28);

        private static readonly Color Oscuro_FilaAlterna = Color.FromArgb(8, 21, 10);

        internal static readonly Tema Claro = new()
        {
            Nombre = "Retro Claro",
            EsOscuro = false,
            Fondo = Claro_Papel,
            Panel = Claro_Panel,
            Superficie = Claro_Superficie,
            Texto = Claro_Texto,
            TextoTenue = Claro_TextoTenue,
            Acento = Claro_Acento,
            Error = Claro_Error,
            Borde = Claro_Borde,
            SeleccionFondo = Claro_Seleccion,
            SeleccionTexto = Claro_Papel,
            CabeceraFondo = Claro_Cabecera,
            GrillaLinea = Claro_Linea,
            GrillaFilaAlterna = Claro_FilaAlterna,
            FuenteDatos = new Font(FamiliaRetro, 9F),
            FuenteNegrita = new Font(FamiliaRetro, 8F, FontStyle.Bold),
            FuenteTitulo = new Font(FamiliaRetro, 15F, FontStyle.Bold)
        };

        internal static readonly Tema Oscuro = new()
        {
            Nombre = "Retro Oscuro",
            EsOscuro = true,
            Fondo = Oscuro_Fondo,
            Panel = Oscuro_Panel,
            Superficie = Oscuro_Superficie,
            Texto = Oscuro_Texto,
            TextoTenue = Oscuro_TextoTenue,
            Acento = Oscuro_Acento,
            Error = Oscuro_Error,
            Borde = Oscuro_Borde,
            SeleccionFondo = Oscuro_Seleccion,
            SeleccionTexto = Oscuro_Fondo,
            CabeceraFondo = Oscuro_Cabecera,
            GrillaLinea = Oscuro_Linea,
            GrillaFilaAlterna = Oscuro_FilaAlterna,
            FuenteDatos = new Font(FamiliaRetro, 9F),
            FuenteNegrita = new Font(FamiliaRetro, 8F, FontStyle.Bold),
            FuenteTitulo = new Font(FamiliaRetro, 15F, FontStyle.Bold)
        };

        internal static Tema Obtener(TipoTema tipo)
        {
            return tipo switch
            {
                TipoTema.Claro => Claro,
                TipoTema.Oscuro => Oscuro,
                _ => Oscuro
            };
        }

        internal static TipoTema Alternar(TipoTema tipo)
        {
            return tipo == TipoTema.Oscuro ? TipoTema.Claro : TipoTema.Oscuro;
        }
    }
}
