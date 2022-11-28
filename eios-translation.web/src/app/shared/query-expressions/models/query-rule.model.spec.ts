import { isQueryRule, queryRulesAreEquivalent } from './query-rule.model';

describe('QueryRule --> isQueryRule', () => {
  const rule = {
    operator: 'in',
    property: 'myProperty',
    value: 'A,B,C',
  };

  const spatialRule = {
    operator: 'pointInShape',
    property: 'locations.geoData',
    value: {
      type: 'FeatureCollection',
      features: [
        {
          type: 'Feature',
          properties: { layerType: 'circle', radius: '233904.54296845218m' },
          geometry: { type: 'Point', coordinates: [9.879741, 43.752976] },
        },
      ],
    },
  };

  const group = {
    name: 'myGroup',
    operator: 'and',
    rules: [rule, spatialRule],

    groups: [],
    limit: 10,
    start: 0,
  };

  it('should recognize valid rule', () => {
    expect(isQueryRule(rule)).toBeTruthy();
    expect(isQueryRule(spatialRule)).toBeTruthy();
  });

  it('should recognize invalid rules because property is missing', () => {
    expect(isQueryRule({ operator: 'in' })).toBeFalse(); // missing property
  });

  it('should recognize invalid rules, because they have a wrong operator', () => {
    expect(isQueryRule(group)).toBeFalse(); // other operator
  });
});

describe('QueryRule --> queryRulesAreEquivalent', () => {
  it('should consider rules that have the same property, operator and value to be equivalent', () => {
    expect(
      queryRulesAreEquivalent(
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A,B,C',
        },
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A,B,C',
        }
      )
    ).toBeTrue();
  });
  it('should consider terms rule that have only value to be equivalent to an equals rule', () => {
    expect(
      queryRulesAreEquivalent(
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A',
        },
        {
          operator: 'equals',
          property: 'myProperty',
          value: 'A',
        }
      )
    ).toBeTrue();
  });
  it('should consider rules that have the same property, operator and value'
  + ' to be equivalent even if they have adifferent name, if checkName param is false', () => {
    expect(
      queryRulesAreEquivalent(
        {
          name: 'prop',
          operator: 'in',
          property: 'prop.myProperty1',
          value: 'A,B,C',
        },
        {
          operator: 'in',
          property: 'prop.myProperty1',
          value: 'A,B,C',
        },
        false
      )
    ).toBeTrue();
  });

  it('should consider rules with different values not to be equivalent', () => {
    expect(
      queryRulesAreEquivalent(
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A,B,D',
        },
        {
          operator: 'in',
          property: 'myProperty',
          value: 'A,C',
        }
      )
    ).toBeFalse();
  });
  it('should consider rules with different property not to be equivalent', () => {
    expect(
      queryRulesAreEquivalent(
        {
          operator: 'in',
          property: 'myProperty1',
          value: 'A,B,C',
        },
        {
          operator: 'in',
          property: 'myProperty2',
          value: 'A,B,C',
        }
      )
    ).toBeFalse();
  });
  it('should consider rules with different path not to be equivalent', () => {
    expect(
      queryRulesAreEquivalent(
        {
          operator: 'in',
          path: 'prop',
          property: 'prop.myProperty1',
          value: 'A,B,C',
        },
        {
          operator: 'in',
          property: 'prop.myProperty1',
          value: 'A,B,C',
        }
      )
    ).toBeFalse();
  });
  
  it('should consider rules that have the same property, operator and value'
    + ' not to be equivalent even if they have adifferent name, if checkName param is true', () => {
    expect(
      queryRulesAreEquivalent(
        {
          name: 'prop',
          operator: 'in',
          property: 'prop.myProperty1',
          value: 'A,B,C',
        },
        {
          operator: 'in',
          property: 'prop.myProperty1',
          value: 'A,B,C',
        }
      )
    ).toBeFalse();
  });
});
