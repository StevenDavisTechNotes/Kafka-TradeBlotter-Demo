export interface Position {
  key: string;
  security: string;
  custodian: string;
  executingBroker: string;
  purchaseDate?: Date;
  displayPurchaseDate: string;

  targetAmount: number;
  stagedAmount: number;
  committedAmount: number;
  doneAmount: number;
  leavesAmount: number;
}