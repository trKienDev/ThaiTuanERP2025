import { ApplicationConfig, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { errorInterceptor } from './core/interceptors/error.interceptor';
import { authInterceptor } from './core/interceptors/auth.interceptor';
import { provideAnimations } from '@angular/platform-browser/animations';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE, provideNativeDateAdapter } from '@angular/material/core';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MondayFirstDateAdapter } from './shared/date/monday-first-date-adapter';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';

export const appConfig: ApplicationConfig = {
      providers: [
            provideZoneChangeDetection({ eventCoalescing: true }), 
            provideRouter(routes),
            provideHttpClient(
                  withInterceptors([
                        authInterceptor,
                        errorInterceptor
                  ])
            ),
            provideAnimations(),
            // mat-date-picker
            { provide: DateAdapter, useClass: MondayFirstDateAdapter },
            { provide: MAT_DATE_LOCALE, useValue: 'vi-VN' },
            { provide: MAT_DATE_FORMATS, useValue: {
                        parse:   { dateInput: 'DD/MM/YYYY' },
                        display: { dateInput: 'dd/MM/yyyy', monthYearLabel: 'MMMM yyyy', dateA11yLabel: 'dd/MM/yyyy', monthYearA11yLabel: 'MMMM yyyy' },
                  }
            },
            { provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: { subscriptSizing: 'dynamic' } },
            // mat-snackbar
            { provide: MAT_SNACK_BAR_DEFAULT_OPTIONS, useValue: {
                  horizontalPosition: 'end',
                  verticalPosition: 'top',
                  duration: 3000
            }}
      ]
};
