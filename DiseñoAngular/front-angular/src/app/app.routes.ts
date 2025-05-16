import { Routes } from '@angular/router';
import { PrincipalComponent } from './principal/principal.component';
import { FormComponent } from './form/form.component';
import { PersonComponent } from './person/person.component';
import { ModuleComponent } from './module/module.component';
import { PermissionComponent } from './permission/permission.component';
import { RolComponent } from './rol/rol.component';
import { UserComponent } from './user/user.component';
import { RoluserComponent } from './roluser/roluser.component';
import { FormmoduleComponent } from './formmodule/formmodule.component';
import { LoginComponent } from './login/login.component';
import { AuthGuard } from './auth/auth.guard'; 

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },

  {
    path: 'principal',
    component: PrincipalComponent,
    canActivate: [AuthGuard], 
    canActivateChild: [AuthGuard],
    children: [
      { path: 'form', component: FormComponent },
      { path: 'person', component: PersonComponent },
      { path: 'module', component: ModuleComponent },
      { path: 'permission', component: PermissionComponent },
      { path: 'rol', component: RolComponent },
      { path: 'user', component: UserComponent },
      { path: 'roluser', component: RoluserComponent },
      { path: 'formmodule', component: FormmoduleComponent },
      { path: '', redirectTo: 'person', pathMatch: 'full' }
    ]
  }
];
