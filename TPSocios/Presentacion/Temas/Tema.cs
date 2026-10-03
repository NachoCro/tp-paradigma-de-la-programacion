namespace TPSocios.Presentacion.Temas
{
    internal enum TipoTema
    {
        Claro,
        Oscuro
    }

    internal sealed class Tema
    {
        internal string Nombre { get; init; } = string.Empty;

        internal bool EsOscuro { get; init; }

        internal Color Fondo { get; init; }

        internal Color Panel { get; init; }

        internal Color Superficie { get; init; }

        internal Color Texto { get; init; }

        internal Color TextoTenue { get; init; }

        internal Color Acento { get; init; }

        internal Color Error { get; init; }

        internal Color Borde { get; init; }

        internal Color SeleccionFondo { get; init; }

        internal Color SeleccionTexto { get; init; }

        internal Color CabeceraFondo { get; init; }

        internal Color GrillaLinea { get; init; }

        internal Color GrillaFilaAlterna { get; init; }

        internal Font FuenteDatos { get; init; } = null!;

        internal Font FuenteNegrita { get; init; } = null!;

        internal Font FuenteTitulo { get; init; } = null!;
    }
}
