import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {FormsModule} from "@angular/forms";
import {RouterModule, Routes} from "@angular/router";
// ag-grid
import {AgGridModule} from "ag-grid-ng2/main";
// application
import {AppComponent} from "./app.component";

// import {ExposureModule} from "./exposure.module";
import {ExposureComponent} from "./exposure.component";
import {MasterComponent} from "./masterdetail-master.component";
import {DetailPanelComponent} from "./detail-panel.component";

// trade entry
import {TradeEntryComponent} from "./trade-entry.component";
// quote entry
import {QuoteEntryComponent} from "./quote-entry.component";

const appRoutes: Routes = [
    {path: 'exposure', component: ExposureComponent, data: {title: "Master Detail Example"}},
    {path: '', redirectTo: 'exposure', pathMatch: 'full'}
];

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        RouterModule.forRoot(appRoutes),
        AgGridModule.withComponents(
            [
                DetailPanelComponent
            ]),
            //ExposureModule
    ],
    declarations: [
        AppComponent,
        TradeEntryComponent,
        QuoteEntryComponent,
        MasterComponent,
        DetailPanelComponent,
        ExposureComponent,
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
