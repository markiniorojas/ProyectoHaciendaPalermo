import { bootstrapApplication } from '@angular/platform-browser';
import { ApplicationConfig } from '@angular/core';
import { provideRouter, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { routes } from './app.routes'; 
import { provideHttpClient } from '@angular/common/http';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
  provideHttpClient()
  ] 
};

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));