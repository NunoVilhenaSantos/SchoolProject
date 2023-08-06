using Microsoft.Data.SqlClient;

namespace SchoolProject.Web.Helpers.Services;

public class DatabaseConnectionVerifier
{
    private readonly IConfiguration _configuration;

    public DatabaseConnectionVerifier(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> CheckDatabaseConnectionAsync()
    {
        var connectionString =
            _configuration.GetConnectionString("SchoolProject-somee");

        try
        {
            await using var connection = new SqlConnection(connectionString);
            await connection.OpenAsync();
            return true;
        }
        catch (SqlException)
        {
            // Se ocorrer um erro de conexão,
            // podemos adicionar algum atraso antes de tentar novamente.
            // Isso evita sobrecarregar o servidor de
            // banco de dados em caso de falha temporária.

            // Aguarda 5 segundos antes de tentar novamente.
            await Task.Delay(5000);
            return false;
        }
    }
}