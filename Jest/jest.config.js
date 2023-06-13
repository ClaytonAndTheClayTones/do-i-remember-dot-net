module.exports = {
  roots: ['<rootDir>'],
  testMatch: ['/**/*.test.ts', '!node_modules'],
  transform: {
    '^.+\\.(ts)$': 'ts-jest',
  },
  transformIgnorePatterns: ['node_modules/'],
  reporters: ['default'],
  globals: {
    RELEASE: 'test',
  },
  preset: "ts-jest",
  setupFilesAfterEnv: ['./QDK/customMatchers.ts'],
  clearMocks: true,
  collectCoverage: true,
  coverageReporters: ['json', 'lcov', 'clover', 'cobertura', 'text'],
  coverageDirectory: 'coverage',
  coveragePathIgnorePatterns: ['/node_modules/'],
  collectCoverageFrom: ['src/**/*.ts', '!src/**/*.test.ts'],
  testTimeout: 30000  
}
