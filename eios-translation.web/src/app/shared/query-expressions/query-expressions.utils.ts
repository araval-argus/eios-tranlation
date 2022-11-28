import { QueryGroup, QueryGroupStub } from './models/query-group.model';
import { getValuesFromQueryRule, QueryRule } from './models/query-rule.model';
import { Query } from './models/query.model';

import { isString } from 'lodash';

export type QueryRuleAccessor = (rule: QueryRule, index?: number) => boolean;

export function getSubgroup(group: QueryGroup, subgroupNames: string[]) {
  const subgroups = group.groups || [];
  if(subgroupNames && subgroups.length > 0) {
    const length = subgroupNames.length;

    if(length > 0) {
      const findGroup = (subgroup: QueryGroup) => subgroupNames[0] === subgroup.name;
      const subgroup = subgroups.find(findGroup);

      if(subgroup) {
        return length === 1 ? subgroup : getSubgroup(subgroup, subgroupNames.slice(1));
      }
    }
  }
  
  return undefined;
}

export function getRuleFromGroup(group: QueryGroup, findRule: QueryRuleAccessor, subgroupNames?: string[]): QueryRule {
  if(group) {
    if(subgroupNames) {
      const subgroup = getSubgroup(group, subgroupNames); 
  
      return getRuleFromGroup(subgroup, findRule);
    }
    else {
      const rules = group.rules || [];
      
      return rules.find(findRule);
    }
  }

  return undefined;
}



export function getRuleFromQuery(query: Query, 
  ruleAccessorOrProperty: QueryRuleAccessor | string,
  subgroupNames?: string[]): QueryRule {
    const findRule = isString(ruleAccessorOrProperty) ? (r: QueryRule) => r.property === ruleAccessorOrProperty : ruleAccessorOrProperty;

    return getRuleFromGroup(query, findRule, subgroupNames);
}

export function getSelectedValuesForProperty(query: Query, 
  ruleAccessorOrProperty: QueryRuleAccessor | string,
  subgroupNames?: string[]) {

    const rule = getRuleFromQuery(query, ruleAccessorOrProperty, subgroupNames);
  
    if(rule) {
      return getValuesFromQueryRule(rule);
    }

    return undefined;
}

function replaceOrAddRuleinArray(
  currentRules: QueryRule[],
  updatedRule: QueryRule,
  matchesRule: QueryRuleAccessor
) {
  if(currentRules && currentRules.length > 0) {
    let ruleFound = false;
    const rules = currentRules.map(currRule => {
      const matchedRule = matchesRule(currRule);
      if(matchedRule) {
        ruleFound = true;

        return updatedRule;
      }
      else {
        return currRule;
      }
    });

    if(ruleFound) {
      return rules;
    }
    else {
      return [...rules, updatedRule];
    }
  }
  else {
    return [updatedRule];
  }
}

function clearRuleinArray(
  currentRules: QueryRule[],
  matchesRule: QueryRuleAccessor
) {
  if(currentRules && currentRules.length > 0) {
    const rules = currentRules.filter(r => !matchesRule(r));

    return rules;
  }
  else {
    return [];
  }
}

export function createGroupWithRule(groupStub: QueryGroupStub, rule: QueryRule): QueryGroup {
  const hasSubgroup = groupStub.groups && groupStub.groups.length > 0;
  const groups = hasSubgroup ? [createGroupWithRule(groupStub.groups[0], rule)]: [];
  const rules = hasSubgroup ? [] : [rule];
  
   return {...groupStub, rules, groups};;
}

/**
 * Returns a new group inserting or updating a specified rule, recursive
 * @param currentGroup the group in which the rule should be found and replaced or inserted, will not be modified
 * @param updatedRule the new or updated version of the rule
 * @param ruleAccessor function to find the rule to be replaced (optional, if not provided updatedRule.property is used)
 * @param subgroup group stub in case the rule should be replaced in a subgroup. The subgroup will inserted if not existing yet
 * @returns 
 */
export function updateQueryRuleInGroup(
  currentGroup: QueryGroup,
  updatedRule: QueryRule,
  ruleAccessor?: QueryRuleAccessor,
  subgroup?: QueryGroupStub
): QueryGroup {
  if (currentGroup) {
    // if ruleAccessor is not provided, use updatedRule.property to fild the rule to be replaced with the updated one
    const findRule: QueryRuleAccessor = ruleAccessor || ( (r: QueryRule) => r.property === updatedRule.property );

    if(subgroup) {
      if(currentGroup.groups && currentGroup.groups.length > 0) {
        const matchesGroup = (g: QueryGroup) => g.name === subgroup.name;
        let groupFound = false;
        // try to find and update subgroup with the rule
        const groups = currentGroup.groups.map(currSubgroup => {
          const matchedGroup = matchesGroup(currSubgroup);

          if(matchedGroup) {
            groupFound = true;
            // recursive update of the subgroup
            const updatedSubgroup = updateQueryRuleInGroup(currSubgroup, updatedRule, findRule, 
              subgroup.groups && subgroup.groups.length > 0 ? subgroup.groups[0]: null);
            // return the updated subgroup
            return updatedSubgroup;
          }
          else {
            // returns the untouched subgroup
            return currSubgroup;
          }
        });

        if(groupFound) {
          // returns the updated group with the updated array of subgroups
          return {...currentGroup, groups};
        }
        else {
          // create new subgroup and insert rule
          const groupToAdd = createGroupWithRule(subgroup, updatedRule);
          // add new subgroup to subgroups and return updated group
          return {...currentGroup, groups: [...groups, groupToAdd]};
        }
      }
      else {
        // create new subgroup and insert rule
        const groupToAdd = createGroupWithRule(subgroup, updatedRule);
        // add new subgroup to subgroups array and return updated group
        return {...currentGroup, groups: [groupToAdd]};
      }
      
    }
    else {
      // updates rules
      const rules = replaceOrAddRuleinArray(currentGroup.rules, updatedRule, findRule);
      // returns the updated group with the updated array of rules
      return {...currentGroup, rules};
    }

  }

  return null;
}
/**
 * Returns an updated query by replacing or inserting an updated rule
 * @param currentQuery the query to be updated, will not be modified
 * @param updatedRule the new or updated version of the rule
 * @param ruleAccessor function to find the rule to be replaced (optional, if not provided updatedRule.property is used)
 * @param subgroup group stub in case the rule should be replaced in a subgroup. The subgroup will inserted if not existing yet
 * @returns 
 */
export function updateQueryRule(
  currentQuery: Query,
  updatedRule: QueryRule,
  ruleAccessor?: QueryRuleAccessor,
  subgroup?: QueryGroupStub
  
): Query {
  if (currentQuery) {
    return {...currentQuery, ...updateQueryRuleInGroup(currentQuery, updatedRule, ruleAccessor, subgroup)};
  }

  return null;
}

/**
 * Returns a new group removing a specified rule, recursive
 * @param currentGroup the group in which the rule should be found and from which it should be removed, will not be modified
 * @param findRule the function to identify the rule to remove
 * @param subgroupNames the subgroup(s) path from which the rules should be removed
 * @returns 
 */
export function clearQueryRuleinGroup(
  currentGroup: QueryGroup,
  findRule: QueryRuleAccessor,
  subgroupNames: string[]): QueryGroup {
  
    if(subgroupNames && subgroupNames.length > 0) {
      const findGroup = (g: QueryGroup) => g.name === subgroupNames[0];
      const groups = currentGroup.groups && currentGroup.groups.length > 0 ?
        currentGroup.groups.map(currSubgroup => {
          if(findGroup(currSubgroup)) {
            return clearQueryRuleinGroup(currSubgroup, findRule, subgroupNames.slice(1));
          }
          else {
            return currSubgroup;
          }
        }).filter(g => g.rules.length > 0 || (g.groups && g.groups.length > 0 )) : [];

        return {...currentGroup, groups};
    }
    else {
      const rules = clearRuleinArray(currentGroup.rules, findRule);

      // if(rules.length === 0 && (!currentGroup.groups || currentGroup.groups.length === 0)) {
      //   return null;
      // }

      // returns the updated group with the updated array of rules
      const updatedGroup = {...currentGroup, rules};

      return updatedGroup;
    }
}

export function clearQueryRule(
  currentQuery: Query, 
  ruleAccessorOrProperty: QueryRuleAccessor | string,
  subgroupNames?: string[]): Query {
  if(currentQuery){
    const findRule = isString(ruleAccessorOrProperty) ? (r: QueryRule) => r.property === ruleAccessorOrProperty : ruleAccessorOrProperty;

    return {...currentQuery, ...clearQueryRuleinGroup(currentQuery, findRule, subgroupNames)}

  }

  return null;
}
