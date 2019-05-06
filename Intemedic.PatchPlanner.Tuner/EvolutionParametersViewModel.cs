using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Intemedic.PatchPlanner.Tuner.PropertyEditors;
using Newtonsoft.Json;
using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner
{
    [SuppressMessage("ReSharper", "CompareOfFloatsByEqualityOperator")]
    internal class EvolutionParametersViewModel : ViewModelBase
    {
        private const string EvolutionConfigFile = "evolution.json";

        public EvolutionParametersViewModel(EvolutionParameters evolutionParameters)
        {
            this.Model = evolutionParameters;
        }

        [Browsable(false)]
        public EvolutionParameters Model { get; }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(PatchCountEditor), "Value")]
        [Category("Initialization")]
        public int InitialPatchCount
        {
            get => this.Model.InitialPatchCount;
            set
            {
                if (this.Model.InitialPatchCount == value)
                {
                    return;
                }

                this.Model.InitialPatchCount = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.InitialPatchCount));
            }
        }


        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(PopulationEditor), "Value")]
        [Category("Breeding")]
        public int Population
        {
            get => this.Model.Population;
            set
            {
                if (this.Model.Population == value)
                {
                    return;
                }

                this.Model.Population = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.Population));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(SeedEditor), "Value")]
        [Category("Breeding")]
        public int Continents
        {
            get => this.Model.Continents;
            set
            {
                if (this.Model.Continents == value)
                {
                    return;
                }

                this.Model.Continents = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.Continents));
            }
        }


        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MaxGenerationCountEditor), "Value")]
        [Category("Evolution")]
        public int MaxGenerationCount
        {
            get => this.Model.MaxGenerationCount;
            set
            {
                if (this.Model.MaxGenerationCount == value)
                {
                    return;
                }

                this.Model.MaxGenerationCount = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.MaxGenerationCount));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(ConquerIntervalEditor), "Value")]
        [Category("Conquer")]
        public int ConquerInterval
        {
            get => this.Model.ConquerInterval;
            set
            {
                if (this.Model.ConquerInterval == value)
                {
                    return;
                }

                this.Model.ConquerInterval = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.ConquerInterval));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MaxGenerationCountEditor), "Value")]
        [Category("Conquer")]
        public int ConquerStartingGeneration
        {
            get => this.Model.ConquerStartingGeneration;
            set
            {
                if (this.Model.ConquerStartingGeneration == value)
                {
                    return;
                }

                this.Model.ConquerStartingGeneration = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.ConquerStartingGeneration));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(KeepRateEditor), "Value")]
        [Category("Breeding")]
        public double KeepRate
        {
            get => this.Model.KeepRate;
            set
            {
                if (this.Model.KeepRate == value)
                {
                    return;
                }

                this.Model.KeepRate = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.KeepRate));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MutationRateEditor), "Value")]
        [Category("Mutation")]
        public double MajorMutationRate
        {
            get => this.Model.MajorMutationRate;
            set
            {
                if (this.Model.MajorMutationRate == value)
                {
                    return;
                }

                this.Model.MajorMutationRate = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.MajorMutationRate));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MutationRateEditor), "Value")]
        [Category("Mutation")]
        public double MinorMutationRate
        {
            get => this.Model.MinorMutationRate;
            set
            {
                if (this.Model.MinorMutationRate == value)
                {
                    return;
                }

                this.Model.MinorMutationRate = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.MinorMutationRate));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MutationRateEditor), "Value")]
        [Category("Mutation")]
        public double PatchCountMutationRate
        {
            get => this.Model.PatchCountMutationRate;
            set
            {
                if (this.Model.PatchCountMutationRate == value)
                {
                    return;
                }

                this.Model.PatchCountMutationRate = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.PatchCountMutationRate));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MutationMagnitudeEditor), "Value")]
        [Category("Mutation")]
        public double MajorMutationMagnitude
        {
            get => this.Model.MajorMutationMagnitude;
            set
            {
                if (this.Model.MajorMutationMagnitude == value)
                {
                    return;
                }

                this.Model.MajorMutationMagnitude = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.MajorMutationMagnitude));
            }
        }

        [Telerik.Windows.Controls.Data.PropertyGrid.Editor(typeof(MutationMagnitudeEditor), "Value")]
        [Category("Mutation")]
        public double MinorMutationMagnitude
        {
            get => this.Model.MinorMutationMagnitude;
            set
            {
                if (this.Model.MinorMutationMagnitude == value)
                {
                    return;
                }

                this.Model.MinorMutationMagnitude = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.MinorMutationMagnitude));
            }
        }

        [Category("Mutation")]
        public bool AdaptiveMutation
        {
            get => this.Model.AdaptiveMutation;
            set
            {
                if (this.Model.AdaptiveMutation == value)
                {
                    return;
                }

                this.Model.AdaptiveMutation = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.AdaptiveMutation));
            }
        }


        [Category("Initialization")]
        public InitializationStrategy InitializationStrategy
        {
            get => this.Model.InitializationStrategy;
            set
            {
                if (this.Model.InitializationStrategy == value)
                {
                    return;
                }

                this.Model.InitializationStrategy = value;
                this.SaveConfig();

                this.RaisePropertyChanged(nameof(this.InitializationStrategy));
            }
        }

        public static EvolutionParametersViewModel Load()
        {
            var evolutionParameters = File.Exists(EvolutionConfigFile)
                ? JsonConvert.DeserializeObject<EvolutionParameters>(File.ReadAllText(EvolutionConfigFile))
                : new EvolutionParameters();

            return new EvolutionParametersViewModel(evolutionParameters);
        }

        private void SaveConfig()
        {
            File.WriteAllText(
                EvolutionConfigFile,
                JsonConvert.SerializeObject(this.Model));
        }
    }
}
