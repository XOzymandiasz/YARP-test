import http from 'k6/http';
import { check } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 10000 },
    ],
};

export default function () {
    const res = http.get('http://host.docker.internal:5000/a/api/ping');

    check(res, {
        'status is 200': (r) => r.status === 200,
    });
}