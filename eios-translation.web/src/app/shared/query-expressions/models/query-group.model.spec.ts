import { isQueryGroup, queryGroupsAreEquivalent } from './query-group.model';

describe('QueryGroup --> isQueryGroup', () => {
  const rule = {
    operator: 'in',
    property: 'myProperty',
    value: 'A,B,C',
  };

  const group = {
    name: 'myGroup',
    operator: 'and',
    rules: [rule],

    groups: [],
    limit: 10,
    start: 0,
  };

  it('should recognize valid groups', () => {
    expect(isQueryGroup(group)).toBeTruthy();
  });

  it('should recognize invalid groups', () => {
    expect(isQueryGroup(rule)).toBeFalse(); // other operator
  });
});

describe('QueryRule --> queryGroupsAreEquivalent', () => {
  it('should recognize same groups with no subgroups', () => {
    expect(
      queryGroupsAreEquivalent(
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
            },
          ],

          groups: [],
        },
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
            },
          ],

          groups: [],
        }
      )
    ).toBeTrue();
  });

  it('should recognize same groups with subgroups', () => {
    expect(
      queryGroupsAreEquivalent(
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
                    value: ['A','B'],
                  },
                  {
                    operator: 'in',
                    property: 'nestedProp.myOtherProperty',
                    value: 'B',
                  }
                ],
                groups: []
              },
            ],
            rules: []
          }]
        },      {
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
                    value: ['A','B'],
                  },
                  {
                    operator: 'in',
                    property: 'nestedProp.myOtherProperty',
                    value: 'B',
                  }
                ],
                groups: []
              },
            ],
            rules: []
          }]
        }
      )
    ).toBeTrue();
  });

  it('should recognize different groups with no subgroups', () => {
    expect(
      queryGroupsAreEquivalent(
        {
          operator: 'and',
          rules: [
            {
              name: 'myProperty_1',
              operator: 'in',
              property: 'myProperty',
              value: 'A,B,C',
            },
            {
              operator: 'in',
              property: 'myOtherProperty',
              value: 'C,D,E',
            },
          ],

          groups: [],
        },
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
            },
          ],

          groups: [],
        }
      )
    ).toBeFalse();
    expect(
      queryGroupsAreEquivalent(
        {
          operator: 'and',
          rules: [
            {
              name: 'myProperty_1',
              operator: 'in',
              property: 'myProperty',
              value: ['A','B','C']
            },
            {
              operator: 'in',
              property: 'myOtherProperty',
              value: 'C,D,E',
            },
          ],

          groups: [],
        },
        {
          operator: 'and',
          rules: [
            {
              name: 'myProperty_1',
              operator: 'in',
              property: 'myProperty',
              value: ['A','C']
            },
            {
              operator: 'in',
              property: 'myOtherProperty',
              value: 'C,D,E',
            },
          ],

          groups: [],
        }
      )
    ).toBeFalse();
  });

  it('should recognize different groups with subgroups', () => {
    expect(
      queryGroupsAreEquivalent(
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
                    value: ['A','B'],
                  },
                  {
                    operator: 'in',
                    property: 'nestedProp.myOtherProperty',
                    value: 'B',
                  }
                ],
                groups: []
              },
            ],
            rules: []
          }]
        },      {
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
                    value: ['A','B', 'C'],
                  },
                  {
                    operator: 'in',
                    property: 'nestedProp.myOtherProperty',
                    value: 'B',
                  }
                ],
                groups: []
              },
            ],
            rules: []
          }]
        }
      )
    ).toBeFalse();
  });
});