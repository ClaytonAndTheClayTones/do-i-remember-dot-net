import * as dotenv from 'dotenv'
import path from 'path'

export const initConfig = (): void => {
  dotenv.config({ path: path.join(__dirname + '/../.env') })
}
