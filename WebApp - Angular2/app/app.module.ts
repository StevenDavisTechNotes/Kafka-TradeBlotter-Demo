import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {FormsModule} from "@angular/forms";
import {RouterModule, Routes} from "@angular/router";
// ag-grid
import {AgGridModule} from "ag-grid-ng2/main";
// application
import {AppComponent} from "./app.component";
// from component
import {FromComponentComponent} from "./from-component.component";
import {SquareComponent} from "./square.component";
import {ParamsComponent} from "./params.component";
import {CubeComponent} from "./cube.component";
import {CurrencyComponent} from "./currency.component";

// master detail
import {MasterComponent} from "./masterdetail-master.component";
import {DetailPanelComponent} from "./detail-panel.component";

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
                SquareComponent,
                CubeComponent,
                ParamsComponent,
                CurrencyComponent,
                DetailPanelComponent
            ])
    ],
    declarations: [
        AppComponent,
        FromComponentComponent,
        SquareComponent,
        CubeComponent,
        ParamsComponent,
        CurrencyComponent,
        MasterComponent,
        DetailPanelComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
