using Telerik.Windows.Controls;

namespace PatchPlanner.PropertyEditors
{
    public class PopulationEditor : RadNumericUpDown
    {
        public PopulationEditor()
        {
            this.Minimum = 10;
            this.Maximum = 100;
            this.IsInteger = true;
            this.SmallChange = 1;
        }
    }
}