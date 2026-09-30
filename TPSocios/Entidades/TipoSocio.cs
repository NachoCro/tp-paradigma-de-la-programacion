namespace TPSocios.Entidades
{
    /// <summary>
    /// Tipos de socio admitidos por el club.
    /// El nombre de cada constante coincide con el valor almacenado en la
    /// columna TipoSocio de la tabla Socios, de modo que al persistir se
    /// utiliza directamente el nombre del enumerado.
    /// </summary>
    public enum TipoSocio
    {
        Menor = 0,
        Mayor = 1,
        Jubilado = 2,
        Familiar = 3
    }
}
