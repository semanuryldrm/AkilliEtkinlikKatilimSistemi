namespace AkilliEtkinlikKatilimSistemi.Exceptions;

public sealed class CapacityFullException : Exception
{
    public CapacityFullException(string eventName)
        : base($"'{eventName}' etkinliği için kontenjan dolmuştur. Yeni katılımcı eklenemez.")
    {
    }
}
