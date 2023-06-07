module.exports = {
  roots: ['<rootDir>'],
  testMatch: ['/**/*.test.ts', '!node_modules'],
  transform: {
    '^.+\\.(ts)$': 'ts-jest',
  },
  transformIgnorePatterns: ['node_modules/(?!@veho/.*)'],
  reporters: ['default'],
  globals: {
    RELEASE: 'test',
  },
  setupFilesAfterEnv: ['./src/jestSetup.ts', 'jest-expect-message'],
  clearMocks: true,
  collectCoverage: true,
  coverageReporters: ['json', 'lcov', 'clover', 'cobertura', 'text'],
  coverageDirectory: 'coverage',
  coveragePathIgnorePatterns: ['/node_modules/'],
  collectCoverageFrom: ['src/**/*.ts', '!src/**/*.test.ts'],
  testTimeout: 5000,
}
