import { NgModule } from "@angular/core";
import { HttpModule } from '@angular/http';

// ag-grid
import { AgGridModule } from "ag-grid-ng2/main";

import { ExposureComponent } from "./exposure.component";
import { ExposureMasterComponent } from "./exposure-master.component";
import { ExposureDetailPanelComponent } from "./exposure-detail-panel.component";

@NgModule({
    imports: [
        AgGridModule.withComponents(
            [
                ExposureDetailPanelComponent
            ]),
        HttpModule,
        //ExposureModule
    ],
    declarations: [
        ExposureMasterComponent,
        ExposureDetailPanelComponent,
        ExposureComponent
    ],
    exports: [
        ExposureComponent
    ]
})
export class ExposureModule {
}