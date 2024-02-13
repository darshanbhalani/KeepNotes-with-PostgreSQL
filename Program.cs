using KeepNotes;
using Npgsql;

namespace KeepNotes_with_PostgreSQL
{
    internal class Program
    {
        private static string connectionString;

        static void Main(string[] args)
        {
            try
            {
                Configuration configuration = new Configuration { 
                    Host = "localhost",
                    Port = "5432",
                    Username = "postgres",
                    Password = "2708",
                    Database = "Keep Notes"
                };

                connectionString = configuration.generateConnectionString();

                using (var connection = new NpgsqlConnection(connectionString: connectionString))
                {
                    connection.Open();
                    int n;
                    do
                    {
                        Console.WriteLine("\n\n");
                        Console.WriteLine(new string('_', 20));
                        Console.WriteLine(new string('-', 20));
                        Console.WriteLine($" :: KEEP NOTES :: ");
                        Console.WriteLine(new string('_', 20));
                        Console.WriteLine(new string('-', 20));
                        Console.WriteLine("Press");
                        Console.WriteLine("1 for Add Note");
                        Console.WriteLine("2 for Delete Note");
                        Console.WriteLine("3 for Update Note");
                        Console.WriteLine("4 for View Note");
                        Console.WriteLine("0 for Exit");
                        Console.WriteLine(new string('.', 20));
                        Console.Write("=> ");
                        n = Convert.ToInt32(Console.ReadLine());

                        NotesHandler notesHandle = new NotesHandler();
                        switch (n)
                        {
                            case 1:
                                notesHandle.AddNote(connection);
                                break;
                            case 2:
                                notesHandle.DeleteNote(connection);
                                break;
                            case 3:
                                notesHandle.UpdateNoteAsync(connection);
                                break;
                            case 4:
                                notesHandle.ViewNote(connection);
                                break;
                            case 0:
                                return;
                            default:
                                Console.WriteLine("Enter Valid Input...");
                                break;
                        }
                    } while (n != 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error : {e.Message}");
            }
        }
    }
}
