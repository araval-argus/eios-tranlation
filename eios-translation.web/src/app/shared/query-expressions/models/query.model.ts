import { isEqual } from 'lodash';

import { AggregationExpression } from './aggregation-expression';
import {
  QueryGroup,
  queryGroupsAreEquivalent,
} from './query-group.model';
import { SortExpression } from './sort-expression.model';

export interface RetrieveResultsExpression {
  start?: number;
  limit?: number;
  sorts?: SortExpression[];
  skipAccurateCount?: boolean;
  queryId?: number;
  includeFields?: string[];
  excludeFields?: string[];
  highlights?: string[];
  collapse?: {
    property: string;
  };
}

export interface Query extends QueryGroup, RetrieveResultsExpression {
  types?: string[],
  /**
   * if string: property the be aggregated (by default type: 'terms')
   */
  aggregations?: (AggregationExpression | string)[];
}

export function queriesAreEquivalent(queryA: Query, queryB: Query, checkName: boolean = true) {
  const sameName = !checkName || (queryA.name === queryB.name);
  const sameAsGroups = queryGroupsAreEquivalent(queryA, queryB, checkName);
  if(sameName && sameAsGroups) {
    const sameLimit = queryA.limit === queryB.limit; 
    const typesA = (queryA.types || []).sort();
    const typesB = (queryB.types || []).sort();
    const sameTypes = isEqual(typesA, typesB);

    if(sameLimit && sameTypes ) {
      return isEqual(queryA.sorts, queryB.sorts);

    }

  }

  return false;
}