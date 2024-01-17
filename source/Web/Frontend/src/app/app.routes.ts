import { Routes } from "@angular/router";
import { AppLayoutNavComponent } from "./layouts/layout-nav/layout-nav.component";

export const ROUTES: Routes = [
    {
        path: "",
        component: AppLayoutNavComponent,
    },
    { path: 'cronjobs', component: AppLayoutNavComponent, loadChildren: () => import('src/app/pages/cronjobs/list.module').then(m => m.CronJobsModule) }
];
