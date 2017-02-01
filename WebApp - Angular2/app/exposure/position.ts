export interface Position {
  Key: string;
  Security: string;
  Custodian: string;
  ExecutingBroker: string;
  PurchaseDate?: Date;
  DisplayPurchaseDate: string;

  TargetAmount: number;
  StagedAmount: number;
  CommittedAmount: number;
  DoneAmount: number;
  LeavesAmount: number;
}