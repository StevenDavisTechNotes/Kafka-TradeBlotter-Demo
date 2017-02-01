export interface Exposure {
  Key: string;
  Security: string;

  SODAmount: number;
  TgtAmount: number;
  StgAmount: number;
  CmtAmount: number;
  DoneAmount: number;

  SODUSDExposure: number;
  TgtUSDExposure: number;
  StgUSDExposure: number;
  CmtUSDExposure: number;
  DoneUSDExposure: number;

  SODIntradayPLUSD: number;
  TgtIntradayPLUSD: number;
  StgIntradayPLUSD: number;
  CmtIntradayPLUSD: number;
  DoneIntradayPLUSD: number;

  Positions: Position[];
}