using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace Intemedic.PatchPlanner.Tuner
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private EvolutionViewModel _evolution;

        public MainWindowViewModel()
        {
            this.StartCommand = new DelegateCommand(this.Start);
        }

        public EvolutionViewModel Evolution
        {
            get => _evolution;
            private set
            {
                if (_evolution == value)
                {
                    return;
                }

                _evolution = value;

                this.RaisePropertyChanged(nameof(this.Evolution));
            }
        }

        public EvolutionParametersViewModel EvolutionParameters { get; }
            = EvolutionParametersViewModel.Load();

        public DelegateCommand StartCommand { get; }

        public AnnotationManager AnnotationManager { get; }
            = new AnnotationManager();

        private void Start(object obj)
        {
            var evolution = new Evolution(
                this.EvolutionParameters.Model,
                this.AnnotationManager.Annotations);

            this.Evolution = new EvolutionViewModel(evolution);

            Task.Run(() => evolution.StartAsync());
        }
    }
}