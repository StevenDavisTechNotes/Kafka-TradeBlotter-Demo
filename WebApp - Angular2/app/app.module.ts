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

// floating row
import {WithFloatingRowComponent} from "./floating-row-renderer.component";
import {StyledComponent} from "./styled-renderer.component";
// full width
import {WithFullWidthComponent} from "./full-width-renderer.component";
import {NameAndAgeRendererComponent} from "./name-age-renderer.component";
// grouped inner
import {MedalRendererComponent} from "./medal-renderer.component";
import {WithGroupRowComponent} from "./group-row-renderer.component";
// filter
import {FilterComponentComponent} from "./filter-component.component";
import {PartialMatchFilterComponent} from "./partial-match-filter.component";
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
                StyledComponent,
                NameAndAgeRendererComponent,
                MedalRendererComponent,
                PartialMatchFilterComponent,
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
        WithFloatingRowComponent,
        StyledComponent,
        WithFullWidthComponent,
        NameAndAgeRendererComponent,
        WithGroupRowComponent,
        MedalRendererComponent,
        FilterComponentComponent,
        PartialMatchFilterComponent,
        MasterComponent,
        DetailPanelComponent
    ],
    bootstrap: [AppComponent]
})
export class AppModule {
}
