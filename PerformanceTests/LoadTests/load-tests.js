import projectTest from './Tests/project-load-tests.js';
import { options as projectTestOptions } from './Tests/project-load-tests.js';
import dataSourceTest from './Tests/project-load-tests.js';
import { options as dataSourceTestOptions } from './Tests/datasource-load-tests.js';


// Get the thresholds of each test
let thresholdOptionsSum = {} // here we use the imported options to create a sum op options to run all tests.
Object.entries(dataSourceTestOptions.thresholds).forEach(item =>
    thresholdOptionsSum[item[0]] = item[1]);
// Object.entries(projectTestOptions.thresholds).forEach(item =>
//     thresholdOptionsSum[item[0]] = item[1]);



export let options = {
    // Test stages
    scenarios: {
        getDataSourceScenario: {
            executor: "ramping-vus",
            stages: [
                { duration: '10s', target: 50 }, // simulate ramp-up of traffic from 60 to 200 users over 320 seconds.
                { duration: '30s', target: 50 }, // stay at 100 users for 200 seconds
                { duration: '10s', target: 0 }, // ramp-down to 0 users
            ],
            // The function the scenario will execute.
            exec: "testDatasourceEndpoints"
        },
        getProjectsScenario: {
            // The executor the scenario will use see : https://k6.io/docs/using-k6/scenarios/executors/
            executor: "ramping-vus",
            stages: [
                { duration: '10s', target: 50 }, // simulate ramp-up of traffic from 60 to 200 users over 320 seconds.
                { duration: '30s', target: 50 }, // stay at 100 users for 200 seconds
                { duration: '10s', target: 0 }, // ramp-down to 0 users
            ],
            // The function the scenario will execute.
            exec: "testProjectEndpoints"
        },   
    },
    //thresholds: thresholdOptionsSum
}

export function testProjectEndpoints() {
    projectTest();
}

export function testDatasourceEndpoints() {
    dataSourceTest();
}
