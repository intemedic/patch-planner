using Telerik.Windows.Controls;

namespace PatchPlanner.PropertyEditors
{
    public class MutationMagnitudeEditor : RadNumericUpDown
    {
        public MutationMagnitudeEditor()
        {
            this.Minimum = 0;
            this.Maximum = 0.5;
            this.IsInteger = false;
            this.NumberDecimalDigits = 3;
            this.SmallChange = 0.01;
        }
    }
}