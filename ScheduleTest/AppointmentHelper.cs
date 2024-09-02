using System;
using System.Collections.Generic;
using System.Linq;
using Janus.Windows.Schedule;

namespace ScheduleTest
{
    public class ResourceHelper
    {
        private static readonly List<OwnerResource> Resources = new List<OwnerResource>();
        private static ResourceHelper instance = null;
        public OwnerResource this[string name]{
            get
            {
                return Resources.FirstOrDefault(x => x.Name == name);
            }
        }

        private ResourceHelper()
        {

        }

        public static ResourceHelper Instance => instance ?? (instance = new ResourceHelper());

        static ResourceHelper()
        {
            InitResourceCollection();
        }
        static void InitResourceCollection()
        {
            var resource1 = new OwnerResource()
                { Id = 1, Name = "Smith", DisplayText = "John, Smith", DepartmentId = 1, DepartmentName = "Radonc" };
            var resource2 = new OwnerResource()
            {
                Id = 2, Name = "Kevin", DisplayText = "Peterson, Kevin", DepartmentId = 1, DepartmentName = "Radonc"
            };
            var resource3 = new OwnerResource()
                { Id = 1, Name = "Smith", DisplayText = "John, Smith", DepartmentId = 2, DepartmentName = "Medonc" };
            Resources.AddRange(new[]
            {
                resource1, resource2, resource3
            });
        }

        public List<OwnerResource> GetAllResources(bool initForGroupLikeResources = false)
        {
            return initForGroupLikeResources ? Resources.Where(x=>x.Name=="Smith").ToList() : Resources.Where(x => x.DepartmentId == 1).ToList();
        }
    }

    public class ResourceWorkingHoursHelper
    {
        public static void SetResourceWorkingHourRange(ScheduleAppointmentOwner owner, bool addExceptions = false)
        {
            var workingHours = new List<ScheduleHourRange>();
            owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Monday,
                TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
            if (addExceptions)
            {
                owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Tuesday,
                    TimeSpan.FromHours(8), TimeSpan.FromHours(11)));
                owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithoutColor(ScheduleDayOfWeek.Tuesday,
                    TimeSpan.FromHours(11), TimeSpan.FromHours(14)));
                owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Tuesday,
                    TimeSpan.FromHours(14), TimeSpan.FromHours(16)));
            }
            else
            {
                owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Tuesday,
                    TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
            }

            owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Wednesday,
                TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
            owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Thursday,
                TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
            owner.WorkingHourSchema.WorkingHoursRange.Add(GetHourRangeWithColor(ScheduleDayOfWeek.Friday,
                TimeSpan.FromHours(8), TimeSpan.FromHours(17)));
        }

        private static ScheduleHourRange GetHourRangeWithColor(ScheduleDayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            var hourRange = new ScheduleHourRange(dayOfWeek, startTime, endTime);
            hourRange.FormatStyle.BackColor =  Helper.GetColorFromRGBString(VFormResource.AvailableHourBackGroundColor);
            return hourRange;
        }

        private static ScheduleHourRange GetHourRangeWithoutColor(ScheduleDayOfWeek dayOfWeek, TimeSpan startTime, TimeSpan endTime)
        {
            var hourRange = new ScheduleHourRange(dayOfWeek, startTime, endTime);
            hourRange.FormatStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.WorkHourBackGroundColor);
            return hourRange;
        }
    }




    public class AppointmentHelper
    {
        private static readonly string[] StatusArray = { "Open", "Completed" };
        private static readonly string[] Notes = { "Test note", "" };
        private static readonly bool[] CheckInStatuses = { false, true };
        private static ResourceHelper resourceHelper = ResourceHelper.Instance;

        public static List<VAppointment> GetAppointments()
        {
            var apptList = new List<VAppointment>();
            var initialStartDate = DateTime.Today;
            for (var i = 0; i < 3; i++)
            {
                var appts = GetAppointments(initialStartDate);
                apptList.AddRange(appts);
                initialStartDate = initialStartDate.AddDays(1);
            }
            return apptList;
        }

        public static List<VAppointment> GetWaitListAppointments()
        {
            var apptList = new List<VAppointment>();
            var activities = GetActivities();
            for (var i = 0; i < 5; i++)
            {
                var appt = GetWaitListAppointment(activities[i]);
                apptList.Add(appt);
            }
            return apptList;
        }

        private static VAppointment GetWaitListAppointment(Activity activity)
        {
            var appointment = new VAppointment
            {
                Activity = activity,
                IsWaitListed = true,
                StrNote = "WaitList appointment",
                Status = "Open"
            };
            appointment.Resources.Add(resourceHelper["Smith"]);
            return appointment;
        }

        private static List<VAppointment> GetAppointments(DateTime startDate)
        {
            var apptList = new List<VAppointment>();
            var initialStartDate = startDate.AddHours(8);
            var activities = GetActivities();
            for (var i = 0; i < 6; i++)
            {
                var index = i % 2;
                if (i == 3)
                {
                    initialStartDate = initialStartDate.AddMinutes(-60);
                }
                var appt = GetAppointment(initialStartDate, activities[i], CheckInStatuses[index],
                    StatusArray[index], Notes[index]);

                if (i < 3)
                {
                    appt.Resources.Add(resourceHelper["Smith"]);
                }
                else if (i < 5)
                {
                    appt.Resources.Add(resourceHelper["Smith"]);
                    appt.Resources.Add(resourceHelper["Kevin"]);
                }
                else
                {
                    appt.Resources.Add(resourceHelper["Kevin"]);
                }

                apptList.Add(appt);
                initialStartDate = initialStartDate.AddMinutes(60);
            }

            return apptList;
        }

        public static VAppointment GetAppointment(DateTime startDate, Activity activity, bool isPatientCheckIn,
            string status, string note)
        {
            var appt = new VAppointment
            {
                StartDate = startDate,
                EndDate = startDate.AddMinutes(activity.Duration),
                IsPatientCheckedIn = isPatientCheckIn,
                Status = status,
                StrNote = note,
                Activity = activity
            };
            return appt;
        }

        public static List<Activity> GetActivities()
        {
            return ActivitiesHelper.GetActivities();
        }
    }
}