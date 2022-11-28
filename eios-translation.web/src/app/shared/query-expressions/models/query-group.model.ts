import { QueryExpression } from "./query-expression.model";
import { QueryRule, queryRulesAreEquivalent } from "./query-rule.model";

const queryGroupOperators = ['all', 'and', 'or', 'not'] as const;
export type QueryGroupOperator = typeof queryGroupOperators[number];

export interface QueryGroup extends QueryExpression {
  operator: QueryGroupOperator;
  rules?: QueryRule[];
  groups?: QueryGroup[];
}

export interface QueryGroupStub  {
 name: string;
 operator: QueryGroupOperator;
 groups?: QueryGroupStub[];
 path?: string;
}

const allQueryGroupOperators = [...queryGroupOperators] as string[];



export function isQueryGroup(qe: QueryExpression) {
  return qe && qe.operator && allQueryGroupOperators.includes(qe.operator);
}

export function getGroupQueryRules(g: QueryGroup) {
  return g.rules || []; 
}

export function queryGroupsAreEquivalent(groupA: QueryGroup, groupB: QueryGroup, checkName: boolean = true): boolean {
  const sameName = !checkName || (groupA.name === groupB.name);
  const sameOp = groupA.operator === groupB.operator;

  const samePath = groupA.path === groupB.path;


  if(sameName && sameOp && samePath) {
    const rulesA = groupA.rules || [];
    const rulesB = groupB.rules || [];
    const subgroupsA = groupA.groups || [];
    const subgroupsB = groupB.groups || [];
    const sameNumberOfRules =  rulesA.length === rulesB.length;
     // || rulesA.filter(r => !!r.focusedValue).length == rulesB.length
    //  || rulesB.filter(r => !!r.focusedValue).length == rulesA.length;
    const sameNumberOfGroups = subgroupsA.length === subgroupsB.length;

    if(sameNumberOfRules && sameNumberOfGroups) {
      const existsRuleNotEquivalentInB = rulesA.find(rA => {
        const equivalentRuleInB = rulesB.find(rB => queryRulesAreEquivalent(rA, rB));
        return !equivalentRuleInB;
      });
              
      if(!existsRuleNotEquivalentInB) {
        const existsGroupNotEquivalentInB = subgroupsA.find(gA => {
          const equivalentGroupInB = subgroupsB.find(gB => queryGroupsAreEquivalent(gA, gB));
          return !equivalentGroupInB;
        });

        return !existsGroupNotEquivalentInB;
      }
    }
  }

  return false;
}