namespace Notes.Models;

internal class Note
{
    public string Filename { get; set; }
    public string Text { get; set; }
    public DateTime Date { get; set; }

    public Note()
    {
        //constructor sets default values for properties incl random file name
        Filename = $"{Path.GetRandomFileName()}.notes.txt";
        Date = DateTime.Now;
        Text = "";
    }
    //Methods for saving and deleting the current note
    public void Save() =>
        File.WriteAllText(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename), Text);

    public void Delete() => 
        File.Delete(System.IO.Path.Combine(FileSystem.AppDataDirectory, Filename));

    public static Note Load(string filename)
    {
        filename = System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);

        if (!File.Exists(filename)) 
            throw new FileNotFoundException("Unable to find on local storage. ", filename);

        return
            new()
            {
                Filename = Path.GetFileName(filename),
                Text = File.ReadAllText(filename),
                Date = File.GetLastWriteTime(filename)
            };
    }

    public static IEnumerable<Note> LoadAll()
    {
        //get the folder where the notes are stored
        string appDataPath = FileSystem.AppDataDirectory;

        //using linq extension to load the * notes.txt files
        return Directory

            // Select the files names from the directory
            .EnumerateFiles(appDataPath, "*.notes.txt")
            //each file name is used to load a note
            .Select(Filename => Note.Load(Path.GetFileName(Filename)))
            // with the final collection of notes order by date
            .OrderByDescending(note => note.Date);
    }
}
