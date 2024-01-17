import { Component } from "@angular/core";
import { RouterModule } from "@angular/router";

@Component({
    selector: "app-nav",
    templateUrl: "./nav.component.html",
    standalone: true,
    imports: [
        RouterModule
    ]
})
export class AppNavComponent {
    constructor() { }

}
