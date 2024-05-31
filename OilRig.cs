using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_4._1
{
    using System;
    using System.Threading.Tasks;

    public class OilRig
    {
        public event Action FireEvent;
        public event Action<int> OilOverflowEvent;
        public event Action ProductionResumed;
        public event Action ProductionStopped;
        public event Action<string> LogUpdate;

        private int oilBuffer = 0;
        private readonly int capacity = 500;
        private bool isProducing = false;
        private Random random = new Random();

        public bool IsProducing => isProducing;

        public void StartProduction()
        {
            if (isProducing) return; // Если уже в процессе, не перезапускаем
            isProducing = true;
            Task.Run(() =>
            {
                while (isProducing)
                {
                    Thread.Sleep(1000);
                    LogUpdate?.Invoke("Проверка цикла производства.");
                    if (random.NextDouble() < 0.1)
                    {
                        isProducing = false;
                        FireEvent?.Invoke();
                        ProductionStopped?.Invoke();
                        LogUpdate?.Invoke("Пожар на вышке! Производство остановлено.");
                        continue;
                    }

                    int producedOil = random.Next(50, 100);
                    oilBuffer += producedOil;
                    LogUpdate?.Invoke($"Произведено {producedOil} баррелей нефти, текущий буфер: {oilBuffer}.");

                    if (oilBuffer >= capacity)
                    {
                        OilOverflowEvent?.Invoke(oilBuffer);
                        LogUpdate?.Invoke($"Переполнение буфера. Отправка {oilBuffer} баррелей на погрузку.");
                        oilBuffer = 0;
                    }
                }
            });
        }

        public void StopProduction()
        {
            isProducing = false;
            ProductionStopped?.Invoke();
            LogUpdate?.Invoke("Производство остановлено вручную.");
        }

        public void ResumeProduction()
        {
            if (!isProducing) // Убедитесь, что производство не запущено
            {
                isProducing = true;
                ProductionResumed?.Invoke();
                LogUpdate?.Invoke("Производство возобновлено после пожара.");
                StopProduction();
                StartProduction();
            }
        }

    }

}
