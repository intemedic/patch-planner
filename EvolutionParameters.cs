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
        [Category("Initialization")]
        public int InitialPatchCount
        {
            get => Settings.Default.InitialPatchCount;
            set
            {
                if (Settings.Default.InitialPatchCount == value)
                {
                    return;
                }
                Settings.Default.InitialPatchCount = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.InitialPatchCount));
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

        [Editor(typeof(SeedEditor), "Value")]
        [Category("Breeding")]
        public int Continents
        {
            get => Settings.Default.Continents;
            set
            {
                if (Settings.Default.Continents == value)
                {
                    return;
                }
                Settings.Default.Continents = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.Continents));
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

        [Editor(typeof(ConquerIntervalEditor), "Value")]
        [Category("Conquer")]
        public int ConquerInterval
        {
            get => Settings.Default.ConquerInterval;
            set
            {
                if (Settings.Default.ConquerInterval == value)
                {
                    return;
                }
                Settings.Default.ConquerInterval = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.ConquerInterval));
            }
        }

        [Editor(typeof(MaxGenerationCountEditor), "Value")]
        [Category("Conquer")]
        public int ConquerStartingGeneration
        {
            get => Settings.Default.ConquerStartingGeneration;
            set
            {
                if (Settings.Default.ConquerStartingGeneration == value)
                {
                    return;
                }
                Settings.Default.ConquerStartingGeneration = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.ConquerStartingGeneration));
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

        [Editor(typeof(MutationRateEditor), "Value")]
        [Category("Mutation")]
        public double PatchCountMutationRate
        {
            get => Settings.Default.PatchCountMutationRate;
            set
            {
                if (Settings.Default.PatchCountMutationRate == value)
                {
                    return;
                }
                Settings.Default.PatchCountMutationRate = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.PatchCountMutationRate));
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

    }
}