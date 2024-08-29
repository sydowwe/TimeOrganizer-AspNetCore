using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TimeOrganizer_net_core.helper;

public class MyIntTime
{
    public int Hours { get;  }
    public int Minutes { get; }
    public int Seconds { get; }
    public MyIntTime(int seconds)
    {
        this.Hours = seconds / 3600;
        this.Minutes = (seconds % 3600) / 60;
        this.Seconds = seconds % 60;
    }

    public int GetInSeconds()
    {
        return Hours * 3600 + Minutes * 60 + Seconds;
    }
}

public class MyIntTimeConverter() : ValueConverter<MyIntTime, int>(
    myIntTime => myIntTime.GetInSeconds(),
    seconds => new MyIntTime(seconds)
);