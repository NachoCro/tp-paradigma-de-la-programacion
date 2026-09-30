namespace TPSocios.Presentacion.Temas
{
    /// <summary>
    /// Identifica el juego de colores activo en el formulario.
    /// </summary>
    internal enum TipoTema
    {
        Claro,
        Oscuro
    }

    /// <summary>
    /// Paleta de colores y tipografías de un tema.
    /// Es una descripción inmutable: el <see cref="AplicadorTema"/> la recorre
    /// y la aplica control por control, sin que ningún control la modifique.
    /// Los dos temas disponibles tienen estética retro: el claro imita el
    /// papel carrotorio de los terminales antiguos y el oscuro el verde
    /// fósforo de un monitor CRT.
    /// </summary>
    internal sealed class Tema
    {
        /// <summary>Nombre legible del tema, para la barra de estado y las pruebas.</summary>
        internal string Nombre { get; init; } = string.Empty;

        /// <summary>Indica si es el tema de fondo oscuro.</summary>
        internal bool EsOscuro { get; init; }

        /// <summary>Color de fondo del formulario.</summary>
        internal Color Fondo { get; init; }

        /// <summary>Color de los contenedores, como el GroupBox de datos.</summary>
        internal Color Panel { get; init; }

        /// <summary>Color de fondo de los controles de captura.</summary>
        internal Color Superficie { get; init; }

        /// <summary>Color de las etiquetas.</summary>
        internal Color Texto { get; init; }

        /// <summary>Color secundario, para textos de apoyo.</summary>
        internal Color TextoTenue { get; init; }

        /// <summary>Color de acento, usado por los botones.</summary>
        internal Color Acento { get; init; }

        /// <summary>Color de los mensajes de error.</summary>
        internal Color Error { get; init; }

        /// <summary>Color de los bordes de los controles.</summary>
        internal Color Borde { get; init; }

        /// <summary>Color de fondo de la fila seleccionada en la grilla.</summary>
        internal Color SeleccionFondo { get; init; }

        /// <summary>Color del texto de la fila seleccionada en la grilla.</summary>
        internal Color SeleccionTexto { get; init; }

        /// <summary>Color de fondo de la cabecera de la grilla.</summary>
        internal Color CabeceraFondo { get; init; }

        /// <summary>Color de las líneas de la grilla.</summary>
        internal Color GrillaLinea { get; init; }

        /// <summary>Color de fondo de las filas impares de la grilla.</summary>
        internal Color GrillaFilaAlterna { get; init; }

        /// <summary>Tipografía monoespaciada para los datos capturados.</summary>
        internal Font FuenteDatos { get; init; } = null!;

        /// <summary>Tipografía monoespaciada en negrita para los botones.</summary>
        internal Font FuenteNegrita { get; init; } = null!;

        /// <summary>Tipografía grande para la animación de error.</summary>
        internal Font FuenteTitulo { get; init; } = null!;
    }
}
