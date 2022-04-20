using JetBrains.Annotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace FollowBot.Class
{
    public class FlasksClass : INotifyPropertyChanged
    {
        private int _slot;
        private bool _enabled;
        private bool _useEs;
        private bool _useMana;
        private int _threshold;

        public FlasksClass()
        {
            
        }

        public FlasksClass(bool enabled, int slot, bool useEs, bool useMana, int threshold)
        {
            Enabled = enabled;
            Slot = slot;
            UseEs = useEs;
            UseMana = useMana;
            Threshold = threshold;
        }

        public int Slot
        {
            get { return _slot; }
            set
            {
                _slot = value;
                NotifyPropertyChanged(nameof(Slot));
            }
        }
        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                NotifyPropertyChanged(nameof(Enabled));
            }
        }
        public bool UseEs
        {
            get { return _useEs; }
            set
            {
                _useEs = value;
                NotifyPropertyChanged(nameof(UseEs));
            }
        }
        public bool UseMana
        {
            get { return _useMana; }
            set
            {
                _useMana = value;
                NotifyPropertyChanged(nameof(UseMana));
            }
        }
        public int Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                NotifyPropertyChanged(nameof(Threshold));
            }
        }

        [JsonIgnore]
        public readonly Stopwatch PostUseDelay = Stopwatch.StartNew();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
