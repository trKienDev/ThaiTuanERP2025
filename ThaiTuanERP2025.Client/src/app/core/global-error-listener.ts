export function installGlobalErrorListeners() {
      if (typeof window === 'undefined') return; // phòng SSR

      window.addEventListener('error', (e: ErrorEvent) => {
            console.error('[GLOBAL ERROR]', e.error || e.message);
      });

      window.addEventListener('unhandledrejection', (e: PromiseRejectionEvent) => {
            console.error('[GLOBAL PROMISE REJECTION]', e.reason);
      });
}