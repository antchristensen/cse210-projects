using System;

public class Comment
{
    
    private string _name;
    private string _text;

    
    public string Name
    {
        get { return _name; }
        set { _name = value; }  
    }

    public string Text
    {
        get { return _text; }
        set { _text = value; }  
    }

    
    public Comment(string name, string text)
    {
        Name = name;  
        Text = text;
    }
}

public class Video
{
    
    private string _title;
    private string _author;
    private int _length;  
    private List<Comment> _comments;

    
    public string Title
    {
        get { return _title; }
        set { _title = value; }  
    }

    public string Author
    {
        get { return _author; }
        set { _author = value; }  
    }

    public int Length
    {
        get { return _length; }
        set { _length = value; }  
    }

    
    public Video(string title, string author, int length)
    {
        Title = title;
        Author = author;
        Length = length;
        _comments = new List<Comment>();  
    }

    
    public void AddComment(Comment comment)
    {
        _comments.Add(comment);
    }

    
    public int GetNumberOfComments()
    {
        return _comments.Count;
    }

    
    public List<Comment> GetComments()
    {
        return _comments;
    }
}

class Program
{
    static void Main()
    {
        
        List<Video> videos = new List<Video>();

        
        Video video1 = new Video("How to Cook like a pro", "John Doe", 600);
        Video video2 = new Video("Top 10 Programming Ideas", "Jane Smith", 1200);
        Video video3 = new Video("Best Travel Destinations", "Emily Clark", 900);

        
        video1.AddComment(new Comment("Eva", "Great video!"));
        video1.AddComment(new Comment("Robert", "Thanks for the tips."));
        video1.AddComment(new Comment("Charlie", "Very helpful!"));

        
        video2.AddComment(new Comment("Dave", "Awesome content!"));
        video2.AddComment(new Comment("Eve", "Learned a lot, thanks!"));
        video2.AddComment(new Comment("Frank", "Please make more videos."));

        
        video3.AddComment(new Comment("Ivy", "This was so inspiring."));
        video3.AddComment(new Comment("Heidi", "I want to visit all these places."));
        video3.AddComment(new Comment("Liam", "Amazing travel guide!"));

        
        videos.Add(video1);
        videos.Add(video2);
        videos.Add(video3);

        
        foreach (Video video in videos)
        {
            Console.WriteLine($"Title: {video.Title}");
            Console.WriteLine($"Author: {video.Author}");
            Console.WriteLine($"Length: {video.Length} seconds");
            Console.WriteLine($"Number of Comments: {video.GetNumberOfComments()}");

            
            foreach (Comment comment in video.GetComments())
            {
                Console.WriteLine($"- {comment.Name}: {comment.Text}");
            }

            Console.WriteLine();  
        }
    }
}