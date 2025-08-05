module.exports = {
    testDir: './',
    use: {
        baseURL: 'https://localhost:44361/api',
        ignoreHTTPSErrors: true, // Moet aanwezig zijn
    },
    projects: [
        {
            name: 'api-tests',
            testMatch: /.*\.test\.js/,
        },
    ],
};