using System.Security.Cryptography.X509Certificates;

namespace KeepNotes_with_PostgreSQL
{
    internal class Configuration
    {
        public string Host {  get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database {  get; set; }

        public string generateConnectionString()
        {
            return $"Host={this.Host};Port={this.Port};Username={this.Username};Password={this.Password};Database={this.Database}"; ;
        }
    }

    
}
