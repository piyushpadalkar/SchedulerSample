using System.Collections.Generic;
using System.Drawing;

namespace ScheduleTest
{
    public class ActivitiesHelper
    {
        private static readonly string[] ActivityNames = { "Blood Test", "Daily Treatment" };
        private static readonly int[] ActivityDurations = { 30, 60, 20, 10, 60, 45 };
        public static List<Activity> GetActivities()
        {
            var activities= new List<Activity>();
            for (var i = 0; i < 6; i++)
            {
                var index = i % 2;
                var activity = new Activity()
                {
                    Title = ActivityNames[index],
                    ActivityColor = index==0?Color.LightGreen:Color.White,
                    Duration = ActivityDurations[i]
                };
                activities.Add(activity);
            }

            return activities;
        }
    }
}