#region

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Janus.Windows.GridEX;
using Janus.Windows.Schedule;
using TriState = Janus.Windows.GridEX.TriState;

#endregion

namespace ScheduleTest
{
    public partial class SchedulerTestForm : Form
    {
        private List<VAppointment> apptList = new List<VAppointment>();
        private List<VAppointment> copyAppointments = new List<VAppointment>();
        public SchedulerTestForm()
        {
            InitializeComponent();
            SetFormatStyles();
            Load += OnSchedulerFormLoad;
        }

        private void schedule_DrawScheduleArea(object sender, DrawScheduleAreaEventArgs e)
        {
            if (e.DateTime.DayOfWeek == DayOfWeek.Monday && e.DateTime.TimeOfDay >= TimeSpan.FromHours(8) && e.DateTime.TimeOfDay <= TimeSpan.FromHours(17))
            {
                var activityDisplayString = "Daily Treatment";
                e.PaintBackground();
                e.Graphics.DrawString(activityDisplayString, Font, Brushes.Gray, e.Bounds);
                e.Handled = true;
            }
        }

        private void OnSchedulerFormLoad(object sender, EventArgs e)
        {
            m_schedule.View = ScheduleView.WorkWeek;
            m_schedule.Date = DateTime.Today;
            AddScheduleOwners();
        }

        private void AddScheduleOwners()
        {
            m_schedule.Owners.Clear();
            var resources = ResourceHelper.Instance.GetAllResources();
            foreach (var resource in resources)
            {
                var owner = new ScheduleAppointmentOwner(resource, resource.DisplayText);
                switch (resource.Id)
                {
                    case 1:
                        ResourceWorkingHoursHelper.SetResourceWorkingHourRange(owner);
                        break;
                    default:
                        ResourceWorkingHoursHelper.SetResourceWorkingHourRange(owner, true);
                        break;
                }
                m_schedule.Owners.Add(owner);
            }
        }

        private void AddScheduleOwnersForGroupLikeResources()
        {
            m_schedule.Owners.Clear();
            var resources = ResourceHelper.Instance.GetAllResources(true);
            var displayText = resources[0].DisplayText + "-"  + string.Join(",", resources.Select(x => x.DepartmentName));
            var owner = new ScheduleAppointmentOwner(resources[0], displayText);
            m_schedule.Owners.Add(owner);
        }

        private void AddSingleScheduleOwners()
        {
            m_schedule.Owners.Clear();
            var resource = ResourceHelper.Instance.GetAllResources().FirstOrDefault();
            if (resource != null)
            {
                var owner = new ScheduleAppointmentOwner(resource, resource.DisplayText);
                ResourceWorkingHoursHelper.SetResourceWorkingHourRange(owner, true);
                m_schedule.Owners.Add(owner);
            }
        }

       

         private void SetFormatStyles()
        {
            m_schedule.ThemedAreas = Janus.Windows.Schedule.ThemedArea.All;
            m_schedule.ThemedAreas = Janus.Windows.Schedule.ThemedArea.GridLine | Janus.Windows.Schedule.ThemedArea.ScrollBars;

            // TimeNavigatorFormatStyle: Style for the Time Column on the right side
            m_schedule.TimeNavigatorFormatStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.BackColorNormal);

            // AllDayAreaBackgroundStyle: Style for range between Titel and Schedule Area
            m_schedule.AllDayAreaBackgroundStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.BackColorNormal);

            // Style of ScrollBar
            m_schedule.ScrollBarStyle.ControlColor = Helper.GetColorFromRGBString( VFormResource.BackColorNormal);

            // General BackColor
            m_schedule.BackColor = Helper.GetColorFromRGBString(VFormResource.BackColorNormal);

            // Style of Header line
            m_schedule.HeadersFormatStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.TitleColor);
            m_schedule.HeadersFormatStyle.Blend = (float) 0.5;
            m_schedule.HeadersFormatStyle.BlendGradient = (float) 0.5;

            // Format Styls Colors of Schedule regions
            m_schedule.WorkingHourBackgroundStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.WorkHourBackGroundColor);
            m_schedule.HourBackgroundStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.HourBackGroundColor);
            m_schedule.MonthDaysFormatStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.MonthBackGroundColor);
            m_schedule.WeekDaysBackgroundStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.MonthBackGroundColor);

            // Appointments
            m_schedule.AppointmentsFormatStyle.BackColor = Helper.GetColorFromRGBString(VFormResource.AppointmentColor);
            m_schedule.AppointmentOutLineColor = Helper.GetColorFromRGBString(VFormResource.TitleColor);
        }
        
        private void RefreshView()
        {
            FetchAppointments();
            RefreshSchedule();
            RefreshTrackerView();
            RefreshWaitListView();
        }

        private void RefreshWaitListView()
        {
            var apptList = AppointmentHelper.GetWaitListAppointments();
            waitListGrid.DataSource = null;
            waitListGrid.DataSource = apptList;
            waitListGrid.Refetch();
        }

        private void FetchAppointments()
        {
            apptList = AppointmentHelper.GetAppointments();
        }

        private void RefreshSchedule()
        {
            m_schedule.Appointments.Clear();
            var scheduleAppointments = MapAppointmentToScheduleAppointment(apptList);
            if (m_schedule.Owners == null || m_schedule.Owners.Count == 0)
            {
                return;
            }

            foreach (var scheduleAppointment in scheduleAppointments)
            {
                m_schedule.Appointments.Add(scheduleAppointment);
            }

            m_schedule.Refetch();
            m_schedule.Refresh();
        }

        private List<ScheduleAppointment> MapAppointmentToScheduleAppointment(List<VAppointment> apptList)
        {
            var scheduleAppointments = new List<ScheduleAppointment>();

            foreach (var scheduleAppt in apptList.SelectMany(appt => 
                         appt.Resources.Select(resource => 
                             new ScheduleAppointment((DateTime)appt.StartDate, (DateTime)appt.EndDate, appt.ToString())
                     {
                         Tag = appt,
                         Owner = resource
                     })))
            {
                SetAppointmentIcon(scheduleAppt);
                SetAppointmentColor(scheduleAppt);
                scheduleAppointments.Add(scheduleAppt);
            }

            return scheduleAppointments;
        }

        private void SetAppointmentColor(ScheduleAppointment scheduleAppointment)
        {
            var appointment = scheduleAppointment.Tag as VAppointment;
            var color = appointment.Activity.ActivityColor;
            scheduleAppointment.FormatStyle.BackColor = color;
        }

        private void SetAppointmentIcon(ScheduleAppointment scheduleAppointment)
        {
            var images = new List<int>();
            scheduleAppointment.ImageIndex1 = scheduleAppointment.ImageIndex2 =
                scheduleAppointment.ImageIndex3 = scheduleAppointment.ImageIndex4 = -1;
            var appointment = scheduleAppointment.Tag as VAppointment;
            if (appointment.Status == "Completed")
            {
                images.Add(0);
            }

            if (!string.IsNullOrEmpty(appointment.StrNote))
            {
                images.Add(1);
            }

            if (images.Count >= 1)
            {
                scheduleAppointment.ImageIndex1 = images[0];
            }

            if (images.Count >= 2)
            {
                scheduleAppointment.ImageIndex1 = images[1];
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var dtRange = m_schedule.DateRange;
            var workingHourRange = m_schedule.WorkingHourSchema.WorkingHoursRange;
            foreach (ScheduleHourRange scheduleHourRange in workingHourRange)
            {
                MessageBox.Show(
                    $"range: {scheduleHourRange.StartTime} - {scheduleHourRange.EndTime} - {scheduleHourRange.Key}");
            }
        }

        private void vCalendar1_DatesChanged(object sender, EventArgs e)
        {
            var dateRange = vCalendar1.DatesRange;
            m_schedule.SelectedDays = dateRange;
            m_schedule.EnsureVisible(dateRange.Start);
        }

        private void vCalendar1_SelectionChanged(object sender, EventArgs e)
        {
            m_schedule.EnsureVisible(vCalendar1.CurrentDate);
        }

        private void m_schedule_AppointmentDoubleClick(object sender, AppointmentEventArgs e)
        {
            var scheduleAppointment = e.Appointment;
            if (scheduleAppointment.Tag is VAppointment appointment)
            {
                MessageBox.Show($"{appointment.Activity.Title} - {appointment.Status}");
            }
        }

        private void refreshToolstripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void addViewMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Add view menu item clicked");
        }

        private void agendaMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Agenda view menu item clicked");
        }

        private void m_schedule_Click(object sender, EventArgs e)
        {
            var date = m_schedule.GetDateTimeAt();
        }

        private void m_schedule_MouseMove(object sender, MouseEventArgs e)
        {
            var hitTest = m_schedule.HitTest(e.X,e.Y);
            if (hitTest != HitTest.Appointment && e.Button != MouseButtons.None)
            {
                toolTip1.SetToolTip(m_schedule,"");
                return;
            }

            var appt = m_schedule.GetAppointmentAt(e.X,e.Y);
            if (appt == null)
            {
                return;
            }
            var vappt = appt.Tag as VAppointment;
            if (vappt == null)
            {
                return;
            }

            var tooltipMsg = new StringBuilder();
            tooltipMsg.AppendLine("Activity: " + vappt.Activity.Title);
            tooltipMsg.AppendLine("Status: " + vappt.Status);
            if (!string.IsNullOrEmpty(vappt.StrNote))
            {
                tooltipMsg.AppendLine("Note: " + vappt.StrNote);
            }
            tooltipMsg.AppendLine("Time: " + vappt.StartDate.Value.ToString("h:mm tt") + " - " + vappt.EndDate.Value.ToString("h:mm tt"));
            
            toolTip1.SetToolTip(m_schedule, tooltipMsg.ToString());
        }

        private void addNewAppointmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("New appointment add menu clicked!!!");
        }

        private void trackerMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Tracker view menu item clicked");
        }

        private void m_schedule_DoubleClick(object sender, EventArgs e)
        {
            var dt = m_schedule.GetDateTimeAt();
            MessageBox.Show($"Create new Appointment at {dt.ToString("MM/dd/yyyy h:mm tt")}");
        }

        private void RefreshTrackerView()
        {
            gridEX1.DataSource = null;
            var apptList = AppointmentHelper.GetAppointments();
            gridEX1.DataSource = apptList;
            gridEX1.Refetch();
        }

        private void GridEX1_FormattingRow(object sender, RowLoadEventArgs e)
        {
            var vdgAppointment = e.Row.DataRow as VAppointment;
            var gridEXRow = e.Row;
            FormatRow(gridEXRow, vdgAppointment);
        }

        protected void FormatRow(GridEXRow gridEXRow, VAppointment vdgAppointment)
        {
            if (vdgAppointment != null)
            {
                gridEXRow.RowStyle = new GridEXFormatStyle();
                if (vdgAppointment.Activity != null)
                {
                    gridEXRow.RowStyle.BackColor = vdgAppointment.Activity.ActivityColor;
                }
                // Make bold if containing nested tasks
                gridEXRow.RowStyle.FontBold = vdgAppointment.Status == "Open" ? TriState.True : TriState.False;

            }
        }

        private void applySettings_Click(object sender, EventArgs e)
        {
            if (singleResourceRadio.Checked)
            {
                AddSingleScheduleOwners();
            }
            else if (multipleResourceRadio.Checked)
            {
                AddScheduleOwners();
            }
            else if(groupLikeResourcesRadio.Checked)
            {
                AddScheduleOwnersForGroupLikeResources();
            }

            if (dayRadio.Checked)
            {
                m_schedule.View = ScheduleView.DayView;
            }
            else if(workWeekRadio.Checked)
            {
                m_schedule.View = ScheduleView.WorkWeek;
            }
            else if(monthRadio.Checked)
            {
                m_schedule.View = ScheduleView.MonthView;

            }

            RefreshView();
        }

        private void m_schedule_DragDrop(object sender, DragEventArgs e)
        {
            if (e.AllowedEffect != DragDropEffects.All || e.Effect != DragDropEffects.All)
            {
                return;
            }

            var point = new Point(e.X, e.Y);
            var schedulePoint = m_schedule.PointToClient(point);
            if (m_schedule.HitTest(schedulePoint) != HitTest.Schedule)
            {
                return;
            }

            var appointments = GetAppointmentForDragDrop(e.Data);
            var dtStart = m_schedule.GetDateTimeAt(schedulePoint);
            if (dtStart < DateTime.Now)
            {
                MessageBox.Show("Selected Datetime is in the past");
            }

            var owner = m_schedule.GetOwnerAt(schedulePoint);
            foreach (var appointment in appointments)
            {
                appointment.StartDate = dtStart;
                appointment.EndDate = dtStart.AddMinutes(appointment.Activity.Duration);
                var resource = owner.Value as OwnerResource;
                appointment.Resources = new List<OwnerResource>(new []{resource});
            }

            apptList.AddRange(appointments);
            RefreshSchedule();
        }

        private List<VAppointment> GetAppointmentForDragDrop(IDataObject data)
        {
            var tmpObj = data.GetData(typeof(VAppointment[]));
            var array = tmpObj as Array;
            var appointments = array?.OfType<VAppointment>().ToArray();
            if (appointments != null)
            {
                foreach (var vAppointment in appointments)
                {
                    vAppointment.IsWaitListed = false;
                }

                return appointments.ToList();
            }

            return new List<VAppointment>();
        }

        private void m_schedule_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data == null)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Move;
        }

        private void m_schedule_DragLeave(object sender, EventArgs e)
        {
            var apptTaskBase = new VAppointment[m_schedule.SelectedAppointments.Count];

            for (var i = 0; i < m_schedule.SelectedAppointments.Count; i++)
            {
                apptTaskBase[i] = m_schedule.SelectedAppointments[i].Tag as VAppointment;
            }

            m_schedule.DoDragDrop(apptTaskBase, DragDropEffects.All);
        }

        private void m_schedule_DragOver(object sender, DragEventArgs e)
        {
            var point = new Point(e.X, e.Y);
            var schedulePoint = m_schedule.PointToClient(point);
            if (m_schedule.HitTest(schedulePoint) != HitTest.Schedule)
            {
                e.Effect = DragDropEffects.None;
                return;
            }

            e.Effect = DragDropEffects.All;
        }

        private void m_schedule_DroppingAppointment(object sender, DroppingAppointmentEventArgs e)
        {

        }

        private void m_schedule_AppointmentDrag(object sender, AppointmentDragEventArgs e)
        {

        }

        private void waitListGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Clicks <= 1 && waitListGrid.HitTest(e.X, e.Y) == GridArea.Cell)
                {
                    var selItems = waitListGrid.SelectedItems;
                    var list = new VAppointment[selItems.Count];
                    for (var i = 0; i < selItems.Count; i++)
                    {
                        if (selItems[i].GetRow().DataRow is VAppointment app)
                        {
                            list[i] = app;
                        }
                    }
                    var dragDropEffects = DragDropEffects.All;
                    waitListGrid.DoDragDrop(list, dragDropEffects);
                }
            }
        }

        private void m_schedule_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                var selAppts = m_schedule.SelectedAppointments;
                if (selAppts != null)
                {
                    foreach (ScheduleAppointment appt in selAppts)
                    {
                        var vAppointment = appt.Tag as VAppointment;
                        apptList.Remove(vAppointment);
                    }
                }
                RefreshSchedule();
            }

            if (e.KeyCode == Keys.F5)
            {
                RefreshView();
            }
        }

        private void Minutes30ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_schedule.Interval = Interval.ThirtyMinutes;
        }

        private void Minutes60ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_schedule.Interval = Interval.SixtyMinutes;
        }

        private void m_schedule_MouseDown(object sender, MouseEventArgs e)
        {
            var point = new Point(e.X, e.Y);
            if (e.Button == MouseButtons.Right && m_schedule.HitTest(point) == HitTest.TimeNavigator)
            {
                m_schedule.ContextMenuStrip = m_TimeNavigatorContextMenu;
            }
            else if (e.Button == MouseButtons.Right)
            {
                m_schedule.ContextMenuStrip = m_ScheduleContextMenu;
            }
        }

        private void m_schedule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == (Keys.Control | Keys.C))
            {
                CopyAppointment();
            }

            if (e.KeyData == (Keys.Control | Keys.V))
            {
                PasteAppointment();
            }
        }

        
        private void PasteAppointment()
        {
            if (!copyAppointments.Any())
            {
                return;
            }

            var dateRange = m_schedule.SelectedDays;
            var owner = m_schedule.CurrentOwner;

            foreach (var appt in copyAppointments)
            {
                var clonedAppt = (VAppointment)appt.Clone();
                clonedAppt.StartDate = dateRange.Start;
                clonedAppt.EndDate = dateRange.End;
                clonedAppt.Resources = new List<OwnerResource>(new[] { (OwnerResource)owner.Value });
                apptList.Add(clonedAppt);
            }
            RefreshSchedule();
        }

        private void CopyAppointment()
        {
            copyAppointments.Clear();
            var selectedAppointments = m_schedule.SelectedAppointments;
            foreach (ScheduleAppointment scheduleSelectedAppointment in selectedAppointments)
            {
                var appointment = (VAppointment)scheduleSelectedAppointment.Tag;
                copyAppointments.Add(appointment);
            }
        }
    }
}