using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner.PropertyEditors
{
    public class AnnotationSizeEditor : RadNumericUpDown
    {
        public AnnotationSizeEditor()
        {
            this.Minimum = 5;
            this.Maximum = 200;
            this.IsInteger = true;
            this.SmallChange = 1;
        }
    }
}