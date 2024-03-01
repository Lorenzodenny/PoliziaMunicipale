using Microsoft.Data.SqlClient;

namespace PoliziaMunicipale.Models
{
    public class DB
    {
        public static string connectionString = "server=DESKTOP-NHB4AFL\\SQLEXPRESS; Initial Catalog=PoliziaMunicipale; Integrated Security=true; TrustServerCertificate=true";
        public static SqlConnection conn = new SqlConnection(connectionString);
    }

}
