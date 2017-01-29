import {Component} from "@angular/core";
import {Router, ActivatedRoute} from "@angular/router";

@Component({
    moduleId: module.id,
    selector: 'my-app',
    templateUrl: 'app.component.html'
})
export class AppComponent {
    constructor(public router: Router,
                private route: ActivatedRoute) {
    }
}
