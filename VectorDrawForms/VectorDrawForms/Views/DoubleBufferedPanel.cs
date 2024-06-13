using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VectorDrawForms.Processors;

namespace VectorDrawForms.Views
{
    public partial class DoubleBufferedPanel : ResizableUserControl
    {
        #region Fields
        /// <summary>
        /// An aggregated dialog processor in the form makes easier the manipulation of the model.
        /// </summary>
        private DialogProcessor dialogProcessor = new DialogProcessor();
        #endregion

        #region Constructor
        public DoubleBufferedPanel()
        {
            InitializeComponent();
        }
        #endregion

        #region Properties
        public DialogProcessor DialogProcessor 
        { 
            get { return dialogProcessor; } 
        }
        #endregion
    }
}
