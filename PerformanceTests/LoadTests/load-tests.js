// Import the test code the default function is what every VU will execute.
import projectTest from './Tests/project-load-tests.js';
import dataSourceTest from './Tests/project-load-tests.js';

// This is the main function for running load tests using K6.
// For more information please read the K6 documentation found here: https://k6.io/docs/
// or the research about performancing testing found in the DEX Drive.

// Scenario runtime in seconds
const defaultScenarioRuntime = 300;
// Amount of virtual users for each test
const virtualUsersPerScenario = 100;

export let options = {
    // Test stages
    scenarios: {
        getDataSourceScenario: {
            // The executor the scenario will use see : https://k6.io/docs/using-k6/scenarios/executors/
            executor: 'constant-arrival-rate',
            // Amount of virtual users
            preAllocatedVUs: virtualUsersPerScenario,
            maxVUs: virtualUsersPerScenario,
            // amount of requests per second
            rate: 200, // 200 RPS, since timeUnit is the default 1s
            // No gracefull stop the time the tests right.
            gracefulStop: '0s',
            // Tracking tags.
            tags: { test_type: 'getDataSourceScenario' },
            // Test duration
            duration: defaultScenarioRuntime + 's',
            // The amount of time before the test will start since the cli is executed.
            startTime: defaultScenarioRuntime * 0 + 's',
            // The function the scenario will execute.
            exec: "testDatasourceEndpoints"
        },
        getProjectsScenario: {
            // The executor the scenario will use see : https://k6.io/docs/using-k6/scenarios/executors/
            executor: 'constant-arrival-rate',
            // Amount of virtual users
            preAllocatedVUs: virtualUsersPerScenario,
            maxVUs: virtualUsersPerScenario,
            // amount of requests per second
            rate: 200, // 200 RPS, since timeUnit is the default 1s
            // No gracefull stop the time the tests right.
            gracefulStop: '0s',
            // Tracking tags.
            tags: { test_type: 'getProjectsScenario' },
            // Test duration
            duration: defaultScenarioRuntime + 's',
            // The amount of time before the test will start since the cli is executed.
            startTime: defaultScenarioRuntime * 1 + 's',
            // The function the scenario will execute.
            exec: "testProjectEndpoints"
        },
    },
    thresholds: {
        // Request duration for getting datasources should be smaller than 250ms for 95% of the requests and under 350ms for 99% of the requests.
        'http_req_duration{test_type:getDataSourceScenario}': ['p(95)<250', 'p(99)<350'],
        // Requests for the getDataSource scenario that are allowed to fail must no be more than 1%.
        'checks{scenario:getDataSourceScenario}': ['rate<0.01'],
        'http_req_duration{test_type:getProjectsScenario}': ['p(95)<250', 'p(99)<350'],
        'checks{scenario:getProjectsScenario}': ['rate<0.01'],
    }
}

// This function will be called using the exec property of the scenario.
export function testProjectEndpoints() {
    projectTest();
}

// This function will be called using the exec property of the scenario.
export function testDatasourceEndpoints() {
    dataSourceTest();
}