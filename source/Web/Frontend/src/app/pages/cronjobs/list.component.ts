import { Component, ViewChild } from "@angular/core";
import { BehaviorSubject, Observable, Subject, map, merge } from "rxjs";
import { DataSource } from '@angular/cdk/collections';
import { CronJobService } from "../../services/cronJob.service";
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Router } from "@angular/router";

@Component({
    selector: "app-list",
    templateUrl: "./list.component.html",
})
export class CronJobsComponent {
    dataSource!: FilesDataSource;
    displayedColumns = ['id', 'uri', 'httpMethod', 'body', 'schecule', 'timeZone', 'menu'];

    @ViewChild(MatPaginator, { static: true })
    paginator!: MatPaginator;

    @ViewChild(MatSort, { static: true })
    sort!: MatSort;

    public _unsubscribeAll: Subject<any> = new Subject();

    constructor(
        public service: CronJobService,
        private _router: Router
    ) { }

    ngOnInit(): void {
        this.initDataSource();
    }

    initDataSource(): void {
        this.dataSource = new FilesDataSource(this.service, this.paginator, this.sort);
    }

    pageUpdated(event: any): void {
        this.service.filters.updatePagination(event);
        this.service.list().subscribe();
    }

    updateTable(): void {
        this.service.list().subscribe();
    }

    edit(id: number): void {
        this._router.navigateByUrl(this._router.url + '/' + id);
    }

    delete(id: number){
        this.service.delete(id).subscribe(_x => this.service.list().subscribe());
    }
}

export class FilesDataSource extends DataSource<any>
{
    public _filterChange = new BehaviorSubject('');
    private _filteredDataChange = new BehaviorSubject('');


    constructor(
        private _service: CronJobService,
        public _matPaginator: MatPaginator,
        public _matSort: MatSort
    ) {
        super();
    }

    /**
     * Connect function called by the table to retrieve one stream containing the data to render.
     *
     * @returns
     */
    connect(): Observable<any[]> {
        const displayDataChanges = [
            this._service.onElementosChanged,
            this._matPaginator.page,
            this._filterChange,
            this._matSort.sortChange
        ];

        return merge(...displayDataChanges)
            .pipe(
                map(() => {
                    const data = this._service.elementos.slice();

                    return data.splice(0, this._matPaginator.pageSize);
                }
                ));
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    // Filtered data
    get filteredData(): any {
        return this._filteredDataChange.value;
    }

    set filteredData(value: any) {
        this._filteredDataChange.next(value);
    }

    // Filter
    get filter(): string {
        return this._filterChange.value;
    }

    set filter(filter: string) {
        this._filterChange.next(filter);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------



    disconnect(): void { }

}
