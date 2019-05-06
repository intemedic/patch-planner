using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using Intemedic.PatchPlanner.Tuner.PropertyEditors;
using Telerik.Windows.Controls;
using EditorAttribute = Telerik.Windows.Controls.Data.PropertyGrid.EditorAttribute;

namespace Intemedic.PatchPlanner.Tuner
{
    internal class AnnotationManager : ViewModelBase
    {
        private AnnotationCollection _annotations;

        public AnnotationManager()
        {
            this.RegenerateCommand = new DelegateCommand(this.GenerateAnnotations);
            this.GenerateAnnotations();
        }

        [Browsable(false)] public DelegateCommand RegenerateCommand { get; }

        [Browsable(false)]
        public AnnotationCollection Annotations
        {
            get => _annotations;
            private set
            {
                if (ReferenceEquals(_annotations, value))
                {
                    return;
                }

                _annotations = value;

                this.RaisePropertyChanged(nameof(this.Annotations));
            }
        }

        [Editor(typeof(AnnotationCountEditor), "Value")]
        public int AnnotationCount
        {
            get => Settings.Default.AnnotationCount;
            set
            {
                if (Settings.Default.AnnotationCount == value)
                {
                    return;
                }

                Settings.Default.AnnotationCount = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.AnnotationCount));
            }
        }

        [Editor(typeof(AnnotationSizeEditor), "Value")]
        public int MinSize
        {
            get => Settings.Default.MinAnnotationSize;
            set
            {
                if (Settings.Default.MinAnnotationSize == value)
                {
                    return;
                }

                Settings.Default.MinAnnotationSize = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MinSize));
            }
        }

        [Editor(typeof(AnnotationSizeEditor), "Value")]
        public int MaxSize
        {
            get => Settings.Default.MaxAnnotationSize;
            set
            {
                if (Settings.Default.MaxAnnotationSize == value)
                {
                    return;
                }

                Settings.Default.MaxAnnotationSize = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.MaxSize));
            }
        }

        public int Seed
        {
            get => Settings.Default.AnnotationRandomSeed;
            set
            {
                if (Settings.Default.AnnotationRandomSeed == value)
                {
                    return;
                }

                Settings.Default.AnnotationRandomSeed = value;
                Settings.Default.Save();

                this.RaisePropertyChanged(nameof(this.Seed));
            }
        }

        private void GenerateAnnotations(object obj)
        {
            this.GenerateAnnotations();
        }

        private void GenerateAnnotations()
        {
            var annotations = new List<Annotation>();

            if (this.MinSize > this.MaxSize)
            {
                this.Annotations = new AnnotationCollection(annotations);
                return;
            }

            var random = new Random(this.Seed);
            for (var i = 0; i < this.AnnotationCount; ++i)
            {
                var width = random.Next(this.MinSize, this.MaxSize + 1);
                var height = random.Next(this.MinSize, this.MaxSize + 1);

                for (var positionIteration = 0; positionIteration < 10; ++positionIteration)
                {
                    var x = random.Next(0, Constants.CanvasSize - width);
                    var y = random.Next(0, Constants.CanvasSize - height);

                    var bounds = new Rectangle(x, y, width, height);

                    var conflictFound = false;
                    foreach (var annotation in annotations)
                    {
                        if (annotation.Bounds.IntersectsWith(bounds))
                        {
                            conflictFound = true;
                            break;
                        }
                    }

                    if (!conflictFound)
                    {
                        annotations.Add(new Annotation(bounds));
                        break;
                    }
                }
            }

            this.Annotations = new AnnotationCollection(annotations);
        }
    }
}