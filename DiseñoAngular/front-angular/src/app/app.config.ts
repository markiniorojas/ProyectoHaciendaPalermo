import { bootstrapApplication } from '@angular/platform-browser';
import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';
import { AppComponent } from './app.component';


export const appConfig : ApplicationConfig = {
    providers: [provideRouter([])] 
  
};

bootstrapApplication(AppComponent, appConfig)
  .catch((err) => console.error(err));  