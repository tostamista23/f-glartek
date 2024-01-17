import { NgModule } from "@angular/core";
import { RouterModule } from "@angular/router";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSortModule } from '@angular/material/sort';
import { CommonModule } from '@angular/common';
import { CronJobsComponent } from "./list.component";
import { CronJobComponent } from "./cronjob/cronjob.component";

import { Route } from '@angular/router';

import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatDividerModule } from '@angular/material/divider';
import { MatDialogModule } from '@angular/material/dialog';
import { MatTableModule } from '@angular/material/table';
import { MatMenuModule } from '@angular/material/menu';

import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { CronJobService } from "../../services/cronJob.service";
import { MatSelectModule } from "@angular/material/select";

const routes: Route[] = [{
    path: '',
    component: CronJobsComponent,
    resolve: {
        data: CronJobService
    }
}, {
    path: ':id',
    component: CronJobComponent,
}];

@NgModule({
    declarations: [CronJobsComponent, CronJobComponent],
    imports: [
        RouterModule.forChild(routes),

        MatPaginatorModule,
        MatInputModule,
        MatSortModule,
        CommonModule,
        FormsModule,
        MatButtonModule,
        MatIconModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatDividerModule,
        MatDialogModule,
        MatTableModule,
        MatMenuModule,
        MatSelectModule,
        MatProgressSpinnerModule,
    ],
    providers: [
        CronJobService
    ]
})
export class CronJobsModule { }
