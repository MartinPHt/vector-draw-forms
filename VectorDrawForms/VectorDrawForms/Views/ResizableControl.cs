using System.Drawing;
using System.Windows.Forms;

namespace VectorDrawForms.Views
{
    public class ResizableUserControl : UserControl
    {
        #region Fields
        private const int ResizeHandleSize = 10;
        private bool isResizing = false;
        private Point lastMousePosition;
        private Cursor currentCursor = Cursors.Default;
        #endregion

        #region Constructor
        public ResizableUserControl()
        {
            this.BackColor = Color.Black;
            this.MouseDown += ResizableUserControl_MouseDown;
            this.MouseMove += ResizableUserControl_MouseMove;
            this.MouseUp += ResizableUserControl_MouseUp;
            this.Paint += ResizableUserControl_Paint;
        }
        #endregion

        #region Properties
        public bool IsResizing 
        { 
            get { return isResizing; } 
            private set 
            { 
                isResizing = value;

                if (value)
                    WasResizingPerformed = true;
            }
        }

        public bool WasResizingPerformed { get; set; }

        /// <summary>
        /// The cursor which will be shown when mouse enters the <see cref="ResizableUserControl"/>.<br></br>
        /// Note that this property does not exclude the case when mouse enters the resize square. Mouse cursor will be changed to SizeNWSE once it enters the resize square no matter the circumstances.
        /// </summary>
        public Cursor CurrentCursor
        {
            get { return currentCursor; }
            set { currentCursor = value; }
        }

        #endregion

        #region Methods
        private void ResizableUserControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (IsInResizeHandle(e.Location))
            {
                IsResizing = true;
                lastMousePosition = e.Location;
            }
        }

        private void ResizableUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                int dx = e.X - lastMousePosition.X;
                int dy = e.Y - lastMousePosition.Y;
                this.Width += dx;
                this.Height += dy;
                lastMousePosition = e.Location;
                this.Invalidate();
            }
            else
            {
                this.Cursor = IsInResizeHandle(e.Location) ? Cursors.SizeNWSE : currentCursor;
            }
        }

        private void ResizableUserControl_MouseUp(object sender, MouseEventArgs e)
        {
            IsResizing = false;
        }

        private void ResizableUserControl_Paint(object sender, PaintEventArgs e)
        {
            using (Brush handleBrush = new SolidBrush(Color.Gray))
            {
                e.Graphics.FillRectangle(handleBrush, this.Width - ResizeHandleSize, this.Height - ResizeHandleSize, ResizeHandleSize, ResizeHandleSize);
            }
        }

        private bool IsInResizeHandle(Point location)
        {
            return location.X >= this.Width - ResizeHandleSize && location.Y >= this.Height - ResizeHandleSize;
        }
        #endregion
    }
}
