using System;
using System.Collections.Generic;


public abstract class Activity
{
    private DateTime _date;
    private int _length; 

    protected Activity(DateTime date, int length)
    {
        _date = date;
        _length = length;
    }

    
    public int Length => _length;

    public virtual double GetDistance() => 0; 
    public virtual double GetSpeed() => (GetDistance() / Length) * 60; 
    public virtual double GetPace() => (Length / GetDistance()); 

    public string GetSummary()
    {
        return $"{_date:dd MMM yyyy} {GetType().Name} ({Length} min) - Distance: {GetDistance():F1} " +
               $"{(GetDistance() < 1 ? "laps" : "miles")} , Speed: {GetSpeed():F1} " +
               $"{(GetSpeed() < 1 ? "kph" : "mph")}, Pace: {GetPace():F2} min per " +
               $"{(GetDistance() < 1 ? "lap" : (GetDistance() < 2 ? "mile" : "km"))}";
    }
}


public class Running : Activity
{
    private double _distance; 

    public Running(DateTime date, int length, double distance) : base(date, length)
    {
        _distance = distance;
    }

    public override double GetDistance() => _distance;
}


public class Cycling : Activity
{
    private double _speed; 

    public Cycling(DateTime date, int length, double speed) : base(date, length)
    {
        _speed = speed;
    }

    public override double GetSpeed() => _speed; 
    public override double GetDistance() => (_speed * Length) / 60; 
}


public class Swimming : Activity
{
    private int _laps; 

    public Swimming(DateTime date, int length, int laps) : base(date, length)
    {
        _laps = laps;
    }

    public override double GetDistance() => (_laps * 50) / 1000.0; 
    public override double GetSpeed() => (GetDistance() / Length) * 60; 
}


class Program
{
    static void Main(string[] args)
    {
        List<Activity> activities = new List<Activity>
        {
            new Running(new DateTime(2022, 11, 3), 30, 3.0), 
            new Cycling(new DateTime(2022, 11, 4), 45, 12.0), 
            new Swimming(new DateTime(2022, 11, 5), 30, 20)  
        };

        foreach (var activity in activities)
        {
            Console.WriteLine(activity.GetSummary());
        }
    }
}
