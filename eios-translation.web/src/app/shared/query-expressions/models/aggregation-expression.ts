import { QueryGroup } from './query-group.model';
import { RetrieveResultsExpression } from './query.model';
import { SortDirection } from './sort-expression.model';

export type AggregationType =
  | 'terms'
  | 'significantTerms'
  | 'date'
  | 'min'
  | 'max'
  | 'sum'
  | 'avg'
  | 'geoGrid'
  | 'histogram';

export interface AggregationExpression {
  type: AggregationType;
  property: string;
  name?: string;
  path?: string;
  filter?: QueryGroup;
  innerAggregations?: AggregationExpression[];
  includeTopHits?: RetrieveResultsExpression;
  computeStats?: boolean;
}

export interface TermsAggregationExpressionBase extends AggregationExpression {
  termsSize?: number;
  termsSort?: string;
  termsSortDirection?: SortDirection;
  include?: string;
  exclude?: string;
  termMissing?: string;
}
export interface TermsAggregationExpression
  extends TermsAggregationExpressionBase {
  type: 'terms';
}

export interface SignificantTermsAggregationExpression
  extends TermsAggregationExpressionBase {
  type: 'significantTerms';
  backgroundFilter?: QueryGroup;
  chiSquare?: {
    includeNegatives?: boolean;
    backgroundIsSuperSet?: boolean;
  };
  googleNormalizedDistance?: {
    backgroundIsSuperSet?: boolean;
  };
}

export interface DateAggregationExpression extends AggregationExpression {
  type: 'date';
  interval: string;
  keyFormat: string;
  computeMovingAverage?: boolean;
  movAvgModel?: string;
  movAvgWindow?: number;
}

export interface HistogramAggregationExpression extends AggregationExpression {
  type: 'histogram';
  interval: number;
}

export interface GeoGridAggregationExpression extends AggregationExpression {
  type: 'geoGrid';
  geoGridPrecision: number | string;
}
