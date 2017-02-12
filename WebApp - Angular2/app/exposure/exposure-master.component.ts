import { Component, AfterViewInit, OnInit } from "@angular/core";

import { GridOptions } from "ag-grid/main";
import * as _ from 'lodash';

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
    public exposures: Object;
    public errorMessage: string = '';
    public isLoading: boolean = true;


    constructor(private exposureService: ExposureService) {
        this.gridOptions = <GridOptions>{};
        let data = [
            { Key: "SEDOL1", Security: "SEDOL1", DoneAmount: 9999, DoneExposureUSD: 333333, DoneIntradayPLUSD: 222222, AvgReturnPerDoneShareUSD: 0.01 },
            { Key: "SEDOL2", Security: "SEDOL2", DoneAmount: 9999, DoneExposureUSD: 333333, DoneIntradayPLUSD: 222222, AvgReturnPerDoneShareUSD: 0.01 },
            { Key: "SEDOL3", Security: "SEDOL3", DoneAmount: 9999, DoneExposureUSD: 333333, DoneIntradayPLUSD: 222222, AvgReturnPerDoneShareUSD: 0.01 }];
        this.gridOptions.rowData = data;
        this.exposures = _.keyBy(data, 'Key');
        this.gridOptions.columnDefs = [
            {
                headerName: 'Security', field: 'Security',
                // left column is going to act as group column, with the expand / contract controls
                cellRenderer: 'group',
                // we don't want the child count - it would be one each time anyway as each parent
                // not has exactly one child node
                cellRendererParams: { suppressCount: false }
            },
            { headerName: 'DoneAmount', field: 'DoneAmount', volatile: true },
            { headerName: 'Exposure (USD)', field: 'DoneExposureUSD', cellFormatter: this.dollarCellFormatter, volatile: true },
            { headerName: 'Intraday P/L (USD)', field: 'DoneIntradayPLUSD', cellFormatter: this.dollarCellFormatter, volatile: true },
            { headerName: 'Return per Share (USD)', field: 'AvgReturnPerDoneShareUSD', cellFormatter: this.dollarCellFormatter, volatile: true },
        ];
    }

    public isFullWidthCell(rowNode) {
        return rowNode.level === 1;
    }

    // Sometimes the gridReady event can fire before the angular component is ready to receive it, so in an angular
    // environment its safer to on you cannot safely rely on AfterViewInit instead before using the API
    public ngOnInit() {
        this.exposureService
            .getAllInterval()            
            .subscribe(
                /* happy path */
                exposures => {
                    this.errorMessage = 'All good';
                    this.updateData(exposures);
                },
                /* error path */ e => this.errorMessage = e,
                /* onComplete */() => this.isLoading = false
            );
    }

    private updateData(rawData: Exposure[]) {
        _.forEach(rawData, item => {
            console.log('Applying update ', item);
            _.extend(this.exposures[item.Key], item);
        });
        this.gridOptions.api.softRefreshView();
        this.gridOptions.api.sizeColumnsToFit();
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