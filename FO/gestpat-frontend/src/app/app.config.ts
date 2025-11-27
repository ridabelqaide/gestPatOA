import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { authTokenInterceptor } from '../app/interceptors/auth-token.interceptor';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient, withInterceptors } from '@angular/common/http'; 
import { FormsModule } from '@angular/forms';   

export const appConfig: ApplicationConfig = {
  providers: [provideZoneChangeDetection({ eventCoalescing: true }), importProvidersFrom(FormsModule), provideRouter(routes), provideClientHydration(), provideHttpClient(withInterceptors([authTokenInterceptor]))]
};
