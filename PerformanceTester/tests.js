import http from 'k6/http';
import { sleep } from 'k6';

export default function() {
    http.get('https://localhost:5001/api/Project?amountOnPage=12');
    sleep(1);
}