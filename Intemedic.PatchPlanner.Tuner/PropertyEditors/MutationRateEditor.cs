using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner.PropertyEditors
{
    public class MutationRateEditor : RadNumericUpDown
    {
        public MutationRateEditor()
        {
            this.Minimum = 0;
            this.Maximum = 0.5;
            this.IsInteger = false;
            this.NumberDecimalDigits = 2;
            this.SmallChange = 0.1;
        }
    }
}