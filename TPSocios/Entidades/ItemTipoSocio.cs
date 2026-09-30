namespace TPSocios.Entidades
{
    /// <summary>
    /// Elemento del ComboBox "Tipo de Socio": expone por separado la
    /// descripción que ve el usuario y el valor enumerado que se persiste.
    /// </summary>
    public sealed class ItemTipoSocio
    {
        public TipoSocio Tipo { get; }

        public string Descripcion { get; }

        public ItemTipoSocio(TipoSocio tipo, string descripcion)
        {
            this.Tipo = tipo;
            this.Descripcion = descripcion;
        }

        public override string ToString()
        {
            return this.Descripcion;
        }
    }
}
