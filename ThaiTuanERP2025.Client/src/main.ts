import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideRouter } from '@angular/router';
import { routes } from './app/app.routes';
import {  provideHttpClient, withInterceptors } from '@angular/common/http';
import { jwtInterceptor } from './app/core/interceptors/jwt.interceptor';
import { BrowserAnimationsModule, provideAnimations } from '@angular/platform-browser/animations';
import { importProvidersFrom } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

bootstrapApplication(AppComponent, {
      providers: [
            provideRouter(routes),
            provideHttpClient(withInterceptors([jwtInterceptor])),
            provideAnimations(),
            importProvidersFrom(MatIconModule),
            importProvidersFrom(BrowserAnimationsModule),
      ]
});