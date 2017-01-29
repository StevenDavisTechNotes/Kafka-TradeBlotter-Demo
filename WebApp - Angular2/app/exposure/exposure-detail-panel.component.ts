import {Component, AfterViewInit} from "@angular/core";
import {GridOptions} from "ag-grid/main";
import {AgRendererComponent} from "ag-grid-ng2/main";

@Component({
    moduleId: module.id,
    selector: 'exposure-detail-panel',
    templateUrl: 'exposure-detail-panel.component.html',
    styleUrls: ['exposure-detail-panel.component.css'],
})
export class ExposureDetailPanelComponent implements AgRendererComponent,AfterViewInit {
    public gridOptions: GridOptions;
    public parentRecord: any;

    constructor() {
        this.gridOptions = <GridOptions>{};
        this.gridOptions.enableSorting = true;
        this.gridOptions.enableFilter = true;
        this.gridOptions.enableColResize = true;
        this.gridOptions.columnDefs = this.createColumnDefs();
    }

    agInit(params: any): void {
        this.parentRecord = params.node.parent.data;
    }

    // Sometimes the gridReady event can fire before the angular component is ready to receive it, so in an angular
    // environment its safer to on you cannot safely rely on AfterViewInit instead before using the API
    ngAfterViewInit() {
        this.gridOptions.api.setRowData(this.parentRecord.callRecords);
        this.gridOptions.api.sizeColumnsToFit();
    }

    onSearchTextChange(newData: string) {
        this.gridOptions.api.setQuickFilter(newData);
    }

    private createColumnDefs() {
        return [{headerName: 'Call ID', field: 'callId', cellClass: 'call-record-cell'},
            {headerName: 'Direction', field: 'direction', cellClass: 'call-record-cell'},
            {headerName: 'Number', field: 'number', cellClass: 'call-record-cell'},
            {
                headerName: 'Duration',
                field: 'duration',
                cellClass: 'call-record-cell',
                cellFormatter: this.secondCellFormatter
            },
            {headerName: 'Switch', field: 'switchCode', cellClass: 'call-record-cell'}];

    }

    private secondCellFormatter(params) {
        return params.value.toLocaleString() + 's';
    };

    // if we don't do this, then the mouse wheel will be picked up by the main
    // grid and scroll the main grid and not this component. this ensures that
    // the wheel move is only picked up by the text field
    consumeMouseWheelOnDetailGrid($event) {
        $event.stopPropagation();
    }

    onButtonClick() {
        window.alert('Sample button pressed!!');
    }
}