using TPSocios.Entidades;

namespace TPSocios.Repositorios
{
    public interface ISocioRepository
    {
        Task<List<Socio>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

        Task<int> InsertarAsync(Socio socio, CancellationToken cancellationToken = default);

        Task ActualizarAsync(Socio socio, CancellationToken cancellationToken = default);

        Task EliminarAsync(int idSocio, CancellationToken cancellationToken = default);

        Task<bool> ExisteLegajoAsync(string legajoSocio, int idSocioExcluir = 0,
                                    CancellationToken cancellationToken = default);
    }
}
