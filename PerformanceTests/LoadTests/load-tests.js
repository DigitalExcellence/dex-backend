import projectTest from './Tests/project-load-tests.js';
import dataSourceTest from './Tests/project-load-tests.js';


const defaultScenarioRuntime = 30;
const virtualUsersPerScenario = 100;

export let options = {
    // Test stages
    scenarios: {
        getDataSourceScenario: {
            executor: "constant-vus",
            vus: virtualUsersPerScenario,
            gracefulStop: '0s',
            tags: { test_type: 'getDataSourceScenario' },
            duration: defaultScenarioRuntime + 's',
            startTime: defaultScenarioRuntime * 0 + 's',
            // The function the scenario will execute.
            exec: "testDatasourceEndpoints"
        },
        getProjectsScenario: {
            // The executor the scenario will use see : https://k6.io/docs/using-k6/scenarios/executors/
            executor: "constant-vus",
            vus: virtualUsersPerScenario,
            gracefulStop: '0s',
            tags: { test_type: 'getProjectsScenario' },
            duration: defaultScenarioRuntime + 's',
            startTime: defaultScenarioRuntime * 1 + 's',
            // The function the scenario will execute.
            exec: "testProjectEndpoints"
        },
    },
    thresholds: {
        'http_req_duration{test_type:getDataSourceScenario}': ['p(95)<250', 'p(99)<350'],
        'http_req_failed{test_type:getDataSourceScenario}': ['rate<0.01'],
        'http_req_duration{test_type:getProjectsScenario}': ['p(95)<250', 'p(99)<350'],
        'http_req_failed{test_type:getProjectsScenario}': ['rate<0.01'],
    }
}

export function testProjectEndpoints() {
    projectTest();
}

export function testDatasourceEndpoints() {
    dataSourceTest();
}