export class CronJob {
    id: number;
    uri: string;
    httpMethod: number;
    body: number;
    schecule: string;
    timeZone: string;

    constructor(x?: any) {
        x = x || {};
        this.id = x.id;
        this.uri = x.uri;
        this.httpMethod = x.httpMethod
        this.body = x.body
        this.schecule = x.schecule
        this.timeZone = x.timeZone
    }
   
}
