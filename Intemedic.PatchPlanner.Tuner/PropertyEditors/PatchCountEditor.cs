using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner.PropertyEditors
{
    public class PatchCountEditor : RadNumericUpDown
    {
        public PatchCountEditor()
        {
            this.Minimum = 10;
            this.Maximum = 50;
            this.IsInteger = true;
            this.SmallChange = 1;
        }
    }
}