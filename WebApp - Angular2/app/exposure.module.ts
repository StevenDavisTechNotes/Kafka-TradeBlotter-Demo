import {NgModule} from "@angular/core";

import {ExposureComponent} from "./exposure.component";
import {MasterComponent} from "./masterdetail-master.component";
import {DetailPanelComponent} from "./detail-panel.component";

@NgModule({
    imports: [],
    declarations: [
        MasterComponent,
        DetailPanelComponent,
        ExposureComponent
    ]
})
export class ExposureModule {
}