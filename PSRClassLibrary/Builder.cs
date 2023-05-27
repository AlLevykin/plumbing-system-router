using System;

namespace PSR
{
    public static class Builder
    {
        public static bool Build(Module module, Action<string> statusCallback = null) 
        {
            statusCallback?.Invoke("Начат расчет системы водоотведения.");
            statusCallback?.Invoke("Построение сетки");
            statusCallback?.Invoke("Расчет минимального покрывающего дерева");
            statusCallback?.Invoke("Подбор фиттингов");
            statusCallback?.Invoke("Подбор редукций");
            statusCallback?.Invoke("Подбор труб");

            return true;
        }
    }
}
