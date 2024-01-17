import { Component } from "@angular/core";
import { CronJobService } from "../../../services/cronJob.service";
import { CronJob } from "../../../models/cronJob.model";
import { ActivatedRoute } from "@angular/router";
import { Subscription } from "rxjs";
import { Location } from '@angular/common';
import cron from 'cron-validate'

@Component({
    selector: "cronjob",
    templateUrl: "./cronjob.component.html",
})
export class CronJobComponent {
    elemento: CronJob = new CronJob();
    private routeSub!: Subscription;

    constructor(
        public service: CronJobService,
        private route: ActivatedRoute,
        public location: Location,
    ) { }

    ngOnInit(): void {
        this.routeSub = this.route.params.subscribe(params => {
            const num = Number(params['id'])

            if (!isNaN(num)) {
                this.service.get(num).subscribe(x => this.elemento = x)
            }
        });
    }

    action() {
        if (!cron(this.elemento.schecule).isValid()) {
            alert("Schecule invalid.")
            return;
        }

        this.elemento.id ? this.update() : this.create();
    }

    update() {
        this.service.update(this.elemento).subscribe(() => {
            //add msg here
            this.back();
        }, () => {
            //add msg error here
        });
    }

    create() {
        this.service.create(this.elemento).subscribe(() => {
            //add msg here
            this.back();
        }, () => {
            //add msg error here
        });
    }

    back() {
        this.location.back();
    }

    ngOnDestroy() {
        this.routeSub.unsubscribe();
    }
}
