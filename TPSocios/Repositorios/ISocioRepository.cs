using TPSocios.Entidades;

namespace TPSocios.Repositorios
{
    /// <summary>
    /// Contrato (interfaz) de persistencia de socios.
    /// Es la abstracción que permite cambiar la implementación, por ejemplo
    /// pasar de SQL Server a un archivo CSV, sin modificar la capa de UI.
    /// </summary>
    public interface ISocioRepository
    {
        /// <summary>
        /// Obtiene todos los socios, ordenados por apellido y nombre.
        /// Es la operación de lectura utilizada para llenar la grilla.
        /// </summary>
        Task<List<Socio>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

        /// <summary>
        /// Da de alta un socio y devuelve el IdSocio generado por la base de datos.
        /// </summary>
        Task<int> InsertarAsync(Socio socio, CancellationToken cancellationToken = default);

        /// <summary>
        /// Actualiza los datos del socio que indica su propiedad IdSocio.
        /// </summary>
        Task ActualizarAsync(Socio socio, CancellationToken cancellationToken = default);

        /// <summary>
        /// Elimina el socio que se identifica con el IdSocio recibido.
        /// </summary>
        Task EliminarAsync(int idSocio, CancellationToken cancellationToken = default);

        /// <summary>
        /// Indica si el legajo ya se encuentra registrado por otro socio.
        /// El parámetro idSocioExcluir permite que, al modificar, el legajo
        /// propio del socio no sea considerado un duplicado.
        /// </summary>
        Task<bool> ExisteLegajoAsync(string legajoSocio, int idSocioExcluir = 0,
                                    CancellationToken cancellationToken = default);
    }
}
