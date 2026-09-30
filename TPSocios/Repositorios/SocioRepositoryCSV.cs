using TPSocios.Entidades;

namespace TPSocios.Repositorios
{
    /// <summary>
    /// Implementación de ISocioRepository sobre un archivo CSV.
    /// Sus operaciones se encuentran sin desarrollar, tal como pide el
    /// enunciado: es la contraparte "vacía" frente al repositorio funcional.
    /// Como la interfaz está completa, esta clase igual debe compilar.
    /// </summary>
    public class SocioRepositoryCSV : ISocioRepository
    {
        public Task<List<Socio>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("La lectura sobre archivo CSV no fue implementada.");
        }

        public Task<int> InsertarAsync(Socio socio, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("El alta sobre archivo CSV no fue implementada.");
        }

        public Task ActualizarAsync(Socio socio, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("La modificación sobre archivo CSV no fue implementada.");
        }

        public Task EliminarAsync(int idSocio, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("La baja sobre archivo CSV no fue implementada.");
        }

        public Task<bool> ExisteLegajoAsync(string legajoSocio, int idSocioExcluir = 0,
                                            CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException("La consulta de legajos sobre archivo CSV no fue implementada.");
        }
    }
}
