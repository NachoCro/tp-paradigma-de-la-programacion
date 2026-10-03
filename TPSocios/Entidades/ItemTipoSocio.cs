namespace TPSocios.Entidades
{
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
