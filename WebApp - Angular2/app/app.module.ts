import {NgModule} from "@angular/core";
import {BrowserModule} from "@angular/platform-browser";
import {FormsModule} from "@angular/forms";
import {RouterModule, Routes} from "@angular/router";
// ag-grid
import {AgGridModule} from "ag-grid-ng2/main";
// application
import {AppComponent} from "./app.component";
// rich grid
import {RichGridComponent} from "./rich-grid.component";
// declarative
import {RichGridDeclarativeComponent} from "./rich-grid-declarative.component";
// from component
import {FromComponentComponent} from "./from-component.component";
import {SquareComponent} from "./square.component";
import {ParamsComponent} from "./params.component";
import {CubeComponent} from "./cube.component";
import {CurrencyComponent} from "./currency.component";

// from rich component
import {FromRichComponent} from "./from-rich.component";
import {ClickableModule} from "./clickable.module";
import {RatioModule} from "./ratio.module";
import {RatioParentComponent} from "./ratio.parent.component";
import {ClickableParentComponent} from "./clickable.parent.component";
// editor
import {EditorComponent} from "./editor-component.component";
import {NumericEditorComponent} from "./numeric-editor.component";
import {MoodEditorComponent} from "./mood-editor.component";
import {MoodRendererComponent} from "./mood-renderer.component";
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
    {path: 'rich-grid', component: RichGridComponent, data: {title: "Rich Grid with Pure JavaScript"}},
    {
        path: 'rich-grid-declarative',
        component: RichGridDeclarativeComponent,
        data: {title: "Rich Grid with Declarative Markup"}
    },
    {path: 'from-component', component: FromComponentComponent, data: {title: "Using Dynamic Components"}},
    {path: 'from-rich-component', component: FromRichComponent, data: {title: "Using Dynamic Components - Richer Example"}},
    {path: 'editor-component', component: EditorComponent, data: {title: "Using Cell Editor Components"}},
    {path: 'floating-row', component: WithFloatingRowComponent, data: {title: "Using Floating Row Renderers"}},
    {path: 'full-width', component: WithFullWidthComponent, data: {title: "Using Full Width Renderers"}},
    {path: 'group-row', component: WithGroupRowComponent, data: {title: "Using Group Row Renderers"}},
    {path: 'filter', component: FilterComponentComponent, data: {title: "With Filters Components"}},
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
                RatioParentComponent,
                ClickableParentComponent,
                NumericEditorComponent,
                MoodEditorComponent,
                MoodRendererComponent,
                StyledComponent,
                NameAndAgeRendererComponent,
                MedalRendererComponent,
                PartialMatchFilterComponent,
                DetailPanelComponent
            ]),
        RatioModule,
        ClickableModule
    ],
    declarations: [
        AppComponent,
        RichGridComponent,
        RichGridDeclarativeComponent,
        FromComponentComponent,
        SquareComponent,
        CubeComponent,
        ParamsComponent,
        CurrencyComponent,
        FromRichComponent,
        EditorComponent,
        NumericEditorComponent,
        MoodEditorComponent,
        MoodRendererComponent,
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
