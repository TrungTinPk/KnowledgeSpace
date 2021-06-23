import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MonthlyNewKbsComponent } from './monthly-new-kbs/monthly-new-kbs.component';
import { MonthlyNewCommentsComponent } from './monthly-new-comments/monthly-new-comments.component';
import { MonthlyNewMembersComponent } from './monthly-new-members/monthly-new-members.component';
import {AuthGuard} from "@app/shared";

const routes: Routes = [
    {
        path: '',
        component: MonthlyNewKbsComponent
    },
    {
        path: 'monthly-new-comments',
        data: {
            functionCode: 'STATISTIC_MONTHLY_COMMENT'
        },
        component: MonthlyNewCommentsComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'monthly-new-kbs',
        data: {
            functionCode: 'STATISTIC_MONTHLY_NEWKB'
        },
        component: MonthlyNewKbsComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'monthly-new-members',
        data: {
            functionCode: 'STATISTIC_MONTHLY_NEWMEMBER'
        },
        component: MonthlyNewMembersComponent,
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StatisticsRoutingModule {}
