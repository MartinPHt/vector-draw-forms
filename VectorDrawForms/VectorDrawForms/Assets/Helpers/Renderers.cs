using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace VectorDrawForms.Assets.Helpers
{
    public class ApplicationToolStripRenderer : ToolStripProfessionalRenderer
    {
        #region Fields
        private Brush backgroundCheckedBrush = Brushes.Gray;
        private Brush backgroundHoveredBrush = Brushes.LightGray;
        #endregion

        #region Contructor
        public ApplicationToolStripRenderer(Brush backgroundHoveredBrush, Brush backgroundCheckedBrush)
        {
            this.backgroundHoveredBrush = backgroundHoveredBrush;
            this.backgroundCheckedBrush = backgroundCheckedBrush;
        }
        #endregion

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {

            var btn = e.Item as ToolStripButton;
            if (btn == null)
                return;
            
            if ((btn.CheckOnClick && btn.Checked))
            {
                Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

                using (GraphicsPath path = ShapeUtility.RoundedRect(bounds, 8))
                {
                    e.Graphics.FillPath(backgroundCheckedBrush, path);
                }
            }
            else if (btn.Selected)
            {
                Rectangle bounds = new Rectangle(Point.Empty, e.Item.Size);

                using (GraphicsPath path = ShapeUtility.RoundedRect(bounds, 8))
                {
                    e.Graphics.FillPath(backgroundHoveredBrush, path);
                }
            }
            else
                base.OnRenderButtonBackground(e);

        }
    }
}
