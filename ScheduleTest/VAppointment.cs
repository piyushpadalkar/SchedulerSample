using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScheduleTest
{
    public class VAppointment: ICloneable
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsPatientCheckedIn { get; set; }
        public string StrNote { get; set; } = string.Empty;
        public string Status { get; set; }
        public Activity Activity { get; set; }
        public bool IsWaitListed { get; set; } = false;
        public List<OwnerResource> Resources { get; set; } = new List<OwnerResource>();

        public string ResourceDisplayText => string.Join(",", Resources.Select(x => x.DisplayText));

        public override string ToString()
        {
            var apptDescription = new StringBuilder();
            apptDescription.Append($"{Activity.Title}");
            if (!string.IsNullOrEmpty(StrNote))
            {
                apptDescription.Append($", {StrNote}");
            }

            if (StartDate.HasValue)
            {
                apptDescription.Append($"{StartDate.Value:h:mm tt} - {EndDate.Value:h:mm tt}");
            }
            
            return apptDescription.ToString();
        }

        public object Clone()
        {
            return (VAppointment)this.MemberwiseClone();
        }
    }
}