import { Routes } from '@angular/router';
import { PrincipalComponent } from './principal/principal.component';
import { FormComponent } from './form/form.component';
import { PersonComponent } from './person/person.component';
import { ModuleComponent } from './module/module.component';

export const routes: Routes = [
    {path: '', redirectTo: 'principal', pathMatch: 'full'},
    {path: 'principal',component: PrincipalComponent,
        children: [
            { path: 'form', component: FormComponent },
            { path: 'person', component: PersonComponent },
            { path: 'module', component: ModuleComponent },
            { path: '', redirectTo: '', pathMatch: 'full'}
        ]
    }
];  
