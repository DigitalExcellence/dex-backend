import http from 'k6/http';
import { check, sleep } from 'k6';
import { Counter, Rate, Trend } from 'k6/metrics';

const DEFAULT_SLEEP_DURATION = 0.1; // Sleep duration in seconds.
const MAX_REQUEST_DURATION = 500; // in ms.
const MAX_FAIL_RATE = 0.1; // Max failrate should not be higher than 10%.

export let GetProjectsWithAmountDuration = new Trend('Get projects with amount Duration');
export let GetProjectsWithAmountFailRate = new Rate('Get projects with amount Fail Rate')
export let GetProjectsWithAmountReqs = new Counter('Get projects with amount Requests');

export let options = {
    thresholds: {
        'Get projects with amount Duration': [`p(99)<${MAX_REQUEST_DURATION}`], // 99% of requests must complete below the max duration
        'Get projects with amount Fail Rate': [`rate<${MAX_FAIL_RATE}`] // Fail rate may not be more as the max failrate.
    }
};

export default function () {
    let res = http.get(`${__ENV.BASE_ADDRESS}/Project?amountOnPage=12`, {
        tags: { name: 'GetProjectsWithAmount' },
    });

    // Count the total amount of requests send to this endpoint.
    GetProjectsWithAmountReqs.add(1);
    // Add the duration of this request.
    GetProjectsWithAmountDuration.add(res.timings.duration);
    // Validate result
    const result = check(res, {
        'Get projects with amount status is 200': (r) => r.status = 200,
    });
    GetProjectsWithAmountFailRate.add(!result);

    sleep(DEFAULT_SLEEP_DURATION);
}
