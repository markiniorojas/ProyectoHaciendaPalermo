import { Routes } from '@angular/router';
import { PrincipalComponent } from './principal/principal.component';
import { FormComponent } from './form/form.component';

export const routes: Routes = [
    {path: '', redirectTo: 'principal', pathMatch: 'full'},
    {path: 'principal',component: PrincipalComponent,
        children: [
            { path: 'form', component: FormComponent },
            { path: '', redirectTo: '', pathMatch: 'full'}
        ]
    }
];  
