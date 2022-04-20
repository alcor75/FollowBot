using JetBrains.Annotations;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace FollowBot.Class
{
    public class DefensiveSkillsClass : INotifyPropertyChanged
    {
        private Stopwatch delaySw = Stopwatch.StartNew();
        private string _name { get; set; }
        private bool _enabled { get; set; }
        private bool _useEs { get; set; }
        private int _threshold { get; set; }
        private int _sleepSeconds { get; set; }
        private bool _castOnLeader { get; set; }

        public DefensiveSkillsClass()
        {
            
        }

        public DefensiveSkillsClass(bool enabled, string name, bool useEs, int threshold, int sleepSeconds, bool castOnLeader)
        {
            Enabled = enabled;
            Name = name;
            UseEs = useEs;
            Threshold = threshold;
            SleepSeconds = sleepSeconds;
            CastOnLeader = castOnLeader;
        }
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged(nameof(Name));
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
        public int Threshold
        {
            get { return _threshold; }
            set
            {
                _threshold = value;
                NotifyPropertyChanged(nameof(Threshold));
            }
        }
        public int SleepSeconds
        {
            get { return _sleepSeconds; }
            set
            {
                _sleepSeconds = value;
                NotifyPropertyChanged(nameof(SleepSeconds));
            }
        }
        public bool CastOnLeader
        {
            get { return _castOnLeader; }
            set
            {
                _castOnLeader = value;
                NotifyPropertyChanged(nameof(CastOnLeader));
            }
        }

        public bool IsReadyToCast
        {
            get
            {
                return  delaySw.ElapsedMilliseconds > SleepSeconds * 1000;
            }
        }
        public void Casted()
        {
            delaySw.Restart();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
