import http from 'k6/http';
import { check } from 'k6';

export const options = {
    vus: 100,
    duration: '10m',
};

export default function () {
    const res = http.get('http://host.docker.internal:5000/a/api/ping');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });
}