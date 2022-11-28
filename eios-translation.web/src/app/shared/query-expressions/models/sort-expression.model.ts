import { isEqual } from "lodash";

export type SortDirection = 'asc' | 'desc';
export interface SortExpression {
  property: string;
  direction: SortDirection;
}

export function sortExpressionsAreEquivalent(sortA: SortExpression, sortB: SortExpression) {
  return isEqual(sortA, sortB);
}