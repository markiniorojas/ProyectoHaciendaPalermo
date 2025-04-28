import { Routes } from '@angular/router';
import { PrincipalComponent } from './principal/principal.component';
import { FormComponent } from './form/form.component';
import { PersonComponent } from './person/person.component';
import { ModuleComponent } from './module/module.component';
import { PermissionComponent } from './permission/permission.component';
import { RolComponent } from './rol/rol.component';
import { UserComponent } from './user/user.component';



export const routes: Routes = [
    {path: '', redirectTo: 'principal', pathMatch: 'full'},
    {path: 'principal',component: PrincipalComponent,
        children: [
            { path: 'form', component: FormComponent },
            { path: 'person', component: PersonComponent },
            { path: 'module', component: ModuleComponent },
            { path: 'permission', component: PermissionComponent },
            { path: 'rol', component: RolComponent},
            { path: 'user', component: UserComponent},
            { path: '', redirectTo: '', pathMatch: 'full'}
        ]
    }
];  
