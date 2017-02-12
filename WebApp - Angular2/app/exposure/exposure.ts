export interface Exposure {
  Key: string;
  QuoteDate: Date;
  Security: string;

  SodAmount: number;
  DoneAmount: number;
  TargetAmount: number;

  SodExposureUSD: number;
  DoneExposureUSD: number;
  TargetExposureUSD: number;

  SodPLUSD: number;
  DoneIntradayPLUSD: number;
  TargetIntradayPLUSD: number;

  positions: Position[];
}