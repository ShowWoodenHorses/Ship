mergeInto(LibraryManager.library, {
  SyncFiles: function () {                         // функция, которую зовём из C#
    if (typeof FS !== 'undefined' && FS.syncfs) {  // проверяем, что FS доступна (виртуальная ФС Emscripten)
      FS.syncfs(false, function (err) {            // false = синхронизация из памяти в IndexedDB
        // можно вывести в консоль, если нужно: console.log('FS synced', err);
      });
    }
  }
});
