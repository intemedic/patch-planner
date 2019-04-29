using Telerik.Windows.Controls;

namespace PatchPlanner.PropertyEditors
{
    public class ConquerIntervalEditor : RadNumericUpDown
    {
        public ConquerIntervalEditor()
        {
            this.Minimum = 10;
            this.Maximum = 10000;
            this.IsInteger = true;
            this.SmallChange = 100;
        }
    }
}