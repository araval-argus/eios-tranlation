import { QueryRule } from './models/query-rule.model';
import { Query } from './models/query.model';
import { clearQueryRule, getRuleFromQuery, getSelectedValuesForProperty, getSubgroup, updateQueryRule } from './query-expressions.utils';

describe('updateQueryRule', () => {
  it('should work with rules at root', () => {
    const updatedQuery_1 = updateQueryRule(
      {
        operator: 'and',
        rules: [],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      {
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      }
    );

    const expectedUpdatedQuery_1 = {
      operator: 'and',
      rules: [
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A',
        },
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_1) === JSON.stringify(expectedUpdatedQuery_1)
    ).toBeTruthy();

    const updatedQuery_2 = updateQueryRule(
      {
        operator: 'and',
        rules: [
          {
            operator: 'in',
            property: 'myProperty',
            value: 'A',
          },
        ],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      {
        operator: 'in',
        property: 'myProperty',
        value: 'A,B',
      }
    );

    const expectedUpdatedQuery_2 = {
      operator: 'and',
      rules: [
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A,B',
        },
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_2) === JSON.stringify(expectedUpdatedQuery_2)
    ).toBeTruthy();
  });

  it('should work with rules at root and a ruleAccessor', () => {
    const updatedQuery_1 = updateQueryRule(
      {
        operator: 'and',
        rules: [],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      {
        name: 'myProperty_1',
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },
      (r) => r.name === 'myProperty_1'
    );

    const expectedUpdatedQuery_1 = {
      operator: 'and',
      rules: [
        {
          name: 'myProperty_1',
          operator: 'in',
          property: 'myProperty',
          value: 'A',
        },
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_1) === JSON.stringify(expectedUpdatedQuery_1)
    ).toBeTruthy();

    const updatedQuery_2 = updateQueryRule(
      {
        operator: 'and',
        rules: [
          {
            name: 'myProperty_1',
            operator: 'in',
            property: 'myProperty',
            value: 'A',
          },
        ],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      {
        name: 'myProperty_1',
        operator: 'in',
        property: 'myProperty',
        value: 'A,B',
      },
      (r) => r.name === 'myProperty_1'
    );

    const expectedUpdatedQuery_2 = {
      operator: 'and',
      rules: [
        {
          name: 'myProperty_1',
          operator: 'in',
          property: 'myProperty',
          value: 'A,B',
        },
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_2) === JSON.stringify(expectedUpdatedQuery_2)
    ).toBeTruthy();

    const updatedQuery_3 = updateQueryRule(
      {
        operator: 'and',
        rules: [
          {
            name: 'myProperty_1',
            operator: 'in',
            property: 'myProperty',
            value: 'A,B',
          },
        ],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      {
        name: 'myProperty_2',
        operator: 'in',
        property: 'myProperty',
        value: 'C,D',
      },
      (r) => r.name === 'myProperty_2'
    );

    const expectedUpdatedQuery_3 = {
      operator: 'and',
      rules: [
        {
          name: 'myProperty_1',
          operator: 'in',
          property: 'myProperty',
          value: 'A,B',
        },
        {
          name: 'myProperty_2',
          operator: 'in',
          property: 'myProperty',
          value: 'C,D',
        },
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_3) === JSON.stringify(expectedUpdatedQuery_3)
    ).toBeTruthy();
  });

  it('should work with rules in subgroup(s)', () => {
    const updatedQuery_1 = updateQueryRule(
      {
        operator: 'and',
        rules: [],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      {
        operator: 'in',
        property: 'nestedProp.myProperty',
        value: 'A',
      },
      null,
      {
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
          },
        ],
      }
    );

    const expectedUpdatedQuery_1 = {
      operator: 'and',
      rules: [],

      groups: [
        {
          name: 'all_of_these',
          operator: 'and',
          path: 'nestedProp',
          
          groups: [
            {
              name: 'any_of_these',
              operator: 'or',
              
              rules: [
                {
                  operator: 'in',
                  property: 'nestedProp.myProperty',
                  value: 'A',
                },
              ],
              groups: []
            },
          ],
          rules: []
        },
      ],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

   
    // console.log(JSON.stringify(updatedQuery_1));
    // console.log(JSON.stringify(expectedUpdatedQuery_1));
    expect(
      JSON.stringify(updatedQuery_1) === JSON.stringify(expectedUpdatedQuery_1)
    ).toBeTruthy();
  });
});

describe('clearQueryRule', () => {
  it('should work with rules at root', () => {
    const updatedQuery_1 = clearQueryRule(
      {
        operator: 'and',
        rules: [{
          operator: 'in',
          property: 'myProperty',
          value: 'A',
        }],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      'myProperty'
    );

    const expectedUpdatedQuery_1 = {
      operator: 'and',
      rules: [],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    // console.log(JSON.stringify(updatedQuery_1));

    expect(
     JSON.stringify(updatedQuery_1) === JSON.stringify(expectedUpdatedQuery_1)
    ).toBeTruthy();

    const updatedQuery_2 = clearQueryRule(
      {
        operator: 'and',
        rules: [
          {
            operator: 'in',
            property: 'myProperty',
            value: 'A,B',
          },
          {
            operator: 'in',
            property: 'myOtherProperty',
            value: 'C,D,E',
          },
        ],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      r => r.property === 'myProperty'
    );

    const expectedUpdatedQuery_2 = {
      operator: 'and',
      rules: [          {
        operator: 'in',
        property: 'myOtherProperty',
        value: 'C,D,E',
      },],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    // expect(
    //   JSON.stringify(updatedQuery_2) === JSON.stringify(expectedUpdatedQuery_2)
    // ).toBeTruthy();
  });

  it('should work with rules at root and a ruleAccessor', () => {
    const updatedQuery_1 = clearQueryRule(
      {
        operator: 'and',
        rules: [      {
          name: 'myProperty_1',
          operator: 'in',
          property: 'myProperty',
          value: 'A',
        }],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },

      (r) => r.name === 'myProperty_1'
    );

    const expectedUpdatedQuery_1 = {
      operator: 'and',
      rules: [
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_1) === JSON.stringify(expectedUpdatedQuery_1)
    ).toBeTruthy();

    const updatedQuery_2 = clearQueryRule(
      {
        operator: 'and',
        rules: [
          {
            name: 'myProperty_1',
            operator: 'in',
            property: 'myProperty',
            value: 'A,B',
          }, 
          {
            operator: 'in',
            property: 'myOtherProperty',
            value: 'C,D,E',
          },,
        ],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },

      (r) => r.name === 'myProperty_1'
    );

    const expectedUpdatedQuery_2 = {
      operator: 'and',
      rules: [          {
        operator: 'in',
        property: 'myOtherProperty',
        value: 'C,D,E',
      },],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_2) === JSON.stringify(expectedUpdatedQuery_2)
    ).toBeTruthy();

    const updatedQuery_3 = clearQueryRule(
      {
        operator: 'and',
        rules: [
          {
            name: 'myProperty_1',
            operator: 'in',
            property: 'myProperty',
            value: 'A,B',
          },
        ],

        groups: [],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      (r) => r.name === 'myProperty_2'
    );

    const expectedUpdatedQuery_3 = {
      operator: 'and',
      rules: [
        {
          name: 'myProperty_1',
          operator: 'in',
          property: 'myProperty',
          value: 'A,B',
        },
      ],

      groups: [],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

    expect(
      JSON.stringify(updatedQuery_3) === JSON.stringify(expectedUpdatedQuery_3)
    ).toBeTruthy();
  });

  it('should work with rules in subgroup(s)', () => {
    const updatedQuery_1 = clearQueryRule(
      {
        operator: 'and',
        rules: [],

        groups: [        {
          name: 'all_of_these',
          operator: 'and',
          path: 'nestedProp',
          
          groups: [
            {
              name: 'any_of_these',
              operator: 'or',
              
              rules: [
                {
                  operator: 'in',
                  property: 'nestedProp.myProperty',
                  value: 'A',
                },
              ],
              groups: []
            },
          ],
          rules: []
        }],
        sorts: [{ property: 'myProp', direction: 'desc' }],
        start: 0,
        limit: 10,
      },
      'nestedProp.myProperty',
      ['all_of_these', 'any_of_these']
    );

    const expectedUpdatedQuery_1 = {
      operator: 'and',
      rules: [],

      groups: [

      ],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    };

   
   
    expect(
      JSON.stringify(updatedQuery_1) === JSON.stringify(expectedUpdatedQuery_1)
    ).toBeTruthy();
  });
});


describe('getSubgroup', () => {
  it('should work with one level', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    expect(getSubgroup(query, ['all_of_these'])).toBeTruthy();

  });

  it('should work with more than one level', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    expect(getSubgroup(query, ['all_of_these', 'any_of_these'])).toBeTruthy();
    

  });

  it('should return undefined with one level if subgroup does not exist', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;

    const subgroup = getSubgroup(query, ['none_of_these']);
    expect(subgroup).toBeUndefined();

  });

  it('should return undefined with more than one level if subgroup does not exist', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;

    const subgroup = getSubgroup(query, ['all_of_these', 'none_of_these']);
    expect(subgroup).toBeUndefined();

  });
});

describe('getRuleFromQuery', () => { 
  it('should work at root', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    expect(getRuleFromQuery(query,  'myProperty')).toBeTruthy();

  });

  it('should work at root and with an accessor', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        name: 'myProperty_A',
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },{
        name: 'myProperty_B',
        operator: 'in',
        property: 'myProperty',
        value: 'B',
      }],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const rule = getRuleFromQuery(query, rule => rule.name === 'myProperty_B');
    expect(rule).toBeTruthy();
    expect(rule.value).toEqual('B');

  });

  it('should work with an accessor based on index', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        name: 'myProperty_A',
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },{
        name: 'myProperty_B',
        operator: 'in',
        property: 'myProperty',
        value: 'B',
      }],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const rule = getRuleFromQuery(query, (rule, index) => index === 1);
    expect(rule).toBeTruthy();
    expect(rule.value).toEqual('B');

  });

  it('should work within subgroups', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    expect(getRuleFromQuery(query,  'nestedProp.myProperty', ['all_of_these', 'any_of_these'])).toBeTruthy();

  });
  it('should work within subgroups and an accessor', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                name: 'nestedProp.myProperty_A',
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
              {
                name: 'nestedProp.myProperty_B',
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'B',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const rule = getRuleFromQuery(query,  r => r.name === 'nestedProp.myProperty_B', ['all_of_these', 'any_of_these']);
    expect(rule).toBeTruthy();
    expect(rule.value).toEqual('B');

  });

  it('should return undefined when the rule does not exist at root', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;

    const rule = getRuleFromQuery(query,  'myPropertyX');

    expect(rule).toBeUndefined();

  });
});

describe('getSelectedValuesForProperty', () => { 
  it('should work at root', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    expect(getSelectedValuesForProperty(query,  'myProperty')).toBeTruthy();

  });

  it('should work at root and with an accessor', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        name: 'myProperty_A',
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },{
        name: 'myProperty_B',
        operator: 'in',
        property: 'myProperty',
        value: ['B','C'],
      }],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const value = getSelectedValuesForProperty(query, rule => rule.name === 'myProperty_B');
    expect(value).toEqual(['B','C']);

  });

  it('should work with an accessor based on index', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        name: 'myProperty_A',
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },{
        name: 'myProperty_B',
        operator: 'in',
        property: 'myProperty',
        value: 'B',
      }],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const value = getSelectedValuesForProperty(query, (rule, index) => index === 1);

    expect(value).toEqual('B');

  });

  it('should work within subgroups', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const value = getSelectedValuesForProperty(query,  'nestedProp.myProperty', ['all_of_these', 'any_of_these']);
    expect(value).toEqual('A');

  });
  it('should work within subgroups and an accessor', () => {
    const query =       {
      operator: 'and',
      rules: [],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                name: 'nestedProp.myProperty_A',
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
              {
                name: 'nestedProp.myProperty_B',
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'B',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;
    const value = getSelectedValuesForProperty(query,  r => r.name === 'nestedProp.myProperty_B', ['all_of_these', 'any_of_these']);
    expect(value).toEqual('B');

  });

  it('should return undefined when the rule does not exist at root', () => {
    const query =       {
      operator: 'and',
      rules: [ {
        operator: 'in',
        property: 'myProperty',
        value: 'A',
      },],

      groups: [{
        name: 'all_of_these',
        operator: 'and',
        path: 'nestedProp',
        
        groups: [
          {
            name: 'any_of_these',
            operator: 'or',
            
            rules: [
              {
                operator: 'in',
                property: 'nestedProp.myProperty',
                value: 'A',
              },
            ],
            groups: []
          },
        ],
        rules: []
      }],
      sorts: [{ property: 'myProp', direction: 'desc' }],
      start: 0,
      limit: 10,
    } as Query;

    const value = getSelectedValuesForProperty(query,  'myPropertyX');

    expect(value).toBeUndefined();

  });
});