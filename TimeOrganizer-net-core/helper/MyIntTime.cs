using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace TimeOrganizer_net_core.helper;

public class MyIntTime
{
    public int hours { get;  }
    public int minutes { get; }
    public int seconds { get; }
    public MyIntTime(int seconds)
    {
        this.hours = seconds / 3600;
        this.minutes = (seconds % 3600) / 60;
        this.seconds = seconds % 60;
    }

    public int getInSeconds()
    {
        return hours * 3600 + minutes * 60 + seconds;
    }
}

public class MyIntTimeConverter() : ValueConverter<MyIntTime, int>(
    myIntTime => myIntTime.getInSeconds(),
    seconds => new MyIntTime(seconds)
);