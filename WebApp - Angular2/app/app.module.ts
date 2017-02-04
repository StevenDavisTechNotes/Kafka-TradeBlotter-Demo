import { NgModule } from "@angular/core";
import { BrowserModule } from "@angular/platform-browser";
import { FormsModule } from "@angular/forms";
import { RouterModule, Routes } from "@angular/router";
// ag-grid
import { AgGridModule } from "ag-grid-ng2/main";
// application
import { AppComponent } from "./app.component";

import { ExposureModule } from "./exposure/exposure.module";
import { ExposureComponent } from "./exposure/exposure.component";
// import {ExposureMasterComponent} from "./exposure/exposure-master.component";
// import {ExposureDetailPanelComponent} from "./exposure/exposure-detail-panel.component";

// trade entry
import { TradeEntryComponent } from "./trade-entry.component";
// quote entry
import { QuoteEntryComponent } from "./quote-entry.component";

const appRoutes: Routes = [
    { path: 'exposure', component: ExposureComponent, data: { title: "Kafka Exposure Analytics Example" } },
    { path: '', redirectTo: 'exposure', pathMatch: 'full' }
];

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        RouterModule.forRoot(appRoutes),
        // AgGridModule.withComponents(
        //     [
        //         ExposureDetailPanelComponent
        //     ]),
        ExposureModule
    ],
    declarations: [
        AppComponent,
        TradeEntryComponent,
        QuoteEntryComponent,
        // ExposureMasterComponent,
        // ExposureDetailPanelComponent,
        // ExposureComponent,
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
