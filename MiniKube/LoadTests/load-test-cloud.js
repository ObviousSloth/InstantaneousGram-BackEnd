import http from 'k6/http';
import { sleep, check } from 'k6';

export let options = {
  stages: [
    { duration: '2m', target: 10 },   // ramp up to 10 users over 2 minutes
    { duration: '5m', target: 10 },   // stay at 10 users for 5 minutes
    { duration: '2m', target: 50 },   // ramp up to 50 users over 2 minutes
    { duration: '5m', target: 50 },   // stay at 50 users for 5 minutes
    { duration: '2m', target: 100 },  // ramp up to 100 users over 2 minutes
    { duration: '5m', target: 100 },  // stay at 100 users for 5 minutes
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
