import { queriesAreEquivalent } from './query.model';

describe('Query --> queriesAreEquivalent', () => {
  it('should recognize same queries with no subgroups', () => {
    expect(
      queriesAreEquivalent(
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        }
      )
    ).toBeTrue();
  });

  it('should recognize same queries with subgroups', () => {
    expect(
      queriesAreEquivalent(
        {
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
                      value: ['A', 'B'],
                    },
                    {
                      operator: 'in',
                      property: 'nestedProp.myOtherProperty',
                      value: 'B',
                    },
                  ],
                  groups: [],
                },
              ],
              rules: [],
            },
          ],
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        },
        {
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
                      value: ['A', 'B'],
                    },
                    {
                      operator: 'in',
                      property: 'nestedProp.myOtherProperty',
                      value: 'B',
                    },
                  ],
                  groups: [],
                },
              ],
              rules: [],
            },
          ],
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        }
      )
    ).toBeTrue();
  });

  it('should recognize different queries with no subgroups', () => {
    expect(
      queriesAreEquivalent(
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        }
      )
    ).toBeFalse();
  });

  it('should recognize different queries with different types', () => {
    expect(
      queriesAreEquivalent(
        {
          types: ['entityA', 'entityB'],
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        },
        {
          types: ['entityB', 'entityC'],
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        }
      )
    ).toBeFalse();
  });
  it('should recognize different queries with different limits', () => {
    expect(
      queriesAreEquivalent(
        {
          types: ['entityA'],
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
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        },
        {
          types: ['entityA'],
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
          limit: 20,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        }
      )
    ).toBeFalse();
  });

  it('should recognize different queries with different sorts', () => {
    expect(
        queriesAreEquivalent(
        {
          types: ['entityA'],
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
          limit: 10,
          start: 0,
          sorts: [
              {
                  property: 'myProperty',
                  direction: 'asc'
              }
          ]
        },
        {
          types: ['entityA'],
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
          limit: 10,
          start: 0,
          sorts: [
              {
                  property: 'myProperty2',
                  direction: 'desc'
              }
          ]
        })
    ).toBeFalse(); 
  });

  it('should recognize different queries with subgroups', () => {
    expect(
      queriesAreEquivalent(
        {
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
                      value: ['A', 'B'],
                    },
                    {
                      operator: 'in',
                      property: 'nestedProp.myOtherProperty',
                      value: 'B',
                    },
                  ],
                  groups: [],
                },
              ],
              rules: [],
            },
          ],
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        },
        {
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
                      value: ['A', 'B', 'C'],
                    },
                    {
                      operator: 'in',
                      property: 'nestedProp.myOtherProperty',
                      value: 'B',
                    },
                  ],
                  groups: [],
                },
              ],
              rules: [],
            },
          ],
          limit: 10,
          start: 0,
          sorts: [
            {
              property: 'myProperty',
              direction: 'desc',
            },
          ],
        }
      )
    ).toBeFalse();
  });
});
