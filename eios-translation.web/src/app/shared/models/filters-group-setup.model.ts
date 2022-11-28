import { QueryRule, QueryRuleOperator, TermsQueryRule } from "../query-expressions/models/query-rule.model";


export interface FilterGroupSetup {
    id: string,
    property?: string,
    operator?: QueryRuleOperator,
    joinQueryRuleTemplate?: TermsQueryRule,
    isTranslationRequired: boolean,
    isTranslationFrontend: boolean
}