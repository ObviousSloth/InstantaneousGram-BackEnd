import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
  stages: [

    { duration: '5m', target: 200 },  // stay at 200 users for 5 minutes
    { duration: '2m', target: 300 },  // ramp up to 300 users over 2 minutes
    { duration: '5m', target: 300 },  // stay at 300 users for 5 minutes
    { duration: '2m', target: 400 },  // ramp up to 400 users over 2 minutes
    { duration: '5m', target: 400 },  // stay at 400 users for 5 minutes
    { duration: '2m', target: 500 },  // ramp up to 500 users over 2 minutes
    { duration: '10m', target: 500 }, // stay at 500 users for 10 minutes
    { duration: '2m', target: 0 },    // ramp down to 0 users over 2 minutes
  ],
};

export default function () {
  let res = http.get('http://instantaneousgram.ddns.net/userprofile/api/test');
  check(res, {
    'status was 200': (r) => r.status == 200,
    'response time was <= 200ms': (r) => r.timings.duration <= 200,
  });
  sleep(1);
}
