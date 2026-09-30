namespace TPSocios.Presentacion.Temas
{
    /// <summary>
    /// Catálogo de los temas retro disponibles.
    /// Las fuentes se crean una sola vez y se reutilizan para todos los
    /// cambios de tema: viven toda la duración del proceso.
    /// </summary>
    internal static class Temas
    {
        /// <summary>Nombre de la fuente monoespaciada elegida para el estilo retro.</summary>
        private const string FamiliaRetro = "Consolas";

        /// <summary>Esquinas redondeadas de la paleta clara.</summary>
        private static readonly Color Claro_Papel = Color.FromArgb(239, 224, 189);

        /// <summary>Contenedores de la paleta clara.</summary>
        private static readonly Color Claro_Panel = Color.FromArgb(227, 207, 160);

        /// <summary>Campos de captura de la paleta clara.</summary>
        private static readonly Color Claro_Superficie = Color.FromArgb(248, 240, 219);

        /// <summary>Texto principal de la paleta clara.</summary>
        private static readonly Color Claro_Texto = Color.FromArgb(63, 45, 12);

        /// <summary>Texto secundario de la paleta clara.</summary>
        private static readonly Color Claro_TextoTenue = Color.FromArgb(122, 98, 52);

        /// <summary>Acento de la paleta clara.</summary>
        private static readonly Color Claro_Acento = Color.FromArgb(168, 88, 12);

        /// <summary>Error de la paleta clara.</summary>
        private static readonly Color Claro_Error = Color.FromArgb(158, 27, 27);

        /// <summary>Bordes de la paleta clara.</summary>
        private static readonly Color Claro_Borde = Color.FromArgb(185, 154, 92);

        /// <summary>Fila seleccionada de la paleta clara.</summary>
        private static readonly Color Claro_Seleccion = Color.FromArgb(192, 138, 62);

        /// <summary>Cabecera de la grilla en la paleta clara.</summary>
        private static readonly Color Claro_Cabecera = Color.FromArgb(205, 178, 122);

        /// <summary>Líneas de la grilla en la paleta clara.</summary>
        private static readonly Color Claro_Linea = Color.FromArgb(192, 162, 101);

        /// <summary>Fila alterna de la grilla en la paleta clara.</summary>
        private static readonly Color Claro_FilaAlterna = Color.FromArgb(232, 215, 174);

        /// <summary>Fondo del formulario en la paleta oscura.</summary>
        private static readonly Color Oscuro_Fondo = Color.FromArgb(5, 10, 5);

        /// <summary>Contenedores de la paleta oscura.</summary>
        private static readonly Color Oscuro_Panel = Color.FromArgb(11, 26, 13);

        /// <summary>Campos de captura de la paleta oscura.</summary>
        private static readonly Color Oscuro_Superficie = Color.FromArgb(14, 36, 18);

        /// <summary>Texto principal de la paleta oscura: verde fósforo.</summary>
        private static readonly Color Oscuro_Texto = Color.FromArgb(59, 227, 110);

        /// <summary>Texto secundario de la paleta oscura.</summary>
        private static readonly Color Oscuro_TextoTenue = Color.FromArgb(31, 122, 61);

        /// <summary>Acento de la paleta oscura: ámbar de terminal.</summary>
        private static readonly Color Oscuro_Acento = Color.FromArgb(255, 176, 0);

        /// <summary>Error de la paleta oscura.</summary>
        private static readonly Color Oscuro_Error = Color.FromArgb(255, 59, 78);

        /// <summary>Bordes de la paleta oscura.</summary>
        private static readonly Color Oscuro_Borde = Color.FromArgb(31, 92, 51);

        /// <summary>Fila seleccionada de la paleta oscura.</summary>
        private static readonly Color Oscuro_Seleccion = Color.FromArgb(42, 122, 63);

        /// <summary>Cabecera de la grilla en la paleta oscura.</summary>
        private static readonly Color Oscuro_Cabecera = Color.FromArgb(20, 66, 31);

        /// <summary>Líneas de la grilla en la paleta oscura.</summary>
        private static readonly Color Oscuro_Linea = Color.FromArgb(18, 61, 28);

        /// <summary>Fila alterna de la grilla en la paleta oscura.</summary>
        private static readonly Color Oscuro_FilaAlterna = Color.FromArgb(8, 21, 10);

        /// <summary>
        /// Tema retro claro: papel de terminal antiguo, con texto marrón
        /// sobre crema y acentos en tono terracota.
        /// </summary>
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

        /// <summary>
        /// Tema retro oscuro: verde fósforo sobre fondo casi negro, como
        /// un monitor CRT de los ochenta.
        /// </summary>
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

        /// <summary>
        /// Devuelve el tema pedido. Ante cualquier valor no previsto devuelve
        /// el tema oscuro, que es el que se aplica al iniciar la aplicación.
        /// </summary>
        internal static Tema Obtener(TipoTema tipo)
        {
            return tipo switch
            {
                TipoTema.Claro => Claro,
                TipoTema.Oscuro => Oscuro,
                _ => Oscuro
            };
        }

        /// <summary>
        /// Devuelve el tema contrario al indicado, para el botón de alternancia.
        /// </summary>
        internal static TipoTema Alternar(TipoTema tipo)
        {
            return tipo == TipoTema.Oscuro ? TipoTema.Claro : TipoTema.Oscuro;
        }
    }
}
