namespace TPSocios.Entidades
{
    public class Socio
    {
        private int _idSocio;
        private string _legajoSocio;
        private string _apellido;
        private string _nombre;
        private string _email;
        private DateTime _fechaNacimiento;
        private decimal _cuotaMensual;
        private TipoSocio _tipoSocio;
        private bool _disponible;

        public Socio()
        {
            this._idSocio = 0;
            this._legajoSocio = string.Empty;
            this._apellido = string.Empty;
            this._nombre = string.Empty;
            this._email = string.Empty;
            this._fechaNacimiento = DateTime.Today;
            this._cuotaMensual = 0m;
            this._tipoSocio = TipoSocio.Menor;
            this._disponible = false;
        }

        public Socio(int idSocio, string legajoSocio, string apellido, string nombre,
                     string email, DateTime fechaNacimiento, decimal cuotaMensual,
                     TipoSocio tipoSocio, bool disponible)
        {
            this._idSocio = idSocio;
            this._legajoSocio = legajoSocio;
            this._apellido = apellido;
            this._nombre = nombre;
            this._email = email;
            this._fechaNacimiento = fechaNacimiento;
            this._cuotaMensual = cuotaMensual;
            this._tipoSocio = tipoSocio;
            this._disponible = disponible;
        }

        public int IdSocio
        {
            get { return this._idSocio; }
            set { this._idSocio = value; }
        }

        public string LegajoSocio
        {
            get { return this._legajoSocio; }
            set { this._legajoSocio = value; }
        }

        public string Apellido
        {
            get { return this._apellido; }
            set { this._apellido = value; }
        }

        public string Nombre
        {
            get { return this._nombre; }
            set { this._nombre = value; }
        }

        public string Email
        {
            get { return this._email; }
            set { this._email = value; }
        }

        public DateTime FechaNacimiento
        {
            get { return this._fechaNacimiento; }
            set { this._fechaNacimiento = value; }
        }

        public decimal CuotaMensual
        {
            get { return this._cuotaMensual; }
            set { this._cuotaMensual = value; }
        }

        public TipoSocio TipoSocio
        {
            get { return this._tipoSocio; }
            set { this._tipoSocio = value; }
        }

        public bool Disponible
        {
            get { return this._disponible; }
            set { this._disponible = value; }
        }

        public int Edad
        {
            get
            {
                int anios = DateTime.Today.Year - this._fechaNacimiento.Year;

                if (this._fechaNacimiento.AddYears(anios) > DateTime.Today)
                {
                    anios--;
                }

                return anios;
            }
        }
    }
}
