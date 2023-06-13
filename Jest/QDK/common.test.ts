import { convertObjectToQueryArgs, mergeUnique } from "./common" 

describe('QDK/common', () => {
  describe('mergeUnique', () => {

    beforeEach(async () => {
      
    })

    afterEach(async () => {

    })

    beforeAll(async () => {  

    })

    it('returns a pass when expected has same properties as received', async () => {
 
      const result = mergeUnique(["123","456","789"], ["123","789","111"]);

      expect(result).toEqual(["123","456","789","111"]); 
    });  
  }) 

  describe("convertObjectToQueryArgs", () => {
    const result = convertObjectToQueryArgs({ key1: "value1", key2: null, key3 : "value3", key4 : undefined, key5 : 123, key6: "asd asd asd", key7: ['111','222','333']})

    expect(result).toEqual("?key1=value1&key3=value3&key5=123&key6=asd%20asd%20asd&key7=111,222,333")
  })
}) 


