import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { KnowledgeBasesComponent } from './knowledge-bases/knowledge-bases.component';
import { CategoriesComponent } from './categories/categories.component';
import { CommentsComponent } from './comments/comments.component';
import { ReportsComponent } from './reports/reports.component';
import {AuthGuard} from "@app/shared";

const routes: Routes = [
    {
        path: '',
        component: KnowledgeBasesComponent
    },
    {
        path: 'categories',
        data: {
            functionCode: 'CONTENT_CATEGORY'
        },
        component: CategoriesComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'comments',
        data: {
            functionCode: 'CONTENT_COMMENT'
        },
        component: CommentsComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'knowledge-bases',
        data: {
            functionCode: 'CONTENT_KNOWLEDGEBASE'
        },
        component: KnowledgeBasesComponent,
        canActivate: [AuthGuard]
    },
    {
        path: 'reports',
        data: {
            functionCode: 'CONTENT_REPORT'
        },
        component: ReportsComponent,
        canActivate: [AuthGuard]
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ContentsRouting {}
