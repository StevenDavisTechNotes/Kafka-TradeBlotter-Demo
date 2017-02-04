export interface Exposure {
  key: string;
  security: string;

  sodAmount: number;
  tgtAmount: number;
  stgAmount: number;
  cmtAmount: number;
  doneAmount: number;

  sodUSDExposure: number;
  tgtUSDExposure: number;
  stgUSDExposure: number;
  cmtUSDExposure: number;
  doneUSDExposure: number;

  sodIntradayPLUSD: number;
  tgtIntradayPLUSD: number;
  stgIntradayPLUSD: number;
  cmtIntradayPLUSD: number;
  doneIntradayPLUSD: number;

  positions: Position[];
}