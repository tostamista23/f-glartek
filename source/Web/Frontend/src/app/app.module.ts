import { HttpClientModule } from "@angular/common/http";
import { APP_INITIALIZER, ErrorHandler, NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { RouterModule } from "@angular/router";
import { AppComponent } from "./app.component";
import { AppErrorHandler } from "./app.error.handler";
import { ROUTES } from "./app.routes";
import { AppSettingsService } from "../settings/settings.service";
import { provideAnimations } from '@angular/platform-browser/animations';


@NgModule({
    bootstrap: [AppComponent],
    declarations: [AppComponent],
    imports: [
        BrowserModule,
        HttpClientModule,
        RouterModule.forRoot(ROUTES),
    ],
    providers: [
        { provide: ErrorHandler, useClass: AppErrorHandler },
        { provide: APP_INITIALIZER, useFactory: (_: AppSettingsService) => () => { }, multi: true },
        provideAnimations()
    ]
})
export class AppModule { }
