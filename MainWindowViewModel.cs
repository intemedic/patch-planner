using System.Threading.Tasks;

namespace PatchPlanner
{
    internal class MainWindowViewModel : ViewModelBase
    {
        private Evolution _evolution;

        public MainWindowViewModel()
        {
            this.StartCommand = new DelegateCommand<object>(this.Start);
        }

        public Evolution Evolution
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

       

        public EvolutionParameters EvolutionParameters { get; }
            = new EvolutionParameters();


        public DelegateCommand<object> StartCommand { get; }

        public AnnotationManager AnnotationManager { get; }
            = new AnnotationManager();

        private void Start(object obj)
        {
            this.Evolution = new Evolution(
                this.EvolutionParameters,
                this.AnnotationManager.Annotations);

            Task.Run(() => this.Evolution.StartAsync());
        }
    }
}