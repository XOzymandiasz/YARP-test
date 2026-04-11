import http from 'k6/http';
import { check } from 'k6';

export const options = {
    scenarios: {
        backend_direct: {
            executor: 'constant-vus',
            vus: 100,
            duration: '1m',
            exec: 'backendTest',
        },
        yarp_gateway: {
            executor: 'constant-vus',
            vus: 100,
            duration: '1m',
            exec: 'yarpTest',
        },
    },
};

export function backendTest() {
    const res = http.get('http://host.docker.internal:5001/api/ping');

    check(res, {
        'backend status 200': (r) => r.status === 200,
    });
}

export function yarpTest() {
    const res = http.get('http://host.docker.internal:5000/a/api/ping');

    check(res, {
        'yarp status 200': (r) => r.status === 200,
    });
}