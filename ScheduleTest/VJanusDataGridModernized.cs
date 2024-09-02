using Janus.Windows.GridEX;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleTest
{
    public partial class VJanusDataGridModernized : VJanusDataGridBase
    {

        #region Readonly Colours

        private readonly Color BACK_COLOUR = Color.FromArgb(246, 249, 251);
        private readonly Color FORE_COLOUR = Color.FromArgb(0, 0, 0);
        private readonly Color GRID_COLOUR = Color.FromArgb( 168, 169, 184 );
        private Color SELECTED_BACK_COLOUR = Color.FromArgb( 20, 75, 133 );
        private readonly Color SELECTED_FORE_COLOUR = Color.FromArgb( 255, 255, 255 );
        private readonly Color GROUP_HEADER_BACK_COLOUR = Color.FromArgb( 152, 182, 201 );
        //private readonly Color BACKGROUND_COLOUR = Color.FromArgb(255, 255, 255);
        private readonly Color BACKGROUND_COLOUR = Color.FromArgb(235, 240, 244);

        private readonly Color BORDER_COLOUR = Color.FromArgb( 168, 168, 183 );

        private readonly Color HEADER_BACK_COLOUR = Color.FromArgb( 187, 206, 214 );
        private readonly Color HEADER_FORE_COLOUR = Color.FromArgb( 59, 94, 130 );

        private readonly Color ROW_BACK_COLOUR = Color.FromArgb( 235, 240, 244 );

        private readonly Color ALTERNATING_ROW_COLOUR = Color.FromArgb( 245, 250, 254 );

        private readonly Color FILTER_BACK_COLOUR = Color.FromArgb( 168, 168, 183 );

        #endregion

        #region Variables
        private string m_validEntriesInd = "Y";
        #endregion

        #region Basic Appearance
        Janus.Windows.GridEX.Appearance FLAT = Janus.Windows.GridEX.Appearance.Flat;
        Janus.Windows.GridEX.TextAlignment NEAR = Janus.Windows.GridEX.TextAlignment.Near;
        Janus.Windows.GridEX.TextAlignment CENTER = Janus.Windows.GridEX.TextAlignment.Center;
        #endregion

        #region Constructor
        public VJanusDataGridModernized()
        {
            base.ThemedAreas = ThemedArea.None;
        }

        public VJanusDataGridModernized(System.ComponentModel.IContainer container) : base()
		{
            base.ThemedAreas = ThemedArea.None;
			// This call is required by the Windows.Forms Form Designer.
			//InitializeComponent();

			///
			/// Required for Windows.Forms Class Composition Designer support
			///
			container.Add(this);
		}

        protected override void OnCreateControl()
        {
            base.ThemedAreas = ThemedArea.None;
            
            base.OnCreateControl();

            SetStyles();

        }
        #endregion

        #region properties
        [Description( "Indicates if all rows in the grid are valid records or not. When invalid, the rows use strikeout font." ),
        Category( "Appearance" )]
        public string ValidEntriesInd
        {
            get
            {
                return m_validEntriesInd;
            }
            set
            {
                if( ( value == "N" ) || ( value == "Y" ) )
                {
                    m_validEntriesInd = value;
                }

                // If the rows in the datagrid are invalid, then turn on the strikeout font for all rows
                if( m_validEntriesInd == "Y" )
                {
                    this.RowFormatStyle.FontStrikeout = TriState.Empty;
                }
                else
                {
                    this.RowFormatStyle.FontStrikeout = TriState.True;
                }
            }
        }

        public bool? AlternatingColorsOverride { get; set; }

        public Color SelectedBackColor
        {
            get => SELECTED_BACK_COLOUR;
            set => SELECTED_BACK_COLOUR = value;
        }
        #endregion

        #region OnSelectionChanged
        protected override void OnSelectionChanged( EventArgs e )
        {
            //while applying the filter the selected row index is either -2 or -3 and 
            //we dont want to invoke SelectionChange event to keep grid responding
            if (!m_bApplyingFilter && this.Row != -3  && this.Row != -2)
            {
                base.OnSelectionChanged(e);
            }

            if (base.FilterRow != null)
            {

                int FILTER_ROW_INDEX = base.FilterRow.Position;

                if (this.Row == FILTER_ROW_INDEX)
                {
                    applyFilterColours();
                }

                else
                {
                    removeFilterColours();
                }

            }

        }
        #endregion

        #region applyFilterColours
        private void applyFilterColours()
        {
            // These colours are only used here, no need for them outside.
            Color FOCUS_CELL_BACK_COLOUR = Color.FromArgb( 099, 142, 172 );
            Color FOCUS_CELL_FORE_COLOUR = Color.FromArgb( 255, 255, 255 );
            Color FOCUS_ROW_BACK_COLOUR = Color.FromArgb( 230, 237, 242 );
            Color FOCUS_ROW_FORE_COLOUR = Color.FromArgb( 0, 0, 0 );

            // These control the style for cell in focus.
            // NOTE: The colour of the filter row is being changed becuase that is what the 
            // focus back colour on the filter row is.
            base.FilterRowFormatStyle.BackColor = FOCUS_CELL_BACK_COLOUR;
            base.FocusCellFormatStyle.ForeColor = FOCUS_CELL_FORE_COLOUR;

            // Hide the colour of the filter thorughout the row EXCEPT where the focus is on.
            base.FilterRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Transparent;

            // These control the seleceted row apperence.
            base.SelectedFormatStyle.BackColor = FOCUS_ROW_BACK_COLOUR;
            base.SelectedFormatStyle.ForeColor = FOCUS_ROW_FORE_COLOUR;

        }
        #endregion

        #region removeFilterColours
        private void removeFilterColours()
        {

            // These control the style for cell in focus.
            base.FilterRowFormatStyle.BackColor = FILTER_BACK_COLOUR;
            base.FocusCellFormatStyle.ForeColor = SELECTED_FORE_COLOUR;

            // Show the back colour of the filter thorughout the row.
            base.FilterRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;

            // These control the seleceted row apperence.
            base.SelectedFormatStyle.BackColor = SELECTED_BACK_COLOUR;
            base.SelectedFormatStyle.ForeColor = SELECTED_FORE_COLOUR;

        }
        #endregion

        #region MoveToAndSelectRow
        /// <summary>
        /// Move to and select the specified row in the datagrid
        /// </summary>
        ///
        /// <param name="rowIndex">The row number to select</param>
        /// 
        ///	AUTHOR	DATE		MR/ER							DESCRIPTION
        ///	------- ----------- ------------------------------- -------------------
        ///	JonK	Feb 19/04	v8.0 ER jmcleod-256 [Dec 1/03]	Initial Creation
        public int MoveToAndSelectRow(int rowIndex)
        {
            int rtnVal;

            // Select the indicated row in the grid.
            if (this.RootTable.GridEX.RowCount > 0 && rowIndex < this.RootTable.GridEX.RowCount)
            {
                this.RootTable.GridEX.MoveTo(rowIndex);
                this.RootTable.GridEX.EnsureVisible(rowIndex);
                this.RootTable.GridEX.Select();

                rtnVal = rowIndex;
            }
            else
            {
                rtnVal = -1;
            }

            return rtnVal;
        }
        #endregion 

        #region VJanusDataGridModernized_GridButtonClicked
        public void VJanusDataGridModernized_GridButtonClicked( int rowNum, GridEXColumn theCol, bool headerClicked )
        {
            // Empty method. 
        }
        #endregion

        #region SetStyles
        public void SetStyles()
        {

            this.AlternatingColors = ( AlternatingColorsOverride != null ) ? AlternatingColorsOverride.Value : true;
            
            // Need to make sure the root table is not null so that
            // it doesn't blow up in the for loop.
            if( this.RootTable != null )
            {

                //this will add margins to properly center / pad the text in each row
                for( int i = 0; i < this.RootTable.Columns.Count; i++ )
                {
                    //this.RootTable.Columns[i].TopMargin = 10;// 6;
                    //this.RootTable.Columns[i].MaxLines = 2;
                    this.RootTable.Columns[ i ].TopMargin = 2;
                    this.RootTable.Columns[ i ].BottomMargin = 2;
                }

            }

            // These control the alternating row apperence.
            base.AlternatingColors = true;
            base.AlternatingRowFormatStyle.Appearance = FLAT;
            base.AlternatingRowFormatStyle.BackColor = ALTERNATING_ROW_COLOUR;
            base.AlternatingRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            base.AlternatingRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.AlternatingRowFormatStyle.ForeColor = FORE_COLOUR;
            base.AlternatingRowFormatStyle.TextAlignment = NEAR;
            base.AlternatingRowFormatStyle.LineAlignment = NEAR;

            // Basic apperence.
            base.BackColor = BACKGROUND_COLOUR;
            base.BorderStyle = Janus.Windows.GridEX.BorderStyle.None;
            base.BlendColor = HEADER_BACK_COLOUR;

            // These control the card style.
            base.CardCaptionFormatStyle.Appearance = FLAT;
            base.CardCaptionFormatStyle.BackColor = BACK_COLOUR;
            base.CardCaptionFormatStyle.BackColorGradient = GRID_COLOUR;
            base.CardCaptionFormatStyle.ForeColor = FORE_COLOUR;

            // These control the card header style.
            base.CardColumnHeaderFormatStyle.Appearance = FLAT;
            base.CardColumnHeaderFormatStyle.BackColor = BACK_COLOUR;
            base.CardColumnHeaderFormatStyle.BackColorGradient = GRID_COLOUR;
            base.CardColumnHeaderFormatStyle.ForeColor = FORE_COLOUR;

            // This control the control style.
            base.ControlStyle.ButtonAppearance = Janus.Windows.GridEX.ButtonAppearance.FlatBorderless;

            // These control the filter row apperence.
            base.FilterRowFormatStyle.Appearance = FLAT;
            base.FilterRowFormatStyle.BackColor = FILTER_BACK_COLOUR;
            base.FilterRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            base.FilterRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.FilterRowFormatStyle.ForeColor = FORE_COLOUR;
            base.FilterRowFormatStyle.TextAlignment = NEAR;
            base.FilterRowFormatStyle.LineAlignment = CENTER;
            this.FilterRowFormatStyle.FontSize = 9F;

            base.Font = new System.Drawing.Font( "Segoe UI", 10 );
            base.FlatBorderColor = BORDER_COLOUR;

            base.DefaultForeColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;

            // These control the style for rows in focus.
            base.FocusCellFormatStyle.Appearance = FLAT;
            base.FocusCellFormatStyle.BackColor = SELECTED_BACK_COLOUR;
            base.FocusCellFormatStyle.BackColorGradient = GRID_COLOUR;
            base.FocusCellFormatStyle.ForeColor = SELECTED_FORE_COLOUR;
            base.FocusCellFormatStyle.TextAlignment = NEAR;
            base.FocusCellFormatStyle.LineAlignment = NEAR;
            base.FocusStyle = Janus.Windows.GridEX.FocusStyle.Solid;

            // These control the grid lines apperence.
            base.GridLineColor = GRID_COLOUR;
            base.GridLines = Janus.Windows.GridEX.GridLines.Both;
            base.GridLineStyle = Janus.Windows.GridEX.GridLineStyle.Solid;

            // These control the group by box style.
            base.GroupByBoxFormatStyle.Appearance = FLAT;
            base.GroupByBoxFormatStyle.BackColor = GROUP_HEADER_BACK_COLOUR;
            base.GroupByBoxFormatStyle.BackColorGradient = GRID_COLOUR;
            base.GroupByBoxFormatStyle.ForeColor = FORE_COLOUR;
            base.GroupByBoxFormatStyle.TextAlignment = NEAR;
            base.GroupByBoxFormatStyle.LineAlignment = CENTER;

            // These control the group by box info style.
            base.GroupByBoxInfoFormatStyle.Appearance = FLAT;
            base.GroupByBoxInfoFormatStyle.BackColor = GROUP_HEADER_BACK_COLOUR;
            base.GroupByBoxInfoFormatStyle.BackColorGradient = GRID_COLOUR;
            base.GroupByBoxInfoFormatStyle.ForeColor = FORE_COLOUR;
            base.GroupByBoxInfoFormatStyle.TextAlignment = NEAR;
            base.GroupByBoxInfoFormatStyle.LineAlignment = CENTER;

            // These control the group row style.
            base.GroupRowFormatStyle.Appearance = FLAT;
            base.GroupRowFormatStyle.BackColor = GROUP_HEADER_BACK_COLOUR;
            base.GroupRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.GroupRowFormatStyle.ForeColor = FORE_COLOUR;
            base.GroupRowFormatStyle.TextAlignment = NEAR;
            base.GroupRowFormatStyle.LineAlignment = CENTER;

            // These control the group total row style.
            base.GroupTotalRowFormatStyle.Appearance = FLAT;
            base.GroupTotalRowFormatStyle.BackColor = GROUP_HEADER_BACK_COLOUR;
            base.GroupTotalRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.GroupTotalRowFormatStyle.ForeColor = FORE_COLOUR;
            base.GroupTotalRowFormatStyle.TextAlignment = NEAR;
            base.GroupTotalRowFormatStyle.LineAlignment = NEAR;

            // These control the header apperence.
            base.HeaderFormatStyle.Appearance = FLAT;
            base.HeaderFormatStyle.BackColor = HEADER_BACK_COLOUR;
            base.HeaderFormatStyle.BackColorGradient = GRID_COLOUR;
            base.HeaderFormatStyle.Font = new Font( "Segoe UI", 10,GraphicsUnit.Point);
            base.HeaderFormatStyle.ForeColor = HEADER_FORE_COLOUR;
            base.HeaderFormatStyle.TextAlignment = NEAR;
            base.HeaderFormatStyle.LineAlignment = CENTER;

            // The Janus Grid will always highlight the selected row
            // even if the Janus Grid is not in focus.
            base.HideSelection = Janus.Windows.GridEX.HideSelection.HighlightInactive;

            // These control the link style.
            base.LinkFormatStyle.Appearance = FLAT;
            base.LinkFormatStyle.BackColor = BACK_COLOUR;
            base.LinkFormatStyle.BackColorGradient = GRID_COLOUR;
            // Make the font look like a true hyperlink.
            base.LinkFormatStyle.Font = new System.Drawing.Font( "Segoe UI", 10, FontStyle.Underline );
            base.LinkFormatStyle.TextAlignment = NEAR;
            base.LinkFormatStyle.LineAlignment = CENTER;

            // These control the new row style.
            base.NewRowFormatStyle.Appearance = FLAT;
            base.NewRowFormatStyle.BackColor = BACK_COLOUR;
            base.NewRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.NewRowFormatStyle.Font = new Font("Segoe UI", 10, GraphicsUnit.Point);
            base.NewRowFormatStyle.ForeColor = FORE_COLOUR;
            base.NewRowFormatStyle.TextAlignment = NEAR;
            base.NewRowFormatStyle.LineAlignment = NEAR;

            // These control the preview row style.
            base.PreviewRowFormatStyle.Appearance = FLAT;
            base.PreviewRowFormatStyle.BackColor = ALTERNATING_ROW_COLOUR;
            base.PreviewRowFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Empty;
            base.PreviewRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.PreviewRowFormatStyle.ForeColor = FORE_COLOUR;
            base.PreviewRowFormatStyle.TextAlignment = NEAR;
            base.PreviewRowFormatStyle.LineAlignment = NEAR;

            // These control the row style.
            base.RowFormatStyle.Appearance = FLAT;
            base.RowFormatStyle.BackColor = ROW_BACK_COLOUR;
            base.RowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.RowFormatStyle.Font = new Font("Segoe UI", 10, GraphicsUnit.Point);
            base.RowFormatStyle.ForeColor = FORE_COLOUR;
            base.RowFormatStyle.TextAlignment = NEAR;
            base.RowFormatStyle.LineAlignment = NEAR;

            // These control the row header style.
            base.RowHeaderFormatStyle.Appearance = FLAT;
            base.RowHeaderFormatStyle.BackColor = BACK_COLOUR;
            base.RowHeaderFormatStyle.BackColorGradient = GRID_COLOUR;
            base.RowHeaderFormatStyle.ForeColor = HEADER_FORE_COLOUR;
            base.RowHeaderFormatStyle.TextAlignment = NEAR;
            base.RowHeaderFormatStyle.LineAlignment = CENTER;
            base.RowHeaderFormatStyle.Font = new Font("Segoe UI", 10, GraphicsUnit.Point);

            // These control the row with errors style.
            base.RowWithErrorsFormatStyle.Appearance = FLAT;
            base.RowWithErrorsFormatStyle.BackColor = BACK_COLOUR;
            base.RowWithErrorsFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Transparent;
            base.RowWithErrorsFormatStyle.BackColorGradient = GRID_COLOUR;
            base.RowWithErrorsFormatStyle.ForeColor = FORE_COLOUR;
            base.RowWithErrorsFormatStyle.TextAlignment = NEAR;
            base.RowWithErrorsFormatStyle.LineAlignment = CENTER;

            // These control the seleceted row apperence.
            base.SelectedFormatStyle.Appearance = FLAT;
            base.SelectedFormatStyle.BackColor = SELECTED_BACK_COLOUR;
            base.SelectedFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            base.SelectedFormatStyle.BackColorGradient = GRID_COLOUR;
            base.SelectedFormatStyle.ForeColor = SELECTED_FORE_COLOUR;
            base.SelectedFormatStyle.TextAlignment = NEAR;
            base.SelectedFormatStyle.LineAlignment = NEAR;

            // These control the seleceted inactive row apperence.
            base.SelectedInactiveFormatStyle.Appearance = FLAT;
            base.SelectedInactiveFormatStyle.BackColor = SELECTED_BACK_COLOUR;
            base.SelectedInactiveFormatStyle.BackColorAlphaMode = Janus.Windows.GridEX.AlphaMode.Opaque;
            base.SelectedInactiveFormatStyle.BackColorGradient = GRID_COLOUR;
            base.SelectedInactiveFormatStyle.ForeColor = SELECTED_FORE_COLOUR;
            base.SelectedInactiveFormatStyle.TextAlignment = NEAR;
            base.SelectedInactiveFormatStyle.LineAlignment = NEAR;

            // These control the seleceted inactive row apperence.
            base.TableHeaderFormatStyle.Appearance = FLAT;
            base.TableHeaderFormatStyle.BackColor = SELECTED_BACK_COLOUR;
            base.TableHeaderFormatStyle.BackColorGradient = GRID_COLOUR;
            base.TableHeaderFormatStyle.ForeColor = SELECTED_FORE_COLOUR;
            base.TableHeaderFormatStyle.TextAlignment = NEAR;
            base.TableHeaderFormatStyle.LineAlignment = CENTER;
            base.TableHeaderFormatStyle.Font = new Font("Segoe UI", 10, GraphicsUnit.Point);

            // These control the seleceted inactive row apperence.
            base.TotalRowFormatStyle.Appearance = FLAT;
            base.TotalRowFormatStyle.BackColor = SELECTED_BACK_COLOUR;
            base.TotalRowFormatStyle.BackColorGradient = GRID_COLOUR;
            base.TotalRowFormatStyle.ForeColor = SELECTED_FORE_COLOUR;
            base.TotalRowFormatStyle.TextAlignment = NEAR;
            base.TotalRowFormatStyle.LineAlignment = CENTER;

        }
        #endregion

    }

}
