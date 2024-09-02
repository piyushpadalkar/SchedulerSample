# Scheduler Test Application

## Introduction
This documents outlines the various use cases available in the sample application using Schedule Control.

## Use Cases
### 1. Add appointments to the Schedule - 
Use the Apply button on the UI to populate appointments on the Schedule control. The appointments will be added for the current date and for next 2 days.
By default the view is configured for multiple resources and work week view.
The dates in the Calendar control are highlighted for which the appointments are present.
![image](../images/singleresource_workweekview.png)

**Expectation:** The test should be able to identify number of appointments displayed in the schedule view.

### 2. Change resources/owners in Schedule - 
User can change the owners/resources displayed in the Schedule using the options present under ***Resource*** group box.
The various options are listed below - <br>
1. Single - When this options is selected and Apply button is pressed, the Schedule will refresh and only appointments for the "John Smith" will be populated.
1. Multiple - When this option is selected and apply button is pressed, the Schedule will refresh and appointments for resources "John Smith and Kevin Peterson" will be populated.
1. Group like resources - This option will display the appointments for resource " John Smith" belonging to Radonc and Medonc departments and title of the column will change to **"John, Smith-Radon,Medonc"**.

**Expectation:** The test should be able to identify the selected resource option from the column title i.e. the caption of the column should be identifyable.
	
### 3. Change Schedule view type -
User can change the Schedule display/view type using the options present in ***View Type*** group box. The varoius options are listed below - <br>

1. Day - The option will change the display of schedule to Day type
![image](../images/singleresource_dayview.png)
1. Work week - The option will change the display of schedule to Work week (typically Monday-Friday)
![image](../images/singleresource_workweekview.png)
3. Month - The option will change the display of schedule to Month view.
![image](../images/singleresource_monthview.png)

**Expectation:** The test should be able to identify the schedule display type. The ***View*** property of the control determines the display type.

### 4. Select single/multiple appointments - 
User can select single/multiple appointment(s) on the Schedule view. 

**Expectation:** The test should be able to identify the number of selected appointment(s). The ***SelectedAppointments*** property of the control provides the information of selected appointment(s).

### 5. Check the appointment information - 
The test should be able to read and provide the information of the appointment displayed in the appointment bubble for the selected appointment.

### 6. Check the appointment tooltip information - 
When mouse hovered over the appointment bubble, a tooltip is displayed with appointment information. The test should be able to identify the tooltip information.

### 7. Copy/Paste appointment - 
User can select a single or multiple appointments to copy and paste on the Schedule using Context menu or Ctrl+C & Ctrl+V options.

**Expectation:** The test should be able to select the appointment and paste it on the schedule at the desired datetime.

### 8. Drag-Drop appointment on the Schedule - 
The test should be able to select single/multiple appointment(s) on the schedule to drag and drop at different datetime on the schedule.

### 9. Drag-Drop waitlist appointment on the Schedule - 
The test should be able to drag and drop the appointment from waitlist gridview to the schedule at desired datetime.

### 10. Select date in the Calendar control - 
The test should be able to select a date in the calendar control to navigate the schedule to the selected date. If the displayed view type is Day view the selected date in calendar will be shown.
If the view type is workweek, then the week for the selected date will be shown.

### 11. Context menu operation - 
The test should be able to select a timeslot in the Schedule to open the context menu. The context menu options should be then selected to perform additional operations.

### 12. Appointment Context menu operation - 
The test should be able to select an appointment to open the context menu and perform operations.

### 13. Double click on selected timeslot to create appointment – 
The test should be able to select the timeslot on the schedule view and perform mouse double click operation to create a new appointment.


### 14. Change time navigator frequency – 
The test should be able to perform right click mouse action on the time navigator bar to change the timeslot display frequency.
The test should be able to identify the selected timeslot frequency. This can be identified using Interval property of the control.


