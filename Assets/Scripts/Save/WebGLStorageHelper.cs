#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif
using UnityEngine;

public static class WebGLStorageHelper
{
#if UNITY_WEBGL && !UNITY_EDITOR
    // Вызываем JS-функцию из .jslib
    [DllImport("__Internal")]
    private static extern void SyncFiles();
#endif

    /// <summary>
    /// Синхронизирует виртуальную файловую систему (RAM → IndexedDB)
    /// чтобы изменения не потерялись после перезапуска страницы.
    /// </summary>
    public static void Flush()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        SyncFiles();
        Debug.Log("[WebGLStorageHelper] FS synced to IndexedDB");
#else
        // В Editor или Standalone эта операция не нужна
        // но выводим лог для прозрачности
        Debug.Log("[WebGLStorageHelper] Not WebGL — sync skipped");
#endif
    }
}
