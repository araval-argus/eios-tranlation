
import { FiltersResult } from './filters-result.model';

import { Query } from './query.model';
import { QueryRuleAccessor } from './../query-expressions/query-expressions.utils';
import { QueryRule, QueryRuleOperator, TermsQueryRule } from '../query-expressions/models/query-rule.model';

export interface FiltersGroup {
    id: string,
    name?: string;
    operator?: QueryRuleOperator,
    property?: string,
    ruleAccessor?: QueryRuleAccessor,
    reqTime?: string,
    searchKey?: string,
    filtersResult: FiltersResult,
    filtersQuery: Query,
    joinQueryRuleTemplate?: TermsQueryRule,
    joinQueryRuleId?: string,
    isTranslationRequired: boolean,
    isTranslationFrontend: boolean,
    error: boolean
}