import { mergeUnique } from "./common";

export type ValueMismatch = {
  expected: any,
  received: any,
  key: string
}


declare global {
  namespace jest {
    interface Matchers<R> {
      toBeWithinRange(a: number, b: number): R;
      toHaveSamePropertiesAs(expected: Record<string,any>, ignorePropertiesForShapeCheck?: string[], ignorePropertiesForValueCheck?: string[]) : R
    }
    
    interface Expect {
      toBeWithinRange(a: number, b: number): any;
      toHaveSamePropertiesAs(expected: Record<string,any>, ignorePropertiesForShapeCheck?: string[], ignorePropertiesForValueCheck?: string[]) : any
    }

    interface InverseAsymmetricMatchers {
      toBeWithinRange(a: number, b: number): any;
      toHaveSamePropertiesAs(expected: Record<string,any>, ignorePropertiesForShapeCheck?: string[], ignorePropertiesForValueCheck?: string[]) : any
    }
  }
}

expect.extend({
  toBeWithinRange : toBeWithinRange,
  toHaveSamePropertiesAs: toHaveSamePropertiesAs
})

export function toBeWithinRange(received: number, floor: number, ceiling: number) {
  const pass = received >= floor && received <= ceiling;
  if (pass) {
    return {
      message: () =>
        `expected ${received} not to be within range ${floor} - ${ceiling}`,
      pass: true,
    };
  } else {
    return {
      message: () =>
        `expected ${received} to be within range ${floor} - ${ceiling}`,
      pass: false,
    };
  }
}

export function toHaveSamePropertiesAs(received: Record<string,any>, expected: Record<string,any>, ignorePropertiesForShapeCheck: string[] = [], ignorePropertiesForValueCheck: string[] = []) {
  const expectedKeysForShape = Object.keys(expected).filter( ( item:string ) => !ignorePropertiesForShapeCheck.includes( item ) );

  const receivedKeysForShape = Object.keys(received).filter( ( item:string ) => !ignorePropertiesForShapeCheck.includes( item ) );

  const keysOnlyInExpected = expectedKeysForShape.filter((item: any) => { return !receivedKeysForShape.includes(item) });

  const keysOnlyInReceived = receivedKeysForShape.filter((item: any) => { return !expectedKeysForShape.includes(item) });
  
  const keysPass = keysOnlyInExpected.length === 0 && keysOnlyInReceived.length === 0;
 
  let messageString = "";

  if (!keysPass) { 

    if(keysOnlyInExpected.length > 0)
    {
      messageString += `Received object was missing the following keys found in the expected object: ['${keysOnlyInExpected.join("', '")}']`;
    }
    
    if(keysOnlyInReceived.length > 0)
    {
      if(messageString.length > 0)
      {
         messageString+="\n";
      }

      messageString += `Expected object was missing the following keys found in the received object: ['${keysOnlyInReceived.join("', '")}']`
    } 
  } 

  const allKeys = mergeUnique<string>(Object.keys(expected), Object.keys(received));

  const allKeysWorthChecking = allKeys.filter( ( item:string ) => !ignorePropertiesForShapeCheck.includes( item ) && !ignorePropertiesForValueCheck.includes(item) && !keysOnlyInExpected.includes(item) && !keysOnlyInReceived.includes(item))

  const mismatches :ValueMismatch[] = []
   
  allKeysWorthChecking.forEach((item: string) => {
    if(expected[item] != received[item])
    {
      mismatches.push({ expected :expected[item], received: received[item], key: item});
    } 
  });
    
  if(mismatches.length > 0)
  {
    if(messageString.length > 0)
    {
      messageString +="\n"
    }

    messageString += "The following keys had mismatched values:";
    
    mismatches.forEach((x:ValueMismatch) =>{
      messageString  += `\n${JSON.stringify(x, null, 2)}`
    })  
  }

  if(messageString.length > 0)
  { 
    return {
      message: () => messageString,
      pass: false,
    };
  } 
  else
  { 
    return {
      message: () =>
        `Received and Expected objects have the same properties`,
      pass: true,
    };
  } 
}