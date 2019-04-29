﻿using Telerik.Windows.Controls;

namespace PatchPlanner.PropertyEditors
{
    public class PopulationEditor : RadNumericUpDown
    {
        public PopulationEditor()
        {
            this.Minimum = 10;
            this.Maximum = 1000;
            this.IsInteger = true;
            this.SmallChange = 10;
        }
    }
}