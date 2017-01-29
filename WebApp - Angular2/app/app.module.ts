import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {FormsModule} from "@angular/forms";
import {RouterModule, Routes} from "@angular/router";
// ag-grid
import {AgGridModule} from "ag-grid-ng2/main";
// application
import {AppComponent} from "./app.component";

// master detail
import {MasterComponent} from "./masterdetail-master.component";
import {DetailPanelComponent} from "./detail-panel.component";

// trade entry
import {TradeEntryComponent} from "./trade-entry.component";
// quote entry
import {QuoteEntryComponent} from "./quote-entry.component";

const appRoutes: Routes = [
    {path: 'master-detail', component: MasterComponent, data: {title: "Master Detail Example"}},
    {path: '', redirectTo: 'master-detail', pathMatch: 'full'}
];

@NgModule({
    imports: [
        BrowserModule,
        FormsModule,
        RouterModule.forRoot(appRoutes),
        AgGridModule.withComponents(
            [
                DetailPanelComponent
            ])
    ],
    declarations: [
        AppComponent,
        MasterComponent,
        DetailPanelComponent,
        TradeEntryComponent,
        QuoteEntryComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
