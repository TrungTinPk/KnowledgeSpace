import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { FunctionsComponent } from './functions/functions.component';
import { UsersComponent } from './users/users.component';
import { RolesComponent } from './roles/roles.component';
import { PermissionsComponent } from './permissions/permissions.component';
import { SystemsRoutingModule } from './systems-routing.module';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { NotificationService } from '@app/shared/services';
import { PanelModule } from 'primeng/panel';
import { ButtonModule } from 'primeng/button';
import { TableModule } from 'primeng/table';
import { PaginatorModule } from 'primeng/paginator';
import { BlockUIModule } from 'primeng/blockui';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ProgressSpinnerModule } from 'primeng/progressspinner';
import { ValidationMessageModule } from '@app/shared';
import { RolesDetailComponent } from './roles/roles-detail/roles-detail.component';
import { InputTextModule } from 'primeng/inputtext';
import { CardModule } from 'primeng/card';
import { ToolbarModule } from 'primeng/toolbar';
import { UsersDetailComponent } from './users/users-detail/users-detail.component';
import { RolesAssignComponent } from './users/roles-assign/roles-assign.component';
import { CheckboxModule } from 'primeng/checkbox';
import { KeyFilterModule } from 'primeng/keyfilter';
import { CalendarModule } from 'primeng/calendar';
import { FunctionsDetailComponent } from './functions/functions-detail/functions-detail.component';
import { CommandsAssignComponent } from './functions/commands-assign/commands-assign.component';
import {TreeTableModule} from "primeng/treetable";
import {SharedDirectivesModule} from "@app/shared/directives/shared-directives.module";


@NgModule({
  declarations: [
    FunctionsComponent,
    UsersComponent,
    RolesComponent,
    PermissionsComponent,
    RolesDetailComponent,
    UsersDetailComponent,
    RolesAssignComponent,
    FunctionsDetailComponent,
    CommandsAssignComponent
  ],
    imports: [
        CommonModule,
        SystemsRoutingModule,
        PanelModule,
        ButtonModule,
        TableModule,
        InputTextModule,
        PaginatorModule,
        BlockUIModule,
        FormsModule,
        ReactiveFormsModule,
        ProgressSpinnerModule,
        ValidationMessageModule,
        ModalModule.forRoot(),
        CardModule,
        ToolbarModule,
        CheckboxModule,
        KeyFilterModule,
        CalendarModule,
        TreeTableModule,
        SharedDirectivesModule
    ],
  providers: [
      NotificationService,
      BsModalService,
      DatePipe
  ]
})
export class SystemsModule { }
