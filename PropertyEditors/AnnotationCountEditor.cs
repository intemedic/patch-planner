using Telerik.Windows.Controls;

namespace PatchPlanner.PropertyEditors
{
    public class AnnotationCountEditor : RadNumericUpDown
    {
        public AnnotationCountEditor()
        {
            this.Minimum = 5;
            this.Maximum = 100;
            this.IsInteger = true;
            this.SmallChange = 1;
        }
    }
}