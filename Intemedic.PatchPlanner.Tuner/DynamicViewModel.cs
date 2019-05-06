using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Intemedic.PatchPlanner.Tuner.Annotations;

namespace Intemedic.PatchPlanner.Tuner
{
    public class DynamicViewModel : DynamicObject, INotifyPropertyChanged
    {
        public DynamicViewModel(object model)
        {
            this.Model = model;
        }

        public object Model { get; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(string propertyName)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var property = this.Model.GetType().GetProperty(binder.Name);

            if (property != null)
            {
                result = property.GetValue(this.Model);
                return true;
            }

            return base.TryGetMember(binder, out result);
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            var property = this.Model.GetType().GetProperty(binder.Name);

            if (property != null)
            {
                property.SetValue(this.Model, value);
                this.OnPropertyChanged(binder.Name);
                return true;
            }

            return base.TrySetMember(binder, value);
        }

        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return this.Model.GetType().GetProperties().Select(p => p.Name);
        }
    }
}