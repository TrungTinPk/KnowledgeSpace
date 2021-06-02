import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MonthlyNewKbsComponent } from './monthly-new-kbs/monthly-new-kbs.component';
import { MonthlyNewCommentsComponent } from './monthly-new-comments/monthly-new-comments.component';
import { MonthlyNewMembersComponent } from './monthly-new-members/monthly-new-members.component';

const routes: Routes = [
    {
        path: '',
        component: MonthlyNewKbsComponent
    },
    {
        path: 'monthly-new-comments',
        component: MonthlyNewCommentsComponent
    },
    {
        path: 'monthly-new-kbs',
        component: MonthlyNewKbsComponent
    },
    {
        path: 'monthly-new-members',
        component: MonthlyNewMembersComponent
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class StatisticsRoutingModule {}
