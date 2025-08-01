using System;
using System.Threading;

namespace Console
{
    // فرض کنید این یک کلاس مدیر رویداد است که در طول عمر برنامه زنده می‌ماند
public static class EventManager
{
    // یک رویداد استاتیک که هرگز از بین نمی‌رود
    public static event EventHandler<string> DataProcessed;

    public static void RaiseDataProcessed(string data)
    {
        DataProcessed?.Invoke(null, data);
    }
}
}

