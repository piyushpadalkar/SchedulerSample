using Janus.Windows.Schedule;
using System.Drawing;
using System.Windows.Forms;

namespace ScheduleTest
{

    public class VJanusSchedule : Janus.Windows.Schedule.Schedule
    {
        
        #region Accessibility Implementation
        protected override AccessibleObject CreateAccessibilityInstance()
        {
            return new VJanusScheduleAccessibleObject(this);
        }


        #region VJanusSchedule Accessible Object
        public sealed class VJanusScheduleAccessibleObject : Control.ControlAccessibleObject
        {
            private Janus.Windows.Schedule.Schedule owner;
            private AppointmentAccessibleObject[] appointments;

            public VJanusScheduleAccessibleObject(Janus.Windows.Schedule.Schedule owner)
                : base(owner)
            {
                this.owner = owner;

            }


            public override AccessibleRole Role
            {
                get
                {

                    return AccessibleRole.Pane;
                }
            }

            public override string Description
            {
                get
                {
                    return "Janus Schedule";
                }
            }

            public override int GetChildCount()
            {
                return owner.Appointments.Count;
            }

            public override AccessibleObject GetChild(int index)
            {
                appointments = new AppointmentAccessibleObject[this.GetChildCount()];
                int i = 0;

                foreach (ScheduleAppointment a in owner.Appointments)
                {
                    appointments[i] = new AppointmentAccessibleObject(owner, a, this);
                    i++;
                }

                return appointments[index];
            }

            public override string Name
            {
                get
                {
                    return owner.Name;
                }
            }
        }
        #endregion


        #region Appointment Accessible Object
        public sealed class AppointmentAccessibleObject : Control.ControlAccessibleObject
        {
            private string name;
            private Point location;
            private Size size;
            private ScheduleAppointment appointment;
            private Janus.Windows.Schedule.Schedule owner;
            private VJanusScheduleAccessibleObject parent;


            public AppointmentAccessibleObject(Janus.Windows.Schedule.Schedule owner, ScheduleAppointment appointment,
                VJanusScheduleAccessibleObject parent)
                : base(owner)
            {
                this.owner = owner;
                this.location = appointment.GetBounds().Location;
                this.size = appointment.GetBounds().Size;
                this.name = appointment.Text;
                this.appointment = appointment;
                this.parent = parent;

            }

            public override string Name
            {
                get
                {
                    return name;
                }
                set
                {
                    name = value;
                }
            }


            public override string Description
            {
                get
                {
                    return appointment.StartTime + " - " + appointment.EndTime;
                }
            }

            public override string Value
            {
                get
                {
                    return name;
                }
                set
                {
                    appointment.Text = value;
                }
            }

            public override AccessibleRole Role
            {
                get
                {
                    return AccessibleRole.Cell;
                }
            }

            public override Rectangle Bounds
            {
                get
                {
                    return new Rectangle(owner.PointToScreen(location), size);
                }
            }

            public override AccessibleObject Parent
            {
                get
                {
                    return parent;
                }
            }

            public override AccessibleStates State
            {
                get
                {
                    AccessibleStates state = AccessibleStates.Selectable;
                    if (appointment.Selected)
                    {
                        state |= AccessibleStates.Selected;

                    }
                    return state;
                }
            }

        #endregion

        }

        #endregion
    }
}