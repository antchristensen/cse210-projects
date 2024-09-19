using System;

class Program
{
    static void Main(string[] args)
    {
        Job job1 = new Job();
        job1._jobTitle = "Manager";
        job1._company = "TC&Sons";
        job1._startYear = 2001;
        job1._endYear = 2024;

        Job job2 = new Job();
        job2._company = "MSD";
        job2._jobTitle = "Bus Driver";
        job2._startYear =2001;
        job2._endYear = 2024;
    }
}