import { toHaveSamePropertiesAs } from "./customMatchers"

describe('QDK/CustomMatchers', () => {
  describe('toHaveTheSamePropertiesAs', () => {

    beforeEach(async () => {
      
    })

    afterEach(async () => {

    })

    beforeAll(async () => {  

    })

    it('returns a pass when expected has same properties as received', async () => {
 
      const result = toHaveSamePropertiesAs({
        key1: "value1",
        key2: "value2",
        key3: "value3"
      },{
        key1: "value1",
        key2: "value2",
        key3: "value3"
    }); 

    expect(result.message()).toEqual("Received and Expected objects have the same properties");
    expect(result.pass).toEqual(true); 
  })

    it('returns a pass when shape differences are ignored', async () => { 
      const result = toHaveSamePropertiesAs({
          key1: "value1",
          key2: "value2",
          key3: "value3"
        },{
          key1: "value1",
          key2: "value2",
          key4: "value4"
      },
      ["key3", "key4"]); 

      expect(result.message()).toEqual("Received and Expected objects have the same properties");
      expect(result.pass).toEqual(true); 
    })

    it('returns expected output for mismatched keys', async () => {
    
      const result = toHaveSamePropertiesAs({
            key1: "value1",
            key2: "value2",
            key4: "value4",
        },{
            key1: "value1",
            key2: "value2",
            key3: "value3"
        });

      expect(result).toBeTruthy();
      expect(result.pass).toEqual(false);
      expect(result.message()).toEqual(`Received object was missing the following keys found in the expected object: ['key3']\nExpected object was missing the following keys found in the received object: ['key4']`);

    })

    it('returns expected output for mismatched keys if expected extra key is ignored', async () => {
    
      const result = toHaveSamePropertiesAs({
            key1: "value1",
            key2: "value2",
            key4: "value4",
        },{
            key1: "value1",
            key2: "value2",
            key3: "value3"
        },
        ["key3"]);

      expect(result).toBeTruthy();
      expect(result.pass).toEqual(false);
      expect(result.message()).toEqual(`Expected object was missing the following keys found in the received object: ['key4']`);

    })

    it('returns expected output for mismatched keys if received extra key is ignored', async () => {
    
      const result = toHaveSamePropertiesAs({
            key1: "value1",
            key2: "value2",
            key4: "value4",
        },{
            key1: "value1",
            key2: "value2",
            key3: "value3"
        },
        ["key4"]);

      expect(result).toBeTruthy();
      expect(result.pass).toEqual(false);
      expect(result.message()).toEqual(`Received object was missing the following keys found in the expected object: ['key3']`);

    })

    it('returns expected output for mismatched key values with all shape matched', async () => {
    
      const result = toHaveSamePropertiesAs({
            key1: "value1",
            key2: "value2",
            key3: "value345",
        },{
            key1: "value1",
            key2: "value234",
            key3: "value3"
        });

      expect(result).toBeTruthy();
      expect(result.pass).toEqual(false);
      expect(result.message()).toEqual(
`The following keys had mismatched values:
{
  "expected": "value234",
  "received": "value2",
  "key": "key2"
}
{
  "expected": "value3",
  "received": "value345",
  "key": "key3"
}`
      ); 
    })
  })

  it('returns expected output, ignoring already-missed shape, ommitted shape, and ommitted value keys', async () => { 
    const result = toHaveSamePropertiesAs({
          key1: "value1", 
          key3: "value3",
          key4: "value4",
          key5: "value521",
          key6: "value6"
      },{
          key1: "value1", 
          key2: "value2", 
          key4: "value456", 
          key6: "value678"
      },["key5"],["key4"]);
 
    expect(result).toBeTruthy();
    expect(result.pass).toEqual(false);
    expect(result.message()).toEqual(
`Received object was missing the following keys found in the expected object: ['key2']
Expected object was missing the following keys found in the received object: ['key3']
The following keys had mismatched values:
{
  "expected": "value678",
  "received": "value6",
  "key": "key6"
}`
    ); 
  }) 
}) 


