﻿using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner.PropertyEditors
{
    public class KeepRateEditor : RadNumericUpDown
    {
        public KeepRateEditor()
        {
            this.Minimum = 0;
            this.Maximum = 1;
            this.IsInteger = false;
            this.NumberDecimalDigits = 2;
            this.SmallChange = 0.1;
        }
    }
}