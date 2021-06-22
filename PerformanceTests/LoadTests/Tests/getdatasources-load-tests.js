import http from 'k6/http';
import { Rate } from 'k6/metrics';

export let errorRate = new Rate('errors');

export default function() {
    const res = http.get(`${__ENV.BASE_ADDRESS}/Datasource`);

    const result = check(res, {
        'status is 200': (r) => r.status == 200,
    });

    if (!result) {
        errorRate.add(1)
    }
}