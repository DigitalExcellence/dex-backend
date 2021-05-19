import http from 'k6/http';
import { check, sleep } from 'k6';
import { Counter, Rate, Trend } from 'k6/metrics';

const DEFAULT_SLEEP_DURATION = 0.1; // Sleep duration in seconds.
const MAX_REQUEST_DURATION = 500; // in ms.
const MAX_FAIL_RATE = 0.01; // Max failrate should not be higher than 1%.

export let GetDataSourceDuration = new Trend('Get datasource Duration');
export let GetDataSourceFailRate = new Rate('Get datasource Fail Rate')
export let GetDataSourceReqs = new Counter('Get datasource Requests');

export let options = {
    thresholds: {
        'Get datasource Duration': [`p(99)<${MAX_REQUEST_DURATION}`], // 99% of requests must complete below the max duration.
        'Get datasource Fail Rate': [`rate<${MAX_FAIL_RATE}`] // Fail rate may not be more as the max failrate.
    }
};

export default function () {
    let res = http.get(`${__ENV.BASE_ADDRESS}/Datasource`, {
        tags: { name: 'GetDatasources' },
    });

    // Count the total amount of requests send to this endpoint.
    GetDataSourceReqs.add(1);
    // Add the duration of this request.
    GetDataSourceDuration.add(res.timings.duration);
    // Validate result
    const result = check(res, {
        'Get Datasources status is 200': (r) => r.status = 200,
    });
    GetDataSourceFailRate.add(!result);

    sleep(DEFAULT_SLEEP_DURATION);
}
