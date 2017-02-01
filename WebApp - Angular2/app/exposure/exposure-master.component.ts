import { Component, AfterViewInit, OnInit } from "@angular/core";

import { GridOptions } from "ag-grid/main";

import { ExposureDetailPanelComponent } from "./exposure-detail-panel.component";
import { ExposureService } from "./exposure-service";
import { Exposure } from "./exposure";
import { Position } from "./position";

@Component({
    moduleId: module.id,
    selector: 'exposure-master',
    templateUrl: 'exposure-master.component.html'
})
export class ExposureMasterComponent implements OnInit {
    public gridOptions: GridOptions;
    public exposures: Exposure[];
    public errorMessage: string = '';
    public isLoading: boolean = true;


    constructor(private exposureService: ExposureService) {
        this.gridOptions = <GridOptions>{};
        this.gridOptions.rowData = [];
        this.gridOptions.columnDefs = this.createColumnDefs();
    }

    private createColumnDefs() {
        return [
            {
                headerName: 'Security', field: 'security',
                // left column is going to act as group column, with the expand / contract controls
                cellRenderer: 'group',
                // we don't want the child count - it would be one each time anyway as each parent
                // not has exactly one child node
                cellRendererParams: { suppressCount: false }
            },
            { headerName: 'Amount', field: 'cmtAmount' },
            { headerName: 'Exposure (USD)', field: 'cmtUSDExposure', cellFormatter: this.dollarCellFormatter },
            { headerName: 'Intraday P/L (USD)', field: 'cmtIntradayPLUSD', cellFormatter: this.dollarCellFormatter }
        ];
    }


    public isFullWidthCell(rowNode) {
        return rowNode.level === 1;
    }

    // Sometimes the gridReady event can fire before the angular component is ready to receive it, so in an angular
    // environment its safer to on you cannot safely rely on AfterViewInit instead before using the API
    public ngOnInit() {
        this.exposureService
            .getAll()
            .subscribe(
                /* happy path */
                exposures => {
                    this.errorMessage = 'All good';
                    this.exposures = exposures;
                    this.gridOptions.rowData = exposures;
                    this.gridOptions.api.refreshView();
                    this.gridOptions.api.sizeColumnsToFit();
                },
                /* error path */ e => this.errorMessage = e,
                /* onComplete */() => this.isLoading = false
            );
    }

    public getFullWidthCellRenderer() {
        return ExposureDetailPanelComponent;
    }

    public getRowHeight(params) {
        let rowIsDetailRow = params.node.level === 1;
        // return 100 when detail row, otherwise return 25
        return rowIsDetailRow ? 200 : 25;
    }

    public getNodeChildDetails(record) {
        if (record.positions) {
            return {
                group: true,
                // the key is used by the default group cellRenderer
                key: record.security,
                // provide ag-Grid with the children of this group
                children: record.positions,
                // for demo, expand the third row by default
                expanded: false
            };
        } else {
            return null;
        }
    }

    private dollarCellFormatter(params) {
        return params.value.toLocaleString('en-US', { style: 'currency', currency: 'USD' });
    };

}