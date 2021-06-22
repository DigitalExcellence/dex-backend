import http from 'k6/http';

// This load test primary goal is to see what response times are for a average to a high load for a long duration.
export default function() {
    http.get(`${__ENV.BASE_ADDRESS}/Project?amountOnPage=12`);
}