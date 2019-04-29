using System.Diagnostics.CodeAnalysis;
using PatchPlanner.PropertyEditors;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Data.PropertyGrid;

namespace PatchPlanner
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class EvolutionParameters : ViewModelBase
    {
        [Editor(typeof(PatchCountEditor), "Value")]
        public int PatchCount
        {
            get => Settings.Default.PatchCount;
            set
            {
                if (Settings.Default.PatchCount == value)
                {
                    return;
                }
                Settings.Default.PatchCount = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.PatchCount));
            }
        }

        [Editor(typeof(PopulationEditor), "Value")]
        public int Population
        {
            get => Settings.Default.Population;
            set
            {
                if (Settings.Default.Population == value)
                {
                    return;
                }
                Settings.Default.Population = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.Population));
            }
        }


        [Editor(typeof(MaxGenerationCountEditor), "Value")]
        public int MaxGenerationCount
        {
            get => Settings.Default.MaxGenerationCount;
            set
            {
                if (Settings.Default.MaxGenerationCount == value)
                {
                    return;
                }
                Settings.Default.MaxGenerationCount = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MaxGenerationCount));
            }
        }

        [Editor(typeof(KeepRateEditor), "Value")]
        public double KeepRate
        {
            get => Settings.Default.KeepRate;
            set
            {
                if (Settings.Default.KeepRate == value)
                {
                    return;
                }
                Settings.Default.KeepRate = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.KeepRate));
            }
        }

        [Editor(typeof(MutationRateEditor), "Value")]
        public double MutationRate
        {
            get => Settings.Default.MutationRate;
            set
            {
                if (Settings.Default.MutationRate == value)
                {
                    return;
                }
                Settings.Default.MutationRate = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MutationRate));
            }
        }

        public bool AdaptiveMutation
        {
            get => Settings.Default.AdaptiveMutation;
            set
            {
                if (Settings.Default.AdaptiveMutation == value)
                {
                    return;
                }
                Settings.Default.AdaptiveMutation = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.AdaptiveMutation));
            }
        }

        public bool Monogenesis
        {
            get => Settings.Default.Monogenesis;
            set
            {
                if (Settings.Default.Monogenesis == value)
                {
                    return;
                }
                Settings.Default.Monogenesis = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.Monogenesis));
            }
        }
    }
}