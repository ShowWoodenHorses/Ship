mergeInto(LibraryManager.library, {
  SyncFiles: function () {
    if (typeof FS !== 'undefined' && FS.syncfs) {
      FS.syncfs(false, function (err) {
        if (err) console.error('FS sync error:', err);
        else console.log('[FSHelper] FS synced');
      });
    } else {
      console.warn('[FSHelper] FS not found');
    }
  }
});
