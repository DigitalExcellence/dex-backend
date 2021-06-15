// Import the test code the default function is what every VU will execute.
import getProjectsTest from './Tests/getprojects-soak-tests.js';

// This is the main function for running load tests using K6.
// For more information please read the K6 documentation found here: https://k6.io/docs/
// or the research about performancing testing found in the DEX Drive.

// Scenario runtime in seconds
const rampUpScenarioRuntime = 60;
const defaultScenarioRuntime = 14400; // 4 hours
const rampDownScenarioRuntime = 60;

// Amount of virtual users for each test
const virtualUsersPerScenario = 50;
const maxVirtualUsersPerScenario = 400;

export let options = {
    scenarios: {
        getProjectsScenario: {
            // The executor the scenario will use see THIS IS VERY IMPORTANT FOR EVERY TEST TYPE : https://k6.io/docs/using-k6/scenarios/executors/
            executor: 'ramping-arrival-rate',
            stages: [
                { target: virtualUsersPerScenario, duration: `${rampUpScenarioRuntime}s` },
                { target: maxVirtualUsersPerScenario, duration: `${defaultScenarioRuntime}s` },
                { target: virtualUsersPerScenario, duration: `${rampDownScenarioRuntime}s` },
            ],
            // Amount of virtual users to start with.
            preAllocatedVUs: virtualUsersPerScenario,
            // Maximun of virtual users that are making requests.
            maxVUs: maxVirtualUsersPerScenario,
            // No gracefull stop the time the tests right.
            gracefulStop: '0s',
            // Tracking tags.
            tags: { test_type: 'getProjectsScenario' },
            // The function the scenario will execute.
            exec: "testGetProjectEndpoints"
        },
    },
}

// This function will be called using the exec property of the scenario.
export function testGetProjectEndpoints() {
    getProjectsTest();
}