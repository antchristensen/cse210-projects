using System;
// I have added the ability to add a location of where said journal entry was at. Added it into Class Entry, and journalApp. This will also help the user remember where i had this experience. 
public class Entry
{
    private string _prompt;     
    private string _response;   
    private string _date;  
    private string _location;      

    public Entry(string prompt, string response, string date, string location)
    {
        _prompt = prompt;       
        _response = response;    
        _date = date;  
        _location = location;          
    }

    public override string ToString()
    {
        return $"Date: {_date}, Location: {_location}, Prompt: {_prompt}, Response: {_response}"; 
    }

    public string ToFileFormat()
    {
        return $"{_prompt}|{_response}|{_date}|{_location}"; 
    }

    public static Entry FromFileFormat(string line)
    {
        var parts = line.Split('|');
        if (parts.Length != 4)
        {
            throw new FormatException("Invalid entry format");
        }
        return new Entry(parts[0], parts[1], parts[2], parts[3]);
    }
}


public class Journal
{
    private List<Entry> entries = new List<Entry>();

    public void AddEntry(Entry entry)
    {
        entries.Add(entry);
    }

    public void SaveToFile(string filename)
    {
        using (StreamWriter writer = new StreamWriter(filename))
        {
            foreach (var entry in entries)
            {
                writer.WriteLine(entry.ToFileFormat());
            }
        }
        Console.WriteLine("Journal saved.\n");
    }

    public void LoadFromFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine("File not found.\n");
            return;
        }

        entries.Clear();
        using (StreamReader reader = new StreamReader(filename))
        {
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                try
                {
                    entries.Add(Entry.FromFileFormat(line));
                }
                catch (FormatException ex)
                {
                    Console.WriteLine($"Error processing line: {line}. {ex.Message}");
                }
            }
        }
        Console.WriteLine("Journal loaded.\n");
    }

    public void DisplayEntries()
    {
        if (entries.Count == 0)
        {
            Console.WriteLine("No entries to display.\n");
            return;
        }
        foreach (var entry in entries)
        {
            Console.WriteLine(entry);
        }
        Console.WriteLine();
    }
}

public class JournalApp
{
    private static Journal journal = new Journal();
    private static string[] prompts = {
        "Who was the most interesting person I interacted with today?",
        "What was the best part of my day?",
        "How did I see the hand of the Lord in my life today?",
        "What was the strongest emotion I felt today?",
        "If I had one thing I could do over today, what would it be?"
    };

    public static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Write a new entry");
            Console.WriteLine("2. Display the journal");
            Console.WriteLine("3. Save the journal to a file");
            Console.WriteLine("4. Load the journal from a file");
            Console.WriteLine("5. Exit");
            Console.Write("What would you like to do? ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    WriteNewEntry();
                    break;
                case "2":
                    journal.DisplayEntries();
                    break;
                case "3":
                    SaveJournal();
                    break;
                case "4":
                    LoadJournal();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please choose again.");
                    break;
            }
        }
    }

    private static void WriteNewEntry()
    {
        var random = new Random();
        string prompt = prompts[random.Next(prompts.Length)];
        Console.WriteLine(prompt);
        Console.Write("Your response: ");
        string response = Console.ReadLine();
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        Console.Write("Location: ");
        string location = Console.ReadLine();

        var entry = new Entry(prompt, response, date, location);
        journal.AddEntry(entry);
        Console.WriteLine("Entry added.\n");

    }

    private static void SaveJournal()
    {
        Console.Write("Enter the filename to save the journal: ");
        string filename = Console.ReadLine();
        journal.SaveToFile(filename);
    }

    private static void LoadJournal()
    {
        Console.Write("Enter the filename to load the journal: ");
        string filename = Console.ReadLine();
        journal.LoadFromFile(filename);
    }
    
}
