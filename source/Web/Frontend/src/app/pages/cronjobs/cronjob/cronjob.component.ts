import { Component } from "@angular/core";
import { CronJobService } from "../../../services/cronJob.service";
import { CronJob } from "../../../models/cronJob.model";
import { ActivatedRoute } from "@angular/router";
import { Observable, Subscription, map, startWith } from "rxjs";
import { Location } from '@angular/common';
import { TimeZoneService } from "../../../services/timeZone.service";
import { FormControl } from "@angular/forms";
var cronValidator = require('cron-expression-validator');

@Component({
    selector: "cronjob",
    templateUrl: "./cronjob.component.html",
})
export class CronJobComponent {
    elemento: CronJob = new CronJob();
    timezones: any[] = [];
    private routeSub!: Subscription;

    filteredOptions!: Observable<any[]>;
    timezoneCtrl = new FormControl('');

    constructor(
        public service: CronJobService,
        private _serviceTS: TimeZoneService,
        private route: ActivatedRoute,
        public location: Location,
    ) { }

    ngOnInit(): void {
        this.routeSub = this.route.params.subscribe(params => {
            const num = Number(params['id'])

            if (!isNaN(num)) {
                this.service.get(num).subscribe(x => {
                    this.elemento = x;
                    this.timezoneCtrl = new FormControl(x.timeZone)
                })
            }
        });

        this._serviceTS.list().subscribe((x: any[]) => this.timezones = x)

        this.filteredOptions = this.timezoneCtrl.valueChanges.pipe(
            startWith(''),
            map(t => (t ? this._filterStates(t) : this.timezones.slice())),
        );
    }

    action() {
        const cron = cronValidator.isValidCronExpression(this.elemento.schecule, { error: true })

        if (typeof cron != "boolean" && !cron.isValid) {
            alert("Schecule invalid. " + cron.errorMessage[0])
            return;
        }
        this.elemento.timeZone = this.timezoneCtrl.value ?? "";
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

    _filterStates(value: string): any[] {
        const filterValue = value.toLowerCase();

        return this.timezones.filter(t => t.name.toLowerCase().includes(filterValue));
    }

    ngOnDestroy() {
        this.routeSub.unsubscribe();
    }
}
