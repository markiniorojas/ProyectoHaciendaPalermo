import { Routes } from '@angular/router';
import { PrincipalComponent } from './principal/principal.component';

export const routes: Routes = [
    {path: 'principal'. component: PrincipalComponent,
        children:{
            {path: 'form', component: }
        }
    }
];
