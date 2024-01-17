export class FilterObject {
    direction: string;
    pageSize: number;
    pageIndex: number;
    searchTerm: string;
    totalPages: number;
    totalRecords: number;
    data: any;

    constructor(objeto?: any) {
        objeto = objeto || {};
        this.pageSize = objeto.pageSize || 20;
        this.pageIndex = (objeto.searchTerm ? 0 : (objeto.pageIndex || 0));

        this.direction = objeto.direction;
        this.searchTerm = objeto.searchTerm;
        this.totalPages = objeto.totalPages;
        this.totalRecords = objeto.totalRecords;
    }


    updatePagination(index: any): void {
        this.pageIndex = index.pageIndex;
        this.pageSize = index.pageSize;
    }

    updateSearch(text: string): void {
        this.firstPageIndex();
        this.searchTerm = text;
    }

    updateFilters(filter: any): void {
        if (filter || filter !== '') {
            this.pageIndex = 0;
        }
    }

    updateTableLength(length: number): void {
        this.totalRecords = length;
    }

    firstPageIndex(): void {
        this.pageIndex = 0;
    }

    pageSizeReset(): void {
        this.pageSize = 10;
    }
}
