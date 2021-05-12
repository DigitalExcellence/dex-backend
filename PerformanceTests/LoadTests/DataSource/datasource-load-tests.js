import http from 'k6/http';
import { sleep } from 'k6';

const DEFAULT_SLEEP_DURATION = 1;

export let options = {
    // Test stages
    stages: [
        { duration: '60s', target: 100 }, // simulate ramp-up of traffic from 60 to 200 users over 320 seconds.
        { duration: '200s', target: 100 }, // stay at 100 users for 200 seconds
        { duration: '60s', target: 0 }, // ramp-down to 0 users
    ],
    thresholds: {
        http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
        //'logged in successfully': ['p(99)<1500'], // 99% of requests must complete below 1.5s
    },
};

export default function(){
    http.get(`${__ENV.BASE_ADDRESS}/Datasource`);
    sleep(DEFAULT_SLEEP_DURATION);
}
