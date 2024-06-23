using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VectorDrawForms
{
    public class ApplicationConstants
    {
        #region Fields
        private Timer selectionLineDashTimer;
        #endregion

        #region Constructors
        public ApplicationConstants()
        {
            PrepareInitialState();
        }
        #endregion

        #region Properties
        public static ApplicationConstants Instance { get; } = new ApplicationConstants();

        private Pen selectionPen = new Pen(Color.Black, 1.5f) { DashStyle = DashStyle.Dash };
        public Pen SelectionPen 
        { 
            get { return selectionPen; } 
        }

        private Pen selectionPenInverted = new Pen(Color.White, 1.5f) { DashStyle = DashStyle.Dash, DashOffset = 2 };
        public Pen SelectionPenInverted
        { 
            get { return selectionPenInverted; } 
        }
        #endregion

        #region Methods
        private void PrepareInitialState()
        {
            selectionLineDashTimer = new Timer();
            selectionLineDashTimer.Interval = 300;
            selectionLineDashTimer.Tick += SelectionLineDashTimer_Tick;
            selectionLineDashTimer.Start();
        }

        private void SelectionLineDashTimer_Tick(object sender, EventArgs e)
        {
            selectionPen.DashOffset -= 2;
            if (selectionPen.DashOffset < -10)
                selectionPen.DashOffset = 0;

            selectionPenInverted.DashOffset += 2;
            if (selectionPenInverted.DashOffset > 10)
                selectionPenInverted.DashOffset = 0;
        }
        #endregion
    }
}
