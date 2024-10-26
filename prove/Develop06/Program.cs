using System;

abstract class Goal
{
    public string Name { get; }
    protected int Points { get; }
    protected bool Completed { get; private set; }

    protected Goal(string name, int points)
    {
        Name = name;
        Points = points;
        Completed = false;
    }

    public abstract int Record();

    public bool IsCompleted() => Completed;

    
    public void MarkComplete()
    {
        if (!Completed)
        {
            Complete();
        }
    }

    protected void Complete() => Completed = true;

    public int GetPoints() => Points;

    public override string ToString()
    {
        return $"[{(Completed ? "X" : " ")}] {Name}";
    }
}

class SimpleGoal : Goal
{
    public SimpleGoal(string name, int points) : base(name, points) { }

    public override int Record()
    {
        if (!Completed)
        {
            MarkComplete(); 
            return GetPoints();
        }
        return 0;
    }
}

class EternalGoal : Goal
{
    public EternalGoal(string name, int points) : base(name, points) { }

    public override int Record() => Completed ? 0 : GetPoints();
}

class ChecklistGoal : Goal
{
    public int CurrentCount { get; private set; }
    private readonly int TotalCount;
    private const int BonusPoints = 500;

    public ChecklistGoal(string name, int points, int totalCount) : base(name, points)
    {
        TotalCount = totalCount;
        CurrentCount = 0;
    }

    public override int Record()
    {
        if (CurrentCount < TotalCount)
        {
            CurrentCount++;
            int awardedPoints = GetPoints();

            if (CurrentCount == TotalCount)
            {
                awardedPoints += BonusPoints;
                MarkComplete(); 
            }
            return awardedPoints;
        }
        return 0;
    }

    public override string ToString()
    {
        return $"[{(Completed ? "X" : " ")}] {Name} (Completed {CurrentCount}/{TotalCount})";
    }
}

class QuestTracker
{
    private readonly List<Goal> goals = new List<Goal>();
    public int Score { get; private set; }

    public void AddGoal(Goal goal) => goals.Add(goal);

    public int RecordGoal(string goalName)
    {
        foreach (var goal in goals)
        {
            if (goal.Name == goalName)
            {
                int points = goal.Record();
                Score += points;
                return points;
            }
        }
        return 0;
    }

    public void DisplayGoals()
    {
        foreach (var goal in goals)
        {
            Console.WriteLine(goal);
        }
    }

    public void DisplayScore() => Console.WriteLine($"Current Score: {Score}");

    public void SaveProgress(string filename)
    {
        using (var writer = new StreamWriter(filename))
        {
            foreach (var goal in goals)
            {
                writer.WriteLine($"{goal.Name},{goal.GetType().Name},{goal.GetPoints()},{goal.IsCompleted()},{(goal is ChecklistGoal checklistGoal ? checklistGoal.CurrentCount : 0)}");
            }
            writer.WriteLine($"Score,{Score}");
        }
    }

    public void LoadProgress(string filename)
    {
        try
        {
            using (var reader = new StreamReader(filename))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');
                    if (parts[0] == "Score")
                    {
                        Score = int.Parse(parts[1]);
                        continue;
                    }

                    string name = parts[0];
                    string type = parts[1];
                    int points = int.Parse(parts[2]);
                    bool completed = bool.Parse(parts[3]);
                    int currentCount = int.Parse(parts[4]);

                    Goal goal = type switch
                    {
                        "SimpleGoal" => new SimpleGoal(name, points),
                        "EternalGoal" => new EternalGoal(name, points),
                        "ChecklistGoal" => new ChecklistGoal(name, points, currentCount),
                        _ => throw new InvalidOperationException("Unknown goal type.")
                    };

                    if (completed)
                    {
                        goal.MarkComplete();
                    }

                    goals.Add(goal);
                }
            }
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Save file not found.");
        }
    }
}

class Program
{
    static void Main()
    {
        QuestTracker tracker = new QuestTracker();
        while (true)
        {
            Console.WriteLine("\n--- Eternal Quest Menu ---");
            Console.WriteLine("1. Add Simple Goal");
            Console.WriteLine("2. Add Eternal Goal");
            Console.WriteLine("3. Add Checklist Goal");
            Console.WriteLine("4. Record Goal Completion");
            Console.WriteLine("5. Display Goals");
            Console.WriteLine("6. Display Score");
            Console.WriteLine("7. Save Progress");
            Console.WriteLine("8. Load Progress");
            Console.WriteLine("9. Exit");
            
            Console.Write("Select an option (1-9): ");
            string choice = Console.ReadLine();
            
            switch (choice)
            {
                case "1":
                    Console.Write("Enter Simple Goal Name: ");
                    string simpleName = Console.ReadLine();
                    Console.Write("Enter Points for Goal: ");
                    int simplePoints = int.Parse(Console.ReadLine());
                    tracker.AddGoal(new SimpleGoal(simpleName, simplePoints));
                    break;
                case "2":
                    Console.Write("Enter Eternal Goal Name: ");
                    string eternalName = Console.ReadLine();
                    Console.Write("Enter Points for Goal: ");
                    int eternalPoints = int.Parse(Console.ReadLine());
                    tracker.AddGoal(new EternalGoal(eternalName, eternalPoints));
                    break;
                case "3":
                    Console.Write("Enter Checklist Goal Name: ");
                    string checklistName = Console.ReadLine();
                    Console.Write("Enter Points per Completion: ");
                    int checklistPoints = int.Parse(Console.ReadLine());
                    Console.Write("Enter Total Completions Needed: ");
                    int totalCount = int.Parse(Console.ReadLine());
                    tracker.AddGoal(new ChecklistGoal(checklistName, checklistPoints, totalCount));
                    break;
                case "4":
                    Console.Write("Enter Goal Name to Record Completion: ");
                    string recordName = Console.ReadLine();
                    int pointsEarned = tracker.RecordGoal(recordName);
                    if (pointsEarned > 0)
                    {
                        Console.WriteLine($"Recorded completion for {recordName}. Points earned: {pointsEarned}.");
                    }
                    else
                    {
                        Console.WriteLine($"No points earned for {recordName}.");
                    }
                    break;
                case "5":
                    tracker.DisplayGoals();
                    break;
                case "6":
                    tracker.DisplayScore();
                    break;
                case "7":
                    Console.Write("Enter filename to save progress: ");
                    string saveFile = Console.ReadLine();
                    tracker.SaveProgress(saveFile);
                    Console.WriteLine("Progress saved.");
                    break;
                case "8":
                    Console.Write("Enter filename to load progress: ");
                    string loadFile = Console.ReadLine();
                    tracker.LoadProgress(loadFile);
                    Console.WriteLine("Progress loaded.");
                    break;
                case "9":
                    Console.WriteLine("Exiting the program.");
                    return;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}
