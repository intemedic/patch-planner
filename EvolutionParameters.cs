using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using PatchPlanner.PropertyEditors;
using Telerik.Windows.Controls;
using EditorAttribute = Telerik.Windows.Controls.Data.PropertyGrid.EditorAttribute;

namespace PatchPlanner
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    public class EvolutionParameters : ViewModelBase
    {
        [Editor(typeof(PatchCountEditor), "Value")]
        [Category("Evolution")]
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
        [Category("Breeding")]
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
        [Category("Evolution")]
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
        [Category("Breeding")]
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
        [Category("Mutation")]
        public double MajorMutationRate
        {
            get => Settings.Default.MajorMutationRate;
            set
            {
                if (Settings.Default.MajorMutationRate == value)
                {
                    return;
                }
                Settings.Default.MajorMutationRate = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MajorMutationRate));
            }
        }

        [Editor(typeof(MutationRateEditor), "Value")]
        [Category("Mutation")]
        public double MinorMutationRate
        {
            get => Settings.Default.MinorMutationRate;
            set
            {
                if (Settings.Default.MinorMutationRate == value)
                {
                    return;
                }
                Settings.Default.MinorMutationRate = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MinorMutationRate));
            }
        }

        [Editor(typeof(MutationMagnitudeEditor), "Value")]
        [Category("Mutation")]
        public double MajorMutationMagnitude
        {
            get => Settings.Default.MajorMutationMagnitude;
            set
            {
                if (Settings.Default.MajorMutationMagnitude == value)
                {
                    return;
                }
                Settings.Default.MajorMutationMagnitude = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MajorMutationMagnitude));
            }
        }

        [Editor(typeof(MutationMagnitudeEditor), "Value")]
        [Category("Mutation")]
        public double MinorMutationMagnitude
        {
            get => Settings.Default.MinorMutationMagnitude;
            set
            {
                if (Settings.Default.MinorMutationMagnitude == value)
                {
                    return;
                }
                Settings.Default.MinorMutationMagnitude = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MinorMutationMagnitude));
            }
        }

        [Category("Mutation")]
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

        [Category("Breeding")]
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