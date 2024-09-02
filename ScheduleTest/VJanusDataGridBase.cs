using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Interop;
using System.Xml.Serialization;
using Janus.Windows.GridEX;


namespace ScheduleTest
{
    public abstract class VJanusDataGridBase : GridEX
    {
        #region Members
        protected Janus.Windows.GridEX.GridEXColumn m_selectedColumn;
        protected Janus.Windows.GridEX.GridEXTable m_selectedTable = null;

        // Context menu for column header
        protected System.Windows.Forms.ContextMenu m_cmColumnHeader;
        protected System.Windows.Forms.MenuItem m_menuItemSortAsc;
        protected System.Windows.Forms.MenuItem m_menuItemSortDesc;
        protected System.Windows.Forms.MenuItem m_menuItemRemoveAllSorts;
        protected System.Windows.Forms.MenuItem m_separator1;
        protected System.Windows.Forms.MenuItem m_menuItemShowHideFilterRow;
        protected System.Windows.Forms.MenuItem m_menuItemRemoveAllFilters;
        protected System.Windows.Forms.MenuItem m_separator2;
        protected System.Windows.Forms.MenuItem m_menuItemHideThisField;
        protected System.Windows.Forms.MenuItem m_menuItemShowFieldChooser;
        protected System.Windows.Forms.MenuItem m_separator3;
        protected System.Windows.Forms.MenuItem m_menuItemColumnAutoResize;
        protected System.Windows.Forms.MenuItem m_menuItemResizeColumns;
        protected System.Windows.Forms.MenuItem m_menuItemFreezeColumn;
        protected System.Windows.Forms.MenuItem m_menuItemUnFreezeAllColumns;
        protected System.Windows.Forms.MenuItem m_separator4;
        protected System.Windows.Forms.MenuItem m_menuItemExport;
        protected System.Windows.Forms.MenuItem m_separator5;
        protected System.Windows.Forms.MenuItem m_menuItemPrint;


        private bool m_bShowColumnHeaderContextMenu = true;
        private bool m_bShowWatermark = false;
        private bool m_bIsListFiltered = false;
        //VA00131700 Rolandb Oct 27/15 Janus Grid 4.0 selection row fix
        protected bool m_bApplyingFilter = false;
        private bool m_bUseVarianLF = true;
        private bool m_bIsDirty = true;
        private bool m_bIsRequired = true;
        private string m_strFilterShowTitle = "Show Data Filter";
        private string m_strFilterHideTitle = "Hide Data Filter";

        #region Filtering issue workaround
        // The following member variables are used to temporarily store state information when 
        // one of the modified accessors is called. These were introduced as a work around
        // to issues seen when working with filtered lists. See VA00086792, VA00098242, VA00098244,
        // VA00098245, VA00098246, VA00098247, VA00098248, VA00098249, VA00098250.
        GridEXFilterCondition m_filterCondition;
        #endregion

        private Form m_parentForm = null;

        private Janus.Windows.GridEX.GridEXPrintDocument m_gridEXPrintDocument;
        //private PrintPreviewDialog m_dlgPrintPreview = null;
        private bool bIsFirstTimeLoad = false;
        private bool m_bPersistFilter = false;
        private bool m_normalizeInput = true;
        private int m_iVisibleColumnNum = 0;
        protected Janus.Windows.GridEX.GridEXColumn m_dragColumn = null;

        public delegate void CurrentRowChangingEventHandler( object sender, CurrentRowChangingEventArgs e );

        [Browsable( true )]
        public event CurrentRowChangingEventHandler CurrentRowChanging;
        public event Action DefaultSortingIcons;
        #endregion

        #region Win32 dll functions
        /// <summary>
        /// Call SetWindowLong function for 32-bit process
        /// </summary>
        /// <param name="hWndChild">Child window handle</param>
        /// <param name="nIndex"></param>
        /// <param name="hWndNewParent">Handle of new parent window</param>
        /// <returns>Returns IntPtr</returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLong")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "return", Justification = "This declaration is not used on 64-bit Windows.")]
        [SuppressMessage("Microsoft.Portability", "CA1901:PInvokeDeclarationsShouldBePortable", MessageId = "2", Justification = "This declaration is not used on 64-bit Windows.")]
        private static extern IntPtr SetWindowLongPtr32(IntPtr hWndChild, int nIndex, IntPtr hWndNewParent);

        /// <summary>
        /// Call SetWindowLongPtr function for 64-bit process which exists for 64-bit
        /// </summary>
        /// <param name="hWndChild">Child window handle</param>
        /// <param name="nIndex"></param>
        /// <param name="hWndNewParent">Handle of new parent window</param>
        /// <returns>Returns IntPtr</returns>
        [DllImport("user32.dll", SetLastError = true, EntryPoint = "SetWindowLongPtr")]
        [SuppressMessage("Microsoft.Interoperability", "CA1400:PInvokeEntryPointsShouldExist", Justification = "Entry point does exist on 64-bit Windows.")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWndChild, int nIndex, IntPtr hWndNewParent);

        private static IntPtr SetWindowLong(IntPtr hWndChild, int nIndex, IntPtr hWndNewParent)
        {
            //check if the process is 32-bit then call the 32-bit version
            //refer https://msdn.microsoft.com/en-us/library/windows/desktop/ms633591(v=vs.85).aspx
            // https://msdn.microsoft.com/en-us/library/windows/desktop/ms644898(v=vs.85).aspx
            // the function SetWindowLongPtr only works with C++, for C# we have to explicitly call the respective methods refer https://www.jmedved.com/2013/07/setwindowlongptr/
            if (IntPtr.Size == 4)
            {
                //call to 32-bit api
                return SetWindowLongPtr32(hWndChild, nIndex, hWndNewParent);
            }
            else
            {
                //call to 64-bit api
                return SetWindowLongPtr64(hWndChild, nIndex, hWndNewParent);
            }
        }
        #endregion

        #region Construction/Destruction

        public VJanusDataGridBase()
        {
            this.InitializeComponent();

            this.m_selectedTable = this.RootTable;

            // Fix: VA00026251 Context Menu in CarePath Column header always showed Show/Hide Filter
            //VA00026231 7.0.26 fix
            GetLocalizedText();

            // ZachD - 1/10/2011 - VA0054984 - Allow dropping columns off grid (and into field chooser):
            SetAllowRemoveColumns();
        }

        // Fix: VA00026251 Context Menu in CarePath Column header always showed Show/Hide Filter
        //VA00026231 7.0.26 fix
        private void GetLocalizedText()
        {
            m_strFilterShowTitle = VFormResource.strShowDataFilter;
            m_strFilterHideTitle = VFormResource.strHideDataFilter;

            this.m_menuItemShowHideFilterRow.Text = m_strFilterHideTitle;
            this.m_menuItemSortAsc.Text = VFormResource.strSortAscending;
            this.m_menuItemSortDesc.Text = VFormResource.strSortDescending;
            this.m_menuItemRemoveAllSorts.Text = VFormResource.strNoSort;
            this.m_menuItemRemoveAllFilters.Text = VFormResource.strNoFilter;
            this.m_menuItemHideThisField.Text = VFormResource.strHideThisColumn;
            this.m_menuItemShowFieldChooser.Text = VFormResource.strShowColumnChooser;
            this.m_menuItemColumnAutoResize.Text = VFormResource.strColumnAutoResize;
            this.m_menuItemResizeColumns.Text = VFormResource.strResizeColumns;
            this.m_menuItemFreezeColumn.Text = VFormResource.strFreezeColumn;
            this.m_menuItemUnFreezeAllColumns.Text = VFormResource.strUnFreezeAllColumns;
            this.m_menuItemExport.Text = VFormResource.strExport;
            this.m_menuItemPrint.Text = VFormResource.strPrint;
            this.BuiltInTexts[ GridEXBuiltInText.FilterRowInfoText ] = VFormResource.strFilterRowInfoText;
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose( bool disposing )
        {
            if( disposing )
            {
                // Clean up any system resources used
                //if (this.m_layoutStream!=null)
                //{
                //  this.m_layoutStream.Close();
                //}
            }
            base.Dispose( disposing );
        }

        #region Component Initialization
        private void InitializeComponent()
        {
            this.m_cmColumnHeader = new ContextMenu();
            this.m_menuItemSortAsc = new System.Windows.Forms.MenuItem();
            this.m_menuItemSortDesc = new System.Windows.Forms.MenuItem();
            this.m_menuItemRemoveAllSorts = new System.Windows.Forms.MenuItem();
            this.m_separator1 = new System.Windows.Forms.MenuItem();
            this.m_menuItemShowHideFilterRow = new System.Windows.Forms.MenuItem();
            this.m_menuItemRemoveAllFilters = new System.Windows.Forms.MenuItem();
            this.m_separator2 = new System.Windows.Forms.MenuItem();
            this.m_menuItemHideThisField = new System.Windows.Forms.MenuItem();
            this.m_menuItemShowFieldChooser = new System.Windows.Forms.MenuItem();
            this.m_separator3 = new System.Windows.Forms.MenuItem();
            this.m_menuItemColumnAutoResize = new System.Windows.Forms.MenuItem();
            this.m_menuItemResizeColumns = new System.Windows.Forms.MenuItem();
            this.m_menuItemFreezeColumn = new System.Windows.Forms.MenuItem();
            this.m_menuItemUnFreezeAllColumns = new System.Windows.Forms.MenuItem();
            this.m_separator4 = new System.Windows.Forms.MenuItem();
            this.m_menuItemExport = new System.Windows.Forms.MenuItem();
            this.m_separator5 = new System.Windows.Forms.MenuItem();
            this.m_menuItemPrint = new System.Windows.Forms.MenuItem();
            this.m_gridEXPrintDocument = new Janus.Windows.GridEX.GridEXPrintDocument();
            ( ( System.ComponentModel.ISupportInitialize )( this ) ).BeginInit();
            this.SuspendLayout();
            // 
            // m_cmColumnHeader
            // 
            this.m_cmColumnHeader.MenuItems.AddRange( new System.Windows.Forms.MenuItem[] {
            this.m_menuItemSortAsc,
            this.m_menuItemSortDesc,
            this.m_menuItemRemoveAllSorts,
            this.m_separator1,
            this.m_menuItemShowHideFilterRow,
            this.m_menuItemRemoveAllFilters,
            this.m_separator2,
            this.m_menuItemHideThisField,
            this.m_menuItemShowFieldChooser,
            this.m_separator3,
            this.m_menuItemColumnAutoResize,
            this.m_menuItemResizeColumns,
            this.m_menuItemFreezeColumn,
            this.m_menuItemUnFreezeAllColumns,
            this.m_separator4,
            this.m_menuItemExport,
            this.m_separator5,
            this.m_menuItemPrint} );
            // 
            // m_menuItemSortAsc
            // 
            this.m_menuItemSortAsc.Index = 0;
            this.m_menuItemSortAsc.Text = "Sort Ascending";
            this.m_menuItemSortAsc.Click += new System.EventHandler( this.OnSortAscending );
            // 
            // m_menuItemSortDesc
            // 
            this.m_menuItemSortDesc.Index = 1;
            this.m_menuItemSortDesc.Text = "Sort Descending";
            this.m_menuItemSortDesc.Click += new System.EventHandler( this.OnSortDescending );
            // 
            // m_menuItemRemoveAllSorts
            // 
            this.m_menuItemRemoveAllSorts.Index = 2;
            this.m_menuItemRemoveAllSorts.Text = "No Sort";
            this.m_menuItemRemoveAllSorts.Click += new System.EventHandler( this.OnNoSorts );
            // 
            // m_separator1
            // 
            this.m_separator1.Index = 3;
            this.m_separator1.Text = "-";
            // 
            // m_menuItemShowHideFilterRow
            // 
            this.m_menuItemShowHideFilterRow.Index = 4;
            this.m_menuItemShowHideFilterRow.Text = "Hide Data Filter";
            this.m_menuItemShowHideFilterRow.Click += new System.EventHandler( this.OnShowHideDataFilterRow );
            // 
            // m_menuItemRemoveAllFilters
            // 
            this.m_menuItemRemoveAllFilters.Index = 5;
            this.m_menuItemRemoveAllFilters.Text = "No Filter";
            this.m_menuItemRemoveAllFilters.Click += new System.EventHandler( this.OnNoFilters );
            // 
            // m_separator2
            // 
            this.m_separator2.Index = 6;
            this.m_separator2.Text = "-";
            // 
            // m_menuItemHideThisField
            // 
            this.m_menuItemHideThisField.Index = 7;
            this.m_menuItemHideThisField.Text = "Hide This Column";
            this.m_menuItemHideThisField.Click += new System.EventHandler( this.OnHideThisField );
            // 
            // m_menuItemShowFieldChooser
            // 
            this.m_menuItemShowFieldChooser.Index = 8;
            this.m_menuItemShowFieldChooser.Text = "Show Column Chooser...";
            this.m_menuItemShowFieldChooser.Click += new System.EventHandler( this.OnShowFieldChooser );
            // 
            // m_separator3
            // 
            this.m_separator3.Index = 9;
            this.m_separator3.Text = "-";
            // 
            // m_menuItemColumnAutoResize
            // 
            this.m_menuItemColumnAutoResize.Index = 10;
            this.m_menuItemColumnAutoResize.Text = "Columns AutoResize";
            this.m_menuItemColumnAutoResize.Click += new System.EventHandler( this.OnColumnAutoResize );
            // 
            // m_menuItemResizeColumns
            // 
            this.m_menuItemResizeColumns.Index = 11;
            this.m_menuItemResizeColumns.Text = "Resize Columns";
            this.m_menuItemResizeColumns.Click += new System.EventHandler( this.OnResizeColumns );
            // 
            // m_menuItemFreezeColumn
            // 
            this.m_menuItemFreezeColumn.Index = 12;
            this.m_menuItemFreezeColumn.Text = "Freeze Column";
            this.m_menuItemFreezeColumn.Click += new System.EventHandler( this.OnFreezeColumn );
            // 
            // m_menuItemUnFreezeAllColumns
            // 
            this.m_menuItemUnFreezeAllColumns.Index = 13;
            this.m_menuItemUnFreezeAllColumns.Text = "UnFreeze All Columns";
            this.m_menuItemUnFreezeAllColumns.Click += new System.EventHandler( this.OnUnFreezeAllColumns );
            // 
            // m_separator4
            // 
            this.m_separator4.Index = 14;
            this.m_separator4.Text = "-";
            // 
            // m_menuItemExport
            // 
            this.m_menuItemExport.Index = 15;
            this.m_menuItemExport.Text = "Export";
            this.m_menuItemExport.Click += new System.EventHandler(this.OnExportClicked);
            // 
            // m_separator5
            // 
            this.m_separator5.Index = 16;
            this.m_separator5.Text = "-";
            // 
            // m_menuItemPrint
            // 
            this.m_menuItemPrint.Index = 17;
            this.m_menuItemPrint.Text = "Print";
            this.m_menuItemPrint.Click += new System.EventHandler( this.OnPrintClicked );
            // 
            // m_gridEXPrintDocument
            // 
            this.m_gridEXPrintDocument.GridEX = this;
            // 
            // VJanusDataGrid
            // 
            this.BuiltInTextsData = "<LocalizableData ID=\"LocalizableStrings\" Collection=\"true\"><FilterRowInfoText>Cli" +
    "ck here to enter filter criteria</FilterRowInfoText></LocalizableData>";
            this.RowCountChanged += new System.EventHandler( this.OnRowCountChanged );
            this.FormattingRow += new Janus.Windows.GridEX.RowLoadEventHandler( this.VJanusDataGrid_FormattingRow );
            this.ClearFilterButtonClick += new Janus.Windows.GridEX.ColumnActionEventHandler( this.OnClearFilterButtonClick );
            this.FilterApplied += new System.EventHandler( this.OnFilterApplied );
            this.VisibleChanged += new System.EventHandler( this.OnVisibleChanged );
            this.SelectionChanged += new System.EventHandler(this.VJanusDataGridBase_SelectionChanged);
            this.Leave += new System.EventHandler( this.VJanusDataGrid_Leave );
            this.MouseDown += new System.Windows.Forms.MouseEventHandler( this.OnMouseDown );
            this.MouseMove += new MouseEventHandler( VJanusDataGrid_MouseMove );
            this.MouseUp += new MouseEventHandler( VJanusDataGridBase_MouseUp );
            this.Resize += new System.EventHandler( this.VJanusDataGrid_Resize );
            ( ( System.ComponentModel.ISupportInitialize )( this ) ).EndInit();
            this.ResumeLayout( false );
            this.GridButtonClicked += VJanusDataGridBase_GridButtonClicked;

        }

        // Place code into another mouse up event.
        //protected virtual void VJanusDataGrid_MouseUp( object sender, MouseEventArgs e )
        //{
        //    if( e.Button == MouseButtons.Left )
        //    {
        //        // No way to get the header context menu if not columns are visible. 
        //        // Therefore, keep at least one column visible.
        //        if( this.m_iVisibleColumnNum == 1 )
        //            this.m_dragColumn.Visible = true;
        //    }
        //}

        protected virtual void VJanusDataGrid_MouseMove( object sender, MouseEventArgs e )
        {
            if( e.Button == MouseButtons.Left )
            {
                // No way to get the header context menu if not columns are visible. 
                // Therefore, keep at least one column visible.
                if( this.m_iVisibleColumnNum == 1 )
                    this.m_dragColumn.Visible = true;
            }
        }
        #endregion

        #endregion

        protected override void OnCreateControl()
        {
            base.OnCreateControl();
        }

        protected override void OnCurrentCellChanging(CurrentCellChangingEventArgs e)
        {
            base.OnCurrentCellChanging(e);

            if (e.Cancel == true)
            {
                return;
            }

            // Prevent the user from leaving the current row until the current row is completed (all required fields filled etc.)
            if (e != null & e.Row != null && !Equals(e.Row, CurrentRow) && CurrentRowChanging != null)
            {
                CurrentRowChangingEventArgs args = new CurrentRowChangingEventArgs(e.Row, e.Column);
                CurrentRowChanging(this, args);

                if (args.Cancel)
                {
                    e.Cancel = true;
                }
            }
        }
        //xiulid    Aug 21, 2012 VA00087840 - Column chooser is disappearing
        protected override void OnDraggingColumn( ColumnActionCancelEventArgs e )
        {
            base.OnDraggingColumn( e );
            //int nVisibleColumnCount = 0;
            this.m_iVisibleColumnNum = 0;
            this.m_selectedTable = this.CurrentTable;
            if( this.m_selectedTable != null )
            {
                foreach( GridEXColumn gCol in this.m_selectedTable.Columns )
                {
                    // Count the number of visible columns
                    if( gCol != null && gCol.Visible )
                    {
                        this.m_iVisibleColumnNum++;
                        this.m_dragColumn = gCol;
                    }
                }
            }

        }

        /// <summary>
        /// Will make sure that the control is greyed out if it is disabled.
        /// </summary>
        protected override void OnEnabledChanged( EventArgs e )
        {
            base.OnEnabledChanged( e );
        }
        #region Context Menu for Columns
        protected virtual void OnMouseDown( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            if( m_bShowColumnHeaderContextMenu )
            {
                if( e.Button == MouseButtons.Right )
                {
                    if( this.HitTest( e.X, e.Y ) == Janus.Windows.GridEX.GridArea.ColumnHeader )
                    {
                        m_selectedTable = this.ColumnFromPoint( e.X, e.Y ).Table;
                        m_selectedColumn = this.ColumnFromPoint( e.X, e.Y );
                        m_menuItemSortAsc.Checked = ( m_selectedColumn.SortOrder == Janus.Windows.GridEX.SortOrder.Ascending );
                        m_menuItemSortDesc.Checked = ( m_selectedColumn.SortOrder == Janus.Windows.GridEX.SortOrder.Descending );

                        UpdateContextMenu();
                        m_cmColumnHeader.Show( this, new System.Drawing.Point( e.X, e.Y ) );
                    }
                }
            }
        }

        private void OnSortAscending( object sender, System.EventArgs e )
        {
            // If Sort by Column (click) is not allowed, then even disable the Column Header Sorting
            if( !this.AutomaticSort )
                return;

            m_menuItemSortAsc.Checked = !m_menuItemSortAsc.Checked;

            bool sortChanged = false;
            if( m_menuItemSortAsc.Checked )
            {
                if( m_selectedColumn.IsSorted )
                {
                    m_selectedColumn.SortKey.SortOrder = Janus.Windows.GridEX.SortOrder.Ascending;
                    sortChanged = true;
                }
                else
                {
                    this.m_selectedTable.SortKeys.Add( new GridEXSortKey(
                      m_selectedColumn,
                      Janus.Windows.GridEX.SortOrder.Ascending ) );
                    sortChanged = true;
                }
            }
            else
            {
                if( m_selectedColumn.IsSorted &&
                  m_selectedColumn.SortKey.SortOrder ==
                  Janus.Windows.GridEX.SortOrder.Ascending )
                {
                    this.m_selectedTable.SortKeys.Remove( m_selectedColumn.SortKey );
                    sortChanged = true;
                }
            }
            if( sortChanged )
            {
                OnSortKeysChanged( EventArgs.Empty );
            }
        }

        private void OnSortDescending( object sender, System.EventArgs e )
        {
            // If Sort by Column (click) is not allowed, then even disable the Column Header Sorting
            if( !this.AutomaticSort )
                return;

            m_menuItemSortDesc.Checked = !m_menuItemSortDesc.Checked;

            bool sortChanged = false;
            if( m_menuItemSortDesc.Checked )
            {
                if( m_selectedColumn.IsSorted )
                {
                    m_selectedColumn.SortKey.SortOrder = Janus.Windows.GridEX.SortOrder.Descending;
                    sortChanged = true;
                }
                else
                {
                    this.m_selectedTable.SortKeys.Add( new GridEXSortKey(
                      m_selectedColumn,
                      Janus.Windows.GridEX.SortOrder.Descending ) );
                    sortChanged = true;
                }
            }
            else
            {
                if( m_selectedColumn.IsSorted &&
                  m_selectedColumn.SortKey.SortOrder ==
                  Janus.Windows.GridEX.SortOrder.Descending )
                {
                    this.m_selectedTable.SortKeys.Remove( m_selectedColumn.SortKey );
                    sortChanged = true;
                }
            }
            if( sortChanged )
            {
                OnSortKeysChanged( EventArgs.Empty );
            }
        }

        private void OnNoSorts( object sender, System.EventArgs e )
        {
            this.ResetAllTableSortKeys();
            OnSortKeysChanged( EventArgs.Empty );
            DefaultSortingIcons?.Invoke();
        }

        private void OnShowHideDataFilterRow( object sender, System.EventArgs e )
        {
            if( this.FilterMode == Janus.Windows.GridEX.FilterMode.None )
            {
                this.FilterMode = Janus.Windows.GridEX.FilterMode.Automatic;
                // Fix: VA00026251 Context Menu in CarePath Column header always showed Show/Hide Filter
                //VA00026231 7.0.26 fix
                this.m_menuItemShowHideFilterRow.Text = m_strFilterHideTitle;

                // ZachD - Sep 08/09 [VA00049863] Should populate the filter row as well, when it is shown.
                this.SetColumnFilters();
            }
            else
            {
                this.FilterMode = Janus.Windows.GridEX.FilterMode.None;
                // Fix: VA00026251 Context Menu in CarePath Column header always showed Show/Hide Filter
                //VA00026231 7.0.26 fix
                this.m_menuItemShowHideFilterRow.Text = m_strFilterShowTitle;
            }
        }
        private void OnNoFilters( object sender, System.EventArgs e )
        {
            RemoveAllFilters();
        }

        public void RemoveAllFilters()
        {
            this.UpdateData(); // Force update of datasource, this action was wiping out users unsaved changes.
            this.RemoveFilters();
            // TODO: To be removed later when groupwise expansion/collapse is supported
            this.ExpandGroups();
        }

        private void OnHideThisField( object sender, System.EventArgs e )
        {
            this.HideColumn( m_selectedColumn.Key );
        }

        private void OnShowFieldChooser( object sender, System.EventArgs e )
        {
            // if the field chooser is already visible -> then hide it first (in case it is behind something)
            //if (this.IsFieldChooserVisible()) 
            //{
            //    this.HideFieldChooser();
            //}

            GridEXFieldChooser gridExFieldChooser = null;
            var isTopMost = false;

            // now show the field chooser
            if( m_parentForm != null )
            {
                gridExFieldChooser = this.ShowFieldChooser(m_parentForm, VFormResource.strFieldChooserTitle);
                isTopMost = m_parentForm.TopMost;
            }
            else
            {
                //VA00057807 - erins - ensure that the field chooser does not hide behind the form
                //this.ShowFieldChooser(null, VFormResource.GetString("strFieldChooserTitle"));
                //ryans Mar 11/13 RT summary enhancement
                //top level control may be null if embedded in PowerBuilder. In that case,
                //Janus does not support window handles so null is the only option left.
                var parentForm = this.GetTopLevelWindowsFormsControl() as Form;
                if( parentForm != null )
                {
                    gridExFieldChooser = this.ShowFieldChooser(parentForm, VFormResource.strFieldChooserTitle);
                    isTopMost = parentForm.TopMost;
                }
                else
                {
                    // If this is a WPF application...
                    
                    gridExFieldChooser = this.ShowFieldChooser(null, VFormResource.strFieldChooserTitle);
                    System.Windows.Window wpfWindow = System.Windows.Application.Current.MainWindow;
                    if (wpfWindow != null)
                    {
                        WindowInteropHelper helper = new WindowInteropHelper(wpfWindow);
                        SetWindowLong(gridExFieldChooser.Handle, -8, helper.Handle);
                    }
                }
            }

            //check if the GridExFieldChooser dialog is not null, then bring it to the front on UI
            //TFS Bug #121750 - the column chooser dialog remains hidden behind the form. Fixed issue.
            if (gridExFieldChooser != null)
            {
                //Make the dialog border style resizable. TFS #123474
                gridExFieldChooser.FormBorderStyle = FormBorderStyle.SizableToolWindow;

                if (isTopMost)
                {
                    gridExFieldChooser.TopMost = isTopMost;
                }
            }
        }

        /// <summary>
        /// Called from OnMouseUp event above (on Column Headers)
        /// </summary>
        private void UpdateContextMenu()
        {
            m_menuItemColumnAutoResize.Checked = this.ColumnAutoResize;
            m_menuItemResizeColumns.Checked = !this.ColumnAutoResize;

            if( this.FilterMode == Janus.Windows.GridEX.FilterMode.None )
            {
                // Fix: VA00026251 Context Menu in CarePath Column header always showed Show/Hide Filter
                //VA00026231 7.0.26 fix
                this.m_menuItemShowHideFilterRow.Text = m_strFilterShowTitle;
            }
            else
            {
                // Fix: VA00026251 Context Menu in CarePath Column header always showed Show/Hide Filter
                //VA00026231 7.0.26 fix
                this.m_menuItemShowHideFilterRow.Text = m_strFilterHideTitle;
            }

            if( m_selectedColumn.Position <= this.FrozenColumns )
            {
                m_menuItemFreezeColumn.Checked = true;
            }
            else
            {
                m_menuItemFreezeColumn.Checked = false;
            }
            if( this.FrozenColumns == -1 )
            {
                m_menuItemUnFreezeAllColumns.Enabled = false;
            }
            else
            {
                m_menuItemUnFreezeAllColumns.Enabled = true;
            }

            // llayne - Dec. 08/09 - VA00058509 (Aria 10.0) or VA00063423 (Aria 11.0)
            //                     - disable HideThisField menu item in the context menu when columnSet is used
            if( m_selectedTable.CellLayoutMode == CellLayoutMode.UseColumnSets )
            {
                this.m_menuItemHideThisField.Enabled = false;
            }
        }

        #region GetSelectedDataRow
        /// <summary>
        /// Gets the selected row from the datasource.
        /// </summary>
        /// <returns></returns>
        public DataRow GetSelectedDataRow()
        {
            if( this.IsDataRowSelected && this.SelectedItems.Count == 1 )
            {
                GridEXRow dgRow = this.SelectedItems[ 0 ].GetRow();

                DataRowView drv = dgRow.DataRow as DataRowView;
                if( drv != null )
                {
                    return drv.Row;
                }
            }

            return null;
        }
        #endregion

        #region SetSelectedDataRow
        /// <summary>
        /// Sets the selected row using the underlying datarow.
        /// </summary>
        /// <returns></returns>
        public void SetSelectedDataRow( DataRow dataRow )
        {
            GridEXRow dgRow = this.GetRow( dataRow );
            if( dgRow != null )
            {
                this.SelectedItems.Clear();
                this.SelectedItems.Add( dgRow.Position );

                this.Row = dgRow.Position;
            }
        }
        #endregion

        #region SetSelectedDataRows
        /// <summary>
        /// Sets the selected rows using the underlying datarows.
        /// </summary>
        /// <returns></returns>
        public void SetSelectedDataRows( DataRow[] dataRows )
        {
            if( dataRows != null )
            {
                int nRowsSelected = 0;

                foreach( DataRow dataRow in dataRows )
                {
                    GridEXRow dgRow = this.GetRow( dataRow );
                    if( dgRow != null )
                    {
                        if( nRowsSelected == 0 )
                        {
                            this.Row = dgRow.Position;
                            this.SelectedItems.Clear();
                        }

                        this.SelectedItems.Add( dgRow.Position );

                        nRowsSelected++;
                    }
                }

                if( nRowsSelected == 0 )
                {
                    this.Row = GridEX.emptyCurrentRow;
                    this.SelectedItems.Clear();
                }
            }
        }
        #endregion

        #region GetSelectedDataRows
        /// <summary>
        /// Gets all selected rows from the datasource.
        /// </summary>
        /// <returns></returns>
        public T[] GetSelectedDataRows<T>() where T : DataRow
        {
            List<T> selectedRows = new List<T>();

            if( this.SelectedItems.Count > 0 )
            {
                foreach( GridEXSelectedItem selItem in this.SelectedItems )
                {
                    GridEXRow dgRow = this.GetRow( selItem.Position );

                    DataRowView drv = dgRow.DataRow as DataRowView;
                    if( drv != null )
                    {
                        T dataRow = drv.Row as T;
                        if( dataRow != null )
                        {
                            selectedRows.Add( dataRow );
                        }
                    }
                }
            }

            return selectedRows.ToArray();
        }
        #endregion

        #region SelectFirstMatchingRow
        /// <summary>
        /// Selects a row based on the value of one column.
        /// </summary>
        /// <param name="strColumnName"></param>
        /// <param name="value"></param>
        public bool SelectFirstMatchingRow( string strDataSetColumnName, object value )
        {
            GridEXRow dgSelectedRow = null;

            GridEXRow[] firstLevelRows = this.GetRows();
            if( firstLevelRows != null && firstLevelRows.Length > 0 )
            {
                dgSelectedRow = GetFirstMatchingRow( firstLevelRows, strDataSetColumnName, value );
            }

            if( dgSelectedRow != null )
            {
                this.Row = dgSelectedRow.Position;

                this.SelectedItems.Clear();
                this.SelectedItems.Add( dgSelectedRow.Position );

                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks and array of rows recursively for a matching column on the underlying datarow.
        /// </summary>
        private GridEXRow GetFirstMatchingRow( GridEXRow[] rows, string strDataSetColumnName, object value )
        {
            GridEXRow dgSelectedRow = null;

            foreach( GridEXRow dgRow in rows )
            {
                // Check the underlying data row for the value:
                DataRowView drv = dgRow.DataRow as DataRowView;
                if( drv != null )
                {
                    DataRow dr = drv.Row;

                    Debug.Assert( dr.Table.Columns.Contains( strDataSetColumnName ), string.Format( "Underlying data does not contain the column '{0}'.", strDataSetColumnName ) );

                    // Value is null and cell is null or DBNull?
                    if( value == null && ( dr.IsNull( strDataSetColumnName ) || dr[ strDataSetColumnName ] == null ) )
                    {
                        dgSelectedRow = dgRow;
                        break;
                    }

                    // Value is not null and cell value is equal?
                    if( value != null
                        && !dr.IsNull( strDataSetColumnName )
                        && dr[ strDataSetColumnName ] != null
                        && value.Equals( dr[ strDataSetColumnName ] )
                        )
                    {
                        dgSelectedRow = dgRow;
                        break;
                    }
                }

                // Check child rows recursively:
                GridEXRow[] childRows = dgRow.GetChildRows();
                if( childRows != null && childRows.Length > 0 )
                {
                    dgSelectedRow = GetFirstMatchingRow( childRows, strDataSetColumnName, value );

                    if( dgSelectedRow != null )
                    {
                        break;
                    }
                }
            }

            return dgSelectedRow;
        }

        #endregion

        #region SelectAllDataRows
        /// <summary>
        /// Selects all data rows.
        /// </summary>
        public void SelectAllDataRows()
        {
            this.SelectedItems.Clear();

            this.SelectAllDataRows( this.GetRows() );
        }

        /// <summary>
        /// Selects all data rows (recursive).
        /// </summary>
        private void SelectAllDataRows( GridEXRow[] rows )
        {
            if( rows != null )
            {
                foreach( GridEXRow dgRow in rows )
                {
                    DataRowView drv = dgRow.DataRow as DataRowView;
                    if( drv != null )
                    {
                        DataRow dr = drv.Row;
                        if( dr != null )
                        {
                            this.SelectedItems.Add( dgRow.Position );
                        }
                    }

                    // Select child rows recursively:
                    GridEXRow[] childRows = dgRow.GetChildRows();
                    SelectAllDataRows( childRows );
                }
            }
        }

        #endregion

        #region GetDataRowFromGridRow
        /// <summary>
        /// Gets a data row from a grid row.
        /// </summary>
        /// <param name="gridRow"></param>
        /// <returns></returns>
        public DataRow GetDataRowFromGridRow( GridEXRow gridRow )
        {
            if( gridRow != null )
            {
                DataRowView drv = gridRow.DataRow as DataRowView;
                if( drv != null )
                {
                    return drv.Row;
                }
            }

            return null;
        }
        #endregion

        #region Filtering issues workaround
        // See VA00086792, VA00098242, VA00098244, VA00098245, VA00098246, VA00098247, VA00098248, VA00098249, VA00098250.

        // ~~ Used in conjunction with ReapplyFilter ~~
        // Temporarily clears the data filter allowing GetAllDataRows(), GetAllRows(), and GetAllChildRecords()
        // to retrieve all GridEx objects; Not simply the visible ones.
        private int TemporarilyClearFilter(out List<int> selectedPositions)
        {
            //acquire the row index that is currently selected by user before removing the filter as this wil be same when we re-apply the filter
            int currRow = this.Row;

            //get the selected items and add it to the collection for later use
            GridEXSelectedItemCollection gridSelectedItems = this.SelectedItems;
            selectedPositions = new List<int>(gridSelectedItems.Count);

            foreach (GridEXSelectedItem gridExSelectedItem in gridSelectedItems)
            {
                selectedPositions.Add(gridExSelectedItem.Position);
            }

            //VA00131700 Rolandb Nov 10/15 Janus Grid 4.0 selection row fix
            // Save state information for the filter condition and current row
            if (this.RootTable != null)
            {
            m_filterCondition = this.RootTable.FilterApplied;

            // Temporarily remove filters so all GridEX items are retrieved
                if (m_filterCondition != null)
                this.RemoveFilters();
        }
            return currRow;
        }

        // ~~ Used in conjunction with TemporarilyClearFilter ~~
        // Restores state information after a call to TemporarilyClearFilter()
        private void ReapplyFilter(int currRow, IList<int> selectedPositions)
        {
            bool lbSkipFilter = false;

            //set the filter applying flag to true
            m_bApplyingFilter = true;

            //detach the selection changed event handler so that will not get fired whenever we are setting the filter row and previously selected row index
            this.SelectionChanged -= VJanusDataGridBase_SelectionChanged;

            try
            {
            // Re-apply the filter
                if (m_filterCondition != null)
            {
                    //apply the filter again
                    this.RootTable.ApplyFilter(m_filterCondition);

                //VA00131700 Rolandb Nov 10/15 Janus Grid 4.0 selection row fix [START]
                //Need to check if the toxicities unqiue filtering needs to be handled. The stored filter 'Approved' should
                //only be applied to the datagrid and not the filter row.
                if (this.RootTable.StoredFilters["ApprovedFilter"] != null)
                {
                    if (m_filterCondition == this.RootTable.StoredFilters["ApprovedFilter"].FilterCondition)
                    {
                        //We want to skip this filter assignment onto the filter row of the grid
                        lbSkipFilter = true;
                    }
                }

                if (this.FilterRow != null && !lbSkipFilter)
                {
                // ApplyFilter does not update the cell contents; This needs to be done manually
                GridEXFilterConditionCollection conditionCollection = m_filterCondition.Conditions;

                    GridEXRow filterRow = this.FilterRow;
                    GridEXCellCollection cellCollection = filterRow.Cells;

                        foreach (GridEXFilterCondition fc in conditionCollection)
                    {
                            foreach (GridEXCell cell in cellCollection)
                        {
                                if(cell.Column.Key == fc.Column.Key && cell.Column.Position >= 0)
                            {
                                this.Row = GridEX.filterRowPosition;
                                this.SetValue(cell.Column, (object)this.ConstructRegexStringFromOperator(fc.ConditionOperator, fc.Value1.ToString()));
                            }
                        }

                        this.UpdateData();
                    }

                    if (currRow < 0 || (currRow >= 0 && this.RowCount > 0 && currRow < this.RowCount))
                    {
                        this.Row = currRow; // Re-select the current row
                    }

                    //clear the selected items before adding new items
                    this.SelectedItems.Clear();

                    //added condition to select the rows only when exists and index is less than row count present in grid
                    bool result = selectedPositions.Any(p => p >= this.RowCount);
                    if (result)
                        return;
                    if (this.RowCount > 0)
                    {
                        // add the selected items from previously stored collection of items
                        foreach (var selectedPosition in selectedPositions)
                        {
                            this.SelectedItems.Add(selectedPosition);
                        }
                    }
                }
                //VA00131700 Rolandb Nov 10/15 Janus Grid 4.0 selection row fix [END]
            }
        }
            finally
            {
                //set the filter applying flag to false
                m_bApplyingFilter = false;

                //attach the selection changed event handler
                this.SelectionChanged += VJanusDataGridBase_SelectionChanged;
            }
        }

        private string ConstructRegexStringFromOperator(ConditionOperator op, string value) {

            if (op == ConditionOperator.Contains) { 
                return "*"+value+"*";
            }
            else if (op == ConditionOperator.BeginsWith){
                return value+"*";
            }
            else if (op == ConditionOperator.EndsWith){
                return "*"+value;
            }
            else {
                return value;
            }
        }
        // This method was added to resolve Janus Datagrid filtering bugs. The functionality is the same  
        // as GetDataRows() but this method also returns filtered results.
        /// <summary>
        /// Gets all data rows in the data grid.
        /// </summary>
        /// <returns>Array of GridEXRow objects</returns>
        public GridEXRow[] GetAllDataRows()
        {
            GridEXRow[] allDataRows;
            int currRow = 0;
            List<int> selectedPositions = new List<int>();
            try
            {
                currRow = TemporarilyClearFilter(out selectedPositions);
                allDataRows = GetDataRows();
            }
            finally
            {
                ReapplyFilter(currRow, selectedPositions);
            }

            return allDataRows;
        }

        // This method was added to resolve Janus Datagrid filtering bugs. The functionality is the same
        // as GetRows() but this method also returns filtered results.
        /// <summary>
        /// Gets all rows in the first hierarchical level of the data grid.
        /// </summary>
        /// <returns>Array of GridEXRow objects</returns>
        public GridEXRow[] GetAllRows()
        {
            GridEXRow[] allRows;
            int currRow = 0;
            List<int> selectedPositions = new List<int>();
            try
            {
                currRow = TemporarilyClearFilter(out selectedPositions);
                allRows = GetRows();
            }
            finally
            {
                ReapplyFilter(currRow, selectedPositions);
            }

            return allRows;
        }

        // This method was added to resolve Janus Datagrid filtering bugs. The functionality is the same
        // as GridExRow::GetChildRecords() but this method also returns filtered results.
        /// <summary>
        /// Gets all child rows to the specified parent.
        /// </summary>
        /// <param name="parentRow">A GridExRow object for which all child records will be returned.</param>
        /// <returns>Array of GridEXRow objects</returns>
        public GridEXRow[] GetAllChildRecords( GridEXRow parentRow )
        {
            GridEXRow[] allChildRecords;
            int currRow = 0;
            List<int> selectedPositions = new List<int>();
            try
            {
                currRow = TemporarilyClearFilter(out selectedPositions);
                allChildRecords = parentRow.GetChildRecords();
            }
            finally
            {
                ReapplyFilter(currRow, selectedPositions);
            }

            return allChildRecords;
        }

        // This method was added to resolve Janus Datagrid filtering bugs. The functionality is the same
        // as GridExRow::GetCheckedRows() but this method also returns filtered results.
        /// <summary>
        /// Gets all checked rows in the datagrid.
        /// </summary>
        /// <returns>Array of GridEXRow objects</returns>
        public GridEXRow[] GetAllCheckedRows()
        {
            GridEXRow[] allCheckedRows;
            int currRow = 0;
            List<int> selectedPositions = new List<int>();
            try
            {
                currRow = TemporarilyClearFilter(out selectedPositions);
                allCheckedRows = GetCheckedRows();
            }
            finally
            {
                ReapplyFilter(currRow, selectedPositions);
            }

            return allCheckedRows;
        }

        #endregion

        private void VJanusDataGrid_FormattingRow( object sender, RowLoadEventArgs e )
        {
            foreach( GridEXCell cell in e.Row.Cells )
            {
                cell.Text = cell.Text.Trim();
            }
        }

        private void OnColumnAutoResize( object sender, System.EventArgs e )
        {
            AutoSizeExpandColumns( true );
        }

        private void OnResizeColumns( object sender, System.EventArgs e )
        {
            AutoSizeExpandColumns( false );
        }

        private void OnFreezeColumn( object sender, System.EventArgs e )
        {
            this.FrozenColumns = m_selectedColumn.Position + 1;
        }

        private void OnUnFreezeAllColumns( object sender, System.EventArgs e )
        {
            this.FrozenColumns = -1;
        }

        private void OnExportClicked(object sender, System.EventArgs e)
        {
        }

        private void OnPrintClicked( object sender, System.EventArgs e )
        {
            
        }

        public bool NormalizeInput
        {
            get
            {
                return m_normalizeInput;
            }
            set
            {
                m_normalizeInput = value;
            }
        }

        protected override void OnUpdatingCell( UpdatingCellEventArgs e )
        {
            EditType et = e.Column.EditType;

            // Ignore any control that doesn't let user enter freetext
            if( this.NormalizeInput && et == EditType.TextBox || et == EditType.MultiColumnCombo || et == EditType.Combo )
            {
                try
                {
                    // Normalize the input text (unicode requirement)
                    if(e.Value!=null)
                    {
                        e.Value = e.Value.ToString();
                    }
                }
                catch
                {
                    MessageBox.Show( VFormResource.BadTextInput, VFormResource.strError, MessageBoxButtons.OK, MessageBoxIcon.Exclamation );
                    e.Cancel = true;
                }
            }

            if( !e.Cancel )
                base.OnUpdatingCell( e );
        }

        #endregion

        #region Methods


       
        public void ShowFilterRow( bool bShow )
        {
            // Set Filter Mode
            this.FilterMode = ( bShow ) ?
              Janus.Windows.GridEX.FilterMode.Automatic :
              Janus.Windows.GridEX.FilterMode.None;

            // ZachD - Sep 08/09 [VA00049863] Should populate the filter row as well, when it is shown.
            this.SetColumnFilters();
        }

        public void ShowRowHeader( bool bShow )
        {
            // Show Row Header
            this.RowHeaders = ( bShow ) ?
              Janus.Windows.GridEX.InheritableBoolean.True :
              Janus.Windows.GridEX.InheritableBoolean.False;
        }

        public void AutoSizeExpandColumns( bool bAutoSize )
        {
            if( bAutoSize )
            {
                this.ColumnAutoResize = true;
            }
            else
            {
                this.ColumnAutoResize = false;
                this.AutoSizeColumns( this.m_selectedTable );
            }
        }

        /// <summary>
        /// This function hides the column
        /// </summary>
        /// <param name="strColumnName"></param>
        public virtual void HideColumn( string strColumnName )
        {
            // Set the current table as default selected table
            if( this.m_selectedTable == null )
            {
                this.m_selectedTable = this.CurrentTable;
            }
            int nVisibleColumnCount = 0;
            GridEXColumn gSelectedColumn = this.m_selectedTable.Columns[ strColumnName ];
            // Log Error
            if( gSelectedColumn == null )
            {
                return;
            }

            // Count the number of visible columns
            foreach( GridEXColumn gCol in this.m_selectedTable.Columns )
            {
                if( gCol.Visible )
                {
                    nVisibleColumnCount++;
                }
            }

            // No way to get the header context menu if not columns are visible. 
            // Therefore, keep at least one column visible.
            if( nVisibleColumnCount == 1 )
            {
               return;
            }

            // Hide the column 
            gSelectedColumn.Visible = false;
        }

        public void ResetCurrentTableSortKeys()
        {
            GridEXTable table = this.CurrentTable;
            ResetSortKeys( table, false );
        }
        public void ResetAllTableSortKeys()
        {
            ResetSortKeys( this.RootTable, true );
        }
        /// <summary>
        /// This is reset of Sort keys for the table. If bRecursive is true
        /// all the sort keys for child tables below the parameter table 
        /// are also reset
        /// </summary>
        /// <param name="table">Parameter table</param>
        /// <param name="bRecursive">If true, apply operation for child tables also</param>
        protected virtual void ResetSortKeys( GridEXTable table, bool bRecursive )
        {
            if( table == null )
                return; // Sanity check

            if( table.SortKeys != null && table.SortKeys.Count > 0 )
            {
                table.SortKeys.Clear();
            }
            if( table.ChildTables == null )
                return; // recursion ends with this

            if( table.ChildTables != null )
            {
                for( int i = 0; i < table.ChildTables.Count; i++ )
                {
                    GridEXTable childTable = table.ChildTables[ i ];
                    if( bRecursive )
                    {
                        // Do same for child table
                        ResetSortKeys( childTable, bRecursive );
                    }
                }
            }
        }

        public virtual new void Refetch()
        {
            base.Refetch();
            // Set the Column Filters to match Column type
            this.SetColumnFilters();
        }

        public override void Refresh()
        {
            base.Refresh();
            if( this.RootTable != null )
                this.SetColumnFilters();
        }

        public virtual void SetColumnFilters( GridEXTable table )
        {
            if( table == null )
            {
                throw new ArgumentNullException(VFormResource.strNullGridExTableReference);
            }
            GridEXColumnCollection gridEXCols = table.Columns;
            if( gridEXCols == null )
            {
                // Localize String
                throw new ArgumentNullException(VFormResource.strNullGridExColumnCollectionReference);
            }

            for( int i = 0; i < gridEXCols.Count; i++ )
            {
                // If the cell is of simple data type (except date, multicolumn valuelist)
                // Set the Filter as Combo type so that all different displayed values
                // in the column will appear in the filter selection combo box
                if( gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.Combo ||
                  gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.DropDownList ||
                  gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.IntegerUpDown ||
                  gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.IntegerUpDownCombo ||
                  gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.NoEdit ||
                  gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.TextBox ||
                  gridEXCols[ i ].EditType == Janus.Windows.GridEX.EditType.Custom )
                {
                    //ryans Jan 9/12 VA00087313
                    //checkbox columns should keep the checkbox filter
                    if( gridEXCols[ i ].ColumnType == ColumnType.CheckBox )
                    {
                        table.Columns[ i ].FilterEditType = FilterEditType.CheckBox;
                    }
                    else
                    {
                        table.Columns[ i ].FilterEditType = Janus.Windows.GridEX.FilterEditType.Combo;
                    }

                    // ZachD - Sep 08/09 [VA00049863] - Make sure that the filter row of this column is selectable.
                    if( table.Columns[ i ].Selectable == false )
                    {
                        table.Columns[ i ].Selectable = true;
                        table.Columns[ i ].SelectableCells = SelectableCells.FilterRowCells;
                    }
                    else
                    {
                        table.Columns[ i ].SelectableCells |= SelectableCells.FilterRowCells;
                    }
                }
            }

            this.SetPersistedFilter( ref bIsFirstTimeLoad, table );
        }

        /// <summary>
        /// This function set the persisted filters in AppSettings File
        /// </summary>
        /// <param name="bApply"></param>
        public void SetPersistedFilter( ref bool bApply, GridEXTable table )
        {
            if( bApply && m_bPersistFilter && table.FilterCondition != null )
            {
                bApply = false;
                GridEXFilterConditionCollection conditionCollection = table.FilterCondition.FilterCondition.Conditions;
                if( this.FilterRow != null )
                {
                    //VA00131700 Rolandb Oct 27/15 Janus Grid 4.0 selection row fix
                    m_bApplyingFilter = true;
                    GridEXRow filterRow = this.FilterRow;
                    GridEXCellCollection cellCollection = filterRow.Cells;

                    foreach( GridEXFilterCondition fc in conditionCollection )
                    {
                        foreach( GridEXCell cell in cellCollection )
                        {
                            if( cell.Column.Key == fc.Column.Key )
                            {
                                //VA00131700  Rolandb Oct 28/15 Moved setting of row to avoid the row being incorrectly set off the filter row
                                this.Row = GridEX.filterRowPosition;
                                this.SetValue(cell.Column, (object)this.ConstructRegexStringFromOperator(fc.ConditionOperator, fc.Value1.ToString()));
                                this.UpdateData();
                            }
                        }
                    }
                    //VA00131700 Rolandb Oct 27/15 Janus Grid 4.0 selection row fix
                    m_bApplyingFilter = false;
                }
            }
        }

        /// <summary>
        /// This functions sets up filter combo values for
        /// simple data type columns (except valuelist and multicolumn combo)
        /// </summary>
        public virtual void SetColumnFilters()
        {
            if( this.m_selectedTable == null )
            {
                // Localize String
                this.m_selectedTable = this.RootTable;
            }
            SetColumnFilters( this.m_selectedTable );
        }

        /// <summary>
        /// Returns an enumerable collection of the databound objects of the selected rows.
        /// </summary>
        /// <typeparam name="T">The type of the databound objects.</typeparam>
        /// <returns>enumerable collection of databound objects of selected rows.</returns>
        public IEnumerable<T> GetSelectedDataboundObjects<T>()
        {
            foreach( GridEXSelectedItem item in SelectedItems )
            {
                GridEXRow row = item.Position >= 0 ? item.GetRow() : null;
                // The object that is bound to the grid row.
                object dataBoundObject = null != row ? row.DataRow : null;

                // Ensure that this is a record, and not a filter row, group header row, etc.
                if( item.RowType == RowType.Record && null != dataBoundObject )
                {
                    // The object that we will return.
                    T objectToReturn;

                    // If the object is a DataRowView, then we must get the underlying DataRow.
                    if( dataBoundObject is DataRowView )
                    {
                        objectToReturn = ( T )Convert.ChangeType( ( ( DataRowView )dataBoundObject ).Row, typeof( T ) );
                    }
                    // If the object is not a DataRowView, then we just return it.
                    else
                    {
                        objectToReturn = ( T )dataBoundObject;
                    }

                    // Yield the object.
                    yield return objectToReturn;
                }
            }
        }


        public virtual bool SelectRow( GridEXColumn keyUniqueCol, long lRowId )
        {
            bool bRet = false;
            try
            {
                // Creating the condition
                IFilterCondition curFilterCondition = this.RootTable.FilterCondition;
                GridEXFilterCondition newFilterCondition =
                  new GridEXFilterCondition( keyUniqueCol, ConditionOperator.Equal, lRowId );

                // Select the row and change cell selection mode to old value
                Janus.Windows.GridEX.CellSelectionMode sel = this.CellSelectionMode;
                this.CellSelectionMode = Janus.Windows.GridEX.CellSelectionMode.EntireRow;

                bRet = this.Find( newFilterCondition, -1, 1 ); // ZachD - VA00066495 - Apr 06, 2010 - Find forwards, not backwards!

                this.CellSelectionMode = sel;
                if( curFilterCondition != null )
                {
                    this.RootTable.FilterCondition = curFilterCondition;
                }
                return bRet;
            }
            catch( Exception ex )
            {
                System.Diagnostics.Trace.WriteLine( String.Format( "SelectRow did not work...{0}{1}",
                  Environment.NewLine, ex.Message ) );
                return bRet;
            }
        }

        #region SetAllowRemoveColumns
        /// <summary>
        /// Sets an option which will either let or not let users drag and drop columns off of a
        /// grid in order to hide them.
        /// </summary>
        private void SetAllowRemoveColumns()
        {
            if( bShowColumnHeaderContextMenu && bEnableShowFieldChooserMenuItem )
            {
                this.AllowRemoveColumns = InheritableBoolean.True;
            }
            else
            {
                this.AllowRemoveColumns = InheritableBoolean.False;
            }
        }
        #endregion

        #endregion

        #region Properties

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Janus.Windows.GridEX.GridEXTable SelectedTable
        {
            get
            {
                return m_selectedTable;
            }
            set
            {
                this.m_selectedTable = value;
            }
        }


        /// <summary>
        /// Sets the parent form for the field chooser.
        /// </summary>
        public new Form ParentForm
        {
            set
            {
                m_parentForm = value;
            }
        }

        /// <summary>
        /// Gets or sets if the datagrid should show the column header context menu.
        /// </summary>
        public bool bShowColumnHeaderContextMenu
        {
            get
            {
                return m_bShowColumnHeaderContextMenu;
            }
            set
            {
                m_bShowColumnHeaderContextMenu = value;

                // ZachD - 1/10/2011 - VA0054984 - Allow dropping columns off grid (and into field chooser):
                SetAllowRemoveColumns();
            }
        }

        /// <summary>
        /// Shows or hides the watermark.
        /// </summary>
        public bool bShowWatermark
        {
            get
            {
                return m_bShowWatermark;
            }
            set
            {
                m_bShowWatermark = value;

                SetRowStyleToTransparentIfShowWatermark();

                GenerateWatermark();
            }
        }

        /// <summary>
        /// Shows or hides the watermark for "List Filtered".
        /// </summary>
        public bool bIsListFiltered
        {
            get
            {
                return m_bIsListFiltered;
            }
            set
            {
                m_bIsListFiltered = value;

                GenerateWatermark();
            }
        }

        /// <summary>
        /// Generates a watermark image.
        /// </summary>
        private void GenerateWatermark()
        {
            try
            {
                if( m_bShowWatermark )
                {
                    System.Drawing.Image existingWatermark = this.WatermarkImage.Image;

                    // Determine what size the image must fit into (i.e. the size of the grid)
                    int width = this.Width < 600 ? this.Width : 600;
                    int height = this.Height < 300 ? this.Height : 300;

                    if( m_bIsListFiltered
                        && width > 50
                        && height > 50
                        )
                    {
                        // Only create a new watermark if one doesn't already exist or the existing
                        // watermark is not the correct size:
                        if( existingWatermark == null
                            || ( Math.Abs( width - existingWatermark.Width ) > 2 ) // Width has changed by more than 2
                            || ( Math.Abs( height - existingWatermark.Height ) > 2 ) // Height has changed by more than 2
                            )
                        {
                            // Create a new image to use as the watermark:
                            Bitmap bitmap = new Bitmap( width, height );

                            // Get drawing properties:
                            Color color = Color.FromArgb( 222, 223, 222 );
                            RectangleF layoutRectangle = new RectangleF( 0f, 0f, ( float )width, ( float )height );
                            using( Font font = new Font( "Calibri", 52.0f ) )
                            using( Brush brush = new SolidBrush( color ) )
                            using( StringFormat stringFormat = new StringFormat() )
                            {
                                stringFormat.Alignment = StringAlignment.Center;
                                stringFormat.LineAlignment = StringAlignment.Far;
                                stringFormat.Trimming = StringTrimming.EllipsisCharacter;

                                // Draw the text:
                                using( Graphics g = Graphics.FromImage( bitmap ) )
                                {
                                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                                    g.DrawString( "Filtered List", font, brush, layoutRectangle, stringFormat );
                                }

                                // Set watermark:
                                this.WatermarkImage.Image = bitmap;
                                this.WatermarkImage.Alpha = 255;
                                this.WatermarkImage.ImageAlign = ContentAlignment.BottomCenter;
                                this.Refresh();

                                // Dispose of the old watermark image, if required:
                                if( existingWatermark != null )
                                {
                                    existingWatermark.Dispose();
                                    existingWatermark = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        // Clear the watermark:
                        this.WatermarkImage.Image = null;
                        this.Refresh();

                        // Dispose of the old watermark image, if required:
                        if( existingWatermark != null )
                        {
                            existingWatermark.Dispose();
                            existingWatermark = null;
                        }
                    }
                }
            }
            catch( Exception ex )
            {
                // Clear the watermark:
                this.WatermarkImage.Image = null;
                this.Refresh();
            }
        }

        public System.Drawing.Printing.PrintDocument PrintDocument
        {
            get
            {
                return this.m_gridEXPrintDocument;
            }
        }

        public Janus.Windows.GridEX.GridEXPrintDocument GridEXPrintDocument
        {
            get
            {
                return m_gridEXPrintDocument;
            }
        }


        public bool bPersistFilter
        {
            get
            {
                return m_bPersistFilter;
            }
            set
            {
                this.m_bPersistFilter = value;
            }
        }

        #region Properties to enable/disable individual context menu items
        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Sort Ascending context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Gets/Sets if the datagrid should enable/disable the "Sort Ascending" menu item in the header context menu.
        /// </summary>
        public bool bEnableSortAscendingMenuItem
        {
            get
            {
                return this.m_menuItemSortAsc.Enabled;
            }
            set
            {
                this.m_menuItemSortAsc.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Sort Descending context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Gets/Sets if the datagrid should enable/disable the "Sort Descending" menu item in the header context menu.
        /// </summary>
        public bool bEnableSortDescendingMenuItem
        {
            get
            {
                return this.m_menuItemSortDesc.Enabled;
            }
            set
            {
                this.m_menuItemSortDesc.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Remove All Sorts context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Remove All Sorts" item in the header context menu.
        /// </summary>
        public bool bEnableRemoveAllSortsMenuItem
        {
            get
            {
                return this.m_menuItemRemoveAllSorts.Enabled;
            }
            set
            {
                this.m_menuItemRemoveAllSorts.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Show/Hide Data Filter context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Show/Hide Data Filter" item in the header context menu.
        /// </summary>
        public bool bEnableShowHideFilterRowMenuItem
        {
            get
            {
                return this.m_menuItemShowHideFilterRow.Enabled;
            }
            set
            {
                this.m_menuItemShowHideFilterRow.Enabled = value;
            }
        }



        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the No Filter context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "No Filter" item in the header context menu.
        /// </summary>
        public bool bEnableRemoveAllFiltersMenuItem
        {
            get
            {
                return this.m_menuItemRemoveAllFilters.Enabled;
            }
            set
            {
                this.m_menuItemRemoveAllFilters.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Hide This Column context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Hide This Column" item in the header context menu.
        /// </summary>
        public bool bEnableHideThisFieldMenuItem
        {
            get
            {
                return this.m_menuItemHideThisField.Enabled;
            }
            set
            {
                this.m_menuItemHideThisField.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Show Column Chooser context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Show Column Chooser" item in the header context menu.
        /// </summary>
        public bool bEnableShowFieldChooserMenuItem
        {
            get
            {
                return this.m_menuItemShowFieldChooser.Enabled;
            }
            set
            {
                this.m_menuItemShowFieldChooser.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Column AutoResize context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Column Auto Resize" item in the header context menu.
        /// </summary>
        public bool bEnableColumnAutoResizeMenuItem
        {
            get
            {
                return this.m_menuItemColumnAutoResize.Enabled;
            }
            set
            {
                this.m_menuItemColumnAutoResize.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Resize Columns context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Resize Columns" item in the header context menu.
        /// </summary>
        public bool bEnableResizeColumnsMenuItem
        {
            get
            {
                return this.m_menuItemResizeColumns.Enabled;
            }
            set
            {
                this.m_menuItemResizeColumns.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Freeze Column context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Freeze Column" item in the header context menu.
        /// </summary>
        public bool bEnableFreezeColumnMenuItem
        {
            get
            {
                return this.m_menuItemFreezeColumn.Enabled;
            }
            set
            {
                this.m_menuItemFreezeColumn.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the UnFreeze All Columns context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "UnFreeze All Columns" item in the header context menu.
        /// </summary>
        public bool bEnableUnFreezeAllColumnsMenuItem
        {
            get
            {
                return this.m_menuItemUnFreezeAllColumns.Enabled;
            }
            set
            {
                this.m_menuItemUnFreezeAllColumns.Enabled = value;
            }
        }

        [Browsable(true)]
        [Category("Menu Items")]
        [Description("Enable/Disable the Export context menu item")]
        [DefaultValue(true)]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Export" item in the header context menu.
        /// </summary>
        public bool bEnableExportMenuItem
        {
            get
            {
                return this.m_menuItemExport.Enabled;
            }
            set
            {
                this.m_menuItemExport.Enabled = value;
            }
        }

        [Browsable( true )]
        [Category( "Menu Items" )]
        [Description( "Enable/Disable the Print context menu item" )]
        [DefaultValue( true )]
        /// <summary>
        /// Sets if the datagrid should enable/disable the "Print" item in the header context menu.
        /// </summary>
        public bool bEnablePrintMenuItem
        {
            get
            {
                return this.m_menuItemPrint.Enabled;
            }
            set
            {
                this.m_menuItemPrint.Enabled = value;
            }
        }
        #endregion Properties to enable/disable individual context menu items

        #endregion

        #region Implementation of IVForm

        public virtual void VInitialize()
        {
        }

        public void VReset()
        {

        }

        
        protected void SetRowStyleToTransparentIfShowWatermark()
        {
            if( m_bShowWatermark == true )
            {
                this.RowFormatStyle.BackColorAlphaMode = AlphaMode.Transparent;
                this.SelectedFormatStyle.BackColorAlphaMode = AlphaMode.Opaque;
            }
        }

        #region Properties
        public bool bIsDirty
        {
            get
            {
                return m_bIsDirty;
            }
            set
            {
                m_bIsDirty = value;
            }
        }

        // This property is slightly different in VJanusDataGrid,
        // which is why the virtual key word is needed.
        public virtual bool bIsRequired
        {
            get
            {
                return m_bIsRequired;
            }
            set
            {
                m_bIsRequired = value;
            }
        }

        public bool bUseVarianLF
        {
            get
            {
                return m_bUseVarianLF;
            }
            set
            {
                m_bUseVarianLF = value;
            }
        }

        // Graham S. Jan 7/10 ARIA 10 Req 450 Implement strQuickLinkName so we can link to it via link label
        private string m_strQuickLinkName = "";

        public string strQuickLinkName
        {
            get
            {
                return this.m_strQuickLinkName;
            }
            set
            {
                this.m_strQuickLinkName = value;
            }
        }

        public bool bIsCompleted
        {
            get
            {
                return true;
            }
        }

        // Graham S. December 16/09
        public bool IsDataRowSelected
        {
            get
            {
                bool dataRowsSelected = false;

                if( this.SelectedItems.Count > 0 )
                {
                    dataRowsSelected = true;

                    foreach( GridEXSelectedItem gridRow in this.SelectedItems )
                    {
                        if( gridRow.RowType != RowType.Record || gridRow.Position < 0 || gridRow.GetRow().DataRow == null )
                        {
                            dataRowsSelected = false;
                            break;
                        }
                    }
                }

                return dataRowsSelected;
            }
        }

        #endregion

        #endregion

        #region Callback Methods
        /// <summary>
        /// Makes sure, that nothing is hanging around after removing
        /// last record
        /// I don't know, why it work, but it works
        /// </summary>
        protected virtual void OnRowCountChanged( object sender, System.EventArgs e )
        {
            if( this.RowCount == 0 )
            {
                this.CheckAllRecords();
            }
        }

        protected virtual void OnVisibleChanged( object sender, System.EventArgs e )
        {
        }

        private void OnFilterApplied( object sender, System.EventArgs e )
        {
            if( this.RootTable.FilterCondition != null )
            {
                //this.SaveGridLayout();
                this.SetColumnFilters();
            }
            //else
            //{ 
            //  this.m_layoutStream = null;
            //}
        }
                #endregion

        #region VJanusDataGrid_Leave
        /// <summary>
        /// When leaving the control, ensure the field chooser is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VJanusDataGrid_Leave( object sender, EventArgs e )
        {
            // ZachD - Apr 01, 2010 - VA00066544 - Hide the field chooser when focus is lost.
            if( this.IsFieldChooserVisible() )
            {
                this.HideFieldChooser();
            }
        }
        #endregion

        #region VJanusDataGrid_Resize
        /// <summary>
        /// When the grid is resized, we may need to resize the watermark image.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VJanusDataGrid_Resize( object sender, EventArgs e )
        {
            GenerateWatermark();
        }
        #endregion

        #region OnClearFilterButtonClick
        /// <summary>
        /// When the clear filter button is clicked for a column on the data filter row
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <remarks>
        /// AUTHOR      DATE        DR              VERSION     DESC
        /// ----------- ----------- --------------- ----------- ----------
        /// KurtB       May 15/12   VA00089181      ARIA 11MR1  Initial creation
        /// </remarks>
        private void OnClearFilterButtonClick( object sender, ColumnActionEventArgs e )
        {

            if( e.Column != null && e.Column.GridEX.RecordCount == 0 )
            {
                if( e.Column.GridEX.RootTable.FilterApplied != null )
                {
                    //This happens after executing the clear filter and there are no rows in the grid after the data changes so all rows become filtered.
                    //There is no text in the filter row for any of the filtered columns.
                    foreach( GridEXFilterCondition filterCondition in e.Column.GridEX.RootTable.FilterApplied.Conditions )
                    {
                        if( filterCondition.Column == e.Column )
                        {
                            //This is the column that clear filter was clicked for but the filter is still there but was not removed automatically.                             
                            //Removing the filter manually gives an error but clearing it does not.
                            //If any other columns have a filter, the filter is still in FilterApplied for the column after clearing but does nothing.
                            filterCondition.Clear();
                            Refetch();
                            break;
                        }
                    }

                }
            }
        }
        #endregion

        // GrahamS - When a GridEX has child child tables the grid displays collapse/expand [+] buttons beside all rows,
        // even those with no data in the child tables (for performance reasons, so it doesn't have to inspect child data for each row.)
        // Then when you click to expand a row with no child data, the collapse/expand button disappears permanently. 
        // We would like to only show the expand button beside rows that have child data - so expand/collapse all rows to
        // hide the unnecessary expand buttons.
        public void RefreshCollapseRowsButtons()
        {
            this.SuspendLayout();
            this.ExpandRecords();
            this.CollapseRecords();
            this.ResumeLayout();
        }

        #region Custom Events & Delegates
        public delegate void GridButtonEventHandler( int rowNum, GridEXColumn theCol, bool headerClicked );
        public virtual event GridButtonEventHandler GridButtonClicked;
        #endregion

        #region CheckGridButtonClicked
        public virtual string CheckGridButtonClicked( object sender, System.Windows.Forms.MouseEventArgs mouseEvent )
        {
            GridEXColumn clickedColumn;
            int clickedRow;
            string rtnCol = "";

            if (RootTable == null)
            {
                return rtnCol;
            }
            // Check for a left mouse click
            if( mouseEvent.Button == MouseButtons.Left )
            {
                // Get the column where the user clicked
                clickedColumn = this.RootTable.GridEX.ColumnFromPoint( mouseEvent.X, mouseEvent.Y );

                // Ensure that they clicked on a column
                if( clickedColumn != null )
                {
                    // Get the row where the user clicked
                    clickedRow = this.RootTable.GridEX.RowPositionFromPoint( mouseEvent.X, mouseEvent.Y );

                    // Check to see if the clicked column displays an image
                    if( ( clickedColumn.ColumnType == ColumnType.Image ) || ( clickedColumn.ColumnType == ColumnType.ImageAndText ) )
                    {
                        // Found a grid button. Return the key name of the column
                        rtnCol = clickedColumn.Key;

                        // Trigger the GridButtonClicked event
                        if( GridButtonClicked != null )
                        {
                            GridButtonClicked( clickedRow, clickedColumn, ( clickedRow == -3 ) );
                        }
                    }
                }
            }

            return rtnCol;
        }
        #endregion

        #region Events

        #region VJanusDataGridBase_MouseUp
        protected virtual void VJanusDataGridBase_MouseUp( object sender, System.Windows.Forms.MouseEventArgs e )
        {
            if( this.DesignMode == false )
            {
                CheckGridButtonClicked( sender, e );
            }

            if( e.Button == MouseButtons.Left )
            {
                // No way to get the header context menu if not columns are visible. 
                // Therefore, keep at least one column visible.
                if( this.m_iVisibleColumnNum == 1 )
                    this.m_dragColumn.Visible = true;
            }

        }
        #endregion

        public void VJanusDataGridBase_GridButtonClicked( int rowNum, GridEXColumn theCol, bool headerClicked )
        {
            // Empty method. 
        }
        #endregion

        #region Static Methods

       
        #endregion

        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void VJanusDataGridBase_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                //The changes are made to avoid the Out Of Memory exception while loading large data in the grid due to
                //GetAllRows method called to set the Accessible Description of the grid.

                //detach the row formatting event on selection change as we are only setting the text of cells. 
                //And in case of large data this can lead to out of memory exception
                this.FormattingRow -= VJanusDataGrid_FormattingRow;
            //VA00131700 Rolandb Oct 27/15 Janus Grid 4.0 selection row fix, perform if not reapply filters
            ///while applying the filter the selected row index is either -2 or -3 and 
            //we dont want to invoke SelectionChange event to keep grid responding
            if (!m_bApplyingFilter && this.Row != -2 && this.Row != -3)
            {
                    this.AccessibleDescription = ""; //clear the descripton
                    if (SelectedItems.Count > 0)
                    {
                        StringBuilder accessibleDescription = new StringBuilder();
                        //use the SelectedItems collection to get the selected row positions in order to set the Accessible Description
                        foreach (GridEXSelectedItem selItem in this.SelectedItems)
                        {
                            accessibleDescription.Append(string.Format("Row{0};", selItem.Position));
                        }
                            AccessibleDescription = accessibleDescription.ToString();
                            accessibleDescription.Clear();
                    }
                }
            }
            finally
            {
                this.FormattingRow += VJanusDataGrid_FormattingRow;
            }
        }

    }
  

    public class VGridColumnInfo
    {
        #region Members
        private int m_nPos = 0;
        private int m_nWidth = 20;
        private string m_strKey = null;
        #endregion

        #region Constructor/Destructor
        public VGridColumnInfo()
        {
        }
        public VGridColumnInfo( int nPos, int nWidth, string strKey )
        {
            m_nPos = nPos;
            m_nWidth = nWidth;
            m_strKey = strKey;
        }
        #endregion

        #region Properties
        [XmlAttribute( AttributeName = "Pos" )]
        /// <summary>
        /// Column Position
        /// </summary>
        public int nPos
        {
            get
            {
                return m_nPos;
            }
            set
            {
                m_nPos = value;
            }
        }

        [XmlAttribute( AttributeName = "Width" )]
        /// <summary>
        /// Column Width
        /// </summary>
        public int nWidth
        {
            get
            {
                return m_nWidth;
            }
            set
            {
                m_nWidth = value;
            }
        }

        [XmlAttribute( AttributeName = "Key" )]
        /// <summary>
        /// Key Column Name
        /// </summary>
        public string strKey
        {
            get
            {
                return m_strKey;
            }
            set
            {
                m_strKey = value;
            }
        }

        #endregion

    }

    public class VGridSortInfo
    {
        #region Members
        private string m_strKey = null;
        private int m_nOrder = 0;
        #endregion

        #region Constructor/Destructor
        public VGridSortInfo()
        {
        }

        public VGridSortInfo( string strKey, int nOrder )
        {
            m_strKey = strKey;
            m_nOrder = nOrder;
        }
        #endregion

        #region Properties
        [XmlAttribute( AttributeName = "Key" )]
        /// <summary>
        /// Sort Key Name
        /// </summary>
        public string strKey
        {
            get
            {
                return m_strKey;
            }
            set
            {
                m_strKey = value;
            }
        }

        [XmlAttribute( AttributeName = "Order" )]
        /// <summary>
        /// Sort Order
        /// </summary>
        public int nOrder
        {
            get
            {
                return m_nOrder;
            }
            set
            {
                m_nOrder = value;
            }
        }

        #endregion
    }

    public class VGridFilterInfo
    {
        #region Members
        private string m_strKey = null;
        private int m_nCondition = 0;
        private string m_strValue = null;
        #endregion

        #region Constructor/Destructor
        public VGridFilterInfo()
        {
        }

        public VGridFilterInfo( string strKey, int nCondition, string strValue )
        {
            m_strKey = strKey;
            m_nCondition = nCondition;
            m_strValue = strValue;
        }
        #endregion

        #region Properties
        [XmlAttribute( AttributeName = "Key" )]
        /// <summary>
        /// Sort Key Name
        /// </summary>
        public string strKey
        {
            get
            {
                return m_strKey;
            }
            set
            {
                m_strKey = value;
            }
        }

        [XmlAttribute( AttributeName = "Condition" )]
        /// <summary>
        /// Filter Condition
        /// </summary>
        public int nCondition
        {
            get
            {
                return m_nCondition;
            }
            set
            {
                m_nCondition = value;
            }
        }

        [XmlAttribute( AttributeName = "Value" )]
        /// <summary>
        /// Sort Order
        /// </summary>
        public string strValue
        {
            get
            {
                return m_strValue;
            }
            set
            {
                m_strValue = value;
            }
        }
        #endregion
    }

    public class VGridExInfo 
    {
        public const string curVersion = "1.0";
        #region private members

        private int m_nColumnsAutoSize = 1;
        private int m_nFrozenColumns = -1;
        private ArrayList m_listColumn = null;
        private ArrayList m_listSort = null;
        private ArrayList m_listFilter = null;
        private string m_version = curVersion;
        private string m_strGridExName = "";

        #endregion

        #region Constructor/Destructor

        public VGridExInfo()
        {
            m_listColumn = new ArrayList();
            m_listSort = new ArrayList();
            m_listFilter = new ArrayList();
        }

        #endregion

        #region Methods

        public void AddVisibleColumnItem( int nPos, int nWidth, string strKey )
        {
            VGridColumnInfo gridColInfo = new VGridColumnInfo( nPos, nWidth, strKey );
            m_listColumn.Add( gridColInfo );
        }

        public void AddSortItem( string strKey, int nSortOrder )
        {
            VGridSortInfo gridSortInfo = new VGridSortInfo( strKey, nSortOrder );
            m_listSort.Add( gridSortInfo );
        }

        public void AddFilterItem( string strKey, int nCondition, string strValue )
        {
            VGridFilterInfo gridFilterInfo = new VGridFilterInfo( strKey, nCondition, strValue );
            m_listFilter.Add( gridFilterInfo );
        }

        public void ClearColumns()
        {
            m_listColumn.Clear();
        }
        public void ClearSorts()
        {
            m_listSort.Clear();
        }
        public void ClearFilters()
        {
            m_listFilter.Clear();
        }
        #endregion

        #region Properties

        [XmlAttribute( AttributeName = "Version" )]
        public string Version
        {
            get
            {
                return m_version;
            }
            set
            {
                m_version = value;
            }
        }
        [XmlAttribute( AttributeName = "GridName" )]
        public string strGridExName
        {
            get
            {
                return m_strGridExName;
            }
            set
            {
                m_strGridExName = value;
            }
        }
        [XmlAttribute( AttributeName = "ColumnsAutoSize" )]
        public int nColumnsAutoSize
        {
            get
            {
                return m_nColumnsAutoSize;
            }
            set
            {
                m_nColumnsAutoSize = value;
            }
        }

        [XmlAttribute( AttributeName = "FrozenColumns" )]
        public int nFrozenColumns
        {
            get
            {
                return m_nFrozenColumns;
            }
            set
            {
                m_nFrozenColumns = value;
            }
        }

        [XmlArray( ElementName = "VisibleColumns", IsNullable = false )]
        /// <summary>
        /// Grid Columns
        /// </summary>
        public VGridColumnInfo[] GridColumns
        {
            get
            {
                if( 0 == m_listColumn.Count )
                {
                    // TODO: Trace and assert
                    return null;
                }
                return m_listColumn.ToArray(
                  typeof( VGridColumnInfo ) ) as VGridColumnInfo[];
            }
            set
            {
                m_listColumn.Clear();
                if( value != null )
                {
                    for( int i = 0; i < value.Length; i++ )
                    {
                        m_listColumn.Add( value[ i ] );
                    }
                }
            }
        }
        [XmlArray( ElementName = "Sorts", IsNullable = false )]
        /// <summary>
        /// Grid Sorts
        /// </summary>
        public VGridSortInfo[] GridSorts
        {
            get
            {
                if( 0 == m_listSort.Count )
                {
                    // TODO: Trace and assert
                    return null;
                }
                return m_listSort.ToArray(
                  typeof( VGridSortInfo ) ) as VGridSortInfo[];
            }
            set
            {
                m_listSort.Clear();
                if( value != null )
                {
                    for( int i = 0; i < value.Length; i++ )
                    {
                        m_listSort.Add( value[ i ] );
                    }
                }
            }
        }

        [XmlArray( ElementName = "Filters", IsNullable = false )]
        /// <summary>
        /// Grid Filters
        /// </summary>

        public VGridFilterInfo[] GridFilters
        {
            get
            {
                if( 0 == m_listFilter.Count )
                {
                    // TODO: Trace and assert
                    return null;
                }
                return m_listFilter.ToArray(
                  typeof( VGridFilterInfo ) ) as VGridFilterInfo[];
            }
            set
            {
                m_listFilter.Clear();
                if( value != null )
                {
                    for( int i = 0; i < value.Length; i++ )
                    {
                        m_listFilter.Add( value[ i ] );
                    }
                }
            }
        }

        #endregion
    }

    public class CurrentRowChangingEventArgs
    {
        private bool m_cancel;
        private GridEXRow m_row;
        private GridEXColumn m_column;

        public CurrentRowChangingEventArgs( GridEXRow row, GridEXColumn col )
        {
            m_cancel = false;
            m_row = row;
            m_column = col;            
        }

        public bool Cancel
        {
            get
            {
                return m_cancel;
            }
            set
            {
                m_cancel = value;
            }
        }

        public GridEXRow Row
        {
            get
            {
                return m_row;
            }
            set
            {
                m_row = value;
            }
        }

        public GridEXColumn Column
        {
            get
            {
                return m_column;
            }
            set
            {
                m_column = value;
            }
        }

    }

    public static class Helper
    {
        public static Control GetTopLevelWindowsFormsControl(this Control ctrl)
        {
            while (ctrl.Parent != null)
            {
                ctrl = ctrl.Parent;
            }

            return ctrl;
        }

        public static Color GetColorFromRGBString(string strRGBColor) 
        {
            string[] rgb = strRGBColor.Split(',');
            return Color.FromArgb(int.Parse(rgb[0]), int.Parse(rgb[1]), int.Parse(rgb[2]));
        }
    }
}
