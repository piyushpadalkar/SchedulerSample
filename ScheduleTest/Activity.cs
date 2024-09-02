using System.Drawing;

namespace ScheduleTest
{
    public class Activity
    {
        public string Title { get; set; }
        public Color ActivityColor { get; set; } = Color.White;
        public int Duration { get; set; }
    }

}