import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';  // Importar provideHttpClient
import { appConfig } from './app/app.config';
import { AppComponent } from './app/app.component';

const appWithHttpConfig = {
  ...appConfig,  // Mantén el resto de la configuración que ya tengas
  providers: [
    ...appConfig.providers,  // Si ya tienes proveedores definidos en appConfig
    provideHttpClient()  // Asegúrate de incluir esto aquí
  ]
};

bootstrapApplication(AppComponent, appWithHttpConfig)
  .catch((err) => console.error(err));