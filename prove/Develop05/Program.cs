using System;

abstract class MindfulnessActivity
{
    private string activityName;
    private string description;
    protected int duration;

    protected MindfulnessActivity(string name, string desc)
    {
        activityName = name;
        description = desc;
    }

    public async Task StartActivity()
    {
        Console.Clear();
        Console.WriteLine($"--- {activityName} ---");
        Console.WriteLine(description);
        Console.Write("Enter the duration for this activity in seconds: ");
        
        if (int.TryParse(Console.ReadLine(), out duration))
        {
            await PauseWithSpinner(2);
            await ExecuteActivity();
            Console.WriteLine($"\nGood job! You've completed the {activityName}.");
            await PauseWithSpinner(3);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }
    }

    protected abstract Task ExecuteActivity();

    protected async Task PauseWithSpinner(int seconds)
    {
        Console.WriteLine("Preparing...");
        for (int i = 0; i < seconds; i++)
        {
            Console.Write($"\r{GetSpinner()}");
            await Task.Delay(1000);
        }
        Console.WriteLine("\r                    "); 
    }

    private string GetSpinner()
    {
        string[] spinner = { "|", "/", "-", "\\" };
        return spinner[(DateTime.Now.Second % spinner.Length)];
    }
}

class BreathingActivity : MindfulnessActivity
{
    public BreathingActivity() : base("Breathing Activity", 
        "This activity will help you relax by walking you through breathing in and out slowly. Clear your mind and focus on your breathing.") { }

    protected override async Task ExecuteActivity()
    {
        Console.WriteLine("Get ready to start breathing...\n");
        for (int i = 0; i < duration / 2; i++)
        {
            Console.WriteLine("Breathe in...");
            await PauseWithSpinner(4);
            Console.WriteLine("Breathe out...");
            await PauseWithSpinner(4);
        }
    }
}

class ReflectionActivity : MindfulnessActivity
{
    private List<string> prompts = new List<string>
    {
        "Think of a time when you stood up for someone else.",
        "Think of a time when you did something really difficult.",
        "Think of a time when you helped someone in need.",
        "Think of a time when you did something truly selfless."
    };

    private List<string> questions = new List<string>
    {
        "Why was this experience meaningful to you?",
        "Have you ever done anything like this before?",
        "How did you get started?",
        "How did you feel when it was complete?",
        "What made this time different than other times when you were not as successful?",
        "What is your favorite thing about this experience?",
        "What could you learn from this experience that applies to other situations?",
        "What did you learn about yourself through this experience?",
        "How can you keep this experience in mind in the future?"
    };

    public ReflectionActivity() : base("Reflection Activity", 
        "This activity will help you reflect on times in your life when you have shown strength and resilience. This will help you recognize the power you have and how you can use it in other aspects of your life.") { }

    protected override async Task ExecuteActivity()
    {
        var randomPrompt = prompts[new Random().Next(prompts.Count)];
        Console.WriteLine(randomPrompt);
        await PauseWithSpinner(4);

        for (int i = 0; i < 3; i++) 
        {
            var randomQuestion = questions[new Random().Next(questions.Count)];
            Console.WriteLine(randomQuestion);
            await PauseWithSpinner(4);
        }
    }
}

class ListingActivity : MindfulnessActivity
{
    private List<string> prompts = new List<string>
    {
        "Who are people that you appreciate?",
        "What are personal strengths of yours?",
        "Who are people that you have helped this week?",
        "When have you felt the Holy Ghost this month?",
        "Who are some of your personal heroes?"
    };

    public ListingActivity() : base("Listing Activity", 
        "This activity will help you reflect on the good things in your life by having you list as many things as you can in a certain area.") { }

    protected override async Task ExecuteActivity()
    {
        var randomPrompt = prompts[new Random().Next(prompts.Count)];
        Console.WriteLine(randomPrompt);
        await PauseWithSpinner(4);

        Console.WriteLine("You have 5 seconds to start thinking. Go!");
        await Task.Delay(5000);

        Console.WriteLine("Type in your responce, hit enter to type another (type 'done' to finish):");
        List<string> items = new List<string>();
        DateTime endTime = DateTime.Now.AddSeconds(duration); 

        while (DateTime.Now < endTime)
        {
            string item = Console.ReadLine();
            if (item.ToLower() == "done") break;
            items.Add(item);
        }

        Console.WriteLine($"\nYou've entered {items.Count} items.");
    }
}

class Program
{
    static async Task Main(string[] args)
    {
        await MainMenu();
    }

    static async Task MainMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("--- Mindfulness Activities ---");
            Console.WriteLine("1. Breathing Activity");
            Console.WriteLine("2. Reflection Activity");
            Console.WriteLine("3. Listing Activity");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an activity (1-4): ");

            string choice = Console.ReadLine();
            if (choice == "4")
            {
                Console.WriteLine("Exiting the program. Thank you!");
                break;
            }
            else if (choice == "1")
            {
                var activity = new BreathingActivity();
                await activity.StartActivity();
            }
            else if (choice == "2")
            {
                var activity = new ReflectionActivity();
                await activity.StartActivity();
            }
            else if (choice == "3")
            {
                var activity = new ListingActivity();
                await activity.StartActivity();
            }
            else
            {
                Console.WriteLine("Invalid choice. Please select again.");
                await Task.Delay(2000);
            }
        }
    }
}
