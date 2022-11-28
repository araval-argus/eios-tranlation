import { FeatureCollection } from 'geojson';
import {
  isEqual,
  isString,
} from 'lodash';

import { QueryExpression } from './query-expression.model';
import { Query } from './query.model';

const spatialRulesOperators = [
  'pointInRadius',
  'pointInBoundingBox',
  'pointInPolygon',
  'pointInShape',
  'shapeWithin',
  'shapeContains',
  'shapeDisjoint',
  'shapeDisjoint',
  'shapeIntersects'
] as const;

const termsRuleOperators = ['in', 'each'] as const; 
const termsRuleOperatorsArray =  Object.freeze([...termsRuleOperators]) as string[]; 
const otherRuleOperators = [
  'equals',
  'simplequerystring',
  'dateRange',
  'numericRange',
  'exists',
  'script'
] as const; 

const allRuleOperators = [...spatialRulesOperators, ...termsRuleOperators, ...otherRuleOperators] as string[];


export type SpatialQueryRuleOperator = typeof spatialRulesOperators[number];
export type TermsRuleOperator = typeof termsRuleOperators[number];
export type QueryRuleOperator = typeof otherRuleOperators[number] 
  | TermsRuleOperator
  | SpatialQueryRuleOperator;

export interface QueryRule extends QueryExpression {
  operator: QueryRuleOperator;
  property: string;
  value?: any;
  focusedValue?: any;
  boost?: number;
}



export interface EqualsQueryRule extends QueryRule {
  operator: 'equals';
  value: string | number | boolean;
}

export interface TermsQueryRule extends QueryRule {
  operator: TermsRuleOperator;
  value?: string | string[] | number[];
  joinQuery?: Query;
  joinProperty?: string;
}

export interface InQueryRule extends TermsQueryRule {
  operator: 'in';
}

export interface EachQueryRule extends TermsQueryRule {
  operator: 'each';
}

export interface ScriptQueryRule extends QueryRule {
  operator: 'script';
  value: string;
}

export interface ExistsQueryRule extends QueryRule {
  operator: 'exists';
}

export interface SimpleQueryStringQueryRule extends QueryRule {
  operator: 'simplequerystring';
  value: string;
}

export interface DateRangeValue {
  gte?: string;
  lte?: string;
  lt?: string;
  gt?: string;
}

export interface DateRangeQueryRule extends QueryRule {
  operator: 'dateRange';
  value: DateRangeValue;
}

export interface NumericRangeValue {
  gte?: number;
  lte?: number;
  lt?: number;
  gt?: number;
}

export interface NumericRangeQueryRule extends QueryRule {
  operator: 'numericRange';
  value: NumericRangeValue;
}

export interface SpatialQueryRule extends QueryRule {
  operator: SpatialQueryRuleOperator;
  value: string | FeatureCollection;
}



/**
 * Returns true if the expression provided is a rule, false otherwise
 * @param qe expression to check
 * @returns 
 */
export function isQueryRule(qe: QueryExpression): boolean {
  return qe && qe.operator && !!qe['property'] && allRuleOperators.includes(qe.operator);
}

export function getValuesFromQueryRule(rule: QueryRule) {
  const isTermRule =  termsRuleOperatorsArray.includes(rule.operator);
  const values = isTermRule && isString(rule.value) && rule.value.includes(',') ? 
        rule.value.split(',') : rule.value;

  return values; 
}

export function queryRulesAreEquivalent(ruleA: QueryRule, ruleB: QueryRule, checkName: boolean = true) {
  const sameProperty= ruleA.property === ruleB.property;
  const samePath = ruleA.path === ruleB.path;
  const sameName = !checkName || (ruleA.name === ruleB.name);

  if(sameProperty && samePath && sameName) {
    const ops = [ruleA.operator, ruleB.operator];
    const sameOp = ruleA.operator === ruleB.operator;


    if(sameOp || ( ops.includes('equals') && ( ops.includes('in') || ops.includes('each') ) ) ) {

      const valueA = getValuesFromQueryRule(ruleA);
      const valueB = getValuesFromQueryRule(ruleB);
      // console.log({valueA, valueB});
      const sameValue = isEqual(valueA, valueB); 

      return sameValue;
    }

  }

  return false;
  
}