// Added a progressive counter that will take 1 the first time, then 2 the 2nd, 3 the 3rd and so on. Also added in the sentence to change to how many will be taken with each enter. 
using System;

class Program
{
    static void Main(string[] args)
    {
        Reference reference = new Reference("Proverbs", 3, 5, 6);
        Scripture scripture = new Scripture(reference, "Trust in the Lord with all thine heart and lean not unto thine own understanding");

        int round = 1; 

        while (true)
        {
            Console.Clear();
            scripture.Display();

            Console.WriteLine($"Press Enter to hide {round} words, or type quit to end the program.");
            string input = Console.ReadLine();

            if (input.ToLower() == "quit")
                break;

            
            scripture.HideRandomWords(round);

            
            if (scripture.AllWordsHidden())
                break; 

            round++; 
        }
    }
}

class Scripture
{
    private Reference _reference;
    private List<Word> _words;
    private Random _random = new Random();

    public Scripture(Reference reference, string text)
    {
        _reference = reference;
        _words = new List<Word>();

        foreach (string word in text.Split(" "))
        {
            _words.Add(new Word(word));
        }
    }

    public void Display()
    {
        Console.Write($"{_reference.ToString()}: ");
        foreach (Word word in _words)
        {
            Console.Write(word.GetDisplayText() + " ");
        }
        Console.WriteLine();
    }

    public void HideRandomWords(int round)
    {
        
        int wordsToHide = Math.Min(round, _words.Count); 
        for (int i = 0; i < wordsToHide; i++)
        {
            int index = _random.Next(_words.Count);  
            _words[index].Hide();
        }
    }

    public bool AllWordsHidden()
    {
        foreach (Word word in _words)
        {
            if (!word.IsHidden())
                return false;
        }
        return true;
    }
}

class Reference
{
    private string _book;
    private int _chapter;
    private int _startVerse;
    private int _endVerse;

    public Reference(string book, int chapter, int verse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = verse;
        _endVerse = verse;
    }

    public Reference(string book, int chapter, int startVerse, int endVerse)
    {
        _book = book;
        _chapter = chapter;
        _startVerse = startVerse;
        _endVerse = endVerse;
    }

    public override string ToString()
    {
        if (_startVerse == _endVerse)
            return ($"{_book} {_chapter}:{_startVerse}");
        else
            return ($"{_book} {_chapter}:{_startVerse}-{_endVerse}");
    }
}

class Word
{
    private string _text;
    private bool _hidden;

    public Word(string text)
    {
        _text = text;
        _hidden = false;
    }

    public void Hide()
    {
        _hidden = true;
    }

    public bool IsHidden()
    {
        return _hidden;
    }

    public string GetDisplayText()
    {
        return _hidden ? "_____" : _text; 
    }
}
