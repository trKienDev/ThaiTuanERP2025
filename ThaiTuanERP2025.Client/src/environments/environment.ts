export const environment = {
      production: false,

      // ===== MAIN ERP SERVER =====
      server: {
            baseUrl: 'https://localhost:7228',
            apiUrl: 'https://localhost:7228/api',
            hubs: {
                  notification: '/hubs/notifications'
            }
      },

      // ===== DRIVE / STORAGE SERVICE =====
      drive: {
            baseUrl: 'https://localhost:7040',
            apiUrl: 'https://localhost:7040/api'
      }
};