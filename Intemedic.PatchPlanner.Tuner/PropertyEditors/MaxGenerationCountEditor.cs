using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner.PropertyEditors
{
    public class MaxGenerationCountEditor : RadNumericUpDown
    {
        public MaxGenerationCountEditor()
        {
            this.Minimum = 1000;
            this.Maximum = 999000;
            this.IsInteger = true;
            this.SmallChange = 1000;
        }
    }
}