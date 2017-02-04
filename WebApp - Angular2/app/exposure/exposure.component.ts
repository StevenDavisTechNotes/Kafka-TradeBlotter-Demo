import {Component} from "@angular/core";

import { ExposureService } from "./exposure-service";

@Component({
    moduleId: module.id,
    selector: 'exposure',
    templateUrl: 'exposure.component.html',
    providers: [ExposureService]
})
export class ExposureComponent {
}