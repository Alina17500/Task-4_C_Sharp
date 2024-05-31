using System.ComponentModel;
using System.Windows.Input;
using System.Runtime.CompilerServices;

namespace Task_4._1
{
    internal class OilRigViewModel : INotifyPropertyChanged
    {
        private OilRig oilRig;
        private Mechanic mechanic;
        private Loader loader;
        private string logText = "Лог событий:\n";
        private bool isFireActive;
        private bool isProductionRunning;

        public string LogText
        {
            get => logText;
            set
            {
                logText = value;
                OnPropertyChanged();
            }
        }

        public bool IsFireActive
        {
            get => isFireActive;
            set
            {
                isFireActive = value;
                OnPropertyChanged();
            }
        }

        public bool IsProductionRunning
        {
            get => isProductionRunning;
            set
            {
                isProductionRunning = value;
                OnPropertyChanged();
            }
        }

        public ICommand ToggleProductionCommand { get; }
        public ICommand ExtinguishFireCommand { get; }

        public OilRigViewModel()
        {
            oilRig = new OilRig();
            mechanic = new Mechanic();
            loader = new Loader();

            oilRig.FireEvent += () =>
            {
                LogText += mechanic.OnFire() + "\n";
                IsFireActive = true;
                IsProductionRunning = false;
            };
            oilRig.ProductionResumed += () =>
            {
                LogText += "Производство возобновлено.\n";
                IsProductionRunning = true; // Убедитесь, что это место правильно обновляет статус
            };
            oilRig.ProductionStopped += () =>
            {
                LogText += "Производство остановлено.\n";
                IsProductionRunning = false;
            };
            oilRig.OilOverflowEvent += (amount) =>
            {
                LogText += loader.LoadOil(amount) + "\n";
            };
            oilRig.LogUpdate += (message) =>
            {
                LogText += message + "\n";
            };

            ToggleProductionCommand = new RelayCommand(ToggleProduction);
            ExtinguishFireCommand = new RelayCommand(ExtinguishFire, () => IsFireActive);
        }

        private void ToggleProduction()
        {
            if (!IsProductionRunning)
            {
                oilRig.StartProduction();
                LogText += "Производство начато.\n";
                IsProductionRunning = true;
            }
            else
            {
                oilRig.StopProduction();
            }
        }

        private void ExtinguishFire()
        {
            LogText += mechanic.OnFire() + "\n";
            oilRig.ResumeProduction();
            IsFireActive = false;
            IsProductionRunning = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
