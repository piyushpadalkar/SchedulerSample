


using Janus.Windows.Schedule;
using Janus.Windows.UI.Tab;
using System.Windows.Forms;

namespace ScheduleTest
{
    partial class SchedulerTestForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchedulerTestForm));
            Janus.Windows.GridEX.GridEXLayout waitListGrid_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            Janus.Windows.GridEX.GridEXLayout gridEX1_DesignTimeLayout = new Janus.Windows.GridEX.GridEXLayout();
            this.m_ScheduleContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.refreshToolstripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewAppointmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.vTabControlModernized1 = new Janus.Windows.UI.Tab.UITab();
            this.m_tabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addViewMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.agendaMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.trackerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.vPanel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rightTblLayoutPanel = new TableLayoutPanel();
            this.waitListGrid = new VJanusDataGridModernized(this.components);
            this.vPanel2 = new Panel();
            this.viewTypeGrpBox = new System.Windows.Forms.GroupBox();
            this.monthRadio = new System.Windows.Forms.RadioButton();
            this.workWeekRadio = new System.Windows.Forms.RadioButton();
            this.dayRadio = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupLikeResourcesRadio = new System.Windows.Forms.RadioButton();
            this.multipleResourceRadio = new System.Windows.Forms.RadioButton();
            this.singleResourceRadio = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gridEX1 = new Janus.Windows.GridEX.GridEX();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.applySettings = new Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.m_TimeNavigatorContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Minutes30ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Minutes60ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.uiTabPage1 = new Janus.Windows.UI.Tab.UITabPage();
            this.uiTabPage2 = new Janus.Windows.UI.Tab.UITabPage();
            this.m_schedule = new ScheduleTest.VJanusSchedule();
            this.vCalendar1 = new Janus.Windows.Schedule.Calendar();
            this.m_ScheduleContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vTabControlModernized1)).BeginInit();
            this.vTabControlModernized1.SuspendLayout();
            this.m_tabContextMenu.SuspendLayout();
            this.vPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.rightTblLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.waitListGrid)).BeginInit();
            this.vPanel2.SuspendLayout();
            this.viewTypeGrpBox.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_TimeNavigatorContextMenu.SuspendLayout();
            this.uiTabPage1.SuspendLayout();
            this.uiTabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_schedule)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.vCalendar1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_ScheduleContextMenu
            // 
            this.m_ScheduleContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolstripMenuItem,
            this.addNewAppointmentToolStripMenuItem});
            this.m_ScheduleContextMenu.Name = "m_ScheduleContextMenu";
            resources.ApplyResources(this.m_ScheduleContextMenu, "m_ScheduleContextMenu");
            // 
            // refreshToolstripMenuItem
            // 
            this.refreshToolstripMenuItem.Name = "refreshToolstripMenuItem";
            resources.ApplyResources(this.refreshToolstripMenuItem, "refreshToolstripMenuItem");
            this.refreshToolstripMenuItem.Click += new System.EventHandler(this.refreshToolstripMenuItem_Click);
            // 
            // addNewAppointmentToolStripMenuItem
            // 
            this.addNewAppointmentToolStripMenuItem.Name = "addNewAppointmentToolStripMenuItem";
            resources.ApplyResources(this.addNewAppointmentToolStripMenuItem, "addNewAppointmentToolStripMenuItem");
            this.addNewAppointmentToolStripMenuItem.Click += new System.EventHandler(this.addNewAppointmentToolStripMenuItem_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "completed.ico");
            this.imageList1.Images.SetKeyName(1, "note.ico");
            // 
            // button2
            // 
            resources.ApplyResources(this.button2, "button2");
            this.button2.Name = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // vTabControlModernized1
            // 
            this.vTabControlModernized1.ContextMenuStrip = this.m_tabContextMenu;
            resources.ApplyResources(this.vTabControlModernized1, "vTabControlModernized1");
            this.vTabControlModernized1.Name = "vTabControlModernized1";
            this.vTabControlModernized1.PageBorder = Janus.Windows.UI.Tab.PageBorder.None;
            this.vTabControlModernized1.PanelFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(142)))), ((int)(((byte)(172)))));
            this.vTabControlModernized1.ShowFocusRectangle = false;
            this.vTabControlModernized1.TabPages.AddRange(new Janus.Windows.UI.Tab.UITabPage[] {
            this.uiTabPage1,
            this.uiTabPage2});
            this.vTabControlModernized1.TabsStateStyles.FormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(208)))), ((int)(((byte)(224)))));
            this.vTabControlModernized1.TabsStateStyles.FormatStyle.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.vTabControlModernized1.TabsStateStyles.FormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(142)))), ((int)(((byte)(172)))));
            this.vTabControlModernized1.TabsStateStyles.HotFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(170)))), ((int)(((byte)(195)))));
            this.vTabControlModernized1.TabsStateStyles.HotFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(64)))), ((int)(((byte)(117)))));
            this.vTabControlModernized1.TabsStateStyles.SelectedFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(142)))), ((int)(((byte)(172)))));
            this.vTabControlModernized1.TabsStateStyles.SelectedFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.vTabControlModernized1.UseThemes = false;
            // 
            // m_tabContextMenu
            // 
            this.m_tabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addViewMenuItem});
            this.m_tabContextMenu.Name = "m_tabContextMenu";
            resources.ApplyResources(this.m_tabContextMenu, "m_tabContextMenu");
            // 
            // addViewMenuItem
            // 
            this.addViewMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.agendaMenuItem,
            this.trackerMenuItem});
            this.addViewMenuItem.Name = "addViewMenuItem";
            resources.ApplyResources(this.addViewMenuItem, "addViewMenuItem");
            this.addViewMenuItem.Click += new System.EventHandler(this.addViewMenuItem_Click);
            // 
            // agendaMenuItem
            // 
            this.agendaMenuItem.Name = "agendaMenuItem";
            resources.ApplyResources(this.agendaMenuItem, "agendaMenuItem");
            this.agendaMenuItem.Click += new System.EventHandler(this.agendaMenuItem_Click);
            // 
            // trackerMenuItem
            // 
            this.trackerMenuItem.Name = "trackerMenuItem";
            resources.ApplyResources(this.trackerMenuItem, "trackerMenuItem");
            this.trackerMenuItem.Click += new System.EventHandler(this.trackerMenuItem_Click);
            // 
            // vPanel1
            // 
            this.vPanel1.Controls.Add(this.tableLayoutPanel2);
            resources.ApplyResources(this.vPanel1, "vPanel1");
            this.vPanel1.Name = "vPanel1";
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.m_schedule, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.rightTblLayoutPanel, 1, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // rightTblLayoutPanel
            // 
            resources.ApplyResources(this.rightTblLayoutPanel, "rightTblLayoutPanel");
            this.rightTblLayoutPanel.Controls.Add(this.waitListGrid, 0, 1);
            this.rightTblLayoutPanel.Controls.Add(this.vCalendar1, 0, 0);
            this.rightTblLayoutPanel.Controls.Add(this.vPanel2, 0, 2);
            this.rightTblLayoutPanel.Name = "rightTblLayoutPanel";
            // 
            // waitListGrid
            // 
            this.waitListGrid.AllowDrop = true;
            this.waitListGrid.AllowRemoveColumns = Janus.Windows.GridEX.InheritableBoolean.True;
            this.waitListGrid.AlternatingColors = true;
            this.waitListGrid.AlternatingColorsOverride = null;
            this.waitListGrid.AlternatingRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.AlternatingRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(254)))));
            this.waitListGrid.AlternatingRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            this.waitListGrid.AlternatingRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.AlternatingRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.AlternatingRowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(244)))));
            this.waitListGrid.bIsDirty = true;
            this.waitListGrid.bIsListFiltered = false;
            this.waitListGrid.bIsRequired = true;
            this.waitListGrid.BlendColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(214)))));
            this.waitListGrid.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            this.waitListGrid.bPersistFilter = false;
            this.waitListGrid.bShowColumnHeaderContextMenu = true;
            this.waitListGrid.bShowWatermark = false;
            resources.ApplyResources(this.waitListGrid, "waitListGrid");
            this.waitListGrid.bUseVarianLF = true;
            this.waitListGrid.CardCaptionFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.CardCaptionFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.waitListGrid.CardCaptionFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.CardCaptionFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.CardColumnHeaderFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.CardColumnHeaderFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.waitListGrid.CardColumnHeaderFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.CardColumnHeaderFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.ControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.FlatBorderless;
            this.waitListGrid.DefaultForeColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            resources.ApplyResources(waitListGrid_DesignTimeLayout, "waitListGrid_DesignTimeLayout");
            this.waitListGrid.DesignTimeLayout = waitListGrid_DesignTimeLayout;
            this.waitListGrid.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
            this.waitListGrid.FilterRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.FilterRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.waitListGrid.FilterRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            this.waitListGrid.FilterRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.FilterRowFormatStyle.FontSize = 9F;
            this.waitListGrid.FilterRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.FilterRowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.FilterRowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.FlatBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(168)))), ((int)(((byte)(183)))));
            this.waitListGrid.FocusCellFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.FocusCellFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.waitListGrid.FocusCellFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.FocusCellFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.waitListGrid.FocusCellFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.FocusCellFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;
            this.waitListGrid.GridLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;
            this.waitListGrid.GroupByBoxFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.GroupByBoxFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(182)))), ((int)(((byte)(201)))));
            this.waitListGrid.GroupByBoxFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.GroupByBoxFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.GroupByBoxFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.GroupByBoxFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.GroupByBoxInfoFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.GroupByBoxInfoFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(182)))), ((int)(((byte)(201)))));
            this.waitListGrid.GroupByBoxInfoFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.GroupByBoxInfoFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.GroupByBoxVisible = false;
            this.waitListGrid.GroupRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.GroupRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(182)))), ((int)(((byte)(201)))));
            this.waitListGrid.GroupRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.GroupRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.GroupTotalRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.GroupTotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(152)))), ((int)(((byte)(182)))), ((int)(((byte)(201)))));
            this.waitListGrid.GroupTotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.GroupTotalRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.GroupTotalRowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.GroupTotalRowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.HeaderFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.HeaderFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(206)))), ((int)(((byte)(214)))));
            this.waitListGrid.HeaderFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.HeaderFormatStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.waitListGrid.HeaderFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(94)))), ((int)(((byte)(130)))));
            this.waitListGrid.HeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.HeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;
            this.waitListGrid.LinkFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.LinkFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.waitListGrid.LinkFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.LinkFormatStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Underline);
            this.waitListGrid.LinkFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.LinkFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.Name = "waitListGrid";
            this.waitListGrid.NewRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.NewRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.waitListGrid.NewRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.NewRowFormatStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.waitListGrid.NewRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.NewRowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.NewRowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.NormalizeInput = true;
            this.waitListGrid.PreviewRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.PreviewRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(254)))));
            this.waitListGrid.PreviewRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.PreviewRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.PreviewRowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.PreviewRowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.RowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.RowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(244)))));
            this.waitListGrid.RowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.RowFormatStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.waitListGrid.RowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.RowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.RowHeaderFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.RowHeaderFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.waitListGrid.RowHeaderFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.RowHeaderFormatStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.waitListGrid.RowHeaderFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(94)))), ((int)(((byte)(130)))));
            this.waitListGrid.RowHeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.RowHeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.RowWithErrorsFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.RowWithErrorsFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(246)))), ((int)(((byte)(249)))), ((int)(((byte)(251)))));
            this.waitListGrid.RowWithErrorsFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Transparent;
            this.waitListGrid.RowWithErrorsFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.RowWithErrorsFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.waitListGrid.RowWithErrorsFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.RowWithErrorsFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.SelectedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.waitListGrid.SelectedFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.SelectedFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.waitListGrid.SelectedFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            this.waitListGrid.SelectedFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.SelectedFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.waitListGrid.SelectedFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.SelectedFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.SelectedInactiveFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.SelectedInactiveFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.waitListGrid.SelectedInactiveFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            this.waitListGrid.SelectedInactiveFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.SelectedInactiveFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.waitListGrid.SelectedInactiveFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.SelectedInactiveFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.strQuickLinkName = "";
            this.waitListGrid.TableHeaderFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.TableHeaderFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.waitListGrid.TableHeaderFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.TableHeaderFormatStyle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.waitListGrid.TableHeaderFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.waitListGrid.TableHeaderFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.TableHeaderFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.ThemedAreas = Janus.Windows.GridEX.ThemedArea.None;
            this.waitListGrid.TotalRowFormatStyle.Appearance = Janus.Windows.GridEX.Appearance.Flat;
            this.waitListGrid.TotalRowFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(75)))), ((int)(((byte)(133)))));
            this.waitListGrid.TotalRowFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(168)))), ((int)(((byte)(169)))), ((int)(((byte)(184)))));
            this.waitListGrid.TotalRowFormatStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.waitListGrid.TotalRowFormatStyle.LineAlignment = Janus.Windows.GridEX.TextAlignment.Center;
            this.waitListGrid.TotalRowFormatStyle.TextAlignment = Janus.Windows.GridEX.TextAlignment.Near;
            this.waitListGrid.ValidEntriesInd = "Y";
            this.waitListGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.waitListGrid_MouseMove);
            // 
            // vPanel2
            // 
            resources.ApplyResources(this.vPanel2, "vPanel2");
            this.vPanel2.Controls.Add(this.viewTypeGrpBox);
            this.vPanel2.Controls.Add(this.groupBox1);
            this.vPanel2.Name = "vPanel2";
            // 
            // viewTypeGrpBox
            // 
            this.viewTypeGrpBox.Controls.Add(this.monthRadio);
            this.viewTypeGrpBox.Controls.Add(this.workWeekRadio);
            this.viewTypeGrpBox.Controls.Add(this.dayRadio);
            resources.ApplyResources(this.viewTypeGrpBox, "viewTypeGrpBox");
            this.viewTypeGrpBox.Name = "viewTypeGrpBox";
            this.viewTypeGrpBox.TabStop = false;
            // 
            // monthRadio
            // 
            resources.ApplyResources(this.monthRadio, "monthRadio");
            this.monthRadio.Name = "monthRadio";
            this.monthRadio.UseVisualStyleBackColor = true;
            // 
            // workWeekRadio
            // 
            resources.ApplyResources(this.workWeekRadio, "workWeekRadio");
            this.workWeekRadio.Checked = true;
            this.workWeekRadio.Name = "workWeekRadio";
            this.workWeekRadio.TabStop = true;
            this.workWeekRadio.UseVisualStyleBackColor = true;
            // 
            // dayRadio
            // 
            resources.ApplyResources(this.dayRadio, "dayRadio");
            this.dayRadio.Name = "dayRadio";
            this.dayRadio.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupLikeResourcesRadio);
            this.groupBox1.Controls.Add(this.multipleResourceRadio);
            this.groupBox1.Controls.Add(this.singleResourceRadio);
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupLikeResourcesRadio
            // 
            resources.ApplyResources(this.groupLikeResourcesRadio, "groupLikeResourcesRadio");
            this.groupLikeResourcesRadio.Name = "groupLikeResourcesRadio";
            this.groupLikeResourcesRadio.UseVisualStyleBackColor = true;
            // 
            // multipleResourceRadio
            // 
            resources.ApplyResources(this.multipleResourceRadio, "multipleResourceRadio");
            this.multipleResourceRadio.Checked = true;
            this.multipleResourceRadio.Name = "multipleResourceRadio";
            this.multipleResourceRadio.TabStop = true;
            this.multipleResourceRadio.UseVisualStyleBackColor = true;
            // 
            // singleResourceRadio
            // 
            resources.ApplyResources(this.singleResourceRadio, "singleResourceRadio");
            this.singleResourceRadio.Name = "singleResourceRadio";
            this.singleResourceRadio.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gridEX1);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // gridEX1
            // 
            resources.ApplyResources(gridEX1_DesignTimeLayout, "gridEX1_DesignTimeLayout");
            this.gridEX1.DesignTimeLayout = gridEX1_DesignTimeLayout;
            resources.ApplyResources(this.gridEX1, "gridEX1");
            this.gridEX1.Name = "gridEX1";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.vTabControlModernized1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.applySettings);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // applySettings
            // 
            resources.ApplyResources(this.applySettings, "applySettings");
            this.applySettings.Name = "applySettings";
            this.applySettings.UseVisualStyleBackColor = true;
            this.applySettings.Click += new System.EventHandler(this.applySettings_Click);
            // 
            // m_TimeNavigatorContextMenu
            // 
            this.m_TimeNavigatorContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Minutes30ToolStripMenuItem,
            this.Minutes60ToolStripMenuItem2});
            this.m_TimeNavigatorContextMenu.Name = "m_ScheduleContextMenu";
            resources.ApplyResources(this.m_TimeNavigatorContextMenu, "m_TimeNavigatorContextMenu");
            // 
            // Minutes30ToolStripMenuItem
            // 
            this.Minutes30ToolStripMenuItem.Name = "Minutes30ToolStripMenuItem";
            resources.ApplyResources(this.Minutes30ToolStripMenuItem, "Minutes30ToolStripMenuItem");
            this.Minutes30ToolStripMenuItem.Click += new System.EventHandler(this.Minutes30ToolStripMenuItem_Click);
            // 
            // Minutes60ToolStripMenuItem2
            // 
            this.Minutes60ToolStripMenuItem2.Name = "Minutes60ToolStripMenuItem2";
            resources.ApplyResources(this.Minutes60ToolStripMenuItem2, "Minutes60ToolStripMenuItem2");
            this.Minutes60ToolStripMenuItem2.Click += new System.EventHandler(this.Minutes60ToolStripMenuItem2_Click);
            // 
            // uiTabPage1
            // 
            this.uiTabPage1.Controls.Add(this.vPanel1);
            resources.ApplyResources(this.uiTabPage1, "uiTabPage1");
            this.uiTabPage1.Name = "uiTabPage1";
            this.uiTabPage1.TabStop = true;
            // 
            // uiTabPage2
            // 
            this.uiTabPage2.Controls.Add(this.panel2);
            resources.ApplyResources(this.uiTabPage2, "uiTabPage2");
            this.uiTabPage2.Name = "uiTabPage2";
            this.uiTabPage2.TabStop = true;
            // 
            // m_schedule
            // 
            this.m_schedule.AddNewMode = Janus.Windows.Schedule.AddNewMode.Manual;
            this.m_schedule.AllowDelete = false;
            this.m_schedule.AllowDrop = true;
            this.m_schedule.AllowEdit = false;
            this.m_schedule.ClockStyle = Janus.Windows.Schedule.ClockStyle.None;
            this.m_schedule.ContextMenuStrip = this.m_ScheduleContextMenu;
            resources.ApplyResources(this.m_schedule, "m_schedule");
            this.m_schedule.Dates.Add(new System.DateTime(2003, 5, 12, 0, 0, 0, 0));
            this.m_schedule.Dates.Add(new System.DateTime(2003, 5, 13, 0, 0, 0, 0));
            this.m_schedule.Dates.Add(new System.DateTime(2003, 5, 14, 0, 0, 0, 0));
            this.m_schedule.Dates.Add(new System.DateTime(2003, 5, 15, 0, 0, 0, 0));
            this.m_schedule.Dates.Add(new System.DateTime(2003, 5, 16, 0, 0, 0, 0));
            this.m_schedule.DayNavigationButtons = true;
            this.m_schedule.ForeColor = System.Drawing.SystemColors.ControlText;
            this.m_schedule.HourBackgroundStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.m_schedule.ImageList = this.imageList1;
            this.m_schedule.MonthDaysFormatStyle.BackColor = System.Drawing.Color.LightBlue;
            this.m_schedule.MultiOwner = true;
            this.m_schedule.Name = "m_schedule";
            this.m_schedule.SelectedFormatStyle.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.m_schedule.SelectedInactiveFormatStyle.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.m_schedule.ShowAllDayArea = false;
            this.m_schedule.ShowTimeHintOnAppointments = Janus.Windows.Schedule.TimeHintOnAppointments.Never;
            this.m_schedule.ShowToolTips = false;
            this.m_schedule.TimeNavigatorFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(247)))), ((int)(((byte)(248)))), ((int)(((byte)(249)))));
            this.m_schedule.TimeNavigatorFormatStyle.BackColorGradient = System.Drawing.Color.FromArgb(((int)(((byte)(206)))), ((int)(((byte)(212)))), ((int)(((byte)(223)))));
            this.m_schedule.VerticalScrollPosition = 16;
            this.m_schedule.View = Janus.Windows.Schedule.ScheduleView.WorkWeek;
            this.m_schedule.VisualStyle = Janus.Windows.Schedule.VisualStyle.VS2010;
            this.m_schedule.WeekDaysBackgroundStyle.BackColor = System.Drawing.Color.White;
            this.m_schedule.WorkingHourBackgroundStyle.BackColor = System.Drawing.Color.PaleGoldenrod;
            this.m_schedule.DrawScheduleArea += new Janus.Windows.Schedule.DrawScheduleEventHandler(this.schedule_DrawScheduleArea);
            this.m_schedule.AppointmentDoubleClick += new Janus.Windows.Schedule.AppointmentEventHandler(this.m_schedule_AppointmentDoubleClick);
            this.m_schedule.DroppingAppointment += new Janus.Windows.Schedule.DroppingAppointmentEventHandler(this.m_schedule_DroppingAppointment);
            this.m_schedule.AppointmentDrag += new Janus.Windows.Schedule.AppointmentDragEventHandler(this.m_schedule_AppointmentDrag);
            this.m_schedule.Click += new System.EventHandler(this.m_schedule_Click);
            this.m_schedule.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_schedule_DragDrop);
            this.m_schedule.DragEnter += new System.Windows.Forms.DragEventHandler(this.m_schedule_DragEnter);
            this.m_schedule.DragOver += new System.Windows.Forms.DragEventHandler(this.m_schedule_DragOver);
            this.m_schedule.DragLeave += new System.EventHandler(this.m_schedule_DragLeave);
            this.m_schedule.DoubleClick += new System.EventHandler(this.m_schedule_DoubleClick);
            this.m_schedule.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_schedule_KeyDown);
            this.m_schedule.KeyUp += new System.Windows.Forms.KeyEventHandler(this.m_schedule_KeyUp);
            this.m_schedule.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_schedule_MouseDown);
            this.m_schedule.MouseMove += new System.Windows.Forms.MouseEventHandler(this.m_schedule_MouseMove);
            // 
            // vCalendar1
            // 
            this.vCalendar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.vCalendar1, "vCalendar1");
            this.vCalendar1.HeadersFormatStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(95)))), ((int)(((byte)(170)))));
            this.vCalendar1.HeadersFormatStyle.Blend = 0.5F;
            this.vCalendar1.HeadersFormatStyle.BlendGradient = 0.5F;
            this.vCalendar1.Name = "vCalendar1";
            this.vCalendar1.Schedule = this.m_schedule;
            this.vCalendar1.UseThemes = false;
            this.vCalendar1.DatesChanged += new System.EventHandler(this.vCalendar1_DatesChanged);
            this.vCalendar1.SelectionChanged += new System.EventHandler(this.vCalendar1_SelectionChanged);
            // 
            // SchedulerTestForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "SchedulerTestForm";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.m_ScheduleContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.vTabControlModernized1)).EndInit();
            this.vTabControlModernized1.ResumeLayout(false);
            this.m_tabContextMenu.ResumeLayout(false);
            this.vPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.rightTblLayoutPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.waitListGrid)).EndInit();
            this.vPanel2.ResumeLayout(false);
            this.viewTypeGrpBox.ResumeLayout(false);
            this.viewTypeGrpBox.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridEX1)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.m_TimeNavigatorContextMenu.ResumeLayout(false);
            this.uiTabPage1.ResumeLayout(false);
            this.uiTabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_schedule)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.vCalendar1)).EndInit();
            this.ResumeLayout(false);

        }
        
        #endregion

        private VJanusSchedule m_schedule;
        private System.Windows.Forms.Button button2;
        private Calendar vCalendar1;
        private System.Windows.Forms.ImageList imageList1;
        private UITab vTabControlModernized1;
        private Janus.Windows.UI.Tab.UITabPage uiTabPage1;
        private System.Windows.Forms.Panel vPanel1;
        private System.Windows.Forms.ContextMenuStrip m_ScheduleContextMenu;
        private System.Windows.Forms.ToolStripMenuItem refreshToolstripMenuItem;
        private System.Windows.Forms.ContextMenuStrip m_tabContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addViewMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.ToolStripMenuItem agendaMenuItem;
        private System.Windows.Forms.ToolStripMenuItem trackerMenuItem;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripMenuItem addNewAppointmentToolStripMenuItem;
        private UITabPage uiTabPage2;
        private System.Windows.Forms.Panel panel2;
        private Janus.Windows.GridEX.GridEX gridEX1;
        private System.Windows.Forms.TableLayoutPanel rightTblLayoutPanel;
        private VJanusDataGridModernized waitListGrid;
        private Panel vPanel2;
        private Button applySettings;
        private System.Windows.Forms.GroupBox viewTypeGrpBox;
        private System.Windows.Forms.RadioButton monthRadio;
        private System.Windows.Forms.RadioButton workWeekRadio;
        private System.Windows.Forms.RadioButton dayRadio;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton groupLikeResourcesRadio;
        private System.Windows.Forms.RadioButton multipleResourceRadio;
        private System.Windows.Forms.RadioButton singleResourceRadio;
        private System.Windows.Forms.ContextMenuStrip m_TimeNavigatorContextMenu;
        private System.Windows.Forms.ToolStripMenuItem Minutes30ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem Minutes60ToolStripMenuItem2;
    }
}


