import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE } from '@angular/material/core';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { MondayFirstDateAdapter } from './shared/date/monday-first-date-adapter';
import { MAT_SNACK_BAR_DEFAULT_OPTIONS } from '@angular/material/snack-bar';
import { MAT_DIALOG_DEFAULT_OPTIONS, MatDialogConfig } from '@angular/material/dialog';
import { CorrelationIdInterceptor } from './core/interceptors/correlation-id.interceptor';
import { httpLoggerInterceptor } from './core/interceptors/http-logger.interceptor';
import { AuthInterceptor } from './core/auth/auth.interceptor';
import { HttpErrorInterceptor } from './core/interceptors/http-error.interceptor';
import { MatIconModule } from '@angular/material/icon';

export const appConfig: ApplicationConfig = {
      providers: [
            provideZoneChangeDetection({ eventCoalescing: true }), 
            provideRouter(routes),
            provideHttpClient(
                  withInterceptors([
                        CorrelationIdInterceptor,
                        httpLoggerInterceptor,
                        AuthInterceptor,
                        HttpErrorInterceptor,
                  ])
            ),
            provideAnimations(),
            importProvidersFrom(MatIconModule),
            importProvidersFrom(BrowserAnimationsModule),
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
            }}, 
            
            // mat-dialog
            {
                  provide: MAT_DIALOG_DEFAULT_OPTIONS, 
                  useValue: <MatDialogConfig> {
                        width: 'fit-content',
                        height: 'fit-content',
                        maxWidth: '90vw',
                        maxHeight: '80vh',
                        disableClose: true,
                        autoFocus: false,
                        restoreFocus: false,
                  }
            }
      ]
};
