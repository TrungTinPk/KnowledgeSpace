import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UsersComponent } from './users/users.component';
import { FunctionsComponent } from './functions/functions.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { RolesComponent } from './roles/roles.component';
import {AuthGuard} from "@app/shared";

const routes: Routes = [
    {
        path: '',
        component: UsersComponent
    },
    {
        path: 'users',
        data: {
            functionCode: 'SYSTEM_USER'
        },
        component: UsersComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'functions',
        data: {
            functionCode: 'SYSTEM_FUNCTION'
        },
        component: FunctionsComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'permissions',
        data: {
            functionCode: 'SYSTEM_PERMISSION'
        },
        component: PermissionsComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'roles',
        data: {
            functionCode: 'SYSTEM_ROLE'
        },
        component: RolesComponent,
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class SystemsRoutingModule {}
