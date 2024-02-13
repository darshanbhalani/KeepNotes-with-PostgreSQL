using KeepNotes_with_PostgreSQL;
using Npgsql;

namespace KeepNotes
{
    internal class NotesHandler
    {
        public void AddNote(NpgsqlConnection connection)
        {
            Note note = new Note();

            note.noteId = DateTime.UtcNow.Ticks.ToString();

            Console.Write("Title :- ");
            note.title = Console.ReadLine();

            Console.Write("Description :- ");
            note.description = Console.ReadLine();

            char c;
            Console.Write("You want to save note ? (y/n) :- ");
            c = Convert.ToChar(Console.ReadLine());

            switch (c)
            {
                case 'y':
                case 'Y':
                    SaveNote(connection, note.noteId, note.title, note.description);
                    break;
                case 'N':
                case 'n':
                    break;
                default:
                    Console.WriteLine("\u274C Enter valid input...");
                    break;
            }
        }
        public void DeleteNote(NpgsqlConnection connection)
        {
            Console.Write("Enter id :- ");
            string id = Convert.ToString(Console.ReadLine());

            bool flag = false;
            using (var cmd = new NpgsqlCommand($"SELECT * FROM Notes WHERE noteId='{id}'", connection))
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    flag = true;
                }
            }

            if (flag)
            {
                char c;
                Console.Write("You want to delete note ? (y/n) :- ");
                c = Convert.ToChar(Console.ReadLine());

                switch (c)
                {
                    case 'y':
                    case 'Y':
                        using (var cmd = new NpgsqlCommand($"DELETE FROM notes where noteId='{id}'", connection))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        Console.WriteLine("Note Deleted...");

                        break;
                    case 'N':
                    case 'n':
                        break;
                    default:
                        Console.WriteLine("Enter valid input...");
                        break;
                }
            }
            else { Console.WriteLine("No records found !"); }
        }
        public async Task UpdateNoteAsync(NpgsqlConnection connection)
        {
            Console.Write("Enter id :- ");
            string id = Convert.ToString(Console.ReadLine());

            bool flag = false;
            await using (var cmd = new NpgsqlCommand($"SELECT * FROM Notes WHERE noteId='{id}'", connection))
            await using (var reader = cmd.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    flag = true;
                }
                while (reader.Read())
                {
                    Console.WriteLine($"Note ID: {reader.GetString(0)}");
                    Console.WriteLine($"Title: {reader.GetString(2)}");
                    Console.WriteLine($"Description: {reader.GetString(3)}");
                    Console.WriteLine($"Created At: {reader.GetString(1)}");
                    Console.WriteLine($"Updated At: {reader.GetString(4)}");
                    Console.WriteLine();
                }
            }

            if (flag)
            {
                char c;
                Console.Write("New Title :- ");
                string? tempTitle = Console.ReadLine();

                Console.Write("New Description :- ");
                string? tempDescription = Console.ReadLine();

                Console.Write("You want to save note ? (y/n) :- ");
                c = Convert.ToChar(Console.ReadLine());

                switch (c)
                {
                    case 'y':
                    case 'Y':
                        string tempQuery;
                        if (string.IsNullOrEmpty(tempTitle) && !string.IsNullOrEmpty(tempDescription))
                        {
                            UpdateData($"UPDATE Notes SET description='{tempDescription}',updatedat='{DateTime.Now}' WHERE noteId='{id}'", connection);

                        }
                        else if (!string.IsNullOrEmpty(tempTitle) && string.IsNullOrEmpty(tempDescription))
                        {
                            UpdateData($"UPDATE Notes SET title='{tempTitle}',updatedat='{DateTime.Now}' WHERE noteId='{id}'", connection);
                        }
                        else if (!string.IsNullOrEmpty(tempTitle) && !string.IsNullOrEmpty(tempDescription))
                        {
                            UpdateData($"UPDATE Notes SET description='{tempDescription}',title='{tempTitle}',updatedat='{DateTime.Now}' WHERE noteId='{id}'", connection);
                        }
                        else
                        {
                            Console.WriteLine("No Data updated...");
                        }
                        break;
                    case 'N':
                    case 'n':
                        break;
                    default:
                        Console.WriteLine("Enter valid input...");
                        break;
                }
            }
            else { Console.WriteLine("No records found !"); }
        }
        public void ViewNote(NpgsqlConnection connection)
        {
                using (var cmd = new NpgsqlCommand("SELECT * FROM Notes", connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"Note ID: {reader.GetString(0)}");
                        Console.WriteLine($"Title: {reader.GetString(2)}");
                        Console.WriteLine($"Description: {reader.GetString(3)}");
                        Console.WriteLine($"Created At: {reader.GetString(1)}");
                        Console.WriteLine($"Updated At: {reader.GetString(4)}");
                        Console.WriteLine();
                    }
                }

            Console.WriteLine(new string('_', 20));
        }
        private void SaveNote(NpgsqlConnection connection, string noteId, string title, string description)
        {
            string query = $"INSERT INTO Notes(noteId, createdAt, title, description, updatedAt) VALUES('{noteId}', '{DateTime.Now.ToString()}', '{title}', '{description}', '{DateTime.Now.ToString()}')";

            using (var cmd = new NpgsqlCommand(query, connection))
            {
                cmd.ExecuteNonQuery();
            }
            Console.WriteLine("✅ Note Saved...");
        }
        private void UpdateData(string query, NpgsqlConnection connection)
        {
            using (var cmd1 = new NpgsqlCommand(query, connection))
            {
                cmd1.ExecuteNonQuery();
            }
            Console.WriteLine("Data updated...");
        }
    }
}